namespace socket1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnStart = new System.Windows.Forms.Button();
            this.cbBoxIP = new System.Windows.Forms.ComboBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnSendMesg = new System.Windows.Forms.Button();
            this.btnShake = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.iPPort = new System.Windows.Forms.Label();
            this.msgSend = new System.Windows.Forms.Label();
            this.form1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.form1BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(130, 15);
            this.txtServer.MaxLength = 15;
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(158, 25);
            this.txtServer.TabIndex = 0;
            this.txtServer.Text = "127.0.0.1";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(294, 15);
            this.txtPort.MaxLength = 15;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(48, 25);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "50000";
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(360, 15);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(84, 25);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "开始监听";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // cbBoxIP
            // 
            this.cbBoxIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBoxIP.FormattingEnabled = true;
            this.cbBoxIP.Location = new System.Drawing.Point(450, 15);
            this.cbBoxIP.Name = "cbBoxIP";
            this.cbBoxIP.Size = new System.Drawing.Size(121, 23);
            this.cbBoxIP.TabIndex = 4;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(24, 66);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(372, 318);
            this.txtLog.TabIndex = 5;
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(413, 66);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMsg.Size = new System.Drawing.Size(355, 318);
            this.txtMsg.TabIndex = 6;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(24, 390);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(553, 25);
            this.txtPath.TabIndex = 7;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(583, 389);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 26);
            this.btnSelect.TabIndex = 8;
            this.btnSelect.Text = "选择";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(664, 390);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(104, 26);
            this.btnSendFile.TabIndex = 9;
            this.btnSendFile.Text = "发送文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnSendMesg
            // 
            this.btnSendMesg.Location = new System.Drawing.Point(605, 39);
            this.btnSendMesg.Name = "btnSendMesg";
            this.btnSendMesg.Size = new System.Drawing.Size(82, 26);
            this.btnSendMesg.TabIndex = 10;
            this.btnSendMesg.Text = "发送消息";
            this.btnSendMesg.UseVisualStyleBackColor = true;
            this.btnSendMesg.Click += new System.EventHandler(this.btnSendMesg_Click);
            // 
            // btnShake
            // 
            this.btnShake.Location = new System.Drawing.Point(693, 37);
            this.btnShake.Name = "btnShake";
            this.btnShake.Size = new System.Drawing.Size(75, 27);
            this.btnShake.TabIndex = 11;
            this.btnShake.Text = "震动";
            this.btnShake.UseVisualStyleBackColor = true;
            this.btnShake.Click += new System.EventHandler(this.btnShake_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(36, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "日志";
            // 
            // iPPort
            // 
            this.iPPort.AutoSize = true;
            this.iPPort.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.iPPort.Location = new System.Drawing.Point(21, 18);
            this.iPPort.Name = "iPPort";
            this.iPPort.Size = new System.Drawing.Size(103, 15);
            this.iPPort.TabIndex = 13;
            this.iPPort.Text = "Self IP:Port";
            // 
            // msgSend
            // 
            this.msgSend.AutoSize = true;
            this.msgSend.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.msgSend.Location = new System.Drawing.Point(428, 50);
            this.msgSend.Name = "msgSend";
            this.msgSend.Size = new System.Drawing.Size(67, 15);
            this.msgSend.TabIndex = 14;
            this.msgSend.Text = "消息发送";
            // 
            // form1BindingSource
            // 
            this.form1BindingSource.DataSource = typeof(socket1.Form1);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(790, 433);
            this.Controls.Add(this.msgSend);
            this.Controls.Add(this.iPPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnShake);
            this.Controls.Add(this.btnSendMesg);
            this.Controls.Add(this.btnSendFile);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.cbBoxIP);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtServer);
            this.Name = "Form1";
            this.Text = "TCP Server";
            ((System.ComponentModel.ISupportInitialize)(this.form1BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cbBoxIP;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnSendMesg;
        private System.Windows.Forms.Button btnShake;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label iPPort;
        private System.Windows.Forms.Label msgSend;
        private System.Windows.Forms.BindingSource form1BindingSource;
    }
}

