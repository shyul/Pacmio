namespace WindowsFormsApplication1
{
    /// <summary>
    /// Form1  designer
    /// </summary>
    partial class Form1
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
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.symbolListsButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.hostTextBox = new System.Windows.Forms.TextBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.configWindowButton = new System.Windows.Forms.Button();
            this.alertWindowButton = new System.Windows.Forms.Button();
            this.historyWindowButton = new System.Windows.Forms.Button();
            this.topListButton = new System.Windows.Forms.Button();
            this.topListConfigButton = new System.Windows.Forms.Button();
            this.imagesButton = new System.Windows.Forms.Button();
            this.oddsMakerButton = new System.Windows.Forms.Button();
            this.pingLabel = new System.Windows.Forms.Label();
            this.accountStatusLabel = new System.Windows.Forms.Label();
            this.oddsMakerLabel = new System.Windows.Forms.Label();
            this.nextPaymentLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.previewTimer = new System.Windows.Forms.Timer(this.components);
            this.previewTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.generalInfoButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tcpIpRadioButton = new System.Windows.Forms.RadioButton();
            this.httpRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.usernameTextBox.Location = new System.Drawing.Point(71, 12);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(172, 20);
            this.usernameTextBox.TabIndex = 1;
            this.usernameTextBox.Text = "philip";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Location = new System.Drawing.Point(71, 38);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '~';
            this.passwordTextBox.Size = new System.Drawing.Size(172, 20);
            this.passwordTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "password";
            // 
            // loginButton
            // 
            this.loginButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.loginButton.Location = new System.Drawing.Point(12, 195);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(227, 23);
            this.loginButton.TabIndex = 8;
            this.loginButton.Text = "Log In";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // symbolListsButton
            // 
            this.symbolListsButton.Location = new System.Drawing.Point(12, 318);
            this.symbolListsButton.Name = "symbolListsButton";
            this.symbolListsButton.Size = new System.Drawing.Size(75, 23);
            this.symbolListsButton.TabIndex = 9;
            this.symbolListsButton.Text = "Symbol Lists";
            this.symbolListsButton.UseVisualStyleBackColor = true;
            this.symbolListsButton.Click += new System.EventHandler(this.symbolListsButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "host";
            // 
            // hostTextBox
            // 
            this.hostTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hostTextBox.Location = new System.Drawing.Point(71, 64);
            this.hostTextBox.Name = "hostTextBox";
            this.hostTextBox.Size = new System.Drawing.Size(172, 20);
            this.hostTextBox.TabIndex = 5;
            this.hostTextBox.Text = "server.trade-ideas.com";
            // 
            // portTextBox
            // 
            this.portTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.portTextBox.Location = new System.Drawing.Point(71, 90);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(172, 20);
            this.portTextBox.TabIndex = 7;
            this.portTextBox.Text = "443";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "port";
            // 
            // configWindowButton
            // 
            this.configWindowButton.Location = new System.Drawing.Point(93, 318);
            this.configWindowButton.Name = "configWindowButton";
            this.configWindowButton.Size = new System.Drawing.Size(94, 23);
            this.configWindowButton.TabIndex = 10;
            this.configWindowButton.Text = "Config Window";
            this.configWindowButton.UseVisualStyleBackColor = true;
            this.configWindowButton.Click += new System.EventHandler(this.configWindowButton_Click);
            // 
            // alertWindowButton
            // 
            this.alertWindowButton.Location = new System.Drawing.Point(12, 289);
            this.alertWindowButton.Name = "alertWindowButton";
            this.alertWindowButton.Size = new System.Drawing.Size(80, 23);
            this.alertWindowButton.TabIndex = 11;
            this.alertWindowButton.Text = "Alert Window";
            this.alertWindowButton.UseVisualStyleBackColor = true;
            this.alertWindowButton.Click += new System.EventHandler(this.alertWindowButton_Click);
            // 
            // historyWindowButton
            // 
            this.historyWindowButton.Location = new System.Drawing.Point(98, 289);
            this.historyWindowButton.Name = "historyWindowButton";
            this.historyWindowButton.Size = new System.Drawing.Size(89, 23);
            this.historyWindowButton.TabIndex = 12;
            this.historyWindowButton.Text = "History Window";
            this.historyWindowButton.UseVisualStyleBackColor = true;
            this.historyWindowButton.Click += new System.EventHandler(this.historyWindowButton_Click);
            // 
            // topListButton
            // 
            this.topListButton.Location = new System.Drawing.Point(12, 260);
            this.topListButton.Name = "topListButton";
            this.topListButton.Size = new System.Drawing.Size(97, 23);
            this.topListButton.TabIndex = 13;
            this.topListButton.Text = "Top List Window";
            this.topListButton.UseVisualStyleBackColor = true;
            this.topListButton.Click += new System.EventHandler(this.topListButton_Click);
            // 
            // topListConfigButton
            // 
            this.topListConfigButton.Location = new System.Drawing.Point(115, 260);
            this.topListConfigButton.Name = "topListConfigButton";
            this.topListConfigButton.Size = new System.Drawing.Size(86, 23);
            this.topListConfigButton.TabIndex = 14;
            this.topListConfigButton.Text = "Top List Config";
            this.topListConfigButton.UseVisualStyleBackColor = true;
            this.topListConfigButton.Click += new System.EventHandler(this.topListConfigButton_Click);
            // 
            // imagesButton
            // 
            this.imagesButton.Location = new System.Drawing.Point(193, 318);
            this.imagesButton.Name = "imagesButton";
            this.imagesButton.Size = new System.Drawing.Size(52, 23);
            this.imagesButton.TabIndex = 15;
            this.imagesButton.Text = "Images";
            this.imagesButton.UseVisualStyleBackColor = true;
            this.imagesButton.Click += new System.EventHandler(this.imagesButton_Click);
            // 
            // oddsMakerButton
            // 
            this.oddsMakerButton.Location = new System.Drawing.Point(12, 347);
            this.oddsMakerButton.Name = "oddsMakerButton";
            this.oddsMakerButton.Size = new System.Drawing.Size(80, 23);
            this.oddsMakerButton.TabIndex = 16;
            this.oddsMakerButton.Text = "Odds Maker";
            this.oddsMakerButton.UseVisualStyleBackColor = true;
            this.oddsMakerButton.Click += new System.EventHandler(this.oddsMakerButton_Click);
            // 
            // pingLabel
            // 
            this.pingLabel.AutoSize = true;
            this.pingLabel.Location = new System.Drawing.Point(13, 236);
            this.pingLabel.Name = "pingLabel";
            this.pingLabel.Size = new System.Drawing.Size(31, 13);
            this.pingLabel.TabIndex = 17;
            this.pingLabel.Text = "Ping:";
            // 
            // accountStatusLabel
            // 
            this.accountStatusLabel.AutoSize = true;
            this.accountStatusLabel.Location = new System.Drawing.Point(13, 382);
            this.accountStatusLabel.Name = "accountStatusLabel";
            this.accountStatusLabel.Size = new System.Drawing.Size(83, 13);
            this.accountStatusLabel.TabIndex = 18;
            this.accountStatusLabel.Text = "Account Status:";
            // 
            // oddsMakerLabel
            // 
            this.oddsMakerLabel.AutoSize = true;
            this.oddsMakerLabel.Location = new System.Drawing.Point(13, 398);
            this.oddsMakerLabel.Name = "oddsMakerLabel";
            this.oddsMakerLabel.Size = new System.Drawing.Size(118, 13);
            this.oddsMakerLabel.TabIndex = 19;
            this.oddsMakerLabel.Text = "OddsMaker Remaining:";
            // 
            // nextPaymentLabel
            // 
            this.nextPaymentLabel.AutoSize = true;
            this.nextPaymentLabel.Location = new System.Drawing.Point(13, 414);
            this.nextPaymentLabel.Name = "nextPaymentLabel";
            this.nextPaymentLabel.Size = new System.Drawing.Size(76, 13);
            this.nextPaymentLabel.TabIndex = 20;
            this.nextPaymentLabel.Text = "Next Payment:";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(13, 430);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(40, 13);
            this.statusLabel.TabIndex = 22;
            this.statusLabel.Text = "Status:";
            // 
            // previewTimer
            // 
            this.previewTimer.Enabled = true;
            this.previewTimer.Interval = 200;
            this.previewTimer.Tick += new System.EventHandler(this.previewTimer_Tick);
            // 
            // previewTextBox
            // 
            this.previewTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.previewTextBox.Location = new System.Drawing.Point(9, 469);
            this.previewTextBox.Multiline = true;
            this.previewTextBox.Name = "previewTextBox";
            this.previewTextBox.Size = new System.Drawing.Size(236, 58);
            this.previewTextBox.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(68, 453);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Message Preview:";
            // 
            // generalInfoButton
            // 
            this.generalInfoButton.Location = new System.Drawing.Point(98, 347);
            this.generalInfoButton.Name = "generalInfoButton";
            this.generalInfoButton.Size = new System.Drawing.Size(75, 23);
            this.generalInfoButton.TabIndex = 25;
            this.generalInfoButton.Text = "General Info";
            this.generalInfoButton.UseVisualStyleBackColor = true;
            this.generalInfoButton.Click += new System.EventHandler(this.generalInfoButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "url";
            // 
            // urlTextBox
            // 
            this.urlTextBox.Location = new System.Drawing.Point(71, 116);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(171, 20);
            this.urlTextBox.TabIndex = 27;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.httpRadioButton);
            this.groupBox1.Controls.Add(this.tcpIpRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 142);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 47);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection Type";
            // 
            // tcpIpRadioButton
            // 
            this.tcpIpRadioButton.AutoSize = true;
            this.tcpIpRadioButton.Checked = true;
            this.tcpIpRadioButton.Location = new System.Drawing.Point(6, 19);
            this.tcpIpRadioButton.Name = "tcpIpRadioButton";
            this.tcpIpRadioButton.Size = new System.Drawing.Size(61, 17);
            this.tcpIpRadioButton.TabIndex = 0;
            this.tcpIpRadioButton.TabStop = true;
            this.tcpIpRadioButton.Text = "TCP/IP";
            this.tcpIpRadioButton.UseVisualStyleBackColor = true;
            // 
            // httpRadioButton
            // 
            this.httpRadioButton.AutoSize = true;
            this.httpRadioButton.Location = new System.Drawing.Point(73, 19);
            this.httpRadioButton.Name = "httpRadioButton";
            this.httpRadioButton.Size = new System.Drawing.Size(54, 17);
            this.httpRadioButton.TabIndex = 1;
            this.httpRadioButton.Text = "HTTP";
            this.httpRadioButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AcceptButton = this.loginButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 535);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.urlTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.generalInfoButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.previewTextBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.nextPaymentLabel);
            this.Controls.Add(this.oddsMakerLabel);
            this.Controls.Add(this.accountStatusLabel);
            this.Controls.Add(this.pingLabel);
            this.Controls.Add(this.oddsMakerButton);
            this.Controls.Add(this.imagesButton);
            this.Controls.Add(this.topListConfigButton);
            this.Controls.Add(this.topListButton);
            this.Controls.Add(this.historyWindowButton);
            this.Controls.Add(this.alertWindowButton);
            this.Controls.Add(this.configWindowButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.hostTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.symbolListsButton);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.usernameTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button symbolListsButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox hostTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button configWindowButton;
        private System.Windows.Forms.Button alertWindowButton;
        private System.Windows.Forms.Button historyWindowButton;
        private System.Windows.Forms.Button topListButton;
        private System.Windows.Forms.Button topListConfigButton;
        private System.Windows.Forms.Button imagesButton;
        private System.Windows.Forms.Button oddsMakerButton;
        private System.Windows.Forms.Label pingLabel;
        private System.Windows.Forms.Label accountStatusLabel;
        private System.Windows.Forms.Label oddsMakerLabel;
        private System.Windows.Forms.Label nextPaymentLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Timer previewTimer;
        private System.Windows.Forms.TextBox previewTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button generalInfoButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton httpRadioButton;
        private System.Windows.Forms.RadioButton tcpIpRadioButton;
    }
}

