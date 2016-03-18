namespace Windows
{
    internal class FileExplorer
    {
        internal static string[] Save(bool close)
        {
            dynamic shell = Utils.getShell();

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
            dynamic shell = Utils.getShell();
            foreach (string file in files)
            {
                shell.Open(file);
            }
        }
    }
}