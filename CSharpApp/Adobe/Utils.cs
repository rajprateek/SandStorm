using System;
using System.Collections.Generic;

namespace Adobe
{
    public class Utils
    {
        public static void RestoreAll(Dictionary<string, string[]> acroDict)
        {
            if (acroDict == null) return;
        }

        public static Dictionary<string, int> SaveAll(bool close)
        {
            return Acrobat.Save(close);
        }

        internal static object getObject(string s)
        {
            try
            {
                return System.Runtime.InteropServices.Marshal.GetActiveObject(s);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}