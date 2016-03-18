using System.Linq;

namespace Windows
{
    internal class FileExplorer
    {
        public const string Name = "FileExplorer";

        internal static string[] Save(bool close)
        {
            dynamic shell = Utils.GetShell();

            string[] files = new string[shell.Windows().Count];

            int i = 0;
            foreach (dynamic window in shell.Windows())
            {
                if (window.LocationURL.StartsWith("file://"))
                {
                    files[i++] = window.LocationURL;
                    if (close)
                    {
                        window.Quit();
                    }
                }
            }

            return files;
        }

        internal static void Restore(string[] files)
        {
            dynamic shell = Utils.GetShell();
            foreach (string file in files.Where(f => f != null && f.Any()))
            {
                shell.Open(file);
            }
        }
    }
}