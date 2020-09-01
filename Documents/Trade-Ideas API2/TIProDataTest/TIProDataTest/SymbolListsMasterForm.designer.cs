namespace WindowsFormsApplication1
{
    partial class SymbolListsMasterForm
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
            this.idNumberTextBox = new System.Windows.Forms.TextBox();
            this.createByIdButton = new System.Windows.Forms.Button();
            this.allListsListBox = new System.Windows.Forms.ListBox();
            this.requestListOfListsButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.deleteAllListsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // idNumberTextBox
            // 
            this.idNumberTextBox.Location = new System.Drawing.Point(12, 14);
            this.idNumberTextBox.Name = "idNumberTextBox";
            this.idNumberTextBox.Size = new System.Drawing.Size(51, 20);
            this.idNumberTextBox.TabIndex = 0;
            // 
            // createByIdButton
            // 
            this.createByIdButton.Location = new System.Drawing.Point(69, 12);
            this.createByIdButton.Name = "createByIdButton";
            this.createByIdButton.Size = new System.Drawing.Size(75, 23);
            this.createByIdButton.TabIndex = 1;
            this.createByIdButton.Text = "Edit by ID #";
            this.createByIdButton.UseVisualStyleBackColor = true;
            this.createByIdButton.Click += new System.EventHandler(this.createByIdButton_Click);
            // 
            // allListsListBox
            // 
            this.allListsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.allListsListBox.FormattingEnabled = true;
            this.allListsListBox.Location = new System.Drawing.Point(12, 92);
            this.allListsListBox.Name = "allListsListBox";
            this.allListsListBox.Size = new System.Drawing.Size(194, 134);
            this.allListsListBox.TabIndex = 2;
            this.allListsListBox.DoubleClick += new System.EventHandler(this.allListsListBox_DoubleClick);
            // 
            // requestListOfListsButton
            // 
            this.requestListOfListsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.requestListOfListsButton.Location = new System.Drawing.Point(12, 63);
            this.requestListOfListsButton.Name = "requestListOfListsButton";
            this.requestListOfListsButton.Size = new System.Drawing.Size(194, 23);
            this.requestListOfListsButton.TabIndex = 3;
            this.requestListOfListsButton.Text = "Request List of Lists";
            this.requestListOfListsButton.UseVisualStyleBackColor = true;
            this.requestListOfListsButton.Click += new System.EventHandler(this.requestListOfListsButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 229);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Double click on one to edit it.";
            // 
            // deleteAllListsButton
            // 
            this.deleteAllListsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteAllListsButton.Location = new System.Drawing.Point(12, 263);
            this.deleteAllListsButton.Name = "deleteAllListsButton";
            this.deleteAllListsButton.Size = new System.Drawing.Size(107, 23);
            this.deleteAllListsButton.TabIndex = 5;
            this.deleteAllListsButton.Text = "Delete All Lists";
            this.deleteAllListsButton.UseVisualStyleBackColor = true;
            this.deleteAllListsButton.Click += new System.EventHandler(this.deleteAllListsButton_Click);
            // 
            // SymbolListsMasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 311);
            this.Controls.Add(this.deleteAllListsButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.requestListOfListsButton);
            this.Controls.Add(this.allListsListBox);
            this.Controls.Add(this.createByIdButton);
            this.Controls.Add(this.idNumberTextBox);
            this.Name = "SymbolListsMasterForm";
            this.Text = "SymbolListsMasterForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SymbolListsMasterForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox idNumberTextBox;
        private System.Windows.Forms.Button createByIdButton;
        private System.Windows.Forms.ListBox allListsListBox;
        private System.Windows.Forms.Button requestListOfListsButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button deleteAllListsButton;
    }
}