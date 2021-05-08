using Candal;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest01
    {
        [Fact]
        public static void Test_PersonRRecord()
        {
            Console.WriteLine("Begin Test_Record");

            PersonRecord rec1 = new PersonRecord();
            rec1.Age.Value = 10;
            rec1.Born.Value = DateTime.Parse("2021-04-05");
            rec1.Salary.Value = Decimal.Parse("1235.56");
            rec1.Name.Value = "PapaGolf";

            byte[] recArray = rec1.Pack();

            PersonRecord rec2 = new PersonRecord();
            rec2.UnPack(recArray);

            Assert.Equal(rec1.GetLength(), rec2.GetLength());

            Assert.Equal(10, rec2.Age.Value);
            Assert.Equal("2021-04-05", rec2.Born.Value.Date.ToString("yyyy-MM-dd"));
            Assert.Equal("1235.56", rec2.Salary.Value.ToString());
            Assert.Equal("PapaGolf", rec2.Name.Value.ToString());
        }

        [Fact]
        public void Test_HeaderRecord()
        {
            HeaderRecord rec1 = new HeaderRecord();
            rec1.LastDeleted.Value = 25;
            rec1.ActiveRecords.Value = 125;
            rec1.TotalRecords.Value = 301;

            byte[] recArray = rec1.Pack();

            HeaderRecord rec2 = new HeaderRecord();
            rec2.UnPack(recArray);

            Assert.Equal(rec1.GetLength(), rec2.GetLength());

            Assert.Equal(25, rec2.LastDeleted.Value );
            Assert.Equal(125, rec2.ActiveRecords.Value);
            Assert.Equal(301, rec2.TotalRecords.Value);
        }

        [Fact]
        public void Test_ComplexRecord()
        {
            ComplexRecord rec1 = new ComplexRecord();
            rec1.Age.Value = 25;
            rec1.Descr.Value = "Any Description";
            rec1.Group2.Address.Value = "Road 1";
            rec1.Group2.Group1.Id.Value = 10;
            rec1.Group2.Group1.Name.Value = "PapaGolf";
            
            byte[] recArray = rec1.Pack();

            ComplexRecord rec2 = new ComplexRecord();
            rec2.UnPack(recArray);

            Assert.Equal(rec1.GetLength(), rec2.GetLength());

            Assert.Equal(25, rec2.Age.Value);
            Assert.Equal("Any Description", rec2.Descr.Value);
            Assert.Equal("Road 1", rec2.Group2.Address.Value);
            Assert.Equal(10, rec2.Group2.Group1.Id.Value);
            Assert.Equal("PapaGolf", rec2.Group2.Group1.Name.Value);
        }

        //private bool ByteArraysCompare(byte[] b1, byte[] b2)
        //{
        //    if (b1.Length != b2.Length)
        //        return false;

        //    for (int x = 0; x < b1.Length; x++)
        //    {
        //        if (b1[x] != b2[x])
        //            return false;
        //    }

        //    return true;
        //}
    }
}

