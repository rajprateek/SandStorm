using Microsoft.Office.Interop.Word;
using System;

namespace Office
{
    internal class Word
    {
        internal static string[] Save(bool save)
        {
            object o = Utils.getObject("Word");

            Application app;

            if (o != null) app = (Application)o;
            else return null;

            Documents docs = app.Documents;

            string[] files = new string[docs.Count];

            int i = 0;

            foreach (Document doc in docs)
            {
                try
                {
                    doc.Save();
                    files[i++] = doc.FullName;
                }
                catch (Exception e)
                {
                }
            }

            if (save)
            {
                app.NormalTemplate.Saved = true;
                app.Quit();
            }

            return files;
        }

        internal static void Restore(string[] urls)
        {
            if (urls == null || urls.Length == 0) return;

            object o = Utils.getObject("Word");

            Application app;

            if (o != null) app = (Application)o;
            else app = new Application();

            foreach (string url in urls)
            {
                Document doc = app.Documents.Open(url);
                app.Visible = true;
                if (Double.Parse(app.Version) >= 15.0)
                    doc.ReturnToLastReadPosition();
            }
        }
    }
}