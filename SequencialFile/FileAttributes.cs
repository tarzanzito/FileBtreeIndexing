namespace Candal
{
    public static class FileAttributes
    {
        public static readonly byte[] INI_MARK = { 40 };             // 5  40(
        public static readonly byte[] END_MARK = { 41 };             // 6  41)

        public static readonly byte[] FIELD_SEPARATOR = { 2 };      // 2  36$
        public static readonly byte[] RECORD_STATUS_ACTIVE = { 3 }; // 3  43+
        public static readonly byte[] RECORD_STATUS_DELETE = { 4 }; // 4  42*
        public static readonly byte[] RECORD_SEPARATOR = { 1 };     // 1  35#
    }
}
