namespace WindowsFormsApplication1
{
    partial class ImageTest
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gifChartButton = new System.Windows.Forms.Button();
            this.textureButton = new System.Windows.Forms.Button();
            this.filterButton = new System.Windows.Forms.Button();
            this.alertButton = new System.Windows.Forms.Button();
            this.additionsTextBox = new System.Windows.Forms.TextBox();
            this.imagePanel = new System.Windows.Forms.Panel();
            this.preloadCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.preloadCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.gifChartButton);
            this.splitContainer1.Panel1.Controls.Add(this.textureButton);
            this.splitContainer1.Panel1.Controls.Add(this.filterButton);
            this.splitContainer1.Panel1.Controls.Add(this.alertButton);
            this.splitContainer1.Panel1.Controls.Add(this.additionsTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.imagePanel);
            this.splitContainer1.Size = new System.Drawing.Size(368, 311);
            this.splitContainer1.SplitterDistance = 155;
            this.splitContainer1.TabIndex = 2;
            // 
            // gifChartButton
            // 
            this.gifChartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gifChartButton.Location = new System.Drawing.Point(290, 90);
            this.gifChartButton.Name = "gifChartButton";
            this.gifChartButton.Size = new System.Drawing.Size(75, 23);
            this.gifChartButton.TabIndex = 4;
            this.gifChartButton.Text = "GIF Chart";
            this.gifChartButton.UseVisualStyleBackColor = true;
            // 
            // textureButton
            // 
            this.textureButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textureButton.Location = new System.Drawing.Point(290, 61);
            this.textureButton.Name = "textureButton";
            this.textureButton.Size = new System.Drawing.Size(75, 23);
            this.textureButton.TabIndex = 3;
            this.textureButton.Text = "Texture";
            this.textureButton.UseVisualStyleBackColor = true;
            this.textureButton.Click += new System.EventHandler(this.textureButton_Click);
            // 
            // filterButton
            // 
            this.filterButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filterButton.Location = new System.Drawing.Point(290, 32);
            this.filterButton.Name = "filterButton";
            this.filterButton.Size = new System.Drawing.Size(75, 23);
            this.filterButton.TabIndex = 2;
            this.filterButton.Text = "Filter";
            this.filterButton.UseVisualStyleBackColor = true;
            this.filterButton.Click += new System.EventHandler(this.filterButton_Click);
            // 
            // alertButton
            // 
            this.alertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.alertButton.Location = new System.Drawing.Point(290, 3);
            this.alertButton.Name = "alertButton";
            this.alertButton.Size = new System.Drawing.Size(75, 23);
            this.alertButton.TabIndex = 1;
            this.alertButton.Text = "Alert";
            this.alertButton.UseVisualStyleBackColor = true;
            this.alertButton.Click += new System.EventHandler(this.alertButton_Click);
            // 
            // additionsTextBox
            // 
            this.additionsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.additionsTextBox.Location = new System.Drawing.Point(3, 3);
            this.additionsTextBox.Multiline = true;
            this.additionsTextBox.Name = "additionsTextBox";
            this.additionsTextBox.Size = new System.Drawing.Size(281, 149);
            this.additionsTextBox.TabIndex = 0;
            // 
            // imagePanel
            // 
            this.imagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.imagePanel.AutoScroll = true;
            this.imagePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imagePanel.Location = new System.Drawing.Point(3, 3);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Size = new System.Drawing.Size(362, 146);
            this.imagePanel.TabIndex = 2;
            // 
            // preloadCheckBox
            // 
            this.preloadCheckBox.AutoSize = true;
            this.preloadCheckBox.Location = new System.Drawing.Point(290, 119);
            this.preloadCheckBox.Name = "preloadCheckBox";
            this.preloadCheckBox.Size = new System.Drawing.Size(62, 17);
            this.preloadCheckBox.TabIndex = 5;
            this.preloadCheckBox.Text = "Preload";
            this.preloadCheckBox.UseVisualStyleBackColor = true;
            // 
            // ImageTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 335);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ImageTest";
            this.Text = "ImageTest";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImageTest_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button gifChartButton;
        private System.Windows.Forms.Button textureButton;
        private System.Windows.Forms.Button filterButton;
        private System.Windows.Forms.Button alertButton;
        private System.Windows.Forms.TextBox additionsTextBox;
        private System.Windows.Forms.Panel imagePanel;
        private System.Windows.Forms.CheckBox preloadCheckBox;


    }
}