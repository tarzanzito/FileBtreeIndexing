using System;

namespace Candal
{
    public class FieldTime : Field
    {
        private static readonly int FIELD_LENGTH = 8;

        private DateTime _value;

        public DateTime Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public FieldTime()
        {
            Clear();
        }

        public override void Clear()
        {
            _value = DateTime.Parse("0001-01-01 00:00:00");
        }

        public override int GetLength()
        {
            return FIELD_LENGTH;
        }

        public override void UnPack(byte[] bytes)
        {
            if (bytes.Length != FIELD_LENGTH)
                throw new Exception("byte array size is not equal to FIELD_LENGTH. " + FIELD_LENGTH.ToString());

            string temp = System.Text.Encoding.ASCII.GetString(bytes);
            _value = System.Convert.ToDateTime(temp);
        }

        public override byte[] Pack()
        {
            string temp = _value.ToString("HH:mm:ss");
            return System.Text.Encoding.ASCII.GetBytes(temp);
        }
    }
}
