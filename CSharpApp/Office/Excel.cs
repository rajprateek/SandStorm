using Microsoft.Office.Interop.Excel;
using System;

namespace Office
{
    internal class Excel
    {
        internal static string[] Save(bool save)
        {
            object o = Utils.getObject("Excel");

            Application app;

            if (o != null) app = (Application)o;
            else return null;

            Workbooks wbs = app.Workbooks;

            string[] files = new string[wbs.Count];

            int i = 0;

            foreach (Workbook wb in wbs)
            {
                try
                {
                    wb.Save();
                    files[i++] = wb.FullName;
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

            object o = Utils.getObject("Excel");

            Application app;

            if (o != null) app = (Application)o;
            else app = new Application();

            foreach (string url in urls)
            {
                Workbook wb = app.Workbooks.Open(url);
                app.Visible = true;
            }
        }
    }
}