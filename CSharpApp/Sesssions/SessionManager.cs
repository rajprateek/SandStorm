using Chrome;
using System.IO;

namespace Sessions
{
    public class SessionManager
    {
        public static void NewSession(string name, bool closeAll, ChromeManager chromeManager)
        {
            Session session = new Session(name);
            session.OfficeDictionary = Office.Utils.SaveAll(closeAll);
            session.tabs = chromeManager.getChromeSession(closeAll);
            session.ExplorerDictionary = Windows.Utils.SaveAll(closeAll);
            session.Serialize();
            chromeManager.tabs = null;
        }

        public static void RestoreSession(string name, ChromeManager chromeManager)
        {
            Session session = Session.DeSerialize(name);
            if (session == null) return;
            Office.Utils.RestoreAll(session.OfficeDictionary);
            chromeManager.openChromeSession(session.tabs);
            Windows.Utils.RestoreAll(session.ExplorerDictionary);
        }

        public static void RemoveSession(string name)
        {
            if (Directory.Exists(Session.ONESYNC_DIR))
            {
                File.Delete(Session.ONESYNC_DIR + "\\" + name + ".1sync");
            }
        }

        public static string[] GetAllSessions()
        {
            if (Directory.Exists(Session.ONESYNC_DIR))
            {
                string[] availableSessions = Directory.GetFiles(Session.ONESYNC_DIR, "*.1sync");

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
                Directory.CreateDirectory(Session.ONESYNC_DIR);
            }
            return new string[] { };
        }
    }
}