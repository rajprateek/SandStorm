using System.Collections.Generic;
using Newtonsoft.Json;
using Sessions;

namespace Office
{
    public class Office : ISessionHandler
    {
        public string Name => "MSOffice";
        public string SaveSession(bool closeApp)
        {
            Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();

            dictionary.Add(Word.Name, Word.Save(closeApp));
            dictionary.Add(PowerPoint.Name, PowerPoint.Save(closeApp));
            dictionary.Add(Excel.Name, Excel.Save(closeApp));

            return JsonConvert.SerializeObject(dictionary);
        }

        public void RestoreSession(string data)
        {
            Dictionary<string, string[]> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(data);

            Word.Restore(dictionary[Word.Name]);
            PowerPoint.Restore(dictionary[PowerPoint.Name]);
            Excel.Restore(dictionary[Excel.Name]);
        }
    }
}
