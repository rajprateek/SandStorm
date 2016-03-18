using System.Linq;

namespace Windows
{
    internal class InternetExplorer
    {
        public const string Name = "InternetExplorer";

        internal static string[] Save(bool close)
        {
            dynamic shell = Utils.GetShell();

            string[] files = new string[shell.Windows().Count];

            int i = 0;
            foreach (dynamic window in shell.Windows())
            {
                if (!window.LocationURL.StartsWith("file://"))
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

        internal static void Restore(string[] urls)
        {
            dynamic shell = Utils.GetShell();
            foreach (string url in urls.Where(u => u != null && u.Any()))
            {
                shell.Open(url);
            }
        }
    }
}