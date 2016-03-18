using Acrobat;
using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Sessions;

namespace Adobe
{
    public class Acrobat : ISessionHandler
    {
        public string Name => "Acrobat";
        public string SaveSession(bool closeApp)
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

            int num = app.GetNumAVDocs();

            AcrobatData[] files = new AcrobatData[num];

            string[] filepaths = FileHandler.GetFilesForProcess(Process.GetProcessesByName(Name)[0], ".pdf");

            for (int i = 0; i < num; ++i)
            {
                AcroAVDoc doc = app.GetAVDoc(i);
                AcroPDDoc pdoc = doc.GetPDDoc();
                string name = pdoc.GetFileName();
                string url = GetFilePath(name, filepaths);
                AcroAVPageView page = doc.GetAVPageView();
                int pageNum = page.GetPageNum();

                files[i] = new AcrobatData
                {
                    Path = url,
                    PageNumber = pageNum
                };
            }

            if (closeApp)
            {
                for (int i = 0; i < num; ++i)
                {
                    AcroAVDoc doc = app.GetAVDoc(0);
                    doc.Close(0);
                }
                while (!app.Hide()) ;
                app.Exit();
            }

            return JsonConvert.SerializeObject(files);
        }

        public void RestoreSession(string data)
        {
            AcrobatData[] files = JsonConvert.DeserializeObject<AcrobatData[]>(data);

            foreach (var file in files)
            {
                string args = $"/A \"page={file.PageNumber}\" \"{file.Path}\"";
                try
                {
                    Process.Start("Acrobat", args);
                    return;
                }
                catch (Exception e)
                {

                }

                try
                {
                    Process.Start("AcroRd32", args);
                    return;
                }
                catch (Exception e)
                {

                }
            }
        }

        private static string GetFilePath(string name, string[] filepaths)
        {
            foreach (string s in filepaths)
            {
                if (s.EndsWith(name))
                {
                    return s;
                }
            }

            return "";
        }
    }

    internal class AcrobatData
    {
        public string Path { get; set; }
        public int PageNumber { get; set; }
    }
}