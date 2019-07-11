namespace SocketServer
{
    partial class ServerForm
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
            this.tb_ServerPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_SendMsg = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_ReceiveMsg = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_StartListen = new System.Windows.Forms.Button();
            this.btn_SendMsg = new System.Windows.Forms.Button();
            this.btn_StopListen = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.lb_ConnectedClinet = new System.Windows.Forms.ListBox();
            this.cb_ServerIP = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器 IP：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口号：";
            // 
            // tb_ServerPort
            // 
            this.tb_ServerPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ServerPort.Location = new System.Drawing.Point(87, 37);
            this.tb_ServerPort.Name = "tb_ServerPort";
            this.tb_ServerPort.Size = new System.Drawing.Size(142, 21);
            this.tb_ServerPort.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(264, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "广播数据给客户端：";
            // 
            // tb_SendMsg
            // 
            this.tb_SendMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_SendMsg.Location = new System.Drawing.Point(266, 102);
            this.tb_SendMsg.Multiline = true;
            this.tb_SendMsg.Name = "tb_SendMsg";
            this.tb_SendMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_SendMsg.Size = new System.Drawing.Size(214, 86);
            this.tb_SendMsg.TabIndex = 3;
            this.tb_SendMsg.Text = "发送给客户端的数据";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "已连接的客户端：";
            // 
            // tb_ReceiveMsg
            // 
            this.tb_ReceiveMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ReceiveMsg.Location = new System.Drawing.Point(15, 228);
            this.tb_ReceiveMsg.Multiline = true;
            this.tb_ReceiveMsg.Name = "tb_ReceiveMsg";
            this.tb_ReceiveMsg.Size = new System.Drawing.Size(465, 86);
            this.tb_ReceiveMsg.TabIndex = 5;
            this.tb_ReceiveMsg.Text = "收到客户端的数据";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "从客户端接收到的数据：";
            // 
            // btn_StartListen
            // 
            this.btn_StartListen.BackColor = System.Drawing.Color.LightGreen;
            this.btn_StartListen.Location = new System.Drawing.Point(324, 10);
            this.btn_StartListen.Name = "btn_StartListen";
            this.btn_StartListen.Size = new System.Drawing.Size(75, 48);
            this.btn_StartListen.TabIndex = 0;
            this.btn_StartListen.Text = "开始监听";
            this.btn_StartListen.UseVisualStyleBackColor = false;
            this.btn_StartListen.Click += new System.EventHandler(this.btn_StartListen_Click);
            // 
            // btn_SendMsg
            // 
            this.btn_SendMsg.Location = new System.Drawing.Point(405, 194);
            this.btn_SendMsg.Name = "btn_SendMsg";
            this.btn_SendMsg.Size = new System.Drawing.Size(75, 28);
            this.btn_SendMsg.TabIndex = 4;
            this.btn_SendMsg.Text = "发送数据";
            this.btn_SendMsg.UseVisualStyleBackColor = true;
            this.btn_SendMsg.Click += new System.EventHandler(this.btn_SendMsg_Click);
            // 
            // btn_StopListen
            // 
            this.btn_StopListen.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btn_StopListen.Location = new System.Drawing.Point(405, 10);
            this.btn_StopListen.Name = "btn_StopListen";
            this.btn_StopListen.Size = new System.Drawing.Size(75, 48);
            this.btn_StopListen.TabIndex = 1;
            this.btn_StopListen.Text = "停止监听";
            this.btn_StopListen.UseVisualStyleBackColor = false;
            this.btn_StopListen.Click += new System.EventHandler(this.btn_StopListen_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(324, 323);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(75, 28);
            this.btn_Clear.TabIndex = 6;
            this.btn_Clear.Text = "清除";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(405, 323);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 28);
            this.btn_Close.TabIndex = 7;
            this.btn_Close.Text = "关闭";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // lb_ConnectedClinet
            // 
            this.lb_ConnectedClinet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_ConnectedClinet.FormattingEnabled = true;
            this.lb_ConnectedClinet.ItemHeight = 12;
            this.lb_ConnectedClinet.Location = new System.Drawing.Point(15, 102);
            this.lb_ConnectedClinet.Name = "lb_ConnectedClinet";
            this.lb_ConnectedClinet.Size = new System.Drawing.Size(214, 86);
            this.lb_ConnectedClinet.TabIndex = 2;
            // 
            // cb_ServerIP
            // 
            this.cb_ServerIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_ServerIP.FormattingEnabled = true;
            this.cb_ServerIP.Location = new System.Drawing.Point(87, 10);
            this.cb_ServerIP.Name = "cb_ServerIP";
            this.cb_ServerIP.Size = new System.Drawing.Size(142, 20);
            this.cb_ServerIP.TabIndex = 10;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 360);
            this.Controls.Add(this.cb_ServerIP);
            this.Controls.Add(this.lb_ConnectedClinet);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.btn_StopListen);
            this.Controls.Add(this.btn_SendMsg);
            this.Controls.Add(this.btn_StartListen);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_ReceiveMsg);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_SendMsg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_ServerPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ServerForm";
            this.Text = "Socket服务器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_ServerPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_SendMsg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_ReceiveMsg;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_StartListen;
        private System.Windows.Forms.Button btn_SendMsg;
        private System.Windows.Forms.Button btn_StopListen;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.ListBox lb_ConnectedClinet;
        private System.Windows.Forms.ComboBox cb_ServerIP;
    }
}

