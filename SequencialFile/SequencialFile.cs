using System;
using System.IO;

namespace Candal
{
    /// <summary>
    /// Sequencial file with fix record length
    /// With management of reuse of deleted records
    /// </summary>
    public class SequencialFile<T> where T : Record, new()
    {
        private T _currentRecord;
        private FileStream _fileStream;
        private HeaderRecord _headerRecord;
        private DeletedRecord _deletedRecord;
        private string _fileName;
        private int _effectiveRecordLength;
        private bool _isRecordFound;
        private bool _isRecordDeleted;
        private bool _isRecordOk;

        public bool IsRecordFound
        {
            get { return _isRecordFound; }
        }

        public bool IsRecordDeleted
        {
            get { return _isRecordDeleted; }
        }

        public HeaderRecord Header
        {
            get
            {
                return _headerRecord;
            }
        }

        public DeletedRecord DeletedRecord
        {
            get
            {
                return _deletedRecord;
            }
        }

        private SequencialFile()
        {
            _fileStream = null;
            _isRecordFound = false;
            _isRecordDeleted = false;
        }

        public SequencialFile(string fileName) : this()
        {
            _fileName = fileName;

            //_currentRecord = (T) Activator.CreateInstance(typeof(T));
            _currentRecord = new T();

            SequencialFileAfter();
        }

        //Special Constructor for BTreeFile class
        public SequencialFile(string fileName, Candal.FieldsGrouping key) : this()
        {
            _fileName = fileName;

            //Special case: T = BtreeRecord. To create a BtreeRecord the constructor needs to know who is freeKey and calculate the freeKey length
            //See: public BtreeRecord(Candal.FieldsGrouping FieldsGroup)
            _currentRecord = (T) Activator.CreateInstance(typeof(T), key);

            SequencialFileAfter();
        }

        private void SequencialFileAfter()
        {
            _headerRecord = new HeaderRecord();

            int rowLength = _currentRecord.GetLength() - FileAttributes.RECORD_STATUS_ACTIVE.Length + FileAttributes.RECORD_SEPARATOR.Length;

            if (rowLength < FieldLong.FIELD_LENGTH)  //less record size is lastDeleted field size
                rowLength = FieldLong.FIELD_LENGTH + FieldLong.FIELD_LENGTH;

            _effectiveRecordLength = rowLength;

            _deletedRecord = new DeletedRecord(_effectiveRecordLength);
        }

        public T GetRecordInstance()
        {
            _currentRecord.Clear();
            return _currentRecord;
        }

        public void Create()
        {
            if (_fileStream != null)
                return;

            _headerRecord.IndexStartAt.Value = 1;
            byte[] headerArray = _headerRecord.Pack(); //3

            try
            {
                _fileStream = new FileStream(_fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                _fileStream.Write(headerArray);
                _fileStream.Flush();
                _fileStream.Close();
                _fileStream = null;
            }
            catch
            {
                _fileStream = null;
                throw;
            }
        }

        public void Open()
        {
            if (_fileStream != null)
                throw new Exception("The File should be close.");

            try
            {
                _fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                HeaderRead();
            }
            catch
            {
                _fileStream = null;
                throw;
            }
        }

        public void Close()
        {
            if (_fileStream == null)
                throw new Exception("The File should be open.");

            try
            {
                _fileStream.Flush();
                _fileStream.Close();
                _fileStream = null;
            }
            catch
            {
                _fileStream = null;
                throw;
            }
        }

        public T ReadRecord(long recordID)
        {
            _currentRecord.Clear();
            _deletedRecord.Clear();

            byte[] byteArray = Read(recordID);

            if (_isRecordFound)
                _currentRecord.UnPack(byteArray);
            else if (_isRecordDeleted)
                _deletedRecord.UnPack(byteArray);

            return _currentRecord;
        }

        public long AddRecord(T record)
        {
            byte[] recordArray = record.Pack();

            return Add(recordArray);
        }
        
        public long UpdateRecord(long recordID, T record)
        {
            byte[] recordArray = record.Pack();

            return Update(recordID, recordArray);
        }

        public long DeleteRecord(long recordID)
        {
            return Delete(recordID);
        }

        private byte[] Read(long recordID)
        {
            if (_fileStream == null)
                throw new Exception("The File should be open.");

            if (recordID < 1)
                throw new Exception("recordID must be greater than 0.");

            byte[] recordArray = new byte[_effectiveRecordLength];

            try
            {
                //seek and read
                long offsetIn = OffsetCalculate(recordID);
                long offsetOut = _fileStream.Seek(offsetIn, SeekOrigin.Begin);
                int qt = _fileStream.Read(recordArray);

                //validates the length of bytes read
                if ((qt > 0) && (qt != _effectiveRecordLength))
                    throw new Exception("Record length error.");

                //get record status
                int startAt = _effectiveRecordLength - FileAttributes.RECORD_STATUS_ACTIVE.Length - FileAttributes.RECORD_SEPARATOR.Length;
                byte[] statusArray = new byte[FileAttributes.RECORD_STATUS_ACTIVE.Length];
                Array.Copy(recordArray, startAt, statusArray, 0, FileAttributes.RECORD_STATUS_ACTIVE.Length);

                //validate record status
                _isRecordDeleted = Utils.ByteArraysEquals(statusArray, FileAttributes.RECORD_STATUS_DELETE);
                bool isRecordActive = Utils.ByteArraysEquals(statusArray, FileAttributes.RECORD_STATUS_ACTIVE);
                _isRecordOk = (qt != 0);
                _isRecordFound = (_isRecordOk && isRecordActive);
            }
            catch
            {
                _fileStream = null;
                throw;
            }

            return recordArray;
        }

        /// <summary>
        /// Verify if the header has any deletedRecord
        /// if Yes then
        ///   search for deleted record referenced in header
        ///   put the record deletedValue into header
        ///   update record 
        ///   update header
        /// else
        ///   add new
        /// </summary>
        /// <param name="dataArray"></param>
        private long Add(byte[] recordArray)
        {
            if (_fileStream == null)
                throw new Exception("The File should be open.");

            if (recordArray.Length != _effectiveRecordLength)
                throw new Exception("Data record array length invalid. see rowLength value in consctructors");

            long recordID = 0;

            try
            {
                //set record status
                int startAt = _effectiveRecordLength - FileAttributes.RECORD_STATUS_ACTIVE.Length - FileAttributes.RECORD_SEPARATOR.Length;
                Array.Copy(FileAttributes.RECORD_STATUS_ACTIVE, 0, recordArray, startAt, FileAttributes.RECORD_STATUS_ACTIVE.Length);

                HeaderRead();

                //get last deleted record from header
                recordID = _headerRecord.LastDeleted.Value;
                if (recordID == 0) //it's an add record
                {
                    //save
                    _fileStream.Seek(0, SeekOrigin.End);
                    _fileStream.Write(recordArray);
                    _fileStream.Flush();

                    //increment totalRecords in header
                    _headerRecord.TotalRecords.Value++;
                    recordID = _headerRecord.TotalRecords.Value;
                }
                else //it's an update record
                {
                    //read last Deleted record 
                    byte[] deletedRecordArray = Read(recordID);

                    if (!_isRecordOk)
                        throw new Exception("Record not found.");
                    if (!_isRecordDeleted)
                        throw new Exception("Record found should be with deleted status.");

                    //get nextDeleteID field from current record V2
                    _deletedRecord.UnPack(deletedRecordArray);
                    long nextDeleted = _deletedRecord.LastDeleted.Value;

                    //update lastDeleteID header with current record info
                    _headerRecord.LastDeleted.Value = nextDeleted; //.Value;

                    //save
                    Update(recordID, recordArray);
                }

                //increment activeRecords in header
                _headerRecord.ActiveRecords.Value++;
                HeaderWrite();
            }
            catch
            {
                _fileStream = null;
                throw;
            }

            return recordID;
        }

        private long Update(long recordID, byte[] recordArray)
        {
            if (_fileStream == null)
                throw new Exception("The File should be open.");

            if (recordArray.Length != _effectiveRecordLength)
                throw new Exception("Data record array length invalid. see rowLength value in consctructors");

            if (recordID < 1)
                throw new Exception("recordID must be greater than 0.");

            try
            {
                //read record
                byte[] recordArrayRead = Read(recordID);
                if (!_isRecordOk)
                    throw new Exception("Record not found.");

                //get record status
                int startAt = _effectiveRecordLength - FileAttributes.RECORD_STATUS_ACTIVE.Length - FileAttributes.RECORD_SEPARATOR.Length;
                byte[] statusArray = new byte[FileAttributes.RECORD_STATUS_ACTIVE.Length];
                Array.Copy(recordArray, startAt, statusArray, 0, FileAttributes.RECORD_STATUS_ACTIVE.Length);

                //set record status
                if (!Utils.ByteArraysEquals(statusArray, FileAttributes.RECORD_STATUS_DELETE))
                    Array.Copy(FileAttributes.RECORD_STATUS_ACTIVE, 0, recordArray, startAt, FileAttributes.RECORD_STATUS_ACTIVE.Length);

                //save
                long offset = OffsetCalculate(recordID);
                _fileStream.Seek(offset, SeekOrigin.Begin);
                _fileStream.Write(recordArray);
                _fileStream.Flush();
            }
            catch
            {
                _fileStream = null;
                throw;
            }

            return recordID;
        }

        /// <summary>
        /// read header
        /// put lastDeletedID header field into the current record
        /// signal record as deleted
        /// update record 
        /// update header set field lastDeleted = recordID
        /// </summary>
        /// <param name="recordID"></param>
        private long Delete(long recordID)
        {
            if (_fileStream == null)
                throw new Exception("The File should be open.");

            if (recordID < 1)
                throw new Exception("recordID must be greater than 0.");

            try
            {
                //read record
                byte[] recordArray = Read(recordID);
                if (!_isRecordFound)
                    throw new Exception("Record not found.");

                if (_isRecordDeleted)
                    throw new Exception("Record is already deleted.");

                HeaderRead();

                //get last deleted field from header
                long lastDeleted = _headerRecord.LastDeleted.Value;

                //mount deleted record
                _deletedRecord.LastDeleted.Value = lastDeleted;
                recordArray = _deletedRecord.Pack(); 

                //save
                Update(recordID, recordArray);

                //update header: passing current recordID into lastDeletedID in headerRecord
                _headerRecord.LastDeleted.Value = recordID;
                _headerRecord.ActiveRecords.Value--;

                HeaderWrite();
            }
            catch
            {
                _fileStream = null;
                throw;
            }

            return recordID;
        }

        public void HeaderRead()
        {
            if (_fileStream == null)
                throw new Exception("The File should be open.");

            try
            {
                byte[] byteArray = new byte[_headerRecord.GetLength()];

                long offsetOut = _fileStream.Seek(0, SeekOrigin.Begin);
                int qt = _fileStream.Read(byteArray);

                if (qt != _headerRecord.GetLength())
                    throw new Exception("Header Record length error.");

                _headerRecord.UnPack(byteArray);
            }
            catch
            {
                _fileStream = null;
                throw;
            }
        }

        public void HeaderWrite()
        {
            if (_fileStream == null)
                throw new Exception("The File should be open.");

            try
            {
                byte[] byteArray = _headerRecord.Pack();

                _fileStream.Seek(0, SeekOrigin.Begin);
                _fileStream.Write(byteArray);
                _fileStream.Flush();
            }
            catch
            {
                _fileStream = null;
                throw;
            }
        }

        private long OffsetCalculate(long recordID)
        {
            return ((recordID - 1) * _effectiveRecordLength) + _headerRecord.GetLength();
        }

        public void Compress() //TODO RECORFIRMAR !!!!
        {
            bool opened = (_fileStream != null);

            try
            {
                if (!opened)
                    Open();

                HeaderRead();

                ////get last deleted record from header
                long activeRecords = _headerRecord.ActiveRecords.Value;


                //create temp file
                HeaderRecord tempHeaderRecord = new HeaderRecord();
                byte[] tempHheaderArray = tempHeaderRecord.Pack();

                FileStream tempFileStream = new FileStream(_fileName + ".tmp", FileMode.Create, FileAccess.Write, FileShare.None);
                tempFileStream.Write(tempHheaderArray);
                tempFileStream.Flush();
                tempFileStream.Close();


                //open temp file
                tempFileStream = new FileStream(_fileName + ".tmp", FileMode.Open, FileAccess.ReadWrite, FileShare.None);

                byte[] recordArray = new byte[_effectiveRecordLength + 1];

                long count = 0;
                long offsetIn = OffsetCalculate(1);
                long offsetOut = _fileStream.Seek(offsetIn, SeekOrigin.Begin);

                bool recordDeleted;
                bool recordActive;
                bool recordFound = true;

                //process records
                while (recordFound)
                {
                    int qt = _fileStream.Read(recordArray);

                    //PMFG rever
                    if ((qt > 0) && (qt != (_effectiveRecordLength + FileAttributes.RECORD_STATUS_ACTIVE.Length + FileAttributes.RECORD_SEPARATOR.Length)))
                        throw new Exception("Record length error.");

                    //get record status
                    byte[] statusArray = new byte[FileAttributes.RECORD_STATUS_ACTIVE.Length];
                    Array.Copy(recordArray, 0, statusArray, _effectiveRecordLength, FileAttributes.RECORD_STATUS_ACTIVE.Length);

                    //validate record status
                    recordDeleted = (Utils.ByteArraysEquals(statusArray, FileAttributes.RECORD_STATUS_DELETE));
                    recordActive = (Utils.ByteArraysEquals(statusArray, FileAttributes.RECORD_STATUS_ACTIVE));

                    //recordFound = ((qt != 0) && recordActive);
                    recordFound = (qt != 0);

                    //save temp file
                    if (recordFound && !recordDeleted)
                    {
                        count++;
                        tempFileStream.Seek(0, SeekOrigin.End);
                        tempFileStream.Write(recordArray);
                        tempFileStream.Flush();
                    }
                }


                //save header temp file
                tempHeaderRecord.ActiveRecords.Value = count;
                tempHeaderRecord.TotalRecords.Value = count;
                tempHeaderRecord.LastDeleted.Value = 0;

                tempHheaderArray = tempHeaderRecord.Pack();

                tempFileStream.Seek(0, SeekOrigin.Begin);
                tempFileStream.Write(tempHheaderArray);
                tempFileStream.Flush();

                tempFileStream.Close();

                //End process


                Close();

                if (activeRecords != count)
                    throw new Exception("Number of records is not consistence");

                File.Delete(_fileName);
                File.Move(_fileName + ".tmp", _fileName, true);


                if (opened)
                    Open();
            }
            catch
            {
                File.Delete(_fileName + ".tmp");
                Close();
                throw;
            }
        }
    }
}
