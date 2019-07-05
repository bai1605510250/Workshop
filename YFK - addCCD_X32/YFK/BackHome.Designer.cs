namespace YFK
{
    partial class BackHome
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
            this.components = new System.ComponentModel.Container();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.timerBackHome = new System.Windows.Forms.Timer(this.components);
            this.timerWeldingProgress = new System.Windows.Forms.Timer(this.components);
            this.timerPriority = new System.Windows.Forms.Timer(this.components);
            this.richTextBox_backHome = new System.Windows.Forms.RichTextBox();
            this.button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(302, 354);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(108, 48);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click_1);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Cursor = System.Windows.Forms.Cursors.Default;
            this.button_Cancel.Location = new System.Drawing.Point(472, 354);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(92, 48);
            this.button_Cancel.TabIndex = 1;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click_1);
            // 
            // timerBackHome
            // 
            this.timerBackHome.Tick += new System.EventHandler(this.timerBackHome_Tick_1);
            // 
            // timerWeldingProgress
            // 
            this.timerWeldingProgress.Tick += new System.EventHandler(this.timerWeldingProgress_Tick_1);
            // 
            // timerPriority
            // 
            this.timerPriority.Tick += new System.EventHandler(this.timerPriority_Tick_1);
            // 
            // richTextBox_backHome
            // 
            this.richTextBox_backHome.Location = new System.Drawing.Point(138, 54);
            this.richTextBox_backHome.Name = "richTextBox_backHome";
            this.richTextBox_backHome.Size = new System.Drawing.Size(478, 187);
            this.richTextBox_backHome.TabIndex = 2;
            this.richTextBox_backHome.Text = "";
            // 
            // button
            // 
            this.button.Location = new System.Drawing.Point(138, 354);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(104, 48);
            this.button.TabIndex = 3;
            this.button.Text = "暂停回零";
            this.button.UseVisualStyleBackColor = true;
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // BackHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button);
            this.Controls.Add(this.richTextBox_backHome);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_ok);
            this.Name = "BackHome";
            this.Text = "BackHome";
            this.Load += new System.EventHandler(this.BackHome_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Timer timerBackHome;
        private System.Windows.Forms.Timer timerWeldingProgress;
        private System.Windows.Forms.Timer timerPriority;
        private System.Windows.Forms.RichTextBox richTextBox_backHome;
        private System.Windows.Forms.Button button;
    }
}