namespace Candal
{
    public class FieldByteArray : Field
    {
        private byte[] _value;
        private int _length;

        public byte[] Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public FieldByteArray(int length)
        {
            _length = length;
            if (_length < 0)
                _length = 0;
            
             Clear();
        }

        public override void Clear()
        {
            if (_length == 0)
                _value = null;
            else
                _value = new byte[_length];
        }

        public override int GetLength()
        {
            return _length;
        }

        public override void UnPack(byte[] bytes)
        {
            _value = bytes;
        }

        public override byte[] Pack()
        {
            return _value;
        }
    }
}
