using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sessions
{
    [Serializable]
    public class Session
    {
        

        /*internal Dictionary<string, string[]> OfficeDictionary { get; set; }

        internal Tab[] tabs { get; set; }

        internal Dictionary<string, string[]> ExplorerDictionary { get; set; }

        private string Name { get; set; }

        private string MacID { get; set; }

        public Session(string name)
        {
            Name = name;
            MacID = FetchMacId();
        }

        internal void Serialize()
        {
            string path = ONESYNC_DIR + "\\" + Name + ".1sync";
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, this);
            stream.Close();
        }

        internal static Session DeSerialize(string name)
        {
            string path = ONESYNC_DIR + "\\" + name + ".1sync";
            if (!File.Exists(path)) return null;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            Session obj = (Session)formatter.Deserialize(stream);
            stream.Close();

            return obj;
        }

        public override bool Equals(object o)
        {
            if (o != null)
            {
                Session s = o as Session;
                if (s == null || this == null) return false;
                return s.Name.Equals(Name);
            }
            return false;
        }

        private string FetchMacId()
        {
            string macAddresses = "";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }*/
    }
}