using System;
using System.Collections.Generic;

namespace Office
{
    public class Utils
    {
        internal static object GetObject(string s)
        {
            try
            {
                return System.Runtime.InteropServices.Marshal.GetActiveObject(s + ".Application");
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}