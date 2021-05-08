namespace XUnitTestProject1
{
    class StringRecord : Candal.Record
    {
        public Candal.FieldString Data;

        public StringRecord()
        {
            Data = new Candal.FieldString(40);
        }
    }
}
