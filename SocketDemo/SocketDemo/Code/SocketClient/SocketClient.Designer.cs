namespace SocketClient
{
    partial class SocketClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_ConnectServer = new System.Windows.Forms.Button();
            this.btn_DisConnectServer = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.tb_SendMsg = new System.Windows.Forms.TextBox();
            this.tb_ReceiveMsg = new System.Windows.Forms.TextBox();
            this.btn_Send = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_ServerIP = new System.Windows.Forms.TextBox();
            this.tb_ServerPort = new System.Windows.Forms.TextBox();
            this.tb_Status = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器IP：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "服务器端口：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "需要发送的消息：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(292, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "接收的消息：";
            // 
            // btn_ConnectServer
            // 
            this.btn_ConnectServer.BackColor = System.Drawing.Color.LightGreen;
            this.btn_ConnectServer.Location = new System.Drawing.Point(294, 14);
            this.btn_ConnectServer.Name = "btn_ConnectServer";
            this.btn_ConnectServer.Size = new System.Drawing.Size(92, 48);
            this.btn_ConnectServer.TabIndex = 4;
            this.btn_ConnectServer.Text = "连接服务器";
            this.btn_ConnectServer.UseVisualStyleBackColor = false;
            this.btn_ConnectServer.Click += new System.EventHandler(this.btn_ConnectionServer_Click);
            // 
            // btn_DisConnectServer
            // 
            this.btn_DisConnectServer.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btn_DisConnectServer.Location = new System.Drawing.Point(402, 14);
            this.btn_DisConnectServer.Name = "btn_DisConnectServer";
            this.btn_DisConnectServer.Size = new System.Drawing.Size(92, 49);
            this.btn_DisConnectServer.TabIndex = 5;
            this.btn_DisConnectServer.Text = "断开连接";
            this.btn_DisConnectServer.UseVisualStyleBackColor = false;
            this.btn_DisConnectServer.Click += new System.EventHandler(this.btn_DisConnectServer_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(359, 350);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(75, 23);
            this.btn_Clear.TabIndex = 6;
            this.btn_Clear.Text = "清除";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(440, 350);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 23);
            this.btn_Close.TabIndex = 7;
            this.btn_Close.Text = "关闭";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // tb_SendMsg
            // 
            this.tb_SendMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_SendMsg.Location = new System.Drawing.Point(12, 111);
            this.tb_SendMsg.Multiline = true;
            this.tb_SendMsg.Name = "tb_SendMsg";
            this.tb_SendMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_SendMsg.Size = new System.Drawing.Size(218, 192);
            this.tb_SendMsg.TabIndex = 8;
            // 
            // tb_ReceiveMsg
            // 
            this.tb_ReceiveMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ReceiveMsg.Location = new System.Drawing.Point(294, 111);
            this.tb_ReceiveMsg.Multiline = true;
            this.tb_ReceiveMsg.Name = "tb_ReceiveMsg";
            this.tb_ReceiveMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_ReceiveMsg.Size = new System.Drawing.Size(221, 192);
            this.tb_ReceiveMsg.TabIndex = 9;
            // 
            // btn_Send
            // 
            this.btn_Send.Location = new System.Drawing.Point(12, 319);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(75, 23);
            this.btn_Send.TabIndex = 10;
            this.btn_Send.Text = "发送";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 361);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "状态：";
            // 
            // tb_ServerIP
            // 
            this.tb_ServerIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ServerIP.Location = new System.Drawing.Point(93, 10);
            this.tb_ServerIP.Name = "tb_ServerIP";
            this.tb_ServerIP.Size = new System.Drawing.Size(137, 21);
            this.tb_ServerIP.TabIndex = 12;
            // 
            // tb_ServerPort
            // 
            this.tb_ServerPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ServerPort.Location = new System.Drawing.Point(93, 46);
            this.tb_ServerPort.Name = "tb_ServerPort";
            this.tb_ServerPort.Size = new System.Drawing.Size(137, 21);
            this.tb_ServerPort.TabIndex = 13;
            // 
            // tb_Status
            // 
            this.tb_Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_Status.Location = new System.Drawing.Point(55, 357);
            this.tb_Status.Name = "tb_Status";
            this.tb_Status.ReadOnly = true;
            this.tb_Status.Size = new System.Drawing.Size(155, 21);
            this.tb_Status.TabIndex = 14;
            // 
            // SocketClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 384);
            this.Controls.Add(this.tb_Status);
            this.Controls.Add(this.tb_ServerPort);
            this.Controls.Add(this.tb_ServerIP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btn_Send);
            this.Controls.Add(this.tb_ReceiveMsg);
            this.Controls.Add(this.tb_SendMsg);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.btn_DisConnectServer);
            this.Controls.Add(this.btn_ConnectServer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SocketClient";
            this.Text = "Socket 客户端";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_ConnectServer;
        private System.Windows.Forms.Button btn_DisConnectServer;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.TextBox tb_SendMsg;
        private System.Windows.Forms.TextBox tb_ReceiveMsg;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_ServerIP;
        private System.Windows.Forms.TextBox tb_ServerPort;
        private System.Windows.Forms.TextBox tb_Status;
    }
}

