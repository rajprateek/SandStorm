using System;
using System.Collections.Generic;

namespace Office
{
    public class Utils
    {
        private const string W = "Word";
        private const string E = "Excel";
        private const string P = "PowerPoint";

        public static void RestoreAll(Dictionary<string, string[]> officeDict)
        {
            if (officeDict == null) return;

            if (officeDict.ContainsKey(W))
                Word.Restore(officeDict[W]);

            if (officeDict.ContainsKey(E))
                Excel.Restore(officeDict[E]);

            if (officeDict.ContainsKey(P))
                PowerPoint.Restore(officeDict[P]);
        }

        public static Dictionary<string, string[]> SaveAll(bool close)
        {
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
            dict.Add(W, Word.Save(close));
            dict.Add(E, Excel.Save(close));
            dict.Add(P, PowerPoint.Save(close));

            return dict;
        }

        internal static object getObject(string s)
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