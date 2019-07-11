using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClient
{
    public partial class SocketClient : Form
    {
        public delegate void UpdateReceiveMsgCallback(string msg);

        byte[] dataBuffer = new byte[10];
        IAsyncResult result;
        public AsyncCallback pfnCallBack;
        public Socket clientSocket;

        //构造函数
        public SocketClient()
        {
            InitializeComponent();

            //初始化
            tb_ServerIP.Text = InitializeInfo();
            tb_ServerPort.Text = "8000";
        }

        private string InitializeInfo()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());

            string ipAddrV4 = String.Empty;
            foreach (IPAddress ipAddr in ipHost.AddressList)
            {
                if (ipAddr.AddressFamily == AddressFamily.InterNetwork)
                    ipAddrV4 = ipAddr.ToString();
            }
            return ipAddrV4;
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            tb_ReceiveMsg.Clear();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            //Close Socket Connection
            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }
            this.Close();
        }

        private void btn_ConnectionServer_Click(object sender, EventArgs e)
        {
            if (tb_ServerIP.Text == "" || tb_ServerPort.Text == "")
            {
                MessageBox.Show("服务器IP和端口号必须填写\n");
                return;
            }
            try
            {
                UpdateStatusMsg(false);

                // Create the socket instance
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Create the end point 
                IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(tb_ServerIP.Text), System.Convert.ToInt32(tb_ServerPort.Text));

                // Connect to the remote host
                clientSocket.Connect(ipEnd);

                if (clientSocket.Connected)
                {
                    UpdateStatusMsg(true);
                    //Wait for data asynchronously 
                    WaitForData();
                }
            }
            catch (SocketException se)
            {
                MessageBox.Show(Environment.NewLine + "连接服务器失败，服务器正在运行么?\n" + se.Message);
                UpdateStatusMsg(false);
            }
        }

        private void UpdateStatusMsg(bool connected)
        {
            btn_ConnectServer.Enabled = !connected;
            btn_DisConnectServer.Enabled = connected;

            this.tb_Status.Text = connected ? "已连接" : "未连接";
        }

        private void WaitForData()
        {
            try
            {
                if (pfnCallBack == null)
                    pfnCallBack = new AsyncCallback(OnDataReceived);

                SocketPacket theSocPkt = new SocketPacket();
                theSocPkt.thisSocket = clientSocket;

                result = clientSocket.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnCallBack, theSocPkt);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        public void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                SocketPacket theSockId = (SocketPacket)asyn.AsyncState;
                int iRx = theSockId.thisSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);

                AppendReceivedMsg(new System.String(chars));

                WaitForData();
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", Environment.NewLine + "数据接收时: Socket 已关闭");
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }
        public class SocketPacket
        {
            public System.Net.Sockets.Socket thisSocket;
            public byte[] dataBuffer = new byte[2048];
        }
        private void AppendReceivedMsg(string msg)
        {
            if (InvokeRequired)
                tb_ReceiveMsg.BeginInvoke(new UpdateReceiveMsgCallback(UpdateReceivedMsg), msg);
            else
                UpdateReceivedMsg(msg);
        }

        private void UpdateReceivedMsg(string msg)
        {
            tb_ReceiveMsg.AppendText(Environment.NewLine + DateTime.Now + "  接收到数据： " + msg);
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = tb_SendMsg.Text.Trim();
  
                //Send Data By byte[]
                byte[] byData = System.Text.Encoding.UTF8.GetBytes(msg.ToString());
                if (clientSocket != null)
                    clientSocket.Send(byData);               
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        private void btn_DisConnectServer_Click(object sender, EventArgs e)
        {
            if (clientSocket != null)
            {
               clientSocket.Close();
               clientSocket = null;
               UpdateStatusMsg(false);
            }
        }
    }
}
