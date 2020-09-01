namespace WindowsFormsApplication1
{
    partial class OddsMakerForm
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
            this.statusLabel = new System.Windows.Forms.Label();
            this.debugResultTextBox = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.entryConditionTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.exitConditionAlertTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.plusRadioButton = new System.Windows.Forms.RadioButton();
            this.MinusRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PercentRadioButton = new System.Windows.Forms.RadioButton();
            this.dollarRadioButton = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.SuccessValueTextBox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.wiggleCheckBox = new System.Windows.Forms.CheckBox();
            this.stopLossTextBox = new System.Windows.Forms.TextBox();
            this.stopLossCheckBox = new System.Windows.Forms.CheckBox();
            this.profitTargetTextBox = new System.Windows.Forms.TextBox();
            this.profitTargetCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.exitConditionTrailingStopTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.noExitRadioButton = new System.Windows.Forms.RadioButton();
            this.AlertExitRadioButton = new System.Windows.Forms.RadioButton();
            this.percentTrailingRadioButton = new System.Windows.Forms.RadioButton();
            this.barsTrailingRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.atCloseDaysTextBox = new System.Windows.Forms.TextBox();
            this.atOpenDaysTextBox = new System.Windows.Forms.TextBox();
            this.beforeCloseMinutesTextBox = new System.Windows.Forms.TextBox();
            this.timeoutMinutesTextBox = new System.Windows.Forms.TextBox();
            this.atOpenDaysRadioButton = new System.Windows.Forms.RadioButton();
            this.afterCloseDaysRadioButton = new System.Windows.Forms.RadioButton();
            this.minutesAfterEntryRadioButton = new System.Windows.Forms.RadioButton();
            this.minutesBeforeCloseRadioButton = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.xmlCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.testTextBox = new System.Windows.Forms.TextBox();
            this.skipTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.entryTimeStopMinutesTextBox = new System.Windows.Forms.TextBox();
            this.entryTimeStartMinutesTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.debugModeCheckBox = new System.Windows.Forms.CheckBox();
            this.locationComboBox = new System.Windows.Forms.ComboBox();
            this.progressRichTextBox = new System.Windows.Forms.RichTextBox();
            this.getCsvFileCheckBox = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.progressTabPage = new System.Windows.Forms.TabPage();
            this.debugTabPage = new System.Windows.Forms.TabPage();
            this.csvTabPage = new System.Windows.Forms.TabPage();
            this.csvTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.progressTabPage.SuspendLayout();
            this.debugTabPage.SuspendLayout();
            this.csvTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(210, 704);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(69, 13);
            this.statusLabel.TabIndex = 29;
            this.statusLabel.Text = "No alerts yet.";
            // 
            // debugResultTextBox
            // 
            this.debugResultTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugResultTextBox.Location = new System.Drawing.Point(3, 3);
            this.debugResultTextBox.Multiline = true;
            this.debugResultTextBox.Name = "debugResultTextBox";
            this.debugResultTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.debugResultTextBox.Size = new System.Drawing.Size(186, 68);
            this.debugResultTextBox.TabIndex = 24;
            this.debugResultTextBox.WordWrap = false;
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(657, 195);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 22;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(576, 195);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 21;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // entryConditionTextBox
            // 
            this.entryConditionTextBox.Location = new System.Drawing.Point(15, 25);
            this.entryConditionTextBox.Name = "entryConditionTextBox";
            this.entryConditionTextBox.Size = new System.Drawing.Size(228, 20);
            this.entryConditionTextBox.TabIndex = 19;
            this.entryConditionTextBox.Text = "O=300000000000030_1D_0&WN=High/Low+Ticker&SL=X1o5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Entry Condition:";
            // 
            // exitConditionAlertTextBox
            // 
            this.exitConditionAlertTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.exitConditionAlertTextBox.Location = new System.Drawing.Point(40, 119);
            this.exitConditionAlertTextBox.Name = "exitConditionAlertTextBox";
            this.exitConditionAlertTextBox.Size = new System.Drawing.Size(142, 20);
            this.exitConditionAlertTextBox.TabIndex = 31;
            this.exitConditionAlertTextBox.Text = "O=3C3C3800000000000000000000000000000000000000_3D_0&MaxSpread=10&MaxUp10=-3&MinFC" +
                "D=0.05&MinVWV=0.05&MinVol=200000&WN=Bullish+Candlestick+Patterns+(Hammers,+Pierc" +
                "ing+and+Engulfing+Patterns)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Exit Alert Value:";
            // 
            // plusRadioButton
            // 
            this.plusRadioButton.AutoSize = true;
            this.plusRadioButton.Checked = true;
            this.plusRadioButton.Location = new System.Drawing.Point(17, 19);
            this.plusRadioButton.Name = "plusRadioButton";
            this.plusRadioButton.Size = new System.Drawing.Size(31, 17);
            this.plusRadioButton.TabIndex = 32;
            this.plusRadioButton.TabStop = true;
            this.plusRadioButton.Text = "+";
            this.plusRadioButton.UseVisualStyleBackColor = true;
            // 
            // MinusRadioButton
            // 
            this.MinusRadioButton.AutoSize = true;
            this.MinusRadioButton.Location = new System.Drawing.Point(17, 35);
            this.MinusRadioButton.Name = "MinusRadioButton";
            this.MinusRadioButton.Size = new System.Drawing.Size(28, 17);
            this.MinusRadioButton.TabIndex = 33;
            this.MinusRadioButton.Text = "-";
            this.MinusRadioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.plusRadioButton);
            this.groupBox1.Controls.Add(this.MinusRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(15, 94);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(116, 61);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Success Direction";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.PercentRadioButton);
            this.groupBox2.Controls.Add(this.dollarRadioButton);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.SuccessValueTextBox);
            this.groupBox2.Location = new System.Drawing.Point(137, 94);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(150, 61);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Success Type";
            // 
            // PercentRadioButton
            // 
            this.PercentRadioButton.AutoSize = true;
            this.PercentRadioButton.Checked = true;
            this.PercentRadioButton.Location = new System.Drawing.Point(17, 19);
            this.PercentRadioButton.Name = "PercentRadioButton";
            this.PercentRadioButton.Size = new System.Drawing.Size(33, 17);
            this.PercentRadioButton.TabIndex = 32;
            this.PercentRadioButton.TabStop = true;
            this.PercentRadioButton.Text = "%";
            this.PercentRadioButton.UseVisualStyleBackColor = true;
            // 
            // dollarRadioButton
            // 
            this.dollarRadioButton.AutoSize = true;
            this.dollarRadioButton.Location = new System.Drawing.Point(17, 35);
            this.dollarRadioButton.Name = "dollarRadioButton";
            this.dollarRadioButton.Size = new System.Drawing.Size(31, 17);
            this.dollarRadioButton.TabIndex = 33;
            this.dollarRadioButton.Text = "$";
            this.dollarRadioButton.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(64, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 36;
            this.label6.Text = "Success Value:";
            // 
            // SuccessValueTextBox
            // 
            this.SuccessValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SuccessValueTextBox.Location = new System.Drawing.Point(67, 35);
            this.SuccessValueTextBox.Name = "SuccessValueTextBox";
            this.SuccessValueTextBox.Size = new System.Drawing.Size(75, 20);
            this.SuccessValueTextBox.TabIndex = 37;
            this.SuccessValueTextBox.Text = "1.0";
            this.SuccessValueTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.wiggleCheckBox);
            this.groupBox3.Controls.Add(this.stopLossTextBox);
            this.groupBox3.Controls.Add(this.stopLossCheckBox);
            this.groupBox3.Controls.Add(this.profitTargetTextBox);
            this.groupBox3.Controls.Add(this.profitTargetCheckBox);
            this.groupBox3.Location = new System.Drawing.Point(362, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(175, 89);
            this.groupBox3.TabIndex = 38;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Profit Target / Stop Loss";
            // 
            // wiggleCheckBox
            // 
            this.wiggleCheckBox.AutoSize = true;
            this.wiggleCheckBox.Location = new System.Drawing.Point(17, 64);
            this.wiggleCheckBox.Name = "wiggleCheckBox";
            this.wiggleCheckBox.Size = new System.Drawing.Size(109, 17);
            this.wiggleCheckBox.TabIndex = 4;
            this.wiggleCheckBox.Text = "Stop Loss Wiggle";
            this.wiggleCheckBox.UseVisualStyleBackColor = true;
            // 
            // stopLossTextBox
            // 
            this.stopLossTextBox.Location = new System.Drawing.Point(107, 39);
            this.stopLossTextBox.Name = "stopLossTextBox";
            this.stopLossTextBox.Size = new System.Drawing.Size(48, 20);
            this.stopLossTextBox.TabIndex = 3;
            this.stopLossTextBox.Text = "1";
            this.stopLossTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // stopLossCheckBox
            // 
            this.stopLossCheckBox.AutoSize = true;
            this.stopLossCheckBox.Location = new System.Drawing.Point(17, 41);
            this.stopLossCheckBox.Name = "stopLossCheckBox";
            this.stopLossCheckBox.Size = new System.Drawing.Size(73, 17);
            this.stopLossCheckBox.TabIndex = 2;
            this.stopLossCheckBox.Text = "Stop Loss";
            this.stopLossCheckBox.UseVisualStyleBackColor = true;
            // 
            // profitTargetTextBox
            // 
            this.profitTargetTextBox.Location = new System.Drawing.Point(107, 16);
            this.profitTargetTextBox.Name = "profitTargetTextBox";
            this.profitTargetTextBox.Size = new System.Drawing.Size(48, 20);
            this.profitTargetTextBox.TabIndex = 1;
            this.profitTargetTextBox.Text = "1";
            this.profitTargetTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // profitTargetCheckBox
            // 
            this.profitTargetCheckBox.AutoSize = true;
            this.profitTargetCheckBox.Location = new System.Drawing.Point(17, 18);
            this.profitTargetCheckBox.Name = "profitTargetCheckBox";
            this.profitTargetCheckBox.Size = new System.Drawing.Size(84, 17);
            this.profitTargetCheckBox.TabIndex = 0;
            this.profitTargetCheckBox.Text = "Profit Target";
            this.profitTargetCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.exitConditionTrailingStopTextBox);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.noExitRadioButton);
            this.groupBox4.Controls.Add(this.AlertExitRadioButton);
            this.groupBox4.Controls.Add(this.percentTrailingRadioButton);
            this.groupBox4.Controls.Add(this.barsTrailingRadioButton);
            this.groupBox4.Controls.Add(this.exitConditionAlertTextBox);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(550, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 177);
            this.groupBox4.TabIndex = 39;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Other Exit Condition";
            // 
            // exitConditionTrailingStopTextBox
            // 
            this.exitConditionTrailingStopTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.exitConditionTrailingStopTextBox.Location = new System.Drawing.Point(139, 62);
            this.exitConditionTrailingStopTextBox.Name = "exitConditionTrailingStopTextBox";
            this.exitConditionTrailingStopTextBox.Size = new System.Drawing.Size(35, 20);
            this.exitConditionTrailingStopTextBox.TabIndex = 39;
            this.exitConditionTrailingStopTextBox.Text = "0.5";
            this.exitConditionTrailingStopTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(37, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Trailing Stop Value";
            // 
            // noExitRadioButton
            // 
            this.noExitRadioButton.AutoSize = true;
            this.noExitRadioButton.Location = new System.Drawing.Point(17, 144);
            this.noExitRadioButton.Name = "noExitRadioButton";
            this.noExitRadioButton.Size = new System.Drawing.Size(51, 17);
            this.noExitRadioButton.TabIndex = 37;
            this.noExitRadioButton.Text = "None";
            this.noExitRadioButton.UseVisualStyleBackColor = true;
            // 
            // AlertExitRadioButton
            // 
            this.AlertExitRadioButton.AutoSize = true;
            this.AlertExitRadioButton.Location = new System.Drawing.Point(17, 82);
            this.AlertExitRadioButton.Name = "AlertExitRadioButton";
            this.AlertExitRadioButton.Size = new System.Drawing.Size(86, 17);
            this.AlertExitRadioButton.TabIndex = 36;
            this.AlertExitRadioButton.Text = "Another Alert";
            this.AlertExitRadioButton.UseVisualStyleBackColor = true;
            // 
            // percentTrailingRadioButton
            // 
            this.percentTrailingRadioButton.AutoSize = true;
            this.percentTrailingRadioButton.Checked = true;
            this.percentTrailingRadioButton.Location = new System.Drawing.Point(17, 19);
            this.percentTrailingRadioButton.Name = "percentTrailingRadioButton";
            this.percentTrailingRadioButton.Size = new System.Drawing.Size(124, 17);
            this.percentTrailingRadioButton.TabIndex = 34;
            this.percentTrailingRadioButton.TabStop = true;
            this.percentTrailingRadioButton.Text = "Percent Trailing Stop";
            this.percentTrailingRadioButton.UseVisualStyleBackColor = true;
            // 
            // barsTrailingRadioButton
            // 
            this.barsTrailingRadioButton.AutoSize = true;
            this.barsTrailingRadioButton.Location = new System.Drawing.Point(17, 42);
            this.barsTrailingRadioButton.Name = "barsTrailingRadioButton";
            this.barsTrailingRadioButton.Size = new System.Drawing.Size(108, 17);
            this.barsTrailingRadioButton.TabIndex = 35;
            this.barsTrailingRadioButton.Text = "Bars Trailing Stop";
            this.barsTrailingRadioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.atCloseDaysTextBox);
            this.groupBox5.Controls.Add(this.atOpenDaysTextBox);
            this.groupBox5.Controls.Add(this.beforeCloseMinutesTextBox);
            this.groupBox5.Controls.Add(this.timeoutMinutesTextBox);
            this.groupBox5.Controls.Add(this.atOpenDaysRadioButton);
            this.groupBox5.Controls.Add(this.afterCloseDaysRadioButton);
            this.groupBox5.Controls.Add(this.minutesAfterEntryRadioButton);
            this.groupBox5.Controls.Add(this.minutesBeforeCloseRadioButton);
            this.groupBox5.Location = new System.Drawing.Point(337, 104);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 118);
            this.groupBox5.TabIndex = 40;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Timeout";
            // 
            // atCloseDaysTextBox
            // 
            this.atCloseDaysTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.atCloseDaysTextBox.Location = new System.Drawing.Point(17, 88);
            this.atCloseDaysTextBox.Name = "atCloseDaysTextBox";
            this.atCloseDaysTextBox.Size = new System.Drawing.Size(31, 20);
            this.atCloseDaysTextBox.TabIndex = 41;
            this.atCloseDaysTextBox.Text = "1";
            this.atCloseDaysTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // atOpenDaysTextBox
            // 
            this.atOpenDaysTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.atOpenDaysTextBox.Location = new System.Drawing.Point(17, 67);
            this.atOpenDaysTextBox.Name = "atOpenDaysTextBox";
            this.atOpenDaysTextBox.Size = new System.Drawing.Size(31, 20);
            this.atOpenDaysTextBox.TabIndex = 40;
            this.atOpenDaysTextBox.Text = "1";
            this.atOpenDaysTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // beforeCloseMinutesTextBox
            // 
            this.beforeCloseMinutesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.beforeCloseMinutesTextBox.Location = new System.Drawing.Point(17, 47);
            this.beforeCloseMinutesTextBox.Name = "beforeCloseMinutesTextBox";
            this.beforeCloseMinutesTextBox.Size = new System.Drawing.Size(31, 20);
            this.beforeCloseMinutesTextBox.TabIndex = 39;
            this.beforeCloseMinutesTextBox.Text = "0";
            this.beforeCloseMinutesTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // timeoutMinutesTextBox
            // 
            this.timeoutMinutesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.timeoutMinutesTextBox.Location = new System.Drawing.Point(17, 28);
            this.timeoutMinutesTextBox.Name = "timeoutMinutesTextBox";
            this.timeoutMinutesTextBox.Size = new System.Drawing.Size(31, 20);
            this.timeoutMinutesTextBox.TabIndex = 38;
            this.timeoutMinutesTextBox.Text = "120";
            this.timeoutMinutesTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // atOpenDaysRadioButton
            // 
            this.atOpenDaysRadioButton.AutoSize = true;
            this.atOpenDaysRadioButton.Location = new System.Drawing.Point(54, 68);
            this.atOpenDaysRadioButton.Name = "atOpenDaysRadioButton";
            this.atOpenDaysRadioButton.Size = new System.Drawing.Size(121, 17);
            this.atOpenDaysRadioButton.TabIndex = 36;
            this.atOpenDaysRadioButton.Text = "At open after X days";
            this.atOpenDaysRadioButton.UseVisualStyleBackColor = true;
            // 
            // afterCloseDaysRadioButton
            // 
            this.afterCloseDaysRadioButton.AutoSize = true;
            this.afterCloseDaysRadioButton.Location = new System.Drawing.Point(53, 91);
            this.afterCloseDaysRadioButton.Name = "afterCloseDaysRadioButton";
            this.afterCloseDaysRadioButton.Size = new System.Drawing.Size(122, 17);
            this.afterCloseDaysRadioButton.TabIndex = 37;
            this.afterCloseDaysRadioButton.Text = "At close after X days";
            this.afterCloseDaysRadioButton.UseVisualStyleBackColor = true;
            // 
            // minutesAfterEntryRadioButton
            // 
            this.minutesAfterEntryRadioButton.AutoSize = true;
            this.minutesAfterEntryRadioButton.Checked = true;
            this.minutesAfterEntryRadioButton.Location = new System.Drawing.Point(54, 28);
            this.minutesAfterEntryRadioButton.Name = "minutesAfterEntryRadioButton";
            this.minutesAfterEntryRadioButton.Size = new System.Drawing.Size(114, 17);
            this.minutesAfterEntryRadioButton.TabIndex = 34;
            this.minutesAfterEntryRadioButton.TabStop = true;
            this.minutesAfterEntryRadioButton.Text = "Minutes After Entry";
            this.minutesAfterEntryRadioButton.UseVisualStyleBackColor = true;
            // 
            // minutesBeforeCloseRadioButton
            // 
            this.minutesBeforeCloseRadioButton.AutoSize = true;
            this.minutesBeforeCloseRadioButton.Location = new System.Drawing.Point(54, 48);
            this.minutesBeforeCloseRadioButton.Name = "minutesBeforeCloseRadioButton";
            this.minutesBeforeCloseRadioButton.Size = new System.Drawing.Size(125, 17);
            this.minutesBeforeCloseRadioButton.TabIndex = 35;
            this.minutesBeforeCloseRadioButton.Text = "Minutes Before Close";
            this.minutesBeforeCloseRadioButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 41;
            this.label3.Text = "Location:";
            // 
            // xmlCheckBox
            // 
            this.xmlCheckBox.AutoSize = true;
            this.xmlCheckBox.Location = new System.Drawing.Point(262, 50);
            this.xmlCheckBox.Name = "xmlCheckBox";
            this.xmlCheckBox.Size = new System.Drawing.Size(73, 17);
            this.xmlCheckBox.TabIndex = 43;
            this.xmlCheckBox.Text = "Xml Mode";
            this.xmlCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.testTextBox);
            this.groupBox6.Controls.Add(this.skipTextBox);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Location = new System.Drawing.Point(15, 159);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(96, 63);
            this.groupBox6.TabIndex = 45;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Days to Test";
            // 
            // testTextBox
            // 
            this.testTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.testTextBox.Location = new System.Drawing.Point(48, 36);
            this.testTextBox.Name = "testTextBox";
            this.testTextBox.Size = new System.Drawing.Size(29, 20);
            this.testTextBox.TabIndex = 40;
            this.testTextBox.Text = "15";
            this.testTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // skipTextBox
            // 
            this.skipTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.skipTextBox.Location = new System.Drawing.Point(48, 15);
            this.skipTextBox.Name = "skipTextBox";
            this.skipTextBox.Size = new System.Drawing.Size(29, 20);
            this.skipTextBox.TabIndex = 39;
            this.skipTextBox.Text = "0";
            this.skipTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 13);
            this.label7.TabIndex = 39;
            this.label7.Text = "Test";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Skip";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.entryTimeStopMinutesTextBox);
            this.groupBox7.Controls.Add(this.entryTimeStartMinutesTextBox);
            this.groupBox7.Controls.Add(this.label8);
            this.groupBox7.Controls.Add(this.label9);
            this.groupBox7.Location = new System.Drawing.Point(117, 159);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(209, 63);
            this.groupBox7.TabIndex = 46;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Entry Time";
            // 
            // entryTimeStopMinutesTextBox
            // 
            this.entryTimeStopMinutesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.entryTimeStopMinutesTextBox.Location = new System.Drawing.Point(139, 36);
            this.entryTimeStopMinutesTextBox.Name = "entryTimeStopMinutesTextBox";
            this.entryTimeStopMinutesTextBox.Size = new System.Drawing.Size(51, 20);
            this.entryTimeStopMinutesTextBox.TabIndex = 40;
            this.entryTimeStopMinutesTextBox.Text = "0";
            this.entryTimeStopMinutesTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // entryTimeStartMinutesTextBox
            // 
            this.entryTimeStartMinutesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.entryTimeStartMinutesTextBox.Location = new System.Drawing.Point(139, 15);
            this.entryTimeStartMinutesTextBox.Name = "entryTimeStartMinutesTextBox";
            this.entryTimeStartMinutesTextBox.Size = new System.Drawing.Size(51, 20);
            this.entryTimeStartMinutesTextBox.TabIndex = 39;
            this.entryTimeStartMinutesTextBox.Text = "30";
            this.entryTimeStartMinutesTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "Entry Time End Minutes";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(122, 13);
            this.label9.TabIndex = 38;
            this.label9.Text = "Entry Time Start Minutes";
            // 
            // debugModeCheckBox
            // 
            this.debugModeCheckBox.AutoSize = true;
            this.debugModeCheckBox.Location = new System.Drawing.Point(262, 27);
            this.debugModeCheckBox.Name = "debugModeCheckBox";
            this.debugModeCheckBox.Size = new System.Drawing.Size(88, 17);
            this.debugModeCheckBox.TabIndex = 47;
            this.debugModeCheckBox.Text = "Debug Mode";
            this.debugModeCheckBox.UseVisualStyleBackColor = true;
            // 
            // locationComboBox
            // 
            this.locationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.locationComboBox.FormattingEnabled = true;
            this.locationComboBox.Items.AddRange(new object[] {
            "US",
            "Canada"});
            this.locationComboBox.Location = new System.Drawing.Point(15, 64);
            this.locationComboBox.Name = "locationComboBox";
            this.locationComboBox.Size = new System.Drawing.Size(121, 21);
            this.locationComboBox.TabIndex = 48;
            // 
            // progressRichTextBox
            // 
            this.progressRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressRichTextBox.Location = new System.Drawing.Point(3, 3);
            this.progressRichTextBox.Name = "progressRichTextBox";
            this.progressRichTextBox.Size = new System.Drawing.Size(721, 356);
            this.progressRichTextBox.TabIndex = 49;
            this.progressRichTextBox.Text = "";
            // 
            // getCsvFileCheckBox
            // 
            this.getCsvFileCheckBox.AutoSize = true;
            this.getCsvFileCheckBox.Location = new System.Drawing.Point(262, 73);
            this.getCsvFileCheckBox.Name = "getCsvFileCheckBox";
            this.getCsvFileCheckBox.Size = new System.Drawing.Size(86, 17);
            this.getCsvFileCheckBox.TabIndex = 50;
            this.getCsvFileCheckBox.Text = "Get CSV File";
            this.getCsvFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.progressTabPage);
            this.tabControl1.Controls.Add(this.debugTabPage);
            this.tabControl1.Controls.Add(this.csvTabPage);
            this.tabControl1.Location = new System.Drawing.Point(15, 228);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(735, 388);
            this.tabControl1.TabIndex = 51;
            // 
            // progressTabPage
            // 
            this.progressTabPage.Controls.Add(this.progressRichTextBox);
            this.progressTabPage.Location = new System.Drawing.Point(4, 22);
            this.progressTabPage.Name = "progressTabPage";
            this.progressTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.progressTabPage.Size = new System.Drawing.Size(727, 362);
            this.progressTabPage.TabIndex = 0;
            this.progressTabPage.Text = "Progress / Results";
            this.progressTabPage.UseVisualStyleBackColor = true;
            // 
            // debugTabPage
            // 
            this.debugTabPage.Controls.Add(this.debugResultTextBox);
            this.debugTabPage.Location = new System.Drawing.Point(4, 22);
            this.debugTabPage.Name = "debugTabPage";
            this.debugTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.debugTabPage.Size = new System.Drawing.Size(727, 362);
            this.debugTabPage.TabIndex = 1;
            this.debugTabPage.Text = "Debug";
            this.debugTabPage.UseVisualStyleBackColor = true;
            // 
            // csvTabPage
            // 
            this.csvTabPage.Controls.Add(this.csvTextBox);
            this.csvTabPage.Location = new System.Drawing.Point(4, 22);
            this.csvTabPage.Name = "csvTabPage";
            this.csvTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.csvTabPage.Size = new System.Drawing.Size(727, 362);
            this.csvTabPage.TabIndex = 2;
            this.csvTabPage.Text = "CSV";
            this.csvTabPage.UseVisualStyleBackColor = true;
            // 
            // csvTextBox
            // 
            this.csvTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.csvTextBox.Location = new System.Drawing.Point(3, 3);
            this.csvTextBox.Multiline = true;
            this.csvTextBox.Name = "csvTextBox";
            this.csvTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.csvTextBox.Size = new System.Drawing.Size(721, 356);
            this.csvTextBox.TabIndex = 0;
            this.csvTextBox.WordWrap = false;
            // 
            // OddsMakerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 628);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.getCsvFileCheckBox);
            this.Controls.Add(this.locationComboBox);
            this.Controls.Add(this.debugModeCheckBox);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.xmlCheckBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.entryConditionTextBox);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(778, 662);
            this.Name = "OddsMakerForm";
            this.Text = "OddsMakerForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.progressTabPage.ResumeLayout(false);
            this.debugTabPage.ResumeLayout(false);
            this.debugTabPage.PerformLayout();
            this.csvTabPage.ResumeLayout(false);
            this.csvTabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.TextBox debugResultTextBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox entryConditionTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox exitConditionAlertTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton plusRadioButton;
        private System.Windows.Forms.RadioButton MinusRadioButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton PercentRadioButton;
        private System.Windows.Forms.RadioButton dollarRadioButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox SuccessValueTextBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox wiggleCheckBox;
        private System.Windows.Forms.TextBox stopLossTextBox;
        private System.Windows.Forms.CheckBox stopLossCheckBox;
        private System.Windows.Forms.TextBox profitTargetTextBox;
        private System.Windows.Forms.CheckBox profitTargetCheckBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton noExitRadioButton;
        private System.Windows.Forms.RadioButton AlertExitRadioButton;
        private System.Windows.Forms.RadioButton percentTrailingRadioButton;
        private System.Windows.Forms.RadioButton barsTrailingRadioButton;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox atCloseDaysTextBox;
        private System.Windows.Forms.TextBox atOpenDaysTextBox;
        private System.Windows.Forms.TextBox beforeCloseMinutesTextBox;
        private System.Windows.Forms.TextBox timeoutMinutesTextBox;
        private System.Windows.Forms.RadioButton atOpenDaysRadioButton;
        private System.Windows.Forms.RadioButton afterCloseDaysRadioButton;
        private System.Windows.Forms.RadioButton minutesAfterEntryRadioButton;
        private System.Windows.Forms.RadioButton minutesBeforeCloseRadioButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox xmlCheckBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox testTextBox;
        private System.Windows.Forms.TextBox skipTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox entryTimeStopMinutesTextBox;
        private System.Windows.Forms.TextBox entryTimeStartMinutesTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox debugModeCheckBox;
        private System.Windows.Forms.TextBox exitConditionTrailingStopTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox locationComboBox;
        private System.Windows.Forms.RichTextBox progressRichTextBox;
        private System.Windows.Forms.CheckBox getCsvFileCheckBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage progressTabPage;
        private System.Windows.Forms.TabPage debugTabPage;
        private System.Windows.Forms.TabPage csvTabPage;
        private System.Windows.Forms.TextBox csvTextBox;
    }
}