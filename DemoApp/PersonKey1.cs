namespace DemoApp
{
    class PersonKey1 : Candal.FieldsGrouping
    {
        public Candal.FieldString Name;

        public PersonKey1()
        {
            Name = new Candal.FieldString(40);
        }
    }
}
