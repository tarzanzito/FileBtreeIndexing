
namespace XUnitTestProject1
{
    public class ExampleFieldGrouping2 : Candal.FieldsGrouping
    {
        public ExampleFieldGrouping1 Group1;
        public Candal.FieldString Address;

        public ExampleFieldGrouping2()
        {
            Group1 = new ExampleFieldGrouping1();
            Address = new Candal.FieldString(40);
        } //62 + 40 + 2 = 104
    }
}
