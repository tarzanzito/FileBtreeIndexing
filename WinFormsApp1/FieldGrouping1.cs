
namespace WinFormsApp1
{
    public class FieldGrouping1 : Candal.FieldsGrouping
    {
        public Candal.FieldLong Id;
        public Candal.FieldString Name;

        public FieldGrouping1()
        {
            Id = new Candal.FieldLong();
            Name = new Candal.FieldString(40);
        } //20 + 40 + 2 = 62

    }
}
