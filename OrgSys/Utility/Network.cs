using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Utility
{
    public class Network
    {
        static List<string> ips = new List<string>();
        static bool[] Lock = new bool[17];
        public List<string> GetIps()
        {           
            string ipBase = getIPAddress();
            string[] ipParts = ipBase.Split('.');
            ipBase = ipParts[0] + "." + ipParts[1] + "." + ipParts[2] + ".";

            for (int i = 0; i < 17 ; i++)
                new Thread(() => { Do(ipBase, i * 15, i); }).Start();

            //new Thread(() => { Do(ipBase, 25, 1); }).Start();
            //new Thread(() => { Do(ipBase, 50, 2); }).Start();
            //new Thread(() => { Do(ipBase, 75, 3); }).Start();
            //new Thread(() => { Do(ipBase, 100, 4); }).Start();
            //new Thread(() => { Do(ipBase, 125, 5); }).Start();
            //new Thread(() => { Do(ipBase, 150, 6); }).Start();
            //new Thread(() => { Do(ipBase, 175, 7); }).Start();
            //new Thread(() => { Do(ipBase, 200, 8); }).Start();
            //new Thread(() => { Do(ipBase, 225, 9); }).Start();

            while (true)
            {
                bool lck = false;
                for (int i = 0; i < Lock.Length; i++)
                    if (Lock[i])
                        lck = true;

                if (!lck)
                    break;               
            }
            return ips;
        }      

        public void Do(string ipBase , int StartFrom , int count)
        {
            if (StartFrom + 25 < 256)
            {
                Lock[count] = true;
                for (int i = StartFrom; i <= StartFrom + 25; i++)
                {
                    string ipText = ipBase + i.ToString();
                    if (Ping(ipText))
                        ips.Add(ipText);
                }
                Lock[count] = false;
            }
        }

        public bool Ping(string ip)
        {
            var ping = new System.Net.NetworkInformation.Ping();

            var result = ping.Send(ip);

            if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
                return true;
            return false;
        }

        public string getIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
    }
}