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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace socket1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        // create socket for watch
        Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        bool startListened = false;
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (startListened)
                {
                    ShowMsg("The server has started listening!", 1);
                }
                else
                {
                    // change the start state
                    startListened = true;
                    IPAddress ip = IPAddress.Any;  // before: IPAddress.Parse(txtServer.Text);  do: string -> IPAddress
                     // Create port object
                    IPEndPoint ep = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
                    //listener port(bind listening port)
                    socketWatch.Bind(ep);
                    // setting up the listening queue
                    socketWatch.Listen(10);
                    ShowMsg("Listening to success!");
                    // create a new thread to accept connections
                    Thread listen = new Thread(Listen);
                    listen.IsBackground = true;
                    listen.Start(socketWatch);
                }
            }
            catch
            {
                ShowMsg("Build connection error!", 1);
            }
            
            
        }

       /// <summary>
       /// a socket to responsible for communication
       /// </summary>
       /// <param name="o">class Socket</param>
        void Listen(object o)
        {
            Socket socketWatch = o as Socket;
            try
            {
                while (true)
                {
                    // wait for the client to connect and create a socket responsible for communication
                    Socket socketSend = socketWatch.Accept();
                    // Log send
                    ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + " Connected!");
                    // When client is successfully connected, server should receive the message from the client
                    Thread receive = new Thread(Receive);
                    receive.IsBackground = true;
                    receive.Start(socketSend);
                }
            }
            catch
            {
                ShowMsg("Connection error! Location: Listen", 1);
            }
            
        }

        /// <summary>
        /// receive the message from the client
        /// </summary>
        /// <param name="obj">Socket Send Object</param>
        void Receive(object obj)
        {
            Socket socketSend = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    int r = socketSend.Receive(buffer);
                    if (r == 0)
                    {
                        ShowMsg(socketSend.RemoteEndPoint.ToString() + ": " + "Close!");
                        socketSend.Dispose();
                        socketSend.Close();
                        break;
                    }
                    string str = Encoding.UTF8.GetString(buffer, 0, r);
                    ShowMsg(socketSend.RemoteEndPoint + ": " + str, 2);

                }
            }
            catch
            {
                ShowMsg("Connection error! Location: Receive", 1);
            }
            
        }

        /// <summary>
        /// Send message to the log box
        /// </summary>
        /// <param name="str">Log message</param>
        /// <param name="level">log level default is INFO, ERROR is 1, MESG is 2</param>
        void ShowMsg(string str,int level = 0)
        {
            string time = DateTime.Now.ToString().Substring(11);
            string state = "[] ";
            switch (level)
            {
                case 0: 
                    state = "[INFO]";
                    break;
                case 1:
                    state = "[ERROR]";
                    break;
                case 2:
                    state = "[MESG]";
                    break;
                default:
                    state = "[NULL]";
                    break;
            }
            txtLog.AppendText(state + "[" + time + "]" + str + "\r\n");
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtPort.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                txtPort.Text = "0";
            }

            if(txtPort.Text == "")
            {
                txtPort.Text = "0";
            }
            
            if(Convert.ToInt32(txtPort.Text) < 0 || Convert.ToInt32(txtPort.Text) > 65535)
            {
                MessageBox.Show("Input Error! The value should range from 0 ~ 65535");
                txtPort.Text = "0";
            }
        }
    }
}
