namespace TestClient
{
    partial class ContractInfo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TextName = new System.Windows.Forms.TextBox();
            this.LbName = new System.Windows.Forms.Label();
            this.LbType = new System.Windows.Forms.Label();
            this.SelectType = new System.Windows.Forms.ComboBox();
            this.LbExchange = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.CheckUseSmart = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // TextName
            // 
            this.TextName.Location = new System.Drawing.Point(44, 5);
            this.TextName.Name = "TextName";
            this.TextName.Size = new System.Drawing.Size(150, 20);
            this.TextName.TabIndex = 0;
            // 
            // LbName
            // 
            this.LbName.AutoSize = true;
            this.LbName.Location = new System.Drawing.Point(3, 8);
            this.LbName.Name = "LbName";
            this.LbName.Size = new System.Drawing.Size(35, 13);
            this.LbName.TabIndex = 12;
            this.LbName.Text = "Name";
            // 
            // LbType
            // 
            this.LbType.AutoSize = true;
            this.LbType.Location = new System.Drawing.Point(204, 8);
            this.LbType.Name = "LbType";
            this.LbType.Size = new System.Drawing.Size(31, 13);
            this.LbType.TabIndex = 13;
            this.LbType.Text = "Type";
            // 
            // SelectType
            // 
            this.SelectType.FormattingEnabled = true;
            this.SelectType.Location = new System.Drawing.Point(240, 5);
            this.SelectType.Name = "SelectType";
            this.SelectType.Size = new System.Drawing.Size(95, 21);
            this.SelectType.TabIndex = 14;
            // 
            // LbExchange
            // 
            this.LbExchange.AutoSize = true;
            this.LbExchange.Location = new System.Drawing.Point(350, 8);
            this.LbExchange.Name = "LbExchange";
            this.LbExchange.Size = new System.Drawing.Size(55, 13);
            this.LbExchange.TabIndex = 15;
            this.LbExchange.Text = "Exchange";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(410, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(95, 21);
            this.comboBox1.TabIndex = 16;
            // 
            // CheckUseSmart
            // 
            this.CheckUseSmart.AutoSize = true;
            this.CheckUseSmart.Checked = true;
            this.CheckUseSmart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckUseSmart.Location = new System.Drawing.Point(521, 7);
            this.CheckUseSmart.Name = "CheckUseSmart";
            this.CheckUseSmart.Size = new System.Drawing.Size(72, 17);
            this.CheckUseSmart.TabIndex = 17;
            this.CheckUseSmart.Text = "UseSmart";
            this.CheckUseSmart.UseVisualStyleBackColor = true;
            // 
            // ContractInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CheckUseSmart);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.LbExchange);
            this.Controls.Add(this.SelectType);
            this.Controls.Add(this.LbType);
            this.Controls.Add(this.LbName);
            this.Controls.Add(this.TextName);
            this.MaximumSize = new System.Drawing.Size(600, 30);
            this.MinimumSize = new System.Drawing.Size(600, 30);
            this.Name = "ContractInfo";
            this.Size = new System.Drawing.Size(600, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextName;
        private System.Windows.Forms.Label LbName;
        private System.Windows.Forms.Label LbType;
        private System.Windows.Forms.ComboBox SelectType;
        private System.Windows.Forms.Label LbExchange;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox CheckUseSmart;
    }
}
