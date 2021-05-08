using Candal;
using System;

namespace DemoApp
{
    class Program
    {
        const string fileName = @"c:\datos\database.dat";
        const string fileKey1 = @"c:\datos\database.k1";

        public static void Main(string[] args)
        {
            //MyXpto testClass = new MyXpto();
            //Type type = testClass.GetType();
            //// Iterate through all the methods of the class.
            //foreach (System.Reflection.FieldInfo  info in type.GetFields())
            //{
            //    // Iterate through all the Attributes for each method.
            //    foreach (Attribute attr in Attribute.GetCustomAttributes(info))
            //    {
            //        // Check for the AnimalType attribute.
            //        if (attr.GetType() == typeof(AnimalTypeAttribute))
            //            Console.WriteLine(
            //                "Method {0} has a pet {1} attribute.",
            //                info.Name, ((AnimalTypeAttribute)attr).Pet);
            //    }
            //}

            Console.WriteLine("Start Program.");

            PersonKey1 a1 = new PersonKey1();
            PersonKey1 a2 = new PersonKey1();

            a1.Name.Value = "aaaaaaaaaaaaaaa";
            a2.Name.Value = "bbbbbbbbbbbbbbb";

            byte[] b1 = a1.Pack();
            byte[] b2 = a2.Pack();

            Test_BtreeFile();

            Console.WriteLine("End Program.");
        }

   ////     public static implicit operator T(MyNullable<T> value)

        private static void Test_BtreeFile()
        {
            Console.WriteLine("Begin Test_Test_BtreeFile");

            // create
            BtreeFile<PersonKey1> btreeFile = new BtreeFile<PersonKey1>(fileKey1);

            btreeFile.Create();

            btreeFile.Open();

            PersonKey1 key1 = new PersonKey1(); // btreeFile.GetInstance(); //traz o BtreeFile._fieldGroupingKey
                                                 //_fieldGroupingKey.Clear()
            //adds
            long recordID;
            key1.Name.Value = "A10000Z";
             recordID = btreeFile.AddKey(1, key1);

            key1.Name.Value = "A07000Z";
             recordID = btreeFile.AddKey(2, key1);

            key1.Name.Value = "A20000Z";
             recordID = btreeFile.AddKey(3, key1);

            key1.Name.Value = "A05000Z";
             recordID = btreeFile.AddKey(4, key1);

            key1.Name.Value = "A09000Z";
            recordID = btreeFile.AddKey(5, key1);

            key1.Name.Value = "A15000Z";
             recordID = btreeFile.AddKey(6, key1);

            key1.Name.Value = "A30000Z";
             recordID = btreeFile.AddKey(7, key1);

            key1.Name.Value = "A04000Z";
             recordID = btreeFile.AddKey(8, key1);

            key1.Name.Value = "A06000Z";
            recordID = btreeFile.AddKey(9, key1);

            key1.Name.Value = "A08000Z";
             recordID = btreeFile.AddKey(10, key1);

            key1.Name.Value = "A09500Z";
            recordID = btreeFile.AddKey(11, key1);

            key1.Name.Value = "A14000Z";
            recordID = btreeFile.AddKey(12, key1);

            key1.Name.Value = "A16000Z";
            recordID = btreeFile.AddKey(13, key1);

            key1.Name.Value = "A25000Z";
            recordID = btreeFile.AddKey(14, key1);

            key1.Name.Value = "A40000Z";
            recordID = btreeFile.AddKey(15, key1);

            key1.Name.Value = "A03500Z";
            recordID = btreeFile.AddKey(16, key1);

            key1.Name.Value = "A04500Z";
            recordID = btreeFile.AddKey(17, key1);

            key1.Name.Value = "A05500Z";
             recordID = btreeFile.AddKey(18, key1);

            key1.Name.Value = "A06100Z";
            recordID = btreeFile.AddKey(19, key1);

            key1.Name.Value = "A07800Z";
            recordID = btreeFile.AddKey(20, key1);

            key1.Name.Value = "A08100Z";
             recordID = btreeFile.AddKey(21, key1);

            key1.Name.Value = "A09400Z";
             recordID = btreeFile.AddKey(22, key1);

            key1.Name.Value = "A09600Z";
             recordID = btreeFile.AddKey(23, key1);

            key1.Name.Value = "A13000Z";
             recordID = btreeFile.AddKey(24, key1);

            key1.Name.Value = "A14500Z";
             recordID = btreeFile.AddKey(25, key1);

            key1.Name.Value = "A15500Z";
             recordID = btreeFile.AddKey(26, key1);

            key1.Name.Value = "A16500Z";
            recordID = btreeFile.AddKey(27, key1);

            key1.Name.Value = "A24000Z";
            recordID = btreeFile.AddKey(28, key1);

            key1.Name.Value = "A26000Z";
             recordID = btreeFile.AddKey(29, key1);

            key1.Name.Value = "A39000Z";
             recordID = btreeFile.AddKey(30, key1);

            key1.Name.Value = "A50000Z";
            recordID = btreeFile.AddKey(31, key1);


            ////////////////////////////////////
            key1.Name.Value = "A39000Z";
            long indexID = btreeFile.SearchByKey(key1);

            //////updates
            ////data = seqFile.StringToRecordBytes(false, "zulu");
            ////recordID = seqFile.Update(3, data);

            //////deletes
            key1.Name.Value = "A10000Z";
            //key1.Name.Value = "A07000Z";
            recordID = btreeFile.DeleteKey(key1);

            //////adds
            ////data = seqFile.StringToRecordBytes(false, "yankee");
            ////recordID = seqFile.Add(data);

            ////data = seqFile.StringToRecordBytes(false, "wiskey");
            ////recordID = seqFile.Add(data);

            //////reads
            ////string strData;
            ////data = seqFile.Read(3);
            ////if (seqFile.RecordFound)
            ////    strData = seqFile.RecordBytesToString(false, data);

            ////data = seqFile.Read(1);
            ////if (seqFile.RecordFound)
            ////    strData = seqFile.RecordBytesToString(false, data);

            ////data = seqFile.Read(111);
            ////if (seqFile.RecordFound)
            ////    strData = seqFile.RecordBytesToString(false, data);

            //btreeFile.Close();

            //Console.WriteLine("End Test_SequencialFileString");
        }

  
 
    }
}
