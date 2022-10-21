using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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
        private Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Save the IP address, port and socket of remote client to the dict
        private Dictionary<string, Socket> dictSocket = new Dictionary<string, Socket>();

        private Dictionary<string, string> dictScoketNames = new Dictionary<string, string>();

        private bool startListened = false;

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
                    // Select IP address to create socket
                    // If select the specified IP address: IPAddress.Parse(txtServer.Text);  do: string -> IPAddress
                    IPAddress ip = IPAddress.Any;
                    // Create port object
                    IPEndPoint ep = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
                    // Listener port(bind listening port)
                    socketWatch.Bind(ep);
                    // Setting the maximum number of monitors
                    socketWatch.Listen(10);
                    ShowMsg("Server starts listening!");
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

        private Socket socketSend;

        /// <summary>
        /// a socket to responsible for communication
        /// </summary>
        /// <param name="o">class Socket</param>
        private void Listen(object o)
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
                    if(cbBoxIP.Text == "")
                    {
                        cbBoxIP.SelectedIndex = cbBoxIP.Items.IndexOf(socketSend.RemoteEndPoint.ToString());
                    }
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
        private void Receive(object obj)
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
                        if (cbBoxIP.Text == "")
                        {
                            if (dictSocket.Keys != null)
                            {
                                foreach (string str in dictSocket.Keys)
                                {
                                    cbBoxIP.SelectedIndex = cbBoxIP.Items.IndexOf(str);
                                    break;
                                }
                            }
                        }
                        socketSend.Close();
                        break;
                    }
                    if (buffer[0] == (byte)msgType.text)
                    {
                        string str = Encoding.UTF8.GetString(buffer, 1, r - 1);
                        ShowMsg(socketSend.RemoteEndPoint + ": " + str, 2);
                    }
                    else if (buffer[0] == (byte)msgType.file)
                    {
                    }
                    else
                    {
                        ShowMsg("Message type error!", 1);
                    }
                }
            }
            catch (Exception e)
            {
                if (e is System.Net.Sockets.SocketException)
                {
                    ShowMsg("The remote host closed an existing connection!" + "Host is " + socketSend.RemoteEndPoint, 1);
                    dictSocket.Remove(socketSend.RemoteEndPoint.ToString());
                    cbBoxIP.Items.Remove(socketSend.RemoteEndPoint.ToString());
                    if(cbBoxIP.Text == "")
                    {
                        if(dictSocket.Keys != null)
                        {
                            foreach(string str in dictSocket.Keys)
                            {
                                cbBoxIP.SelectedIndex = cbBoxIP.Items.IndexOf(str);
                                break;
                            }
                        }
                    }
                    Thread.CurrentThread.Abort();
                }
                //else if()
                //{
                //}
                else
                {
                    ShowMsg("Unknown error! Location: Receive", 1);
                    Thread.CurrentThread.Abort();
                }
            }
        }

        /// <summary>
        /// Send message to the log textbox
        /// </summary>
        /// <param name="str">Log message</param>
        /// <param name="level">log level default is INFO, ERROR is 1, MESG is 2</param>
        private void ShowMsg(string str, int level = 0)
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

            if (txtPort.Text == "")
            {
                txtPort.Text = "0";
            }

            if (Convert.ToInt32(txtPort.Text) < 0 || Convert.ToInt32(txtPort.Text) > 65535)
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
                byte[] initBuffer = System.Text.Encoding.UTF8.GetBytes(str);
                byte[] buffer = MessageHeaders(msgType.text, initBuffer);
                ShowMsg("buffer length is " + buffer.Length);
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
        private byte[] MessageHeaders(msgType type, byte[] buffer)
        {
            try
            {
                List<byte> list = new List<byte>();
                list.Add((byte)type);
                list.AddRange(buffer);
                byte[] newBuffer = list.ToArray();
                return newBuffer;
            }
            catch
            {
                byte[] fail = null;
                return fail;
            }
        }

        /// <summary>
        /// define the message types
        /// </summary>
        private enum msgType
        {
            text,
            file,
            shake
        }

        /// <summary>
        /// Select the file to send
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string path = @"C:\Users\" + Environment.UserName + @"\Desktop";
            ofd.InitialDirectory = path;
            ofd.Title = "Open";
            ofd.Filter = "All files|*.*";
            ofd.ShowDialog();
            txtPath.Text = ofd.FileName;
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            try
            {
                string path = txtPath.Text;
                using (FileStream fsRead = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] initBuffer = new byte[1024 * 1024 * 2];
                    int r = fsRead.Read(initBuffer, 0, initBuffer.Length);
                    byte[] buffer = MessageHeaders(msgType.file, initBuffer);
                    if (buffer != null)
                    {
                        ShowMsg("Preparing to send file!");
                    }
                    else
                    {
                        ShowMsg("File create failure!", 1);
                    }
                    dictSocket[cbBoxIP.SelectedItem.ToString()].Send(buffer, 0, r + 1, SocketFlags.None);
                }
            }
            catch(Exception ex)
            {
                if(ex is System.ArgumentException)
                {
                    ShowMsg("File path is empty!", 1);
                    MessageBox.Show("File path is empty!", "Error");
                }
                if(ex is System.NullReferenceException)
                {
                    ShowMsg("No remote client is selected!", 1);
                }
            }
        }

        private void btnShake_Click(object sender, EventArgs e)
        {
            byte[] buffer = new byte[1];
            buffer[0] = 2;
            dictSocket[cbBoxIP.SelectedItem.ToString()].Send(buffer);
        }
    }
}