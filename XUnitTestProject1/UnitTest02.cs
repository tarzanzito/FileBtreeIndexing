using Candal;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest02
    {
        const string fileName1 = @"databaseString.dat";
        const string fileName2 = @"databasePerson.dat";
        const string fileName3 = @"databaseComplex.dat";

        [Fact]
        public void Test_SequencialFileString()
        {
            // create
            SequencialFile<StringRecord> seqFile = new SequencialFile<StringRecord>(fileName1);
            seqFile.Create();

            seqFile.Open();

            StringRecord rec = new StringRecord();

            //adds
            rec.Data.Value = "alfa";
            long recordID = seqFile.AddRecord(rec);
            Assert.Equal(1, recordID);

            rec.Data.Value = "bravo";
            recordID = seqFile.AddRecord(rec);
            Assert.Equal(2, recordID);

            rec.Data.Value = "charley";
            recordID = seqFile.AddRecord(rec);
            Assert.Equal(3, recordID);

            rec.Data.Value = "delta";
            recordID = seqFile.AddRecord(rec);
            Assert.Equal(4, recordID);

            rec.Data.Value = "echo";
            recordID = seqFile.AddRecord(rec);
            Assert.Equal(5, recordID);

            rec.Data.Value = "foxtrot";
            recordID = seqFile.AddRecord(rec);
            Assert.Equal(6, recordID);

            rec.Data.Value = "golf";
            recordID = seqFile.AddRecord(rec);
            Assert.Equal(7, recordID);

            //updates
            rec.Data.Value = "zulu";
            recordID = seqFile.UpdateRecord(3, rec);

            //deletes
            recordID = seqFile.DeleteRecord(2);
            recordID = seqFile.DeleteRecord(4);

            //adds
            rec.Data.Value = "yankee";
            recordID = seqFile.AddRecord(rec);

            rec.Data.Value = "wiskey";
            recordID = seqFile.AddRecord(rec);

            //reads
            rec = seqFile.ReadRecord(3);
            Assert.True(seqFile.IsRecordFound);

            rec = seqFile.ReadRecord(1);
            Assert.True(seqFile.IsRecordFound);

            rec = seqFile.ReadRecord(111);
            Assert.False(seqFile.IsRecordFound);

            seqFile.Close();

        }
        
        [Fact]
        public void Test_SequencialFilePersonRecord()
        {
            // create
            SequencialFile<PersonRecord> seqFile = new SequencialFile<PersonRecord>(fileName2);
            seqFile.Create();

            seqFile.Open();

            PersonRecord rec = new PersonRecord();
            rec.Name.Value = "Alfa";
            rec.Age.Value = 10;
            rec.Born.Value = DateTime.Parse("2021-01-31");
            rec.Salary.Value = Decimal.Parse("123.24");

            //adds
            long recordID = seqFile.AddRecord(rec);
            Assert.Equal(1, recordID);

            rec.Name.Value = "Bravo";
            rec.Age.Value = 5;
            rec.Born.Value = DateTime.Parse("2021-03-31");
            rec.Salary.Value = Decimal.Parse("125.00");

            recordID = seqFile.AddRecord(rec);
            Assert.Equal(2, recordID);

            rec.Name.Value = "Charley";
            rec.Age.Value = 15;
            rec.Born.Value = DateTime.Parse("2021-06-15");
            rec.Salary.Value = Decimal.Parse("1001.40");

            recordID = seqFile.AddRecord(rec);
            Assert.Equal(3, recordID);

            rec.Name.Value = "Delta";
            rec.Age.Value = 7;
            rec.Born.Value = DateTime.Parse("2021-01-01");
            rec.Salary.Value = Decimal.Parse("200.85");

            recordID = seqFile.AddRecord(rec);
            Assert.Equal(4, recordID);

            rec.Name.Value = "Echo";
            rec.Age.Value = 20;
            rec.Born.Value = DateTime.Parse("2019-01-18");
            rec.Salary.Value = Decimal.Parse("800.15");

            recordID = seqFile.AddRecord(rec);
            Assert.Equal(5, recordID);

            rec.Name.Value = "Foxtrot";
            rec.Age.Value = 2;
            rec.Born.Value = DateTime.Parse("2015-07-19");
            rec.Salary.Value = Decimal.Parse("1230.71");

            recordID = seqFile.AddRecord(rec);
            Assert.Equal(6, recordID);

            rec.Name.Value = "Golf";
            rec.Age.Value = 6;
            rec.Born.Value = DateTime.Parse("2020-09-29");
            rec.Salary.Value = Decimal.Parse("750.14");

            recordID = seqFile.AddRecord(rec);
            Assert.Equal(7, recordID);

            //updates
            rec.Name.Value = "Zulu";
            rec.Age.Value = 4;
            rec.Born.Value = DateTime.Parse("2017-10-05");
            rec.Salary.Value = Decimal.Parse("600.50");
            recordID = seqFile.UpdateRecord(3, rec);

            //deletes
            recordID = seqFile.DeleteRecord(2);
            recordID = seqFile.DeleteRecord(4);

            //adds
            rec.Name.Value = "Yankee";
            rec.Age.Value = 12;
            rec.Born.Value = DateTime.Parse("2001-11-23");
            rec.Salary.Value = Decimal.Parse("720.45");
            recordID = seqFile.AddRecord(rec);

            rec.Name.Value = "Wiskey";
            rec.Age.Value = 11;
            rec.Born.Value = DateTime.Parse("2002-08-28");
            rec.Salary.Value = Decimal.Parse("321.42");
            recordID = seqFile.AddRecord(rec);

            //reads
            rec = seqFile.ReadRecord(3);
            Assert.True(seqFile.IsRecordFound);

            rec = seqFile.ReadRecord(1);
            Assert.True(seqFile.IsRecordFound);

            rec = seqFile.ReadRecord(111);
            Assert.False(seqFile.IsRecordFound);

            seqFile.Close();
        }

        [Fact]
        public void Test_SequencialFileComplexRecord()
        {
            // create
            SequencialFile<ComplexRecord> seqFile = new SequencialFile<ComplexRecord>(fileName3);
            seqFile.Create();

            seqFile.Open();

            ComplexRecord rec1 = new ComplexRecord();
            rec1.Age.Value = 21;
            rec1.Descr.Value = "Description 1";
            rec1.Group2.Address.Value = "Road 1";
            rec1.Group2.Group1.Id.Value = 11;
            rec1.Group2.Group1.Name.Value = "Alfa";

            //adds
            long recordID = seqFile.AddRecord(rec1);
            Assert.Equal(1, recordID);

            rec1.Age.Value = 22;
            rec1.Descr.Value = "Description2";
            rec1.Group2.Address.Value = "Road 2";
            rec1.Group2.Group1.Id.Value = 12;
            rec1.Group2.Group1.Name.Value = "Bravo";

            recordID = seqFile.AddRecord(rec1);
            Assert.Equal(2, recordID);

            rec1.Age.Value = 23;
            rec1.Descr.Value = "Description 3";
            rec1.Group2.Address.Value = "Road 3";
            rec1.Group2.Group1.Id.Value = 13;
            rec1.Group2.Group1.Name.Value = "Charley"; ;

            recordID = seqFile.AddRecord(rec1);
            Assert.Equal(3, recordID);

            rec1.Age.Value = 24;
            rec1.Descr.Value = "Description 4";
            rec1.Group2.Address.Value = "Road 4";
            rec1.Group2.Group1.Id.Value = 14;
            rec1.Group2.Group1.Name.Value = "Delta"; ;

            recordID = seqFile.AddRecord(rec1);
            Assert.Equal(4, recordID);

            rec1.Age.Value = 25;
            rec1.Descr.Value = "Description 5";
            rec1.Group2.Address.Value = "Road 5";
            rec1.Group2.Group1.Id.Value = 15;
            rec1.Group2.Group1.Name.Value = "Echo"; 

            recordID = seqFile.AddRecord(rec1);
            Assert.Equal(5, recordID);

            rec1.Age.Value = 26;
            rec1.Descr.Value = "Description 6";
            rec1.Group2.Address.Value = "Road 6";
            rec1.Group2.Group1.Id.Value = 16;
            rec1.Group2.Group1.Name.Value = "Foxtrot"; 

            recordID = seqFile.AddRecord(rec1);
            Assert.Equal(6, recordID);

            rec1.Age.Value = 27;
            rec1.Descr.Value = "Description 7";
            rec1.Group2.Address.Value = "Road 7";
            rec1.Group2.Group1.Id.Value = 17;
            rec1.Group2.Group1.Name.Value = "Golf";

            recordID = seqFile.AddRecord(rec1);
            Assert.Equal(7, recordID);

            //updates
            rec1.Age.Value = 31;
            rec1.Descr.Value = "Description 31";
            rec1.Group2.Address.Value = "Road 31";
            rec1.Group2.Group1.Id.Value = 1;
            rec1.Group2.Group1.Name.Value = "Zulu";

            recordID = seqFile.UpdateRecord(3, rec1);

            //deletes
            recordID = seqFile.DeleteRecord(2);
            recordID = seqFile.DeleteRecord(4);

            //adds
            rec1.Age.Value = 32;
            rec1.Descr.Value = "Description 32";
            rec1.Group2.Address.Value = "Road 32";
            rec1.Group2.Group1.Id.Value = 2;
            rec1.Group2.Group1.Name.Value = "Yankee";

            recordID = seqFile.AddRecord(rec1);


            rec1.Age.Value = 33;
            rec1.Descr.Value = "Description 33";
            rec1.Group2.Address.Value = "Road 33";
            rec1.Group2.Group1.Id.Value = 3;
            rec1.Group2.Group1.Name.Value = "Wiskey";

            recordID = seqFile.AddRecord(rec1);

            //reads
            rec1 = seqFile.ReadRecord(3);
            Assert.True(seqFile.IsRecordFound);

            rec1 = seqFile.ReadRecord(1);
            Assert.True(seqFile.IsRecordFound);

            rec1 = seqFile.ReadRecord(111);
            Assert.False(seqFile.IsRecordFound);

            seqFile.Close();
        }


        //[Fact]
        public void Test_CompressFile()
        {
            SequencialFile<PersonRecord> seqFile = new SequencialFile<PersonRecord>(fileName2);
            //seqFile.Compress();
        }

        private bool ByteArraysCompare(byte[] b1, byte[] b2)
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
    }
}

