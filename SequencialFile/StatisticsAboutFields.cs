using System;
using System.Collections.Generic;
using System.Text;

namespace Candal
{
    //private static Dictionary<int, string>
    public static class StatisticsAboutFields
    {
        public static void AddRegistry(Candal.Field field)
        {
            string name = field.GetType().Name;
            int code = field.GetHashCode();
            System.Diagnostics.Debug.WriteLine(name + ":" + code.ToString());

        }


    }
}
