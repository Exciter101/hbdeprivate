using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eclipse.Comms
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public Form1()
        {
            InitializeComponent();
            timer.Interval = 500;
            timer.Enabled = true;
            timer.Tick += timer_Tick;
        }
        void timer_Tick(object sender, EventArgs e)
        {
            foreach (var str in ServerCommon.RunningLog)
            {
                textBox1.AppendText(str);
            }
            ServerCommon.RunningLog.Clear();
        }

        public static void DoWork()
        {
            try
            {
                // set the TcpListener on port 13000 
                int port = 13000;
                TcpListener server = new TcpListener(IPAddress.Any, port);

                // Start listening for client requests
                server.Start();


                // Buffer for reading data 
                byte[] bytes = new byte[1024];
                string data;

                //Enter the listening loop 
                while (true)
                {
                    ServerCommon.Log("Waiting for a connection... ");

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    ServerCommon.Log("Connected!");

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    i = stream.Read(bytes, 0, bytes.Length);

                    while (i != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        ServerCommon.Log(String.Format("Received: {0}", data));

                        // Process the data sent by the client.
                        //HANDLE MESSAGE
                        string ReturnMessage = ServerCommon.Proccess(data);

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(ReturnMessage);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        ServerCommon.Log(String.Format("Sent: {0}", ReturnMessage));

                        i = stream.Read(bytes, 0, bytes.Length);

                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            //myList.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //todo: start in new thread
                ThreadStart myThreadDelegate = new ThreadStart(DoWork);
                Thread myThread = new Thread(myThreadDelegate);
                myThread.Start();

            }
            catch (Exception err)
            {
                Console.WriteLine("Error..... " + err.StackTrace);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client client = new Client();
            client.Show();
        }

    }
}
