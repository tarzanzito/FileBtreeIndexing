namespace Candal
{
    public class HeaderRecord : Record
    {
        private FieldByteArray iniMark = new FieldByteArray(FileAttributes.INI_MARK.Length);

        public FieldLong TotalRecords = new FieldLong(); //20
        public FieldLong ActiveRecords = new FieldLong();  //20
        public FieldLong LastDeleted = new FieldLong();  //20
        public FieldLong IndexStartAt = new FieldLong();  //20

        private FieldByteArray endMark = new FieldByteArray(FileAttributes.END_MARK.Length);

        public HeaderRecord()
        {
            iniMark.Value = FileAttributes.INI_MARK;
            endMark.Value = FileAttributes.END_MARK;
        }
    }
}
