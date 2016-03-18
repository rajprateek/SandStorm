using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Sessions
{
    public class SessionManager
    {
        internal static string OnesyncDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\OneSync";

        private readonly IEnumerable<ISessionHandler> _sessionHandlers;

        public SessionManager(IEnumerable<ISessionHandler> sessionHandlers)
        {
            this._sessionHandlers = sessionHandlers;
        }

        public void NewSession(string name, bool closeAll)
        {
            Dictionary<string, string> session = _sessionHandlers.ToDictionary(sessionHandler => sessionHandler.Name, sessionHandler => sessionHandler.SaveSession(closeAll));

            string res = JsonConvert.SerializeObject(session);
            string path = GetPath(name);
            File.WriteAllText(path, res);

            /*Session session = new Session(name);
            session.OfficeDictionary = Office.Utils.SaveAll(closeAll);
            session.tabs = chromeManager.getChromeSession(closeAll);
            session.ExplorerDictionary = Windows.Utils.SaveAll(closeAll);
            session.Serialize();
            chromeManager.tabs = null;*/
        }

        public void RestoreSession(string name)
        {
            string path = GetPath(name);

            var res = File.ReadAllText(path);

            Dictionary<string, string> session = JsonConvert.DeserializeObject<Dictionary<string, string>>(res);

            foreach (var sessionHandler in _sessionHandlers)
            {
                sessionHandler.RestoreSession(session[sessionHandler.Name]);
            }

            /*Session session = Session.DeSerialize(name);
            if (session == null) return;
            Office.Utils.RestoreAll(session.OfficeDictionary);
            chromeManager.openChromeSession(session.tabs);
            Windows.Utils.RestoreAll(session.ExplorerDictionary);*/
        }

        public static void RemoveSession(string name)
        {
            if (Directory.Exists(OnesyncDir))
            {
                File.Delete(GetPath(name));
            }
        }

        public static string[] GetAllSessionNames()
        {
            if (Directory.Exists(OnesyncDir))
            {
                string[] availableSessions = Directory.GetFiles(OnesyncDir, "*.1sync");

                if (availableSessions.Length == 0) return new string[0];

                string[] allSessionNames = new string[availableSessions.Length];
                int i = 0;
                foreach (string session in availableSessions)
                {
                    int startIndex = session.LastIndexOf("\\");
                    int endIndex = session.LastIndexOf(".");
                    string fileName = session.Substring(startIndex + 1);
                    allSessionNames[i++] = fileName.Substring(0, fileName.Length - 6);
                }

                return allSessionNames;
            }
            else
            {
                Directory.CreateDirectory(OnesyncDir);
            }
            return new string[] { };
        }

        private static string GetPath(string name)
        {
            return OnesyncDir + "\\" + name + ".1sync";
        }
    }
}