using System;

namespace Candal
{
    public class FieldBoolean : Field
    {
        public static readonly int FIELD_LENGTH = 1;

        private bool _value;

        public bool Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public FieldBoolean()
        {
            Clear();
        }

        public override void Clear()
        {
            _value = true;
        }

        public override int GetLength()
        {
            return FIELD_LENGTH;
        }

        public override void UnPack(byte[] bytes)
        {
            if (bytes.Length != FIELD_LENGTH)
                throw new Exception("byte array size is not equal to FIELD_LENGTH. " + FIELD_LENGTH.ToString());

            if (bytes[0] == 0)
                _value = false;
            else
                _value = true;
        }

        public override byte[] Pack()
        {
            byte[] bytes = new byte[1];
            if (_value)
                bytes[0] = 1;
            else
                bytes[0] = 0;

            return bytes;
        }
    }
}
