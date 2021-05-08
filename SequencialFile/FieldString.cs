using System;

namespace Candal
{
    public class FieldString : Field
    {
        private string _value;
        private int _length;
        private bool _useUnicode;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public bool UseUnicode
        {
            get { return _useUnicode; }
        }

        public FieldString(int length)
        {
            _length = length;
            if (_length < 1)
                _length = 1;

            Clear();
        }

        public FieldString(int length, bool useUnicode) : this(length)
        {
            _useUnicode = useUnicode;
        }

        public override int GetLength()
        {
            return _length;
        }

        public override void UnPack(byte[] bytes)
        {
            if (bytes.Length != _length)
                throw new Exception("byte array size is not equal to LENGTH. " + _length.ToString());

            if (_useUnicode)
                _value = System.Text.Encoding.Unicode.GetString(bytes);
            else
                _value = System.Text.Encoding.ASCII.GetString(bytes);

            _value = _value.Replace("\0", "");
        }

        public override byte[] Pack()
        {
            byte[] byteArrayR;

            if (_useUnicode)
                byteArrayR = System.Text.Encoding.Unicode.GetBytes(_value);
            else
                byteArrayR = System.Text.Encoding.ASCII.GetBytes(_value);

            byte[] byteArrayW = new byte[_length];
            Array.Copy(byteArrayR, 0, byteArrayW, 0, _value.Length);

            return byteArrayW;
        }

        public override void Clear()
        {
            _value = "";
        }
    }
}
