using Microsoft.Office.Interop.PowerPoint;
using System;

namespace Office
{
    internal class PowerPoint
    {
        public const string Name = "PowerPoint";

        internal static string[] Save(bool save)
        {
            object o = Utils.GetObject(Name);

            Application app;

            if (o != null) app = (Application)o;
            else return null;

            Presentations ppts = app.Presentations;

            string[] files = new string[ppts.Count];

            int i = 0;

            foreach (Presentation ppt in ppts)
            {
                try
                {
                    ppt.Save();
                    files[i++] = ppt.FullName;
                }
                catch (Exception e)
                {
                }
            }

            if (save)
            {
                app.Quit();
            }

            return files;
        }

        internal static void Restore(string[] urls)
        {
            if (urls == null || urls.Length == 0) return;

            object o = Utils.GetObject(Name);

            Application app;

            if (o != null) app = (Application)o;
            else app = new Application();

            foreach (string url in urls)
            {
                Presentation ppt = app.Presentations.Open(url);
            }
        }
    }
}