namespace TestClient
{
    partial class TestMultiPeriodDataSource
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
            this.GridView = new System.Windows.Forms.DataGridView();
            this.LbHistDataEnd = new System.Windows.Forms.Label();
            this.LbHistDataStart = new System.Windows.Forms.Label();
            this.DateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.DateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.LbDataSource = new System.Windows.Forms.Label();
            this.ComboBoxDataSource = new System.Windows.Forms.ComboBox();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.BtnRemove = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnRefresh = new System.Windows.Forms.Button();
            this.DateTimePickerGetStart = new System.Windows.Forms.DateTimePicker();
            this.BtnGetStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            this.SuspendLayout();
            // 
            // GridView
            // 
            this.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridView.Location = new System.Drawing.Point(12, 12);
            this.GridView.Name = "GridView";
            this.GridView.Size = new System.Drawing.Size(776, 603);
            this.GridView.TabIndex = 0;
            // 
            // LbHistDataEnd
            // 
            this.LbHistDataEnd.AutoSize = true;
            this.LbHistDataEnd.Location = new System.Drawing.Point(36, 668);
            this.LbHistDataEnd.Name = "LbHistDataEnd";
            this.LbHistDataEnd.Size = new System.Drawing.Size(26, 13);
            this.LbHistDataEnd.TabIndex = 58;
            this.LbHistDataEnd.Text = "End";
            // 
            // LbHistDataStart
            // 
            this.LbHistDataStart.AutoSize = true;
            this.LbHistDataStart.Location = new System.Drawing.Point(32, 640);
            this.LbHistDataStart.Name = "LbHistDataStart";
            this.LbHistDataStart.Size = new System.Drawing.Size(29, 13);
            this.LbHistDataStart.TabIndex = 57;
            this.LbHistDataStart.Text = "Start";
            // 
            // DateTimePickerEnd
            // 
            this.DateTimePickerEnd.Location = new System.Drawing.Point(69, 663);
            this.DateTimePickerEnd.Name = "DateTimePickerEnd";
            this.DateTimePickerEnd.Size = new System.Drawing.Size(208, 20);
            this.DateTimePickerEnd.TabIndex = 55;
            this.DateTimePickerEnd.Value = new System.DateTime(2018, 8, 16, 0, 0, 0, 0);
            // 
            // DateTimePickerStart
            // 
            this.DateTimePickerStart.Location = new System.Drawing.Point(69, 635);
            this.DateTimePickerStart.Name = "DateTimePickerStart";
            this.DateTimePickerStart.Size = new System.Drawing.Size(208, 20);
            this.DateTimePickerStart.TabIndex = 56;
            this.DateTimePickerStart.Value = new System.DateTime(2018, 8, 16, 0, 0, 0, 0);
            // 
            // LbDataSource
            // 
            this.LbDataSource.AutoSize = true;
            this.LbDataSource.Location = new System.Drawing.Point(45, 703);
            this.LbDataSource.Name = "LbDataSource";
            this.LbDataSource.Size = new System.Drawing.Size(67, 13);
            this.LbDataSource.TabIndex = 59;
            this.LbDataSource.Text = "Data Source";
            // 
            // ComboBoxDataSource
            // 
            this.ComboBoxDataSource.FormattingEnabled = true;
            this.ComboBoxDataSource.Location = new System.Drawing.Point(118, 700);
            this.ComboBoxDataSource.Name = "ComboBoxDataSource";
            this.ComboBoxDataSource.Size = new System.Drawing.Size(159, 21);
            this.ComboBoxDataSource.TabIndex = 60;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(333, 668);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 23);
            this.BtnAdd.TabIndex = 61;
            this.BtnAdd.Text = "Add";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnRemove
            // 
            this.BtnRemove.Location = new System.Drawing.Point(429, 668);
            this.BtnRemove.Name = "BtnRemove";
            this.BtnRemove.Size = new System.Drawing.Size(75, 23);
            this.BtnRemove.TabIndex = 62;
            this.BtnRemove.Text = "Remove";
            this.BtnRemove.UseVisualStyleBackColor = true;
            this.BtnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(523, 668);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(75, 23);
            this.BtnClear.TabIndex = 63;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnRefresh
            // 
            this.BtnRefresh.Location = new System.Drawing.Point(617, 668);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(75, 23);
            this.BtnRefresh.TabIndex = 64;
            this.BtnRefresh.Text = "Refresh";
            this.BtnRefresh.UseVisualStyleBackColor = true;
            this.BtnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // DateTimePickerGetStart
            // 
            this.DateTimePickerGetStart.Location = new System.Drawing.Point(333, 731);
            this.DateTimePickerGetStart.Name = "DateTimePickerGetStart";
            this.DateTimePickerGetStart.Size = new System.Drawing.Size(208, 20);
            this.DateTimePickerGetStart.TabIndex = 65;
            this.DateTimePickerGetStart.Value = new System.DateTime(2018, 8, 16, 0, 0, 0, 0);
            // 
            // BtnGetStart
            // 
            this.BtnGetStart.Location = new System.Drawing.Point(559, 731);
            this.BtnGetStart.Name = "BtnGetStart";
            this.BtnGetStart.Size = new System.Drawing.Size(75, 23);
            this.BtnGetStart.TabIndex = 66;
            this.BtnGetStart.Text = "Get Start";
            this.BtnGetStart.UseVisualStyleBackColor = true;
            this.BtnGetStart.Click += new System.EventHandler(this.BtnGetStart_Click);
            // 
            // TestMultiPeriodDataSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 799);
            this.Controls.Add(this.BtnGetStart);
            this.Controls.Add(this.DateTimePickerGetStart);
            this.Controls.Add(this.BtnRefresh);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.BtnRemove);
            this.Controls.Add(this.BtnAdd);
            this.Controls.Add(this.LbDataSource);
            this.Controls.Add(this.ComboBoxDataSource);
            this.Controls.Add(this.LbHistDataEnd);
            this.Controls.Add(this.LbHistDataStart);
            this.Controls.Add(this.DateTimePickerEnd);
            this.Controls.Add(this.DateTimePickerStart);
            this.Controls.Add(this.GridView);
            this.Name = "TestMultiPeriodDataSource";
            this.Text = "Test";
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView GridView;
        private System.Windows.Forms.Label LbHistDataEnd;
        private System.Windows.Forms.Label LbHistDataStart;
        private System.Windows.Forms.DateTimePicker DateTimePickerEnd;
        private System.Windows.Forms.DateTimePicker DateTimePickerStart;
        private System.Windows.Forms.Label LbDataSource;
        private System.Windows.Forms.ComboBox ComboBoxDataSource;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Button BtnRemove;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button BtnRefresh;
        private System.Windows.Forms.DateTimePicker DateTimePickerGetStart;
        private System.Windows.Forms.Button BtnGetStart;
    }
}