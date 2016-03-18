using System;

namespace Windows
{
    internal class Utils
    {
        private const string App = "Shell.Application";

        internal static dynamic GetShell()
        {
            Type t = Type.GetTypeFromProgID(App);
            return Activator.CreateInstance(t);
        }
    }
}