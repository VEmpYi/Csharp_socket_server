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
            if (startListened)
            {
                ShowMsg("The server has started listening!", 1);
            }
            else
            {
                IPAddress ip = IPAddress.Any;  // before: IPAddress.Parse(txtServer.Text);  do: string -> IPAddress
                // Create port object
                IPEndPoint ep = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
                //listener port(bind listening port)
                socketWatch.Bind(ep);
                // setting up the listening queue
                socketWatch.Listen(10);
                ShowMsg("Listening to success!");
                // create a new thread to accept connections
                Thread th = new Thread(Listen);
                th.IsBackground = true;
                th.Start(socketWatch);
            }
            
        }

       /// <summary>
       /// a socket to responsible for communication
       /// </summary>
       /// <param name="o">class Socket</param>
        void Listen(object o)
        {
            Socket socketWatch = o as Socket;
            while (true)
            {
                // wait for the client to connect and create a socket responsible for communication
                Socket socketSend = socketWatch.Accept();
                // Log send
                ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + " Connected!");
            }
        }

        void ShowMsg(string str,int level = 0)
        {
            System.DateTime time = DateTime.Now;
            string hour = time.Hour.ToString();
            string minute = time.Minute.ToString();
            string second = time.Second.ToString();
            string s = hour + ":" + minute + ":" + second + " ";
            string state = "[] ";
            switch (level)
            {
                case 0: 
                    state = "[INFO] ";
                    break;
                case 1:
                    state = "[ERROR] ";
                    break ;
                default:
                    state = "[NULL] ";
                    break;
            }  
            txtLog.AppendText(state+s+ str + "\r\n");
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
