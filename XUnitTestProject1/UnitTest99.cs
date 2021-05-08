using Candal;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest99
    {
        const string fileName1 = @"databaseString.dat";
        const string fileName2 = @"databasePerson.dat";
        const string fileName3 = @"databaseComplex.dat";

        [Fact]
        public void Test_TextEncoding()
        {
            //test bytes length
            string actualText = "具有靜電產生裝置之影像輸入裝置";

            byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(actualText);
            string bbbb = System.Text.Encoding.UTF8.GetString(utf8Bytes);

            byte[] uBytes = System.Text.Encoding.Unicode.GetBytes(actualText);
            string aaaa = System.Text.Encoding.Unicode.GetString(uBytes);

            string actualText2 = "ABCDEFGHIJKLMNO";

            byte[] utf8Bytes2 = System.Text.Encoding.UTF8.GetBytes(actualText2);
            string bbbb2 = System.Text.Encoding.UTF8.GetString(utf8Bytes2);

            byte[] uBytes2 = System.Text.Encoding.Unicode.GetBytes(actualText2);
            string aaaa2 = System.Text.Encoding.Unicode.GetString(uBytes2);

             //Decimal de = new decimal(123456789.123);
        }

    }
}

