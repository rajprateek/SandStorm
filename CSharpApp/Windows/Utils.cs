using System;
using System.Collections.Generic;

namespace Windows
{
    public class Utils
    {
        private const string FE = "FileExplorer";
        private const string IE = "InternetExplorer";
        private const string APP = "Shell.Application";

        public static Dictionary<string, string[]> SaveAll(bool close)
        {
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>();

            dict.Add(FE, FileExplorer.Save(close));
            dict.Add(IE, InternetExplorer.Save(close));

            return dict;
        }

        public static void RestoreAll(Dictionary<string, string[]> dict)
        {
            if (dict == null) return;

            if (dict.ContainsKey(FE))
            {
                FileExplorer.Restore(dict[FE]);
            }
            if (dict.ContainsKey(IE))
            {
                InternetExplorer.Restore(dict[IE]);
            }
        }

        internal static dynamic getShell()
        {
            Type t = Type.GetTypeFromProgID(APP);
            return Activator.CreateInstance(t);
        }
    }
}