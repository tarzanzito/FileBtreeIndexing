using System;
using System.Reflection;

namespace Candal
{
    public abstract class FieldsGrouping : Field
    {
        private bool _isDefinitionsLoaded = false;
        private int _validFieldCount;
        private int _totalLength;

        private int[] _lengthArray;
        private object[] _objectFieldArray;
        private MethodInfo[] _methodClearArray;
        private MethodInfo[] _methodPackArray;
        private MethodInfo[] _methodUnPackArray;

        protected bool IsDefinitionsLoaded
        {
            get { return _isDefinitionsLoaded; }
        }

        public override int GetLength()
        {
            LoadDefinitions();
            return _totalLength;
        }

        protected virtual void LoadDefinitions()
        {
            //improves performance. prevents methods from always doing the same calculations
            if (_isDefinitionsLoaded) 
                return;

            StatisticsAboutFields.AddRegistry(this);

             _totalLength = 0;

            try
            {
                FieldInfo[] fieldInfoArray = this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                int fieldCount = fieldInfoArray.Length;

                _validFieldCount = 0;
                for (int i = 0; i < fieldCount; i++)
                {
                    if ((fieldInfoArray[i].FieldType.BaseType == typeof(Candal.Field)) || (fieldInfoArray[i].FieldType.BaseType == typeof(Candal.FieldsGrouping)))
                        _validFieldCount++;
                }

                _objectFieldArray = new object[_validFieldCount];
                _lengthArray = new int[_validFieldCount];
                _methodClearArray = new MethodInfo[_validFieldCount];
                _methodPackArray = new MethodInfo[_validFieldCount];
                _methodUnPackArray = new MethodInfo[_validFieldCount];


                //get Field instances
                int o = 0;
                for (int i = 0; i < fieldCount; i++)
                {
                    if ((fieldInfoArray[i].FieldType.BaseType == typeof(Candal.Field)) || (fieldInfoArray[i].FieldType.BaseType == typeof(Candal.FieldsGrouping)))
                    {
                        object obj = fieldInfoArray[i].GetValue(this); //get object instance

                        //save obj field
                        _objectFieldArray[o] = obj;

                        MethodInfo method = fieldInfoArray[i].FieldType.GetMethod("GetLength");
                        object result = method.Invoke(obj, null); //execute method obj.GetLength()
                        int fieldLength = (int)result;

                        //save obj field length
                        _lengthArray[o] = fieldLength;

                        _totalLength += fieldLength;
                        if (fieldInfoArray[i].FieldType.BaseType == typeof(Candal.Field))
                            _totalLength += FileAttributes.FIELD_SEPARATOR.Length;

                        //save methods
                        _methodClearArray[o] = fieldInfoArray[i].FieldType.GetMethod("Clear"); 
                        _methodPackArray[o] = fieldInfoArray[i].FieldType.GetMethod("Pack");
                        _methodUnPackArray[o] = fieldInfoArray[i].FieldType.GetMethod("UnPack");
                        o++;
                    }
                }

                _isDefinitionsLoaded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public override byte[] Pack()
        {
            LoadDefinitions();

            byte[] recordArray = null;

            try
            {
                recordArray = new byte[_totalLength];

                int startAt = 0;

                for (int i = 0; i < _validFieldCount; i++)
                {
                    object obj = _objectFieldArray[i];
                    object result = _methodPackArray[i].Invoke(obj, null); //execute Field->Pack()
                    byte[] byteArray = (byte[])result;

                    Array.Copy(byteArray, 0, recordArray, startAt, byteArray.Length);
                    startAt += byteArray.Length;

                    if (obj.GetType().BaseType == typeof(Candal.Field))
                    {
                        Array.Copy(FileAttributes.FIELD_SEPARATOR, 0, recordArray, startAt, FileAttributes.FIELD_SEPARATOR.Length);
                        startAt += FileAttributes.FIELD_SEPARATOR.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return recordArray;
        }

        public override void UnPack(byte[] bytes)
        {
            LoadDefinitions();

            try
            {
                Clear();

                int startAt = 0;

                for (int i = 0; i < _validFieldCount; i++)
                {
                    object obj = _objectFieldArray[i];
                    byte[] fieldArray = new byte[_lengthArray[i]];
                    Array.Copy(bytes, startAt, fieldArray, 0, _lengthArray[i]);

                    object[] parameters = new object[] { fieldArray };
                    _methodUnPackArray[i].Invoke(_objectFieldArray[i], parameters); //execute Field->UnPack()

                    startAt += _lengthArray[i];
                    if (obj.GetType().BaseType == typeof(Candal.Field))
                        startAt += FileAttributes.FIELD_SEPARATOR.Length;
                }

                if (bytes.Length != startAt)
                    throw new Exception("Number of bytes counted is not equal to total bytes.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public override void Clear()
        {
            LoadDefinitions();

            try
            {
                for (int i = 0; i < _validFieldCount; i++)
                    _methodClearArray[i].Invoke(_objectFieldArray[i], null); //execute Field->Clear()
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
