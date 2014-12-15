using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JSONSharp;
using Eclipse.ShadowBot.Comms;
namespace Eclipse.Comms
{
    public partial class Client : Form
    {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public Client()
        {
            InitializeComponent();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            timer.Interval = 500;
            timer.Enabled = true;
            timer.Tick += timer_Tick;

        }

        void timer_Tick(object sender, EventArgs e)
        {
            foreach (var str in ClientCommon.RunningLog)
            {
                textBox1.AppendText(str);
            }
            ClientCommon.RunningLog.Clear();
        }
        public static void DoWork()
        {
            WowMessage cr = new WowMessage() { Type = "Broadcast", Level = 97, Location = new WowLocation() { X = 10f, Y = 10f, Z = 10f }, Name = "Elf", ZoneId = 518 };
            string str = cr.ToJSON();
            ClientCommon.SendMessage(str);
         }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //todo: start in new thread
                DoWork();
            }

            catch (Exception err)
            {
                Console.WriteLine("Error..... " + err.StackTrace);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WowMessage cr = new WowMessage() { Type = "GetChar", Name = "Elf" };
            string str = cr.ToJSON();
            var response = ClientCommon.SendMessage(str);
            WowCharacter chr = JSON.Deserialize<WowCharacter>(response);
 
        }
    }
}
