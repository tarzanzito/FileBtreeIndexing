namespace Candal
{
    public abstract class Field
    {
        public abstract void Clear();
        public abstract int GetLength();
        public abstract void UnPack(byte[] bytes);
        public abstract byte[] Pack();
    }
}
