using System;
using System.Reflection;

namespace Candal
{
    public abstract class Record : FieldsGrouping
    {
        private bool _deletedRecordStatus = false;
        private int _totalLength;

        protected bool DeletedRecordStatus
        {
            get { return _deletedRecordStatus;  }
            set { _deletedRecordStatus = value; }
        }

        public override int GetLength()
        {
                this.LoadDefinitions();
                return _totalLength;
        }

        protected override void LoadDefinitions()
        {
            if (base.IsDefinitionsLoaded)
                return;

            base.LoadDefinitions();

            _totalLength = base.GetLength();
            _totalLength += (FileAttributes.RECORD_STATUS_ACTIVE.Length + FileAttributes.RECORD_SEPARATOR.Length);
        }

        public override byte[] Pack()
        {
            this.LoadDefinitions();

            byte[] recordArray = null;

            try
            {
                byte[] groupingArray = base.Pack();

                recordArray = new byte[_totalLength];

                //append record marks
                Array.Copy(groupingArray, 0, recordArray, 0, groupingArray.Length);

                int startAt = groupingArray.Length;

                //append status mark
                if (_deletedRecordStatus)
                    Array.Copy(FileAttributes.RECORD_STATUS_DELETE, 0, recordArray, startAt, FileAttributes.RECORD_STATUS_DELETE.Length);
                else
                    Array.Copy(FileAttributes.RECORD_STATUS_ACTIVE, 0, recordArray, startAt, FileAttributes.RECORD_STATUS_ACTIVE.Length);

                //append separator record mark
                startAt += FileAttributes.RECORD_STATUS_ACTIVE.Length;
                Array.Copy(FileAttributes.RECORD_SEPARATOR, 0, recordArray, startAt, FileAttributes.RECORD_SEPARATOR.Length);

                startAt += FileAttributes.RECORD_SEPARATOR.Length;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return recordArray;
        }

        public override void UnPack(byte[] bytes)
        {
            LoadDefinitions();

            try
            {
                if (bytes.Length != this.GetLength())
                    throw new Exception("Number of bytes counted not equal to incomming bytes.");
                
                Clear();
                
                //reduce Record array to FieldGrouping array (remove status and record sparator marks)
                int len = this.GetLength() - FileAttributes.RECORD_STATUS_ACTIVE.Length - FileAttributes.RECORD_SEPARATOR.Length;
                byte[] rowArray = new byte[len];
                Array.Copy(bytes, 0, rowArray, 0, len);

                base.UnPack(rowArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public override void Clear()
        {
            this.LoadDefinitions();

            try
            {
                base.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
