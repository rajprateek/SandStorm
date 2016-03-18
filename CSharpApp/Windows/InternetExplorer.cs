namespace Windows
{
    internal class InternetExplorer
    {
        internal static string[] Save(bool close)
        {
            dynamic shell = Utils.getShell();

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
            dynamic shell = Utils.getShell();
            foreach (string url in urls)
            {
                shell.Open(url);
            }
        }
    }
}