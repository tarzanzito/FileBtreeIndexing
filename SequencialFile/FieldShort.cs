using System;

namespace Candal
{
    public class FieldShort : Field
    {
        public static readonly int FIELD_LENGTH = 6;

        private short _value;
        private string _format;

        public short Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public FieldShort()
        {
            Clear();
            SetFormat();
        }

        public override void Clear()
        {
            _value = 0;
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
            _value = System.Convert.ToInt16(temp);
        }

        public override byte[] Pack()
        {
            string temp = _value.ToString(_format);
            return System.Text.Encoding.ASCII.GetBytes(temp);
        }

        private void SetFormat()
        {
            string temp = new String('0', FIELD_LENGTH - 1);
            _format = "+" + temp + ";-" + temp;  // base "+0;-0"
        }
    }
}
