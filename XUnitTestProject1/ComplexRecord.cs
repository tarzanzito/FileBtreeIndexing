﻿namespace XUnitTestProject1
{
    class ComplexRecord : Candal.Record
    {
        public Candal.FieldString Descr;
        public Candal.FieldShort Age;
        public ExampleFieldGrouping2 Group2;


        public ComplexRecord()
        {
            Descr = new Candal.FieldString(40);
            Age = new Candal.FieldShort();
            Group2 = new ExampleFieldGrouping2();
        } // 104 + 40 + 6 + 3 = 153
    }
}
