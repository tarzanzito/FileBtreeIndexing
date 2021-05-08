namespace Candal
{
    public class BtreeRecord : Record
    {
        public FieldLong LeftIndex;
        public FieldLong RightIndex;
        public FieldLong RecordID;
        public FieldByteArray FreeKey;

        public BtreeRecord()
        {
            throw new System.Exception("Cannot use constructor(). Use constructor(Candal.FieldsGrouping FieldsGroup)");
        }

        public BtreeRecord(Candal.FieldsGrouping FieldsGroup)
        {
            LeftIndex = new FieldLong();
            RightIndex = new FieldLong();
            RecordID = new FieldLong();

            int len = FieldsGroup.GetLength();
            FreeKey = new FieldByteArray(len);
        }
    }
}
