using System;
using System.Diagnostics;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using Sessions;

namespace Chrome
{
    public class ChromeManager : ISessionHandler
    {
        public static Tab[] tabs;
        public static WebSocketServer socket;

        public string Name => "Chrome";
        public string SaveSession(bool closeApp)
        {
            if (Process.GetProcessesByName("chrome").Length > 0)
            {
                string id = "";
                id = closeApp ? "2" : "1";
                string stringToSend = "{\"type\":" + id + "}";

                socket.WebSocketServices["/extension"].Sessions.Broadcast(stringToSend);
                Debug.WriteLine("HERE");


                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Debug.WriteLine("HERE");
                Debug.WriteLine(tabs);


                while (tabs == null)
                {
                    //Debug.WriteLine("HERE");
                    //Debug.WriteLine(tabs);

                    //float time = (float)stopWatch.Elapsed.TotalSeconds;
                    //if (time > 2.0f)
                    //{
                    //    return new Tab[] { };
                    //}
                }
                Array.Sort(tabs);
                Debug.WriteLine(tabs);
                Console.WriteLine("HERE");

                Debug.WriteLine("HERE");

                return JsonConvert.SerializeObject(tabs);
            }
            else
            {
                Debug.WriteLine("HERE2");
                Console.WriteLine("HERE2");
                return JsonConvert.SerializeObject(new Tab[] { });
            }
        }

        public void RestoreSession(string data)
        {
            Tab[] tabsToRestore = JsonConvert.DeserializeObject<Tab[]>(data);

            if (tabsToRestore.Length == 0) return;

            Process.Start("chrome.exe");
            string msgToSend = "{\"type\":3, \"data\":" + JsonConvert.SerializeObject(tabsToRestore) + "}";
            socket.WebSocketServices["/extension"].Sessions.Broadcast(msgToSend);
        }
    }


}