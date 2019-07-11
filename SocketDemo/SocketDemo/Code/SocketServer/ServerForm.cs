using System;
using System.Collections;
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

namespace SocketServer
{
    public partial class ServerForm : Form
    {
        public delegate void UpdateReceiveMsgCallback(string msg);

        public delegate void UpdateConnectedClientListCallback();

        private System.Collections.ArrayList workerSocketList = ArrayList.Synchronized(new System.Collections.ArrayList());

        private int clientCount = 0;

        public AsyncCallback pfnWorkerCallBack;
        private Socket mainSocket;

        //构造函数
        public ServerForm()
        {
            InitializeComponent();

            //初始化
            InitializeInfo();

            tb_ServerPort.Text = "8000";
            tb_SendMsg.Text = "";
            tb_ReceiveMsg.Text = "";
            lb_ConnectedClinet.Items.Clear();
        }

        private void InitializeInfo()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            List<IPAddress> ipAddressList = new List<IPAddress>();

            foreach (IPAddress ipAddr in ipHost.AddressList)
            {
                if (ipAddr.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddressList.Add(ipAddr);
                }
            }
            cb_ServerIP.DataSource = ipAddressList;
        }

        //关闭
        private void btn_Close_Click(object sender, EventArgs e)
        {
            //Close Socket Connection
            this.CloseSockets();

            this.Close();
        }

        //关闭Socket
        private void CloseSockets()
        {
            if (mainSocket != null)
                mainSocket.Close();

            Socket workerSocket = null;
            for (int i = 0; i < workerSocketList.Count; i++)
            {
                workerSocket = (Socket)workerSocketList[i];
                if (workerSocket != null)
                {
                    workerSocket.Close();
                    workerSocket = null;
                }
            }
        }

        private void btn_StartListen_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_ServerPort.Text == "")
                {
                    MessageBox.Show("请输入端口号！");
                    return;
                }
                IPAddress ipAddress = cb_ServerIP.SelectedItem as IPAddress;
                int port = System.Convert.ToInt32(tb_ServerPort.Text.Trim());

                //创建监听Socket
                mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //邦定IP
                IPEndPoint ipLocal = new IPEndPoint(ipAddress, port);
                mainSocket.Bind(ipLocal);

                //开始监听
                mainSocket.Listen(4);

                //创建Call Back为任意客户端连接
                mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);

                SwitchStatus(true);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        private void SwitchStatus(bool isListening)
        {
            this.btn_StartListen.Enabled = !isListening;
            this.btn_StopListen.Enabled = isListening;
        }

        //回调函数，客户端连接时被调用
        public void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                // 创建一个新的 Socket 
                Socket workerSocket = mainSocket.EndAccept(asyn);

                // 递增客户端数目
                Interlocked.Increment(ref clientCount);

                // 添加到客户端数组中
                workerSocketList.Add(workerSocket);

                //发送一个消息
                string msg = "Welcome 客户端 " + clientCount + "\n";
                SendMsgToClient(msg, clientCount);

                //刷新已连接的客户端列表
                RefreshConnectedClientList();

                //指定这个Socket处理接收到的数据
                WaitForData(workerSocket, clientCount);

                // Main Socket继续等待客户端的连接
                mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\n 客户端连接: Socket 已关闭\n");
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        //发送消息给客户端
        private void SendMsgToClient(string msg, int clientNumber)
        {
            byte[] byData = System.Text.Encoding.UTF8.GetBytes(msg);

            Socket workerSocket = (Socket)workerSocketList[clientNumber - 1];
            workerSocket.Send(byData);
        }

        //清除接收到的数据
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            tb_ReceiveMsg.Clear();
        }

        //等待客户端的数据
        private void WaitForData(System.Net.Sockets.Socket skt, int clientNumber)
        {
            try
            {
                if (pfnWorkerCallBack == null)
                    pfnWorkerCallBack = new AsyncCallback(OnDataReceived);

                SocketPacket theSocPkt = new SocketPacket(skt, clientNumber);
                skt.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnWorkerCallBack, theSocPkt);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        //Call Back, Socket检测到任意客户端写入数据时
        public void OnDataReceived(IAsyncResult asyn)
        {
            SocketPacket socketData = (SocketPacket)asyn.AsyncState;
            try
            {
                int iRx = socketData.currSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];

                System.Text.Decoder decoder = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = decoder.GetChars(socketData.dataBuffer, 0, iRx, chars, 0);

                System.String szData = new System.String(chars);

                AppendReceivedMsg(Environment.NewLine + "Client " + socketData.clientNO + " Data:" + new System.String(chars));

                //For Debug
                //string replyMsg = "Server 回复:" + szData.ToUpper();

                string replyMsg = "Server 回复: 接收完成";
                byte[] byData = System.Text.Encoding.UTF8.GetBytes(replyMsg);

                Socket workerSocket = (Socket)socketData.currSocket;
                workerSocket.Send(byData);

                WaitForData(socketData.currSocket, socketData.clientNO);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\n 数据接收时: Socket 已关闭\n");
            }
            catch (SocketException se)
            {
                if (se.ErrorCode == 10054) // 连接被管道重置
                {
                    string msg = "Client " + socketData.clientNO + " 断开连接" + "\n";
                    AppendReceivedMsg(msg);

                    workerSocketList[socketData.clientNO - 1] = null;
                    RefreshConnectedClientList();
                }
                else
                    MessageBox.Show(se.Message);
            }
        }

        private void AppendReceivedMsg(string msg)
        {
            if (InvokeRequired)
                tb_ReceiveMsg.BeginInvoke(new UpdateReceiveMsgCallback(UpdateReceivedMsg), msg);
            else
                UpdateReceivedMsg(msg);
        }

        private void RefreshConnectedClientList()
        {
            if (InvokeRequired)
                lb_ConnectedClinet.BeginInvoke(new UpdateConnectedClientListCallback(RefreshClientList), null);
            else
                RefreshClientList();
        }

        private void RefreshClientList()
        {
            lb_ConnectedClinet.Items.Clear();
            for (int i = 0; i < workerSocketList.Count; i++)
            {
                string clientKey = Convert.ToString(i + 1);
                Socket workerSocket = (Socket)workerSocketList[i];
                if (workerSocket != null)
                {
                    if (workerSocket.Connected)
                        lb_ConnectedClinet.Items.Add("Client [" + clientKey + "] IP:" + (workerSocket.RemoteEndPoint as IPEndPoint).Address.ToString());
                }
            }
        }

        private void UpdateReceivedMsg(string msg)
        {
            tb_ReceiveMsg.AppendText(Environment.NewLine + DateTime.Now + "接收到数据：" + msg);
        }

        private void btn_StopListen_Click(object sender, EventArgs e)
        {
            CloseSockets();
            SwitchStatus(false);
        }

        internal class SocketPacket
        {
            public System.Net.Sockets.Socket currSocket;
            public int clientNO;

            public byte[] dataBuffer = new byte[8192];

            public SocketPacket(System.Net.Sockets.Socket socket, int clientNumber)
            {
                currSocket = socket;
                clientNO = clientNumber;
            }
        }

        private void btn_SendMsg_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] byData = System.Text.Encoding.UTF8.GetBytes("服务器发送消息: " + tb_SendMsg.Text);
                Socket workerSocket = null;
                for (int i = 0; i < workerSocketList.Count; i++)
                {
                    workerSocket = (Socket)workerSocketList[i];
                    if (workerSocket != null)
                    {
                        if (workerSocket.Connected)
                            workerSocket.Send(byData);
                    }
                }
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }
    }
}
