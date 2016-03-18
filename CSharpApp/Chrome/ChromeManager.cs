using System;
using System.Diagnostics;
using WebSocketSharp.Server;
using Newtonsoft.Json;



namespace Chrome
{
    public class ChromeManager
    {
        public Tab[] tabs;
        public WebSocketServer socket;

        public  Tab[] getChromeSession(bool closeAll)
        {
            if (Process.GetProcessesByName("chrome").Length > 0)
            {
                string id = "";
                if (closeAll)
                {
                    id = "2";
                }
                else
                {
                    id = "1";
                }
                string stringToSend = "{\"type\":" + id + "}";

                socket.WebSocketServices["/extension"].Sessions.Broadcast(stringToSend);
                Debug.WriteLine("HERE");

                
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Debug.WriteLine("HERE");
                Debug.WriteLine(tabs);


                while (tabs == null)
                {
                    Debug.WriteLine("HERE");
                    Debug.WriteLine(tabs);

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

                return tabs;
            }
            else
            {
                Debug.WriteLine("HERE2");
                Console.WriteLine("HERE2");
                return new Tab[] { };
            }

        }

        public  void openChromeSession(Tab[] tabs2)
        {
            if (tabs2.Length == 0) return;

            Process.Start("chrome.exe");
            string msgToSend = "{\"type\":3, \"data\":" + JsonConvert.SerializeObject(tabs2) + "}";
            socket.WebSocketServices["/extension"].Sessions.Broadcast(msgToSend);


        }


    }


}