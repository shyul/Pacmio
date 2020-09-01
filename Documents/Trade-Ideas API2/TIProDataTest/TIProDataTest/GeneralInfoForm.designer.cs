namespace WindowsFormsApplication1
{
    /// <summary>
    /// GeneralInfoForm class  Designer
    /// </summary>
    partial class GeneralInfoForm
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
            this.requestUpdateButton = new System.Windows.Forms.Button();
            this.lastRequestLabel = new System.Windows.Forms.Label();
            this.lastReceivedabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.alertInternalCodeTextBox = new System.Windows.Forms.TextBox();
            this.getAlertInfoButton = new System.Windows.Forms.Button();
            this.alertColorLabel = new System.Windows.Forms.Label();
            this.alertDescriptionLabel = new System.Windows.Forms.Label();
            this.alertFilterLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // requestUpdateButton
            // 
            this.requestUpdateButton.Location = new System.Drawing.Point(12, 12);
            this.requestUpdateButton.Name = "requestUpdateButton";
            this.requestUpdateButton.Size = new System.Drawing.Size(107, 23);
            this.requestUpdateButton.TabIndex = 0;
            this.requestUpdateButton.Text = "Request Update";
            this.requestUpdateButton.UseVisualStyleBackColor = true;
            this.requestUpdateButton.Click += new System.EventHandler(this.requestUpdateButton_Click);
            // 
            // lastRequestLabel
            // 
            this.lastRequestLabel.AutoSize = true;
            this.lastRequestLabel.Location = new System.Drawing.Point(12, 38);
            this.lastRequestLabel.Name = "lastRequestLabel";
            this.lastRequestLabel.Size = new System.Drawing.Size(35, 13);
            this.lastRequestLabel.TabIndex = 1;
            this.lastRequestLabel.Text = "label1";
            // 
            // lastReceivedabel
            // 
            this.lastReceivedabel.AutoSize = true;
            this.lastReceivedabel.Location = new System.Drawing.Point(12, 51);
            this.lastReceivedabel.Name = "lastReceivedabel";
            this.lastReceivedabel.Size = new System.Drawing.Size(35, 13);
            this.lastReceivedabel.TabIndex = 2;
            this.lastReceivedabel.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Alert Internal Code:";
            // 
            // alertInternalCodeTextBox
            // 
            this.alertInternalCodeTextBox.Location = new System.Drawing.Point(115, 82);
            this.alertInternalCodeTextBox.Name = "alertInternalCodeTextBox";
            this.alertInternalCodeTextBox.Size = new System.Drawing.Size(100, 20);
            this.alertInternalCodeTextBox.TabIndex = 4;
            // 
            // getAlertInfoButton
            // 
            this.getAlertInfoButton.Location = new System.Drawing.Point(12, 108);
            this.getAlertInfoButton.Name = "getAlertInfoButton";
            this.getAlertInfoButton.Size = new System.Drawing.Size(142, 23);
            this.getAlertInfoButton.TabIndex = 5;
            this.getAlertInfoButton.Text = "Get Alert Info";
            this.getAlertInfoButton.UseVisualStyleBackColor = true;
            this.getAlertInfoButton.Click += new System.EventHandler(this.getAlertInfoButton_Click);
            // 
            // alertColorLabel
            // 
            this.alertColorLabel.AutoSize = true;
            this.alertColorLabel.Location = new System.Drawing.Point(12, 134);
            this.alertColorLabel.Name = "alertColorLabel";
            this.alertColorLabel.Size = new System.Drawing.Size(35, 13);
            this.alertColorLabel.TabIndex = 6;
            this.alertColorLabel.Text = "label2";
            // 
            // alertDescriptionLabel
            // 
            this.alertDescriptionLabel.AutoSize = true;
            this.alertDescriptionLabel.Location = new System.Drawing.Point(12, 147);
            this.alertDescriptionLabel.Name = "alertDescriptionLabel";
            this.alertDescriptionLabel.Size = new System.Drawing.Size(35, 13);
            this.alertDescriptionLabel.TabIndex = 7;
            this.alertDescriptionLabel.Text = "label3";
            // 
            // alertFilterLabel
            // 
            this.alertFilterLabel.AutoSize = true;
            this.alertFilterLabel.Location = new System.Drawing.Point(12, 160);
            this.alertFilterLabel.Name = "alertFilterLabel";
            this.alertFilterLabel.Size = new System.Drawing.Size(35, 13);
            this.alertFilterLabel.TabIndex = 8;
            this.alertFilterLabel.Text = "label4";
            // 
            // GeneralInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.alertFilterLabel);
            this.Controls.Add(this.alertDescriptionLabel);
            this.Controls.Add(this.alertColorLabel);
            this.Controls.Add(this.getAlertInfoButton);
            this.Controls.Add(this.alertInternalCodeTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lastReceivedabel);
            this.Controls.Add(this.lastRequestLabel);
            this.Controls.Add(this.requestUpdateButton);
            this.Name = "GeneralInfoForm";
            this.Text = "GeneralInfoForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button requestUpdateButton;
        private System.Windows.Forms.Label lastRequestLabel;
        private System.Windows.Forms.Label lastReceivedabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox alertInternalCodeTextBox;
        private System.Windows.Forms.Button getAlertInfoButton;
        private System.Windows.Forms.Label alertColorLabel;
        private System.Windows.Forms.Label alertDescriptionLabel;
        private System.Windows.Forms.Label alertFilterLabel;
    }
}