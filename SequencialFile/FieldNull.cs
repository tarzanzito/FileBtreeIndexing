using System;

namespace Candal
{
    public class FieldNull : Field
    {
        private byte[] _value;
        private int _length;

        public byte[] Value
        {
            get { return _value; }
        }

        public FieldNull(int length)
        {
            _value = null;

            _length = length;
            if (_length < 0)
                _length = 0;

            Clear();
        }

        public override void Clear()
        {
            if ((_length >  0) && (_value == null))
                _value = new byte[_length];
        }

        public override int GetLength()
        {
            return _length;
        }

        public override void UnPack(byte[] bytes)
        {
        }

        public override byte[] Pack()
        {
            return _value;
        }
    }
}
