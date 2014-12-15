using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSONSharp;
using Eclipse.ShadowBot.Comms;
namespace Eclipse.Comms
{
    public static class ServerCommon
    {
        public static List<WowClient> ListeningClients = new List<WowClient>();
        public static List<WowMessage> Messages = new List<WowMessage>();
        public static List<WowCharacter> Characters = new List<WowCharacter>();
        public static List<String> RunningLog = new List<string>();
        public static void Log(string Text)
        {
            RunningLog.Add(string.Format("{0}: {1} \r\n", DateTime.Now, Text));
        }
        public static string Proccess(string data)
        {
            WowMessage obj = JSON.Deserialize<WowMessage>(data);
            switch (obj.Type){
                case "Broadcast":
                    var chr = Characters.Where(c => c.Name == obj.Name).FirstOrDefault();
                    if (chr != null)
                    {
                        chr.Name = obj.Name;
                        chr.Level = obj.Level;
                        chr.Location = obj.Location;
                        chr.ZoneId = obj.ZoneId;
                    }
                    else
                    {
                        WowCharacter wc = new WowCharacter() { Level=obj.Level, Location=obj.Location, ZoneId=obj.ZoneId, Name=obj.Name };
                        Characters.Add(wc);
                    }
                    return "OK.";
                case "GetChar":
                    return Characters.Where(c => c.Name == obj.Name).FirstOrDefault().ToJSON();
                case "LetsBeFriends":
                    //ToDo: Return a Port number to launch a server on the client side.
                    //ToDo: Add client code to launch a listening server.
                    return null;
                default:
                    return "";
        }

        }
    }
}
