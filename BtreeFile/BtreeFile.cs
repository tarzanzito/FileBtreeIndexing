using System;

namespace Candal
{
    /// <summary>
    /// where - é uma restrição para T. Neste caso apenas pode ser : Candal.FieldsGrouping ou seus descendentes
    /// new() - pode dentro da classe criar instancias de T (T x = new T();)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BtreeFile<T> where T : Candal.FieldsGrouping, new()
    {
        private T _fieldGroupingKey;
        private BtreeRecord _btreeRecord;
        private BtreeRecord _btreeRecordStack;
        private long _indexParent;

        private SequencialFile<BtreeRecord> _sequencialFile;
        private bool _isKeyFound;
        private bool _isKeyDeleted;

        public BtreeFile(string fileName)
        {
            _fieldGroupingKey = new T(); // T = PersonKey1
            //_fieldGroupingKey = (T) Activator.CreateInstance(typeof(T)); (arternative)

            _sequencialFile = new SequencialFile<BtreeRecord>(fileName, _fieldGroupingKey);
            _btreeRecord = _sequencialFile.GetRecordInstance();
            _btreeRecordStack = new BtreeRecord(_fieldGroupingKey);
        }

        public bool IsKeyFound
        {
            get
            {
                return _isKeyFound;
            }
        }

        public bool IsKeyDeleted
        {
            get
            {
                return _isKeyDeleted;
            }
        }

        public HeaderRecord Header
        {
            get
            {
                return _sequencialFile.Header;
            }
        }

        public void Create()
        {
            _sequencialFile.Create();
        }

        public void Open()
        {
            _sequencialFile.Open();
        }

        public void Close()
        {
            _sequencialFile.Close();
        }

        //public BtreeRecord ReadById(long id)
        //{
        //    return Read(id);
        //}

        public long SearchByKey(FieldsGrouping rowKey)
        {
            bool isResolved = false;
            long indexPos;
            long indexNext = 0;
            int compRes;
            byte[] keyIn;
            byte[] keyRead;

            _isKeyFound = false;
            _indexParent = 0;

            _sequencialFile.HeaderRead();
            indexPos = this._sequencialFile.Header.IndexStartAt.Value;

            keyIn = rowKey.Pack();

            while (!isResolved)
            {
                _btreeRecord = Read(indexPos);
                if (!_isKeyFound)
                {
                    isResolved = true;
                    break;
                    //throw new Exception("RecordID not found.");
                }

                keyRead = _btreeRecord.FreeKey.Value;
                compRes = Utils.ByteArraysCompare(keyIn, keyRead);

                switch (compRes)
                {
                    case -1:
                        indexNext = _btreeRecord.LeftIndex.Value;
                        goto case 9;

                    case 1:
                        indexNext = _btreeRecord.RightIndex.Value;
                        goto case 9;

                    case 9: //common
                        if (indexNext == 0)
                        {
                            _isKeyFound = false;
                            isResolved = true;
                            //throw new Exception("Key not found. B");
                        }
                        else
                        {
                            _indexParent = indexPos;
                            indexPos = indexNext;
                        }
                        break;

                    default:
                        _isKeyFound = true;
                        isResolved = true;
                        break;
                }

            }

            return indexPos; 
        }

        public BtreeRecord Read(long index)
        {
            _btreeRecord = _sequencialFile.ReadRecord(index);
            _isKeyFound = _sequencialFile.IsRecordFound;
            _isKeyDeleted = _sequencialFile.IsRecordDeleted;

            if (_isKeyFound)
            {
                byte[] freeKeyArray = _btreeRecord.FreeKey.Value;
                _fieldGroupingKey.UnPack(freeKeyArray);
            }

            return _btreeRecord;
        }

        private long Add(long recordID, FieldsGrouping rowKey)
        {
            byte[] freeKeyArray = rowKey.Pack();

           _btreeRecord.LeftIndex.Value = 0;
           _btreeRecord.RightIndex.Value = 0;
           _btreeRecord.RecordID.Value = recordID;
           _btreeRecord.FreeKey.Value = freeKeyArray;

            return _sequencialFile.AddRecord(_btreeRecord);
        }

        private long UpdateLeft(long index, long leftIndex)
        {
            _btreeRecord.LeftIndex.Value = leftIndex;

            return _sequencialFile.UpdateRecord(index, _btreeRecord);
        }
        
        private long UpdateRight(long index, long rightIndex)
        {
            _btreeRecord.RightIndex.Value = rightIndex;

            return _sequencialFile.UpdateRecord(index, _btreeRecord);
        }
        
        private long UpdateRecordID(long index, long recordID)
        {
            _btreeRecord.RecordID.Value = recordID;

            return _sequencialFile.UpdateRecord(index, _btreeRecord);
        }

        //private long DeleteKey1(FieldsGrouping rowKey)
        //{
        //    long indexID = SearchByKey(rowKey);

        //    if (!_isKeyFound)
        //                throw new Exception("Key not found.");
            
        //        if (!_isKeyDeleted)
        //        {
        //            _btreeRecord.RecordID.Value = 0;
        //            indexID = _sequencialFile.UpdateRecord(indexID, _btreeRecord);
        //        }
        //        else
        //            throw new Exception("Key is deleted.");

        //    return indexID;
        //}

        public long DeleteKey(FieldsGrouping rowKey)
        {
            long indexID = SearchByKey(rowKey);
            if (!_isKeyFound)
                throw new Exception("Key not found.");

            //about current record
            long recLeft = _btreeRecord.LeftIndex.Value;
            long recRight = _btreeRecord.RightIndex.Value;
            bool isLeftSide = true;

            if (_indexParent > 0)
            {
                //read parent node
                Read(_indexParent);

                isLeftSide = (_btreeRecord.LeftIndex.Value == indexID);

                //update parent node: with new node location left or right
                if (isLeftSide)
                    UpdateLeft(_indexParent, recLeft);
                else
                    UpdateLeft(_indexParent, recRight);
            }

            //find last empty node tree
            long indexNew = 0;
            long indexPos = 0;

            //start at
            ////Read(indexID);
            if (isLeftSide)
            {
                indexNew = recLeft;
                //if (_btreeRecord.LeftIndex.Value > 0)
                //    indexNew = _btreeRecord.LeftIndex.Value;
            }
            else
            { 
                indexNew = recRight;
                //if (_btreeRecord.RightIndex.Value > 0)
                //    indexNew = _btreeRecord.RightIndex.Value;
            }

            if (indexNew == 0)
                return 0;

            //has children then find last empty
            while (indexNew > 0)
            {
                indexPos = indexNew;
                _btreeRecord = Read(indexNew);
                if (!_isKeyFound)
                    throw new Exception("RecordID not found.");

                if (isLeftSide)
                    indexNew = _btreeRecord.RightIndex.Value;
                else
                    indexNew = _btreeRecord.LeftIndex.Value;
            }

            //set node tree
            if (isLeftSide)
                UpdateRight(indexPos, recRight);
            else
                UpdateLeft(indexPos, recLeft);

            //delete record
            _sequencialFile.DeleteRecord(indexID);

            //Case delete first row of database: set Header.starAt equal recLeft     
            if (_indexParent == 0)
            {
                Header.ActiveRecords.Value = recLeft;
                _sequencialFile.HeaderWrite();
            }

            return indexID;
        }

        public long AddKey(long recordID, FieldsGrouping rowKey)
        {
            bool isResolved = false; 
            long indexPos = 1;
            long indexNext = 0;
            long indexNew = 0;
            int compareResult;
            byte[] keyNew;
            byte[] keyRead;

            keyNew = rowKey.Pack();

            while (!isResolved) 
            {
                _btreeRecord = Read(indexPos);  //_btreeRecord A
                if (!_isKeyFound)
                {
                    if (indexPos == 1)
                    {
                        indexNew = Add(recordID, rowKey);
                        break;
                    }
                    else
                        throw new Exception("RecordID not found. File integrity error.");
                }

                PutRecordOnStack(); //save _btreeRecord A
                keyRead = _btreeRecord.FreeKey.Value;
                compareResult = Utils.ByteArraysCompare(keyNew, keyRead);

                switch (compareResult)
                {
                    case -1:
                        indexNext = _btreeRecord.LeftIndex.Value;
                        if (indexNext == 0)
                        {
                            indexNew = Add(recordID, rowKey); //_btreeRecord B
                            GetRecordFromStack();
                            UpdateLeft(indexPos, indexNew); //_btreeRecord A
                            isResolved = true;
                        }
                        else
                            indexPos = indexNext;
                        break;

                    case 1:
                        indexNext = _btreeRecord.RightIndex.Value;
                        if (indexNext == 0)
                        {
                            indexNew = Add(recordID, rowKey); //_btreeRecord = B
                            GetRecordFromStack();
                            UpdateRight(indexPos, indexNew); //_btreeRecord = A
                            isResolved = true;
                        }
                        else
                            indexPos = indexNext;
                        break;

                    case 9:

                    default:
                        if (!_isKeyDeleted)
                            throw new Exception("Duplicate key found.");
                        else
                            UpdateRecordID(indexPos, recordID); //_btreeRecord = B
                        break;
                }
            }

            return indexNew;
        }

        private void GetRecordFromStack()
        {
            _btreeRecord.LeftIndex.Value = _btreeRecordStack.LeftIndex.Value;
            _btreeRecord.RightIndex.Value = _btreeRecordStack.RightIndex.Value;
            _btreeRecord.RecordID.Value = _btreeRecordStack.RecordID.Value;
            _btreeRecord.FreeKey.Value = _btreeRecordStack.FreeKey.Value;
        }

        private void PutRecordOnStack()
        {
            _btreeRecordStack.LeftIndex.Value = _btreeRecord.LeftIndex.Value;
            _btreeRecordStack.RightIndex.Value = _btreeRecord.RightIndex.Value;
            _btreeRecordStack.RecordID.Value = _btreeRecord.RecordID.Value;
            _btreeRecordStack.FreeKey.Value = _btreeRecord.FreeKey.Value;
        }

    }
}
