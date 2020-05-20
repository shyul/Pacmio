namespace TestClient
{
    partial class TestFreqAlign
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
            this.LbTimeUnit = new System.Windows.Forms.Label();
            this.ComboBoxTimeUnit = new System.Windows.Forms.ComboBox();
            this.DateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.BtnAlign = new System.Windows.Forms.Button();
            this.NumUpDownFreq = new System.Windows.Forms.NumericUpDown();
            this.NumAlign = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDownFreq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumAlign)).BeginInit();
            this.SuspendLayout();
            // 
            // LbTimeUnit
            // 
            this.LbTimeUnit.AutoSize = true;
            this.LbTimeUnit.Location = new System.Drawing.Point(161, 97);
            this.LbTimeUnit.Name = "LbTimeUnit";
            this.LbTimeUnit.Size = new System.Drawing.Size(52, 13);
            this.LbTimeUnit.TabIndex = 50;
            this.LbTimeUnit.Text = "Time Unit";
            // 
            // ComboBoxTimeUnit
            // 
            this.ComboBoxTimeUnit.FormattingEnabled = true;
            this.ComboBoxTimeUnit.Location = new System.Drawing.Point(219, 94);
            this.ComboBoxTimeUnit.Name = "ComboBoxTimeUnit";
            this.ComboBoxTimeUnit.Size = new System.Drawing.Size(100, 21);
            this.ComboBoxTimeUnit.TabIndex = 51;
            // 
            // DateTimePicker
            // 
            this.DateTimePicker.CustomFormat = "ddd MM/dd/yyyy HH:mm:ss";
            this.DateTimePicker.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTimePicker.Location = new System.Drawing.Point(164, 65);
            this.DateTimePicker.Name = "DateTimePicker";
            this.DateTimePicker.Size = new System.Drawing.Size(218, 23);
            this.DateTimePicker.TabIndex = 56;
            this.DateTimePicker.Value = new System.DateTime(2018, 8, 16, 0, 0, 0, 0);
            // 
            // BtnAlign
            // 
            this.BtnAlign.Location = new System.Drawing.Point(270, 135);
            this.BtnAlign.Name = "BtnAlign";
            this.BtnAlign.Size = new System.Drawing.Size(75, 23);
            this.BtnAlign.TabIndex = 62;
            this.BtnAlign.Text = "Align";
            this.BtnAlign.UseVisualStyleBackColor = true;
            this.BtnAlign.Click += new System.EventHandler(this.BtnAlign_Click);
            // 
            // NumUpDownFreq
            // 
            this.NumUpDownFreq.Location = new System.Drawing.Point(325, 94);
            this.NumUpDownFreq.Name = "NumUpDownFreq";
            this.NumUpDownFreq.Size = new System.Drawing.Size(57, 20);
            this.NumUpDownFreq.TabIndex = 63;
            this.NumUpDownFreq.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // NumAlign
            // 
            this.NumAlign.Location = new System.Drawing.Point(207, 138);
            this.NumAlign.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.NumAlign.Name = "NumAlign";
            this.NumAlign.Size = new System.Drawing.Size(57, 20);
            this.NumAlign.TabIndex = 65;
            this.NumAlign.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // TestFreqAlign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 234);
            this.Controls.Add(this.NumAlign);
            this.Controls.Add(this.NumUpDownFreq);
            this.Controls.Add(this.BtnAlign);
            this.Controls.Add(this.DateTimePicker);
            this.Controls.Add(this.LbTimeUnit);
            this.Controls.Add(this.ComboBoxTimeUnit);
            this.Name = "TestFreqAlign";
            this.Text = "TestFreqAlign";
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDownFreq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumAlign)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LbTimeUnit;
        private System.Windows.Forms.ComboBox ComboBoxTimeUnit;
        private System.Windows.Forms.DateTimePicker DateTimePicker;
        private System.Windows.Forms.Button BtnAlign;
        private System.Windows.Forms.NumericUpDown NumUpDownFreq;
        private System.Windows.Forms.NumericUpDown NumAlign;
    }
}