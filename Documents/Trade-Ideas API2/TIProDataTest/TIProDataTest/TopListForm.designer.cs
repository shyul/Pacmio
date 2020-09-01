namespace WindowsFormsApplication1
{
    partial class TopListForm
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
            this.configTextBox = new System.Windows.Forms.TextBox();
            this.messageCountLabel = new System.Windows.Forms.Label();
            this.rowCountLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.asTextTabPage = new System.Windows.Forms.TabPage();
            this.resultsTextBox = new System.Windows.Forms.TextBox();
            this.asTableTabPage = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.asTextTabPage.SuspendLayout();
            this.asTableTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Config:";
            // 
            // configTextBox
            // 
            this.configTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.configTextBox.Location = new System.Drawing.Point(15, 25);
            this.configTextBox.Name = "configTextBox";
            this.configTextBox.Size = new System.Drawing.Size(265, 20);
            this.configTextBox.TabIndex = 1;
            // 
            // messageCountLabel
            // 
            this.messageCountLabel.AutoSize = true;
            this.messageCountLabel.Location = new System.Drawing.Point(174, 61);
            this.messageCountLabel.Name = "messageCountLabel";
            this.messageCountLabel.Size = new System.Drawing.Size(81, 13);
            this.messageCountLabel.TabIndex = 13;
            this.messageCountLabel.Text = "Message Count";
            // 
            // rowCountLabel
            // 
            this.rowCountLabel.AutoSize = true;
            this.rowCountLabel.Location = new System.Drawing.Point(174, 48);
            this.rowCountLabel.Name = "rowCountLabel";
            this.rowCountLabel.Size = new System.Drawing.Size(60, 13);
            this.rowCountLabel.TabIndex = 12;
            this.rowCountLabel.Text = "Row Count";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(93, 51);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 9;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 51);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 8;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.asTextTabPage);
            this.tabControl1.Controls.Add(this.asTableTabPage);
            this.tabControl1.Location = new System.Drawing.Point(15, 81);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(265, 173);
            this.tabControl1.TabIndex = 14;
            // 
            // asTextTabPage
            // 
            this.asTextTabPage.Controls.Add(this.resultsTextBox);
            this.asTextTabPage.Location = new System.Drawing.Point(4, 22);
            this.asTextTabPage.Name = "asTextTabPage";
            this.asTextTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.asTextTabPage.Size = new System.Drawing.Size(257, 147);
            this.asTextTabPage.TabIndex = 0;
            this.asTextTabPage.Text = "As Text";
            this.asTextTabPage.UseVisualStyleBackColor = true;
            // 
            // resultsTextBox
            // 
            this.resultsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsTextBox.Location = new System.Drawing.Point(6, 9);
            this.resultsTextBox.Multiline = true;
            this.resultsTextBox.Name = "resultsTextBox";
            this.resultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.resultsTextBox.Size = new System.Drawing.Size(245, 132);
            this.resultsTextBox.TabIndex = 11;
            this.resultsTextBox.WordWrap = false;
            // 
            // asTableTabPage
            // 
            this.asTableTabPage.Controls.Add(this.dataGridView1);
            this.asTableTabPage.Location = new System.Drawing.Point(4, 22);
            this.asTableTabPage.Name = "asTableTabPage";
            this.asTableTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.asTableTabPage.Size = new System.Drawing.Size(257, 147);
            this.asTableTabPage.TabIndex = 1;
            this.asTableTabPage.Text = "As Table";
            this.asTableTabPage.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(251, 141);
            this.dataGridView1.TabIndex = 0;
            // 
            // TopListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.messageCountLabel);
            this.Controls.Add(this.rowCountLabel);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.configTextBox);
            this.Controls.Add(this.label1);
            this.Name = "TopListForm";
            this.Text = "Top List";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TopListForm_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.asTextTabPage.ResumeLayout(false);
            this.asTextTabPage.PerformLayout();
            this.asTableTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox configTextBox;
        private System.Windows.Forms.Label messageCountLabel;
        private System.Windows.Forms.Label rowCountLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage asTextTabPage;
        private System.Windows.Forms.TextBox resultsTextBox;
        private System.Windows.Forms.TabPage asTableTabPage;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}