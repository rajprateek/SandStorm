using System.Collections.Generic;
using Newtonsoft.Json;
using Sessions;

namespace Windows
{
    public class Windows : ISessionHandler
    {
        public string Name => "Windows";

        public string SaveSession(bool closeApp)
        {
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>
            {
                {FileExplorer.Name, FileExplorer.Save(closeApp)},
                {InternetExplorer.Name, InternetExplorer.Save(closeApp)}
            };

            return JsonConvert.SerializeObject(dict);
        }

        public void RestoreSession(string data)
        {
            Dictionary<string, string[]> dict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(data);

            FileExplorer.Restore(dict[FileExplorer.Name]);
            InternetExplorer.Restore(dict[InternetExplorer.Name]);
        }
    }
}
