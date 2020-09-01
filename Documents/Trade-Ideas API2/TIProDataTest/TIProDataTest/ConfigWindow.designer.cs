namespace WindowsFormsApplication1
{
    /// <summary>
    /// ConfigWindow Class. Designer section
    /// </summary>
    partial class ConfigWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigWindow));
            this.label1 = new System.Windows.Forms.Label();
            this.configStringTextBox = new System.Windows.Forms.TextBox();
            this.requestServerInfoButton = new System.Windows.Forms.Button();
            this.getFromConfigButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.strategiesTabPage = new System.Windows.Forms.TabPage();
            this.strategyDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.strategyTreeView = new System.Windows.Forms.TreeView();
            this.strategyImageList = new System.Windows.Forms.ImageList(this.components);
            this.alertsTabPage = new System.Windows.Forms.TabPage();
            this.setAlertQualityButton = new System.Windows.Forms.Button();
            this.alertQualityTextBox = new System.Windows.Forms.TextBox();
            this.removeAlertButton = new System.Windows.Forms.Button();
            this.addAlertButton = new System.Windows.Forms.Button();
            this.alertCodetextBox = new System.Windows.Forms.TextBox();
            this.alertsListBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.filtersTabPage = new System.Windows.Forms.TabPage();
            this.filterValueTextBox = new System.Windows.Forms.TextBox();
            this.setMaxButton = new System.Windows.Forms.Button();
            this.setMinButton = new System.Windows.Forms.Button();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.filtersListBox = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ExchangesTabPage = new System.Windows.Forms.TabPage();
            this.setExchangesButton = new System.Windows.Forms.Button();
            this.readExchangesButton = new System.Windows.Forms.Button();
            this.exchangesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.symbolListsTabPage = new System.Windows.Forms.TabPage();
            this.setSymbolListsButton = new System.Windows.Forms.Button();
            this.readSymbolListsButton = new System.Windows.Forms.Button();
            this.symbolListsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.windowNameTabPage = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.windowNameTextBox = new System.Windows.Forms.TextBox();
            this.searchTabPage = new System.Windows.Forms.TabPage();
            this.searchTimeLabel = new System.Windows.Forms.Label();
            this.searchNowButton = new System.Windows.Forms.Button();
            this.searchResultsTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.categoriesComboBox = new System.Windows.Forms.ComboBox();
            this.searchTextTextBox = new System.Windows.Forms.TextBox();
            this.columnsTabPage = new System.Windows.Forms.TabPage();
            this.removeColumnsButton = new System.Windows.Forms.Button();
            this.addColumnsButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.currentColumnsListBox = new System.Windows.Forms.ListBox();
            this.allColumnsListBox = new System.Windows.Forms.ListBox();
            this.setColumnsButton = new System.Windows.Forms.Button();
            this.readColumnsButton = new System.Windows.Forms.Button();
            this.sortTabPage = new System.Windows.Forms.TabPage();
            this.sortDirectionGroupBox = new System.Windows.Forms.GroupBox();
            this.sortNoneRadioButton = new System.Windows.Forms.RadioButton();
            this.sortBottomRadioButton = new System.Windows.Forms.RadioButton();
            this.sortTopRadioButton = new System.Windows.Forms.RadioButton();
            this.sortFieldListBox = new System.Windows.Forms.ListBox();
            this.timePeriodTabPage = new System.Windows.Forms.TabPage();
            this.recentStatusLabel = new System.Windows.Forms.Label();
            this.flipButton = new System.Windows.Forms.Button();
            this.historyCheckBox = new System.Windows.Forms.CheckBox();
            this.historyDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.outsideMarketHoursCheckBox = new System.Windows.Forms.CheckBox();
            this.setTimePeriodButton = new System.Windows.Forms.Button();
            this.readTimePeriodButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.strategiesTabPage.SuspendLayout();
            this.alertsTabPage.SuspendLayout();
            this.filtersTabPage.SuspendLayout();
            this.ExchangesTabPage.SuspendLayout();
            this.symbolListsTabPage.SuspendLayout();
            this.windowNameTabPage.SuspendLayout();
            this.searchTabPage.SuspendLayout();
            this.columnsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.sortTabPage.SuspendLayout();
            this.sortDirectionGroupBox.SuspendLayout();
            this.timePeriodTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Config String:";
            // 
            // configStringTextBox
            // 
            this.configStringTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.configStringTextBox.Location = new System.Drawing.Point(12, 25);
            this.configStringTextBox.Name = "configStringTextBox";
            this.configStringTextBox.Size = new System.Drawing.Size(419, 20);
            this.configStringTextBox.TabIndex = 2;
            this.configStringTextBox.Text = "O=3_41D_0&MinPrice=5&WN=Hi+Dave";
            // 
            // requestServerInfoButton
            // 
            this.requestServerInfoButton.Location = new System.Drawing.Point(12, 51);
            this.requestServerInfoButton.Name = "requestServerInfoButton";
            this.requestServerInfoButton.Size = new System.Drawing.Size(115, 23);
            this.requestServerInfoButton.TabIndex = 3;
            this.requestServerInfoButton.Text = "Request Server Info";
            this.requestServerInfoButton.UseVisualStyleBackColor = true;
            this.requestServerInfoButton.Click += new System.EventHandler(this.requestServerInfoButton_Click);
            // 
            // getFromConfigButton
            // 
            this.getFromConfigButton.Enabled = false;
            this.getFromConfigButton.Location = new System.Drawing.Point(171, 51);
            this.getFromConfigButton.Name = "getFromConfigButton";
            this.getFromConfigButton.Size = new System.Drawing.Size(96, 23);
            this.getFromConfigButton.TabIndex = 4;
            this.getFromConfigButton.Text = "Get from Config";
            this.getFromConfigButton.UseVisualStyleBackColor = true;
            this.getFromConfigButton.Click += new System.EventHandler(this.getFromConfigButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Double Click a Strategy to Load it:";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.strategiesTabPage);
            this.tabControl1.Controls.Add(this.alertsTabPage);
            this.tabControl1.Controls.Add(this.filtersTabPage);
            this.tabControl1.Controls.Add(this.ExchangesTabPage);
            this.tabControl1.Controls.Add(this.symbolListsTabPage);
            this.tabControl1.Controls.Add(this.windowNameTabPage);
            this.tabControl1.Controls.Add(this.searchTabPage);
            this.tabControl1.Controls.Add(this.columnsTabPage);
            this.tabControl1.Controls.Add(this.sortTabPage);
            this.tabControl1.Controls.Add(this.timePeriodTabPage);
            this.tabControl1.Location = new System.Drawing.Point(12, 86);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(419, 286);
            this.tabControl1.TabIndex = 13;
            // 
            // strategiesTabPage
            // 
            this.strategiesTabPage.Controls.Add(this.strategyDescriptionTextBox);
            this.strategiesTabPage.Controls.Add(this.strategyTreeView);
            this.strategiesTabPage.Controls.Add(this.label2);
            this.strategiesTabPage.Location = new System.Drawing.Point(4, 22);
            this.strategiesTabPage.Name = "strategiesTabPage";
            this.strategiesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.strategiesTabPage.Size = new System.Drawing.Size(411, 260);
            this.strategiesTabPage.TabIndex = 0;
            this.strategiesTabPage.Text = "Strategies";
            this.strategiesTabPage.UseVisualStyleBackColor = true;
            // 
            // strategyDescriptionTextBox
            // 
            this.strategyDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.strategyDescriptionTextBox.Location = new System.Drawing.Point(6, 186);
            this.strategyDescriptionTextBox.Multiline = true;
            this.strategyDescriptionTextBox.Name = "strategyDescriptionTextBox";
            this.strategyDescriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.strategyDescriptionTextBox.Size = new System.Drawing.Size(399, 68);
            this.strategyDescriptionTextBox.TabIndex = 7;
            // 
            // strategyTreeView
            // 
            this.strategyTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.strategyTreeView.ImageIndex = 0;
            this.strategyTreeView.ImageList = this.strategyImageList;
            this.strategyTreeView.Location = new System.Drawing.Point(6, 20);
            this.strategyTreeView.Name = "strategyTreeView";
            this.strategyTreeView.SelectedImageIndex = 0;
            this.strategyTreeView.Size = new System.Drawing.Size(399, 160);
            this.strategyTreeView.TabIndex = 6;
            this.strategyTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.strategyTreeView_AfterSelect);
            this.strategyTreeView.DoubleClick += new System.EventHandler(this.strategyTreeView_DoubleClick);
            // 
            // strategyImageList
            // 
            this.strategyImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("strategyImageList.ImageStream")));
            this.strategyImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.strategyImageList.Images.SetKeyName(0, "BlueInfo.bmp");
            this.strategyImageList.Images.SetKeyName(1, "CurrentlyActiveArrow.bmp");
            this.strategyImageList.Images.SetKeyName(2, "YellowStar.bmp");
            this.strategyImageList.Images.SetKeyName(3, "Folder.bmp");
            this.strategyImageList.Images.SetKeyName(4, "GreenArrowUp.bmp");
            this.strategyImageList.Images.SetKeyName(5, "RedArrowDown.bmp");
            this.strategyImageList.Images.SetKeyName(6, "SmileyFace.bmp");
            // 
            // alertsTabPage
            // 
            this.alertsTabPage.Controls.Add(this.setAlertQualityButton);
            this.alertsTabPage.Controls.Add(this.alertQualityTextBox);
            this.alertsTabPage.Controls.Add(this.removeAlertButton);
            this.alertsTabPage.Controls.Add(this.addAlertButton);
            this.alertsTabPage.Controls.Add(this.alertCodetextBox);
            this.alertsTabPage.Controls.Add(this.alertsListBox);
            this.alertsTabPage.Controls.Add(this.label3);
            this.alertsTabPage.Location = new System.Drawing.Point(4, 22);
            this.alertsTabPage.Name = "alertsTabPage";
            this.alertsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.alertsTabPage.Size = new System.Drawing.Size(411, 260);
            this.alertsTabPage.TabIndex = 1;
            this.alertsTabPage.Text = "Alerts";
            this.alertsTabPage.UseVisualStyleBackColor = true;
            // 
            // setAlertQualityButton
            // 
            this.setAlertQualityButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setAlertQualityButton.Location = new System.Drawing.Point(235, 231);
            this.setAlertQualityButton.Name = "setAlertQualityButton";
            this.setAlertQualityButton.Size = new System.Drawing.Size(67, 23);
            this.setAlertQualityButton.TabIndex = 19;
            this.setAlertQualityButton.Text = "Set Quality";
            this.setAlertQualityButton.UseVisualStyleBackColor = true;
            this.setAlertQualityButton.Click += new System.EventHandler(this.setAlertQualityButton_Click);
            // 
            // alertQualityTextBox
            // 
            this.alertQualityTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.alertQualityTextBox.Location = new System.Drawing.Point(174, 233);
            this.alertQualityTextBox.Name = "alertQualityTextBox";
            this.alertQualityTextBox.Size = new System.Drawing.Size(55, 20);
            this.alertQualityTextBox.TabIndex = 18;
            // 
            // removeAlertButton
            // 
            this.removeAlertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeAlertButton.Location = new System.Drawing.Point(112, 231);
            this.removeAlertButton.Name = "removeAlertButton";
            this.removeAlertButton.Size = new System.Drawing.Size(56, 23);
            this.removeAlertButton.TabIndex = 17;
            this.removeAlertButton.Text = "Remove";
            this.removeAlertButton.UseVisualStyleBackColor = true;
            this.removeAlertButton.Click += new System.EventHandler(this.removeAlertButton_Click);
            // 
            // addAlertButton
            // 
            this.addAlertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addAlertButton.Location = new System.Drawing.Point(64, 231);
            this.addAlertButton.Name = "addAlertButton";
            this.addAlertButton.Size = new System.Drawing.Size(42, 23);
            this.addAlertButton.TabIndex = 16;
            this.addAlertButton.Text = "Add";
            this.addAlertButton.UseVisualStyleBackColor = true;
            this.addAlertButton.Click += new System.EventHandler(this.addAlertButton_Click);
            // 
            // alertCodetextBox
            // 
            this.alertCodetextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.alertCodetextBox.Location = new System.Drawing.Point(3, 233);
            this.alertCodetextBox.Name = "alertCodetextBox";
            this.alertCodetextBox.Size = new System.Drawing.Size(55, 20);
            this.alertCodetextBox.TabIndex = 15;
            // 
            // alertsListBox
            // 
            this.alertsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.alertsListBox.FormattingEnabled = true;
            this.alertsListBox.Location = new System.Drawing.Point(3, 19);
            this.alertsListBox.Name = "alertsListBox";
            this.alertsListBox.Size = new System.Drawing.Size(402, 212);
            this.alertsListBox.TabIndex = 14;
            this.alertsListBox.DoubleClick += new System.EventHandler(this.alertsListBox_DoubleClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Double click on an alert or type the code below.";
            // 
            // filtersTabPage
            // 
            this.filtersTabPage.Controls.Add(this.filterValueTextBox);
            this.filtersTabPage.Controls.Add(this.setMaxButton);
            this.filtersTabPage.Controls.Add(this.setMinButton);
            this.filtersTabPage.Controls.Add(this.filterTextBox);
            this.filtersTabPage.Controls.Add(this.filtersListBox);
            this.filtersTabPage.Controls.Add(this.label4);
            this.filtersTabPage.Location = new System.Drawing.Point(4, 22);
            this.filtersTabPage.Name = "filtersTabPage";
            this.filtersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.filtersTabPage.Size = new System.Drawing.Size(411, 260);
            this.filtersTabPage.TabIndex = 2;
            this.filtersTabPage.Text = "Filters";
            this.filtersTabPage.UseVisualStyleBackColor = true;
            // 
            // filterValueTextBox
            // 
            this.filterValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.filterValueTextBox.Location = new System.Drawing.Point(189, 234);
            this.filterValueTextBox.Name = "filterValueTextBox";
            this.filterValueTextBox.Size = new System.Drawing.Size(92, 20);
            this.filterValueTextBox.TabIndex = 22;
            // 
            // setMaxButton
            // 
            this.setMaxButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setMaxButton.Location = new System.Drawing.Point(127, 232);
            this.setMaxButton.Name = "setMaxButton";
            this.setMaxButton.Size = new System.Drawing.Size(56, 23);
            this.setMaxButton.TabIndex = 21;
            this.setMaxButton.Text = "Set Max";
            this.setMaxButton.UseVisualStyleBackColor = true;
            this.setMaxButton.Click += new System.EventHandler(this.setMaxButton_Click);
            // 
            // setMinButton
            // 
            this.setMinButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setMinButton.Location = new System.Drawing.Point(67, 232);
            this.setMinButton.Name = "setMinButton";
            this.setMinButton.Size = new System.Drawing.Size(54, 23);
            this.setMinButton.TabIndex = 20;
            this.setMinButton.Text = "Set Min";
            this.setMinButton.UseVisualStyleBackColor = true;
            this.setMinButton.Click += new System.EventHandler(this.setMinButton_Click);
            // 
            // filterTextBox
            // 
            this.filterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.filterTextBox.Location = new System.Drawing.Point(6, 234);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(55, 20);
            this.filterTextBox.TabIndex = 19;
            // 
            // filtersListBox
            // 
            this.filtersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.filtersListBox.FormattingEnabled = true;
            this.filtersListBox.Location = new System.Drawing.Point(6, 19);
            this.filtersListBox.Name = "filtersListBox";
            this.filtersListBox.Size = new System.Drawing.Size(399, 212);
            this.filtersListBox.TabIndex = 1;
            this.filtersListBox.DoubleClick += new System.EventHandler(this.filtersListBox_DoubleClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(223, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Double click on a filter or type the code below";
            // 
            // ExchangesTabPage
            // 
            this.ExchangesTabPage.Controls.Add(this.setExchangesButton);
            this.ExchangesTabPage.Controls.Add(this.readExchangesButton);
            this.ExchangesTabPage.Controls.Add(this.exchangesCheckedListBox);
            this.ExchangesTabPage.Location = new System.Drawing.Point(4, 22);
            this.ExchangesTabPage.Name = "ExchangesTabPage";
            this.ExchangesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ExchangesTabPage.Size = new System.Drawing.Size(411, 260);
            this.ExchangesTabPage.TabIndex = 3;
            this.ExchangesTabPage.Text = "Exchanges";
            this.ExchangesTabPage.UseVisualStyleBackColor = true;
            // 
            // setExchangesButton
            // 
            this.setExchangesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setExchangesButton.Location = new System.Drawing.Point(58, 231);
            this.setExchangesButton.Name = "setExchangesButton";
            this.setExchangesButton.Size = new System.Drawing.Size(34, 23);
            this.setExchangesButton.TabIndex = 2;
            this.setExchangesButton.Text = "Set";
            this.setExchangesButton.UseVisualStyleBackColor = true;
            this.setExchangesButton.Click += new System.EventHandler(this.setExchangesButton_Click);
            // 
            // readExchangesButton
            // 
            this.readExchangesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.readExchangesButton.Location = new System.Drawing.Point(6, 231);
            this.readExchangesButton.Name = "readExchangesButton";
            this.readExchangesButton.Size = new System.Drawing.Size(46, 23);
            this.readExchangesButton.TabIndex = 1;
            this.readExchangesButton.Text = "Read";
            this.readExchangesButton.UseVisualStyleBackColor = true;
            this.readExchangesButton.Click += new System.EventHandler(this.readExchangesButton_Click);
            // 
            // exchangesCheckedListBox
            // 
            this.exchangesCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.exchangesCheckedListBox.CheckOnClick = true;
            this.exchangesCheckedListBox.FormattingEnabled = true;
            this.exchangesCheckedListBox.Location = new System.Drawing.Point(6, 6);
            this.exchangesCheckedListBox.Name = "exchangesCheckedListBox";
            this.exchangesCheckedListBox.Size = new System.Drawing.Size(399, 214);
            this.exchangesCheckedListBox.TabIndex = 0;
            // 
            // symbolListsTabPage
            // 
            this.symbolListsTabPage.Controls.Add(this.setSymbolListsButton);
            this.symbolListsTabPage.Controls.Add(this.readSymbolListsButton);
            this.symbolListsTabPage.Controls.Add(this.symbolListsCheckedListBox);
            this.symbolListsTabPage.Location = new System.Drawing.Point(4, 22);
            this.symbolListsTabPage.Name = "symbolListsTabPage";
            this.symbolListsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.symbolListsTabPage.Size = new System.Drawing.Size(411, 260);
            this.symbolListsTabPage.TabIndex = 4;
            this.symbolListsTabPage.Text = "Symbol Lists";
            this.symbolListsTabPage.UseVisualStyleBackColor = true;
            // 
            // setSymbolListsButton
            // 
            this.setSymbolListsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setSymbolListsButton.Location = new System.Drawing.Point(58, 231);
            this.setSymbolListsButton.Name = "setSymbolListsButton";
            this.setSymbolListsButton.Size = new System.Drawing.Size(34, 23);
            this.setSymbolListsButton.TabIndex = 5;
            this.setSymbolListsButton.Text = "Set";
            this.setSymbolListsButton.UseVisualStyleBackColor = true;
            this.setSymbolListsButton.Click += new System.EventHandler(this.setSymbolListsButton_Click);
            // 
            // readSymbolListsButton
            // 
            this.readSymbolListsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.readSymbolListsButton.Location = new System.Drawing.Point(6, 231);
            this.readSymbolListsButton.Name = "readSymbolListsButton";
            this.readSymbolListsButton.Size = new System.Drawing.Size(46, 23);
            this.readSymbolListsButton.TabIndex = 4;
            this.readSymbolListsButton.Text = "Read";
            this.readSymbolListsButton.UseVisualStyleBackColor = true;
            this.readSymbolListsButton.Click += new System.EventHandler(this.readSymbolListsButton_Click);
            // 
            // symbolListsCheckedListBox
            // 
            this.symbolListsCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.symbolListsCheckedListBox.CheckOnClick = true;
            this.symbolListsCheckedListBox.FormattingEnabled = true;
            this.symbolListsCheckedListBox.Location = new System.Drawing.Point(6, 6);
            this.symbolListsCheckedListBox.Name = "symbolListsCheckedListBox";
            this.symbolListsCheckedListBox.Size = new System.Drawing.Size(399, 214);
            this.symbolListsCheckedListBox.TabIndex = 3;
            // 
            // windowNameTabPage
            // 
            this.windowNameTabPage.Controls.Add(this.label5);
            this.windowNameTabPage.Controls.Add(this.windowNameTextBox);
            this.windowNameTabPage.Location = new System.Drawing.Point(4, 22);
            this.windowNameTabPage.Name = "windowNameTabPage";
            this.windowNameTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.windowNameTabPage.Size = new System.Drawing.Size(411, 260);
            this.windowNameTabPage.TabIndex = 5;
            this.windowNameTabPage.Text = "Name";
            this.windowNameTabPage.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(259, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "The window name automatically updates as you type.";
            // 
            // windowNameTextBox
            // 
            this.windowNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.windowNameTextBox.Location = new System.Drawing.Point(9, 19);
            this.windowNameTextBox.Name = "windowNameTextBox";
            this.windowNameTextBox.Size = new System.Drawing.Size(396, 20);
            this.windowNameTextBox.TabIndex = 0;
            this.windowNameTextBox.TextChanged += new System.EventHandler(this.windowNameTextBox_TextChanged);
            // 
            // searchTabPage
            // 
            this.searchTabPage.Controls.Add(this.searchTimeLabel);
            this.searchTabPage.Controls.Add(this.searchNowButton);
            this.searchTabPage.Controls.Add(this.searchResultsTextBox);
            this.searchTabPage.Controls.Add(this.label7);
            this.searchTabPage.Controls.Add(this.label6);
            this.searchTabPage.Controls.Add(this.categoriesComboBox);
            this.searchTabPage.Controls.Add(this.searchTextTextBox);
            this.searchTabPage.Location = new System.Drawing.Point(4, 22);
            this.searchTabPage.Name = "searchTabPage";
            this.searchTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.searchTabPage.Size = new System.Drawing.Size(411, 260);
            this.searchTabPage.TabIndex = 6;
            this.searchTabPage.Text = "Search";
            this.searchTabPage.UseVisualStyleBackColor = true;
            // 
            // searchTimeLabel
            // 
            this.searchTimeLabel.AutoSize = true;
            this.searchTimeLabel.Location = new System.Drawing.Point(91, 64);
            this.searchTimeLabel.Name = "searchTimeLabel";
            this.searchTimeLabel.Size = new System.Drawing.Size(67, 13);
            this.searchTimeLabel.TabIndex = 6;
            this.searchTimeLabel.Text = "Search Time";
            // 
            // searchNowButton
            // 
            this.searchNowButton.Location = new System.Drawing.Point(9, 59);
            this.searchNowButton.Name = "searchNowButton";
            this.searchNowButton.Size = new System.Drawing.Size(76, 23);
            this.searchNowButton.TabIndex = 5;
            this.searchNowButton.Text = "Search Now";
            this.searchNowButton.UseVisualStyleBackColor = true;
            this.searchNowButton.Click += new System.EventHandler(this.searchNowButton_Click);
            // 
            // searchResultsTextBox
            // 
            this.searchResultsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.searchResultsTextBox.Location = new System.Drawing.Point(6, 88);
            this.searchResultsTextBox.Multiline = true;
            this.searchResultsTextBox.Name = "searchResultsTextBox";
            this.searchResultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.searchResultsTextBox.Size = new System.Drawing.Size(399, 166);
            this.searchResultsTextBox.TabIndex = 4;
            this.searchResultsTextBox.WordWrap = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Category:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Text:";
            // 
            // categoriesComboBox
            // 
            this.categoriesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.categoriesComboBox.FormattingEnabled = true;
            this.categoriesComboBox.Location = new System.Drawing.Point(64, 32);
            this.categoriesComboBox.Name = "categoriesComboBox";
            this.categoriesComboBox.Size = new System.Drawing.Size(341, 21);
            this.categoriesComboBox.TabIndex = 1;
            // 
            // searchTextTextBox
            // 
            this.searchTextTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextTextBox.Location = new System.Drawing.Point(43, 6);
            this.searchTextTextBox.Name = "searchTextTextBox";
            this.searchTextTextBox.Size = new System.Drawing.Size(362, 20);
            this.searchTextTextBox.TabIndex = 0;
            // 
            // columnsTabPage
            // 
            this.columnsTabPage.Controls.Add(this.removeColumnsButton);
            this.columnsTabPage.Controls.Add(this.addColumnsButton);
            this.columnsTabPage.Controls.Add(this.splitContainer1);
            this.columnsTabPage.Controls.Add(this.setColumnsButton);
            this.columnsTabPage.Controls.Add(this.readColumnsButton);
            this.columnsTabPage.Location = new System.Drawing.Point(4, 22);
            this.columnsTabPage.Name = "columnsTabPage";
            this.columnsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.columnsTabPage.Size = new System.Drawing.Size(411, 260);
            this.columnsTabPage.TabIndex = 7;
            this.columnsTabPage.Text = "Columns";
            this.columnsTabPage.UseVisualStyleBackColor = true;
            // 
            // removeColumnsButton
            // 
            this.removeColumnsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeColumnsButton.Location = new System.Drawing.Point(176, 231);
            this.removeColumnsButton.Name = "removeColumnsButton";
            this.removeColumnsButton.Size = new System.Drawing.Size(59, 23);
            this.removeColumnsButton.TabIndex = 16;
            this.removeColumnsButton.Text = "Remove";
            this.removeColumnsButton.UseVisualStyleBackColor = true;
            this.removeColumnsButton.Click += new System.EventHandler(this.removeColumnsButton_Click);
            // 
            // addColumnsButton
            // 
            this.addColumnsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addColumnsButton.Location = new System.Drawing.Point(126, 231);
            this.addColumnsButton.Name = "addColumnsButton";
            this.addColumnsButton.Size = new System.Drawing.Size(44, 23);
            this.addColumnsButton.TabIndex = 6;
            this.addColumnsButton.Text = "Add";
            this.addColumnsButton.UseVisualStyleBackColor = true;
            this.addColumnsButton.Click += new System.EventHandler(this.addColumnsButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(6, 6);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.currentColumnsListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.allColumnsListBox);
            this.splitContainer1.Size = new System.Drawing.Size(399, 219);
            this.splitContainer1.SplitterDistance = 198;
            this.splitContainer1.TabIndex = 5;
            // 
            // currentColumnsListBox
            // 
            this.currentColumnsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.currentColumnsListBox.FormattingEnabled = true;
            this.currentColumnsListBox.Location = new System.Drawing.Point(0, 0);
            this.currentColumnsListBox.Name = "currentColumnsListBox";
            this.currentColumnsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.currentColumnsListBox.Size = new System.Drawing.Size(195, 212);
            this.currentColumnsListBox.TabIndex = 0;
            // 
            // allColumnsListBox
            // 
            this.allColumnsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.allColumnsListBox.FormattingEnabled = true;
            this.allColumnsListBox.Location = new System.Drawing.Point(3, 0);
            this.allColumnsListBox.Name = "allColumnsListBox";
            this.allColumnsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.allColumnsListBox.Size = new System.Drawing.Size(191, 212);
            this.allColumnsListBox.TabIndex = 0;
            // 
            // setColumnsButton
            // 
            this.setColumnsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setColumnsButton.Location = new System.Drawing.Point(58, 231);
            this.setColumnsButton.Name = "setColumnsButton";
            this.setColumnsButton.Size = new System.Drawing.Size(34, 23);
            this.setColumnsButton.TabIndex = 4;
            this.setColumnsButton.Text = "Set";
            this.setColumnsButton.UseVisualStyleBackColor = true;
            this.setColumnsButton.Click += new System.EventHandler(this.setColumnsButton_Click);
            // 
            // readColumnsButton
            // 
            this.readColumnsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.readColumnsButton.Location = new System.Drawing.Point(6, 231);
            this.readColumnsButton.Name = "readColumnsButton";
            this.readColumnsButton.Size = new System.Drawing.Size(46, 23);
            this.readColumnsButton.TabIndex = 3;
            this.readColumnsButton.Text = "Read";
            this.readColumnsButton.UseVisualStyleBackColor = true;
            this.readColumnsButton.Click += new System.EventHandler(this.readColumnsButton_Click);
            // 
            // sortTabPage
            // 
            this.sortTabPage.Controls.Add(this.sortDirectionGroupBox);
            this.sortTabPage.Controls.Add(this.sortFieldListBox);
            this.sortTabPage.Location = new System.Drawing.Point(4, 22);
            this.sortTabPage.Name = "sortTabPage";
            this.sortTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.sortTabPage.Size = new System.Drawing.Size(411, 260);
            this.sortTabPage.TabIndex = 8;
            this.sortTabPage.Text = "Sort";
            this.sortTabPage.UseVisualStyleBackColor = true;
            // 
            // sortDirectionGroupBox
            // 
            this.sortDirectionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sortDirectionGroupBox.Controls.Add(this.sortNoneRadioButton);
            this.sortDirectionGroupBox.Controls.Add(this.sortBottomRadioButton);
            this.sortDirectionGroupBox.Controls.Add(this.sortTopRadioButton);
            this.sortDirectionGroupBox.Location = new System.Drawing.Point(6, 6);
            this.sortDirectionGroupBox.Name = "sortDirectionGroupBox";
            this.sortDirectionGroupBox.Size = new System.Drawing.Size(399, 46);
            this.sortDirectionGroupBox.TabIndex = 1;
            this.sortDirectionGroupBox.TabStop = false;
            this.sortDirectionGroupBox.Text = "Direction";
            // 
            // sortNoneRadioButton
            // 
            this.sortNoneRadioButton.AutoSize = true;
            this.sortNoneRadioButton.Checked = true;
            this.sortNoneRadioButton.Location = new System.Drawing.Point(216, 19);
            this.sortNoneRadioButton.Name = "sortNoneRadioButton";
            this.sortNoneRadioButton.Size = new System.Drawing.Size(51, 17);
            this.sortNoneRadioButton.TabIndex = 2;
            this.sortNoneRadioButton.TabStop = true;
            this.sortNoneRadioButton.Text = "None";
            this.sortNoneRadioButton.UseVisualStyleBackColor = true;
            this.sortNoneRadioButton.CheckedChanged += new System.EventHandler(this.sortNoneRadioButton_CheckedChanged);
            // 
            // sortBottomRadioButton
            // 
            this.sortBottomRadioButton.AutoSize = true;
            this.sortBottomRadioButton.Location = new System.Drawing.Point(109, 19);
            this.sortBottomRadioButton.Name = "sortBottomRadioButton";
            this.sortBottomRadioButton.Size = new System.Drawing.Size(101, 17);
            this.sortBottomRadioButton.TabIndex = 1;
            this.sortBottomRadioButton.Text = "Smallest on Top";
            this.sortBottomRadioButton.UseVisualStyleBackColor = true;
            this.sortBottomRadioButton.CheckedChanged += new System.EventHandler(this.sortBottomRadioButton_CheckedChanged);
            // 
            // sortTopRadioButton
            // 
            this.sortTopRadioButton.AutoSize = true;
            this.sortTopRadioButton.Location = new System.Drawing.Point(6, 19);
            this.sortTopRadioButton.Name = "sortTopRadioButton";
            this.sortTopRadioButton.Size = new System.Drawing.Size(97, 17);
            this.sortTopRadioButton.TabIndex = 0;
            this.sortTopRadioButton.Text = "Biggest on Top";
            this.sortTopRadioButton.UseVisualStyleBackColor = true;
            this.sortTopRadioButton.CheckedChanged += new System.EventHandler(this.sortTopRadioButton_CheckedChanged);
            // 
            // sortFieldListBox
            // 
            this.sortFieldListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sortFieldListBox.FormattingEnabled = true;
            this.sortFieldListBox.Location = new System.Drawing.Point(6, 58);
            this.sortFieldListBox.Name = "sortFieldListBox";
            this.sortFieldListBox.Size = new System.Drawing.Size(399, 199);
            this.sortFieldListBox.TabIndex = 0;
            this.sortFieldListBox.SelectedIndexChanged += new System.EventHandler(this.sortFieldListBox_SelectedIndexChanged);
            // 
            // timePeriodTabPage
            // 
            this.timePeriodTabPage.Controls.Add(this.setTimePeriodButton);
            this.timePeriodTabPage.Controls.Add(this.readTimePeriodButton);
            this.timePeriodTabPage.Controls.Add(this.outsideMarketHoursCheckBox);
            this.timePeriodTabPage.Controls.Add(this.historyDateTimePicker);
            this.timePeriodTabPage.Controls.Add(this.historyCheckBox);
            this.timePeriodTabPage.Location = new System.Drawing.Point(4, 22);
            this.timePeriodTabPage.Name = "timePeriodTabPage";
            this.timePeriodTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.timePeriodTabPage.Size = new System.Drawing.Size(411, 260);
            this.timePeriodTabPage.TabIndex = 9;
            this.timePeriodTabPage.Text = "Time Period";
            this.timePeriodTabPage.UseVisualStyleBackColor = true;
            // 
            // recentStatusLabel
            // 
            this.recentStatusLabel.AutoSize = true;
            this.recentStatusLabel.Location = new System.Drawing.Point(273, 56);
            this.recentStatusLabel.Name = "recentStatusLabel";
            this.recentStatusLabel.Size = new System.Drawing.Size(99, 13);
            this.recentStatusLabel.TabIndex = 14;
            this.recentStatusLabel.Text = "Nothing loaded yet.";
            // 
            // flipButton
            // 
            this.flipButton.Location = new System.Drawing.Point(133, 51);
            this.flipButton.Name = "flipButton";
            this.flipButton.Size = new System.Drawing.Size(32, 23);
            this.flipButton.TabIndex = 15;
            this.flipButton.Text = "Flip";
            this.flipButton.UseVisualStyleBackColor = true;
            this.flipButton.Click += new System.EventHandler(this.flipButton_Click);
            // 
            // historyCheckBox
            // 
            this.historyCheckBox.AutoSize = true;
            this.historyCheckBox.Location = new System.Drawing.Point(6, 6);
            this.historyCheckBox.Name = "historyCheckBox";
            this.historyCheckBox.Size = new System.Drawing.Size(58, 17);
            this.historyCheckBox.TabIndex = 0;
            this.historyCheckBox.Text = "History";
            this.historyCheckBox.UseVisualStyleBackColor = true;
            // 
            // historyDateTimePicker
            // 
            this.historyDateTimePicker.CustomFormat = "h:mm tt M/dd/yyyy";
            this.historyDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.historyDateTimePicker.Location = new System.Drawing.Point(6, 29);
            this.historyDateTimePicker.Name = "historyDateTimePicker";
            this.historyDateTimePicker.Size = new System.Drawing.Size(158, 20);
            this.historyDateTimePicker.TabIndex = 1;
            // 
            // outsideMarketHoursCheckBox
            // 
            this.outsideMarketHoursCheckBox.AutoSize = true;
            this.outsideMarketHoursCheckBox.Location = new System.Drawing.Point(6, 55);
            this.outsideMarketHoursCheckBox.Name = "outsideMarketHoursCheckBox";
            this.outsideMarketHoursCheckBox.Size = new System.Drawing.Size(158, 17);
            this.outsideMarketHoursCheckBox.TabIndex = 2;
            this.outsideMarketHoursCheckBox.Text = "Include pre and post market";
            this.outsideMarketHoursCheckBox.UseVisualStyleBackColor = true;
            // 
            // setTimePeriodButton
            // 
            this.setTimePeriodButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setTimePeriodButton.Location = new System.Drawing.Point(58, 231);
            this.setTimePeriodButton.Name = "setTimePeriodButton";
            this.setTimePeriodButton.Size = new System.Drawing.Size(34, 23);
            this.setTimePeriodButton.TabIndex = 7;
            this.setTimePeriodButton.Text = "Set";
            this.setTimePeriodButton.UseVisualStyleBackColor = true;
            this.setTimePeriodButton.Click += new System.EventHandler(this.setTimePeriodButton_Click);
            // 
            // readTimePeriodButton
            // 
            this.readTimePeriodButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.readTimePeriodButton.Location = new System.Drawing.Point(6, 231);
            this.readTimePeriodButton.Name = "readTimePeriodButton";
            this.readTimePeriodButton.Size = new System.Drawing.Size(46, 23);
            this.readTimePeriodButton.TabIndex = 6;
            this.readTimePeriodButton.Text = "Read";
            this.readTimePeriodButton.UseVisualStyleBackColor = true;
            this.readTimePeriodButton.Click += new System.EventHandler(this.readTimePeriodButton_Click);
            // 
            // ConfigWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 384);
            this.Controls.Add(this.flipButton);
            this.Controls.Add(this.recentStatusLabel);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.getFromConfigButton);
            this.Controls.Add(this.requestServerInfoButton);
            this.Controls.Add(this.configStringTextBox);
            this.Controls.Add(this.label1);
            this.Name = "ConfigWindow";
            this.Text = "ConfigWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigWindow_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.strategiesTabPage.ResumeLayout(false);
            this.strategiesTabPage.PerformLayout();
            this.alertsTabPage.ResumeLayout(false);
            this.alertsTabPage.PerformLayout();
            this.filtersTabPage.ResumeLayout(false);
            this.filtersTabPage.PerformLayout();
            this.ExchangesTabPage.ResumeLayout(false);
            this.symbolListsTabPage.ResumeLayout(false);
            this.windowNameTabPage.ResumeLayout(false);
            this.windowNameTabPage.PerformLayout();
            this.searchTabPage.ResumeLayout(false);
            this.searchTabPage.PerformLayout();
            this.columnsTabPage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.sortTabPage.ResumeLayout(false);
            this.sortDirectionGroupBox.ResumeLayout(false);
            this.sortDirectionGroupBox.PerformLayout();
            this.timePeriodTabPage.ResumeLayout(false);
            this.timePeriodTabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox configStringTextBox;
        private System.Windows.Forms.Button requestServerInfoButton;
        private System.Windows.Forms.Button getFromConfigButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage strategiesTabPage;
        private System.Windows.Forms.TreeView strategyTreeView;
        private System.Windows.Forms.TabPage alertsTabPage;
        private System.Windows.Forms.Button setAlertQualityButton;
        private System.Windows.Forms.TextBox alertQualityTextBox;
        private System.Windows.Forms.Button removeAlertButton;
        private System.Windows.Forms.Button addAlertButton;
        private System.Windows.Forms.TextBox alertCodetextBox;
        private System.Windows.Forms.ListBox alertsListBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage filtersTabPage;
        private System.Windows.Forms.TabPage ExchangesTabPage;
        private System.Windows.Forms.TabPage symbolListsTabPage;
        private System.Windows.Forms.TabPage windowNameTabPage;
        private System.Windows.Forms.TextBox filterValueTextBox;
        private System.Windows.Forms.Button setMaxButton;
        private System.Windows.Forms.Button setMinButton;
        private System.Windows.Forms.TextBox filterTextBox;
        private System.Windows.Forms.ListBox filtersListBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button setExchangesButton;
        private System.Windows.Forms.Button readExchangesButton;
        private System.Windows.Forms.CheckedListBox exchangesCheckedListBox;
        private System.Windows.Forms.Button setSymbolListsButton;
        private System.Windows.Forms.Button readSymbolListsButton;
        private System.Windows.Forms.CheckedListBox symbolListsCheckedListBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox windowNameTextBox;
        private System.Windows.Forms.Label recentStatusLabel;
        private System.Windows.Forms.TextBox strategyDescriptionTextBox;
        private System.Windows.Forms.Button flipButton;
        private System.Windows.Forms.TabPage searchTabPage;
        private System.Windows.Forms.TextBox searchResultsTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox categoriesComboBox;
        private System.Windows.Forms.TextBox searchTextTextBox;
        private System.Windows.Forms.Label searchTimeLabel;
        private System.Windows.Forms.Button searchNowButton;
        private System.Windows.Forms.TabPage columnsTabPage;
        private System.Windows.Forms.TabPage sortTabPage;
        private System.Windows.Forms.TabPage timePeriodTabPage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox currentColumnsListBox;
        private System.Windows.Forms.ListBox allColumnsListBox;
        private System.Windows.Forms.Button setColumnsButton;
        private System.Windows.Forms.Button readColumnsButton;
        private System.Windows.Forms.GroupBox sortDirectionGroupBox;
        private System.Windows.Forms.RadioButton sortNoneRadioButton;
        private System.Windows.Forms.RadioButton sortBottomRadioButton;
        private System.Windows.Forms.RadioButton sortTopRadioButton;
        private System.Windows.Forms.ListBox sortFieldListBox;
        private System.Windows.Forms.Button removeColumnsButton;
        private System.Windows.Forms.Button addColumnsButton;
        private System.Windows.Forms.ImageList strategyImageList;
        private System.Windows.Forms.Button setTimePeriodButton;
        private System.Windows.Forms.Button readTimePeriodButton;
        private System.Windows.Forms.CheckBox outsideMarketHoursCheckBox;
        private System.Windows.Forms.DateTimePicker historyDateTimePicker;
        private System.Windows.Forms.CheckBox historyCheckBox;

    }
}