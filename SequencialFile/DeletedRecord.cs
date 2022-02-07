namespace Candal
{
    public class DeletedRecord : Record
    {
        public FieldLong LastDeleted = new FieldLong();
        public FieldNull Remaining;

        public DeletedRecord(int rowLength)
        {
            int fieldLength = LastDeleted.GetLength();

            int remainingLength = rowLength - fieldLength - (FileAttributes.FIELD_SEPARATOR.Length * 2) // *2 ->Two fields
                - FileAttributes.RECORD_STATUS_ACTIVE.Length - FileAttributes.RECORD_SEPARATOR.Length;

            Remaining = new FieldNull(remainingLength);

            base.DeletedRecordStatus = true;
        }
    }
}
