using System;
using System.Collections.Generic;
using System.Text;

namespace Candal
{
    public static class Utils
    {
        public static bool ByteArraysEquals(byte[] b1, byte[] b2)
        {
            if (b1.Length != b2.Length)
                return false;

            for (int x = 0; x < b1.Length; x++)
            {
                if (b1[x] != b2[x])
                    return false;
            }

            return true;
        }

        public static int ByteArraysCompare(byte[] b1, byte[] b2)
        {
            int minLen;
            
            if (b1.Length > b2.Length)
                minLen = b2.Length;
            else
                minLen = b1.Length;


            for (int x = 0; x < minLen; x++)
            {
                if (b1[x] == b2[x])
                    continue;

                if (b1[x] > b2[x])
                    return 1;
                else
                    return -1;
            }

            if (b1.Length > b2.Length)
                return 1;
            if (b1.Length < b2.Length)
                return -1;

            return 0;
        }

        //public static byte[] StringToRecordBytes(bool useUnicode, string data)
        //{
        //    byte[] byteArrayR;

        //    if (useUnicode)
        //        byteArrayR = System.Text.Encoding.Unicode.GetBytes(data);
        //    else
        //        byteArrayR = System.Text.Encoding.ASCII.GetBytes(data);

        //    if (byteArrayR.Length > _rowLength)
        //        throw new Exception("String data has size greater than defined 'Length Record'.");

        //    byte[] byteArrayW = new byte[_effectiveRecordLength];
        //    Array.Copy(byteArrayR, 0, byteArrayW, 0, byteArrayR.Length);

        //    int startAt = _effectiveRecordLength - FileAttributes.FIELD_SEPARATOR.Length - FileAttributes.RECORD_STATUS_ACTIVE.Length - FileAttributes.RECORD_SEPARATOR.Length;
        //    Array.Copy(FileAttributes.FIELD_SEPARATOR, 0, byteArrayW, startAt, FileAttributes.FIELD_SEPARATOR.Length);

        //    startAt += FileAttributes.FIELD_SEPARATOR.Length;
        //    Array.Copy(FileAttributes.RECORD_STATUS_ACTIVE, 0, byteArrayW, startAt, FileAttributes.RECORD_STATUS_ACTIVE.Length);

        //    startAt += FileAttributes.RECORD_STATUS_ACTIVE.Length;
        //    Array.Copy(FileAttributes.RECORD_SEPARATOR, 0, byteArrayW, startAt, FileAttributes.RECORD_SEPARATOR.Length);

        //    return byteArrayW;
        //}

        //public static string RecordBytesToString(bool useUnicode, byte[] bytes)
        //{
        //    int arrayLength = this._rowLength;
        //    if (bytes.Length < _rowLength)
        //        arrayLength = bytes.Length;

        //    byte[] byteArrayW = new byte[arrayLength];
        //    Array.Copy(bytes, 0, byteArrayW, 0, arrayLength);

        //    string retValue;

        //    if (useUnicode)
        //        retValue = System.Text.Encoding.Unicode.GetString(byteArrayW);
        //    else
        //        retValue = System.Text.Encoding.ASCII.GetString(byteArrayW);

        //    return retValue;
        //}

    }
}
