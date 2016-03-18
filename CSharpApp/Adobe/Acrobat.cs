using Acrobat;
using System;
using System.Collections.Generic;

namespace Adobe
{
    internal class Acrobat
    {
        internal static Dictionary<string, int> Save(bool close)
        {
            AcroApp app;

            try
            {
                app = new AcroApp();
            }
            catch (Exception e)
            {
                return null;
            }

            Dictionary<string, int> dict = new Dictionary<string, int>();

            int num = app.GetNumAVDocs();

            for (int i = 0; i < num; ++i)
            {
                AcroAVDoc doc = app.GetAVDoc(i);
                AcroPDDoc pdoc = doc.GetPDDoc();
                string url = pdoc.GetFileName();
                AcroAVPageView page = doc.GetAVPageView();
                int pageNum = page.GetPageNum();
                dict.Add(url, pageNum);
            }

            return dict;
        }
    }
}