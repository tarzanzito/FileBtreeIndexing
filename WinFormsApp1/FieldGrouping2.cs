
namespace WinFormsApp1
{
    public class FieldGrouping2 : Candal.FieldsGrouping
    {
        public FieldGrouping1 Group1;
        public Candal.FieldString Address;

        public FieldGrouping2()
        {
            Group1 = new FieldGrouping1();
            Address = new Candal.FieldString(40);
        } //62 + 40 + 2 = 104
    }
}
