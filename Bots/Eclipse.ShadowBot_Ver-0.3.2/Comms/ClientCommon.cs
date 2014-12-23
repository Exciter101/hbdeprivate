using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.Comms
{
    public static class ClientCommon
    {
        public static List<String> RunningLog = new List<string>();
        public static void Log(string Text)
        {
            RunningLog.Add(string.Format("{0}: {1} \r\n", DateTime.Now, Text));
        }
        public static string SendMessage(string msgText)
        {
            TcpClient tcpclnt = new TcpClient();
            ClientCommon.Log("Connecting.....");

            tcpclnt.Connect("127.0.0.1", 13000);
            // use the ipaddress as in the server program

            ClientCommon.Log("Connected");

            Stream stm = tcpclnt.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(msgText);
            ClientCommon.Log("Transmitting.....");

            stm.Write(ba, 0, ba.Length);

            byte[] bb = new byte[100];
            int k = stm.Read(bb, 0, 100);
            var recmessage = string.Empty;
            for (int i = 0; i < k; i++)
            {
                recmessage += Convert.ToChar(bb[i]).ToString();
            }
            ClientCommon.Log(recmessage);
            tcpclnt.Close();
            return recmessage;
        }
    }
}
