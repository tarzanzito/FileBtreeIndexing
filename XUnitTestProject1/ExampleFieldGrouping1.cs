
namespace XUnitTestProject1
{
    public class ExampleFieldGrouping1 : Candal.FieldsGrouping
    {
        public Candal.FieldLong Id;
        public Candal.FieldString Name;

        public ExampleFieldGrouping1()
        {
            Id = new Candal.FieldLong();
            Name = new Candal.FieldString(40);
        } //20 + 40 + 2 = 62

    }
}
