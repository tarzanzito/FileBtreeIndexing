using System;

namespace Candal
{
    public class Fieldecimal : Field
    {
        public static readonly int FIELD_LENGTH = 30;

        private Decimal _value;
        private short _decimalPlaces;
        private string _format;

        public Decimal Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public short DecimalPlaces
        {
            get { return _decimalPlaces; }
            set
            {
                _decimalPlaces = value;
                if (_decimalPlaces < 0)
                    _decimalPlaces = 0;
                SetFormat();
            }
        }

        public Fieldecimal()
        {
            Clear();
            DecimalPlaces = 2;
        }

        public override void Clear()
        {
            _value = 0;
        }

        public Fieldecimal(short decimalPlaces)
        {
            _value = 0;
            DecimalPlaces = decimalPlaces;
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
            _value = System.Convert.ToDecimal(temp);
        }

        public override byte[] Pack()
        {
            string temp = _value.ToString(_format); // note: this make round (>=5 -> +1)
            return System.Text.Encoding.ASCII.GetBytes(temp);
        }

        private void SetFormat()
        {
            int lenInt = FIELD_LENGTH - 1 - _decimalPlaces;
            if (_decimalPlaces > 0)
                lenInt--;

            string temp = new String('0', lenInt) + "." + new String('0', _decimalPlaces);
            _format = "+" + temp + ";-" + temp;  // base "+0;-0"
        }
    }
}
