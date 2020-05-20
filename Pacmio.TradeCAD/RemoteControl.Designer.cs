namespace Pacmio.TradeCAD
{
    partial class RemoteControlForm
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
            this.BtnTestChart = new System.Windows.Forms.Button();
            this.BtnTestData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnTestChart
            // 
            this.BtnTestChart.Location = new System.Drawing.Point(12, 12);
            this.BtnTestChart.Name = "BtnTestChart";
            this.BtnTestChart.Size = new System.Drawing.Size(213, 23);
            this.BtnTestChart.TabIndex = 0;
            this.BtnTestChart.Text = "Test Chart";
            this.BtnTestChart.UseVisualStyleBackColor = true;
            this.BtnTestChart.Click += new System.EventHandler(this.BtnTestChart_Click);
            // 
            // BtnTestData
            // 
            this.BtnTestData.Location = new System.Drawing.Point(12, 41);
            this.BtnTestData.Name = "BtnTestData";
            this.BtnTestData.Size = new System.Drawing.Size(213, 23);
            this.BtnTestData.TabIndex = 1;
            this.BtnTestData.Text = "Test Data";
            this.BtnTestData.UseVisualStyleBackColor = true;
            this.BtnTestData.Click += new System.EventHandler(this.BtnTestData_Click);
            // 
            // RemoteControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 354);
            this.Controls.Add(this.BtnTestData);
            this.Controls.Add(this.BtnTestChart);
            this.Name = "RemoteControlForm";
            this.Text = "Remote Control";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnTestChart;
        private System.Windows.Forms.Button BtnTestData;
    }
}