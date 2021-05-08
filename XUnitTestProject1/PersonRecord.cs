namespace XUnitTestProject1
{
    class PersonRecord : Candal.Record
    {
        public Candal.FieldString Name;
        public Candal.FieldShort Age;
        public Candal.FieldDate Born;
        public Candal.Fieldecimal Salary;

        public PersonRecord()
        {
            Name = new Candal.FieldString(40);
            Age = new Candal.FieldShort();
            Born = new Candal.FieldDate();
            Salary = new Candal.Fieldecimal();
        }
    }
}
