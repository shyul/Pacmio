namespace WindowsFormsApplication1
{
    partial class SymbolListForm
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
            this.listIdLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listNameTextBox = new System.Windows.Forms.TextBox();
            this.setListNameButton = new System.Windows.Forms.Button();
            this.symbolListTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.sendSymbolsButton = new System.Windows.Forms.Button();
            this.singleSymbolTextBox = new System.Windows.Forms.TextBox();
            this.addSingleSymbolButton = new System.Windows.Forms.Button();
            this.removeSingleSymbolButton = new System.Windows.Forms.Button();
            this.loadInfoButton = new System.Windows.Forms.Button();
            this.deleteListButton = new System.Windows.Forms.Button();
            this.requestSymbolsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listIdLabel
            // 
            this.listIdLabel.AutoSize = true;
            this.listIdLabel.Location = new System.Drawing.Point(12, 9);
            this.listIdLabel.Name = "listIdLabel";
            this.listIdLabel.Size = new System.Drawing.Size(28, 13);
            this.listIdLabel.TabIndex = 0;
            this.listIdLabel.Text = "ID #";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // listNameTextBox
            // 
            this.listNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listNameTextBox.Location = new System.Drawing.Point(53, 24);
            this.listNameTextBox.Name = "listNameTextBox";
            this.listNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.listNameTextBox.TabIndex = 2;
            // 
            // setListNameButton
            // 
            this.setListNameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setListNameButton.Location = new System.Drawing.Point(159, 22);
            this.setListNameButton.Name = "setListNameButton";
            this.setListNameButton.Size = new System.Drawing.Size(75, 23);
            this.setListNameButton.TabIndex = 3;
            this.setListNameButton.Text = "Set Name";
            this.setListNameButton.UseVisualStyleBackColor = true;
            this.setListNameButton.Click += new System.EventHandler(this.setListNameButton_Click);
            // 
            // symbolListTextBox
            // 
            this.symbolListTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.symbolListTextBox.Location = new System.Drawing.Point(15, 85);
            this.symbolListTextBox.Multiline = true;
            this.symbolListTextBox.Name = "symbolListTextBox";
            this.symbolListTextBox.Size = new System.Drawing.Size(160, 114);
            this.symbolListTextBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Symbols";
            // 
            // sendSymbolsButton
            // 
            this.sendSymbolsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sendSymbolsButton.Location = new System.Drawing.Point(181, 85);
            this.sendSymbolsButton.Name = "sendSymbolsButton";
            this.sendSymbolsButton.Size = new System.Drawing.Size(75, 23);
            this.sendSymbolsButton.TabIndex = 6;
            this.sendSymbolsButton.Text = "Send";
            this.sendSymbolsButton.UseVisualStyleBackColor = true;
            this.sendSymbolsButton.Click += new System.EventHandler(this.sendSymbolsButton_Click);
            // 
            // singleSymbolTextBox
            // 
            this.singleSymbolTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.singleSymbolTextBox.Location = new System.Drawing.Point(15, 224);
            this.singleSymbolTextBox.Name = "singleSymbolTextBox";
            this.singleSymbolTextBox.Size = new System.Drawing.Size(100, 20);
            this.singleSymbolTextBox.TabIndex = 8;
            // 
            // addSingleSymbolButton
            // 
            this.addSingleSymbolButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addSingleSymbolButton.Location = new System.Drawing.Point(121, 222);
            this.addSingleSymbolButton.Name = "addSingleSymbolButton";
            this.addSingleSymbolButton.Size = new System.Drawing.Size(54, 23);
            this.addSingleSymbolButton.TabIndex = 9;
            this.addSingleSymbolButton.Text = "Add";
            this.addSingleSymbolButton.UseVisualStyleBackColor = true;
            this.addSingleSymbolButton.Click += new System.EventHandler(this.addSingleSymbolButton_Click);
            // 
            // removeSingleSymbolButton
            // 
            this.removeSingleSymbolButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeSingleSymbolButton.Location = new System.Drawing.Point(181, 222);
            this.removeSingleSymbolButton.Name = "removeSingleSymbolButton";
            this.removeSingleSymbolButton.Size = new System.Drawing.Size(75, 23);
            this.removeSingleSymbolButton.TabIndex = 10;
            this.removeSingleSymbolButton.Text = "Remove";
            this.removeSingleSymbolButton.UseVisualStyleBackColor = true;
            this.removeSingleSymbolButton.Click += new System.EventHandler(this.removeSingleSymbolButton_Click);
            // 
            // loadInfoButton
            // 
            this.loadInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loadInfoButton.Location = new System.Drawing.Point(15, 268);
            this.loadInfoButton.Name = "loadInfoButton";
            this.loadInfoButton.Size = new System.Drawing.Size(115, 23);
            this.loadInfoButton.TabIndex = 11;
            this.loadInfoButton.Text = "Load Settings";
            this.loadInfoButton.UseVisualStyleBackColor = true;
            this.loadInfoButton.Click += new System.EventHandler(this.loadInfoButton_Click);
            // 
            // deleteListButton
            // 
            this.deleteListButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteListButton.Location = new System.Drawing.Point(141, 268);
            this.deleteListButton.Name = "deleteListButton";
            this.deleteListButton.Size = new System.Drawing.Size(115, 23);
            this.deleteListButton.TabIndex = 12;
            this.deleteListButton.Text = "Delete List";
            this.deleteListButton.UseVisualStyleBackColor = true;
            this.deleteListButton.Click += new System.EventHandler(this.deleteListButton_Click);
            // 
            // requestSymbolsButton
            // 
            this.requestSymbolsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.requestSymbolsButton.Location = new System.Drawing.Point(181, 114);
            this.requestSymbolsButton.Name = "requestSymbolsButton";
            this.requestSymbolsButton.Size = new System.Drawing.Size(75, 23);
            this.requestSymbolsButton.TabIndex = 7;
            this.requestSymbolsButton.Text = "Request";
            this.requestSymbolsButton.UseVisualStyleBackColor = true;
            this.requestSymbolsButton.Click += new System.EventHandler(this.requestSymbolsButton_Click);
            // 
            // SymbolListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 310);
            this.Controls.Add(this.requestSymbolsButton);
            this.Controls.Add(this.deleteListButton);
            this.Controls.Add(this.loadInfoButton);
            this.Controls.Add(this.removeSingleSymbolButton);
            this.Controls.Add(this.addSingleSymbolButton);
            this.Controls.Add(this.singleSymbolTextBox);
            this.Controls.Add(this.sendSymbolsButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.symbolListTextBox);
            this.Controls.Add(this.setListNameButton);
            this.Controls.Add(this.listNameTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listIdLabel);
            this.Name = "SymbolListForm";
            this.Text = "SymbolList";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label listIdLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox listNameTextBox;
        private System.Windows.Forms.Button setListNameButton;
        private System.Windows.Forms.TextBox symbolListTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button sendSymbolsButton;
        private System.Windows.Forms.TextBox singleSymbolTextBox;
        private System.Windows.Forms.Button addSingleSymbolButton;
        private System.Windows.Forms.Button removeSingleSymbolButton;
        private System.Windows.Forms.Button loadInfoButton;
        private System.Windows.Forms.Button deleteListButton;
        private System.Windows.Forms.Button requestSymbolsButton;
    }
}