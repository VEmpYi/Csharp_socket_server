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

        // Create socket for watch
        Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Save the IP address, port and socket of remote client to the dict
        Dictionary<string, Socket> dictSocket = new Dictionary<string, Socket>();
        Dictionary<string, string> dictScoketNames = new Dictionary<string, string>();

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

        Socket socketSend;
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
                    // Wait for the client to connect and create a socket responsible for communication
                    socketSend = socketWatch.Accept();
                    // Save the socket
                    string ip = socketSend.RemoteEndPoint.ToString();
                    dictSocket.Add(ip, socketSend);
                    cbBoxIP.Items.Add(ip);
                    // Log send
                    ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + " Connected!");
                    // When client is successfully connected, server should receive the message from the client
                    Thread receive = new Thread(Receive);
                    receive.IsBackground = true;
                    dictScoketNames.Add(ip, receive.Name);
                    receive.Start(dictSocket[ip]);
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
                        dictSocket.Remove(socketSend.RemoteEndPoint.ToString());
                        cbBoxIP.Items.Remove(socketSend.RemoteEndPoint.ToString());
                        // socketSend.Dispose();
                        socketSend.Close();
                        break;
                    }
                    string str = Encoding.UTF8.GetString(buffer, 0, r);
                    ShowMsg(socketSend.RemoteEndPoint + ": " + str, 2);
                    
                }
            }
            catch(Exception e)
            {
                if(!(e is System.Net.Sockets.SocketException))
                {
                    ShowMsg("Connection error! Location: Receive", 1);
                }
                else
                {
                    
                }
                
            }
            
        }

        /// <summary>
        /// Send message to the log textbox
        /// </summary>
        /// <param name="str">Log message</param>
        /// <param name="level">log level default is INFO, ERROR is 1, MESG is 2</param>
        void ShowMsg(string str,int level = 0)
        {
            string time = DateTime.Now.ToString().Substring(11);
            string state;
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

        /// <summary>
        /// Send message to client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendMesg_Click(object sender, EventArgs e)
        {
            try
            {
                string str = txtMsg.Text;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
                bool answer = MessageHeaders(msgType.text, ref buffer);
                // Get the IP address selected from the comboBox list
                string ip = cbBoxIP.SelectedItem.ToString();
                // Send message to the specified IP
                dictSocket[ip].Send(buffer);
                ShowMsg("Send to " + dictSocket[ip].RemoteEndPoint + ": " + str, 2);
                txtMsg.Clear();
            }
            catch
            {
                ShowMsg("Message send failure!", 1);
            }

        }

        /// <summary>
        /// Adds a message type header to the message to be sent
        /// </summary>
        /// <param name="type">message type</param>
        /// <param name="buffer">the message to be sent</param>
        /// <returns>whether the action was successful</returns>
        private bool MessageHeaders(msgType type, ref byte[] buffer)
        {
            try
            {
                List<byte> list = new List<byte>();
                list.Add((byte)type);
                list.AddRange(buffer);
                buffer = list.ToArray();
                return true;
            }
            catch
            {
                return false;
            }
        }

        enum msgType
        {
            text,
            file,
            shake
        }
    }
}
