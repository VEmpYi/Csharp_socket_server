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
        private Socket socketWatch;

        // Save the IP address, port and socket of remote client to the dict
        private Dictionary<string, Socket> dictSocket = new Dictionary<string, Socket>();

        // Indicate the status of whether to start listening
        private bool startListened = false;
        
        // Create listening thread
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
                    // Create socketWatch
                    socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
                    // Save the socket ip to the dictionary and add to the text IP box
                    string ip = socketSend.RemoteEndPoint.ToString();
                    dictSocket.Add(ip, socketSend);
                    cbBoxIP.Items.Add(ip);
                    // Check whether the text box is empty. if it is empty, item 1 is selected by default.
                    if(cbBoxIP.Text == "")
                    {
                        cbBoxIP.SelectedIndex = cbBoxIP.Items.IndexOf(socketSend.RemoteEndPoint.ToString());
                    }
                    // Log send
                    ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + " Connected!");
                    // When client is successfully connected, server should receive the message from the client
                    Thread receive = new Thread(Receive);
                    receive.IsBackground = true;
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
                    byte[] buffer = new byte[1024 * 1024 * 10];
                    int r = socketSend.Receive(buffer);
                    // If r == 0, the connection may be closed
                    if (r == 0)
                    {
                        ShowMsg(socketSend.RemoteEndPoint.ToString() + ": " + "Close!");
                        // Remove the IP from the dictionary and text IP-box
                        dictSocket.Remove(socketSend.RemoteEndPoint.ToString());
                        cbBoxIP.Items.Remove(socketSend.RemoteEndPoint.ToString());
                        // Select the item 1 to display by default
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
                        // Close the connection
                        socketSend.Close();
                        Thread.CurrentThread.Abort();
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
            // Get the current time
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

        /// <summary>
        /// Try to ensure the correct port number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            // Ensure only hav number
            if (System.Text.RegularExpressions.Regex.IsMatch(txtPort.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                txtPort.Text = "0";
            }
            // Ensure the content is not empty
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
                // Get the text content  from the text message box
                string str = txtMsg.Text;
                // Convert the string to the bytes
                byte[] initBuffer = System.Text.Encoding.UTF8.GetBytes(str);
                // Add a message header for the content based on the message type
                byte[] buffer = MessageHeaders(msgType.text, initBuffer);
                if(buffer == null)  return;
                // Get the IP address selected from the comboBox list
                string ip = cbBoxIP.SelectedItem.ToString();
                // Send message to the specified IP
                dictSocket[ip].Send(buffer);
                ShowMsg("Send to " + dictSocket[ip].RemoteEndPoint + ": " + str, 2);
                // Clear the content of text message box
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
        /// <returns>Encapsulated data packet</returns>
        private byte[] MessageHeaders(msgType type, byte[] buffer)
        {
            try
            {
                List<byte> list = new List<byte>();
                // Add the message header
                list.Add((byte)type);
                // Add the message content
                list.AddRange(buffer);
                // list<> -> byte[]
                byte[] newBuffer = list.ToArray();
                return newBuffer;
            }
            catch
            {
                ShowMsg("Content processing failure!", 1);
                return null;
            }
        }

        /// <summary>
        /// Adds a message type header and filename to the message to be sent
        /// </summary>
        /// <param name="type">message type</param>
        /// <param name="fileName">fileName</param>
        /// <param name="buffer">the message to be sent</param>
        /// <returns>Encapsulated data packet</returns>
        private byte[] MessageHeaders(msgType type, string fileName, byte[] buffer)
        {
            try
            {
                List<byte> list = new List<byte>();
                // Add the message type
                list.Add((byte)type);
                // Add the length of the file name
                if (fileName.Length > 255)
                {
                    ShowMsg("File name is too long!", 1);
                    return null;
                }
                // Add the file name
                byte[] btFileName = Encoding.UTF8.GetBytes(fileName);
                // Add the length of file name
                list.Add((byte)btFileName.Length);
                // Add the file name
                list.AddRange(btFileName);
                // Add the message
                list.AddRange(buffer);
                byte[] newBuffer = list.ToArray();
                return newBuffer;
            }
            catch
            {
                ShowMsg("Content processing failure!", 1);
                return null;
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
            // Create a open file dialog to get the file path
            OpenFileDialog ofd = new OpenFileDialog();
            string path = @"C:\Users\" + Environment.UserName + @"\Desktop";
            ofd.InitialDirectory = path;
            ofd.Title = "Open";
            ofd.Filter = "All files|*.*";
            ofd.ShowDialog();
            // Add the path to the path text-box
            txtPath.Text = ofd.FileName;
        }

        /// <summary>
        /// Send file to the specified client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (startListened)
                {
                    string path = txtPath.Text;
                    string fileName = Path.GetFileName(path);
                    // Reads the file from the specified path
                    using (FileStream fsRead = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        byte[] initBuffer = new byte[1024 * 1024 * 10]; // 10MB
                        int r = fsRead.Read(initBuffer, 0, initBuffer.Length);
                        byte[] buffer = MessageHeaders(msgType.file, fileName, initBuffer);
                        if (buffer != null)
                        {
                            ShowMsg("Preparing to send file!");
                        }
                        else
                        {
                            ShowMsg("File read failure!", 1);
                            return;
                        }
                        // Send file
                        dictSocket[cbBoxIP.SelectedItem.ToString()].Send(buffer, 0, r + 2 + buffer[1], SocketFlags.None);
                    }
                }
                else
                {
                    ShowMsg("No listening started!", 1);
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

        /// <summary>
        /// Shake the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShake_Click(object sender, EventArgs e)
        {
            //byte[] buffer = new byte[1];
            //buffer[0] = 2;
            //dictSocket[cbBoxIP.SelectedItem.ToString()].Send(buffer);
        }
    }
}