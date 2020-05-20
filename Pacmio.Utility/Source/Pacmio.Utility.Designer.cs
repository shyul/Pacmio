namespace Pacmio.Utility
{
    partial class DataUtility
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
            this.BtnConnect = new System.Windows.Forms.Button();
            this.BtnAccountSum = new System.Windows.Forms.Button();
            this.BtnAccountUpdate = new System.Windows.Forms.Button();
            this.BtnPostions = new System.Windows.Forms.Button();
            this.BtnRequestIBHistData = new System.Windows.Forms.Button();
            this.BtnOrder = new System.Windows.Forms.Button();
            this.BtnGetSymbolSamples = new System.Windows.Forms.Button();
            this.BoxContract = new System.Windows.Forms.TextBox();
            this.BtnScanAllFundamentals = new System.Windows.Forms.Button();
            this.BtnExportBarTableCSV = new System.Windows.Forms.Button();
            this.BtnScanFundamentalSummary = new System.Windows.Forms.Button();
            this.BtnSaveSampleFundamental = new System.Windows.Forms.Button();
            this.BtnGetAllFundamentals = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.BtnLoadEOD = new System.Windows.Forms.Button();
            this.BtnRequestEquityInfo = new System.Windows.Forms.Button();
            this.BtnImportFolderFundamentalData = new System.Windows.Forms.Button();
            this.BtnHistoricalDataAdjust = new System.Windows.Forms.Button();
            this.BtnHistoricalDataCounterAdjust = new System.Windows.Forms.Button();
            this.StatusProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.StatusGroupBox = new System.Windows.Forms.GroupBox();
            this.messageBox = new System.Windows.Forms.TextBox();
            this.buttonMarketDepth = new System.Windows.Forms.Button();
            this.buttonMarketData = new System.Windows.Forms.Button();
            this.BtnMatchSymbols = new System.Windows.Forms.Button();
            this.BtnTest = new System.Windows.Forms.Button();
            this.BtnRescanUnknownSymbols = new System.Windows.Forms.Button();
            this.BtnImportSymbols = new System.Windows.Forms.Button();
            this.BtnExportSymbols = new System.Windows.Forms.Button();
            this.GBoxConnection = new System.Windows.Forms.GroupBox();
            this.LbClientId = new System.Windows.Forms.Label();
            this.TextBoxClientId = new System.Windows.Forms.TextBox();
            this.LbPort = new System.Windows.Forms.Label();
            this.TextBoxHostPort = new System.Windows.Forms.TextBox();
            this.LbHost = new System.Windows.Forms.Label();
            this.TextBoxHostAddress = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.TabControlMain = new System.Windows.Forms.TabControl();
            this.tabSymbols = new System.Windows.Forms.TabPage();
            this.BtnApplyETFList = new System.Windows.Forms.Button();
            this.BtnDownloadAllList = new System.Windows.Forms.Button();
            this.BtnRequestAllSymbolInfo = new System.Windows.Forms.Button();
            this.BtnMatchSymbolsEOD = new System.Windows.Forms.Button();
            this.tabAccount = new System.Windows.Forms.TabPage();
            this.tabOrder = new System.Windows.Forms.TabPage();
            this.tabMarketData = new System.Windows.Forms.TabPage();
            this.tabBarTables = new System.Windows.Forms.TabPage();
            this.TabControlHistoricalData = new System.Windows.Forms.TabControl();
            this.tabPageBasic = new System.Windows.Forms.TabPage();
            this.BtnInitBt = new System.Windows.Forms.Button();
            this.BtnSaveAllBarTable = new System.Windows.Forms.Button();
            this.CkBoxAdjustDividend = new System.Windows.Forms.CheckBox();
            this.BtnHistDataApplyAdj = new System.Windows.Forms.Button();
            this.BarTableDataSourceGridView = new System.Windows.Forms.DataGridView();
            this.tabPageIB = new System.Windows.Forms.TabPage();
            this.tabPageQuandl = new System.Windows.Forms.TabPage();
            this.GBoxRequestIBHistData = new System.Windows.Forms.GroupBox();
            this.CkBoxIBHistDataRTHOnly = new System.Windows.Forms.CheckBox();
            this.LbHistDataEnd = new System.Windows.Forms.Label();
            this.LbHistDataStart = new System.Windows.Forms.Label();
            this.BtnRefreshBarTableInfoUIs = new System.Windows.Forms.Button();
            this.LbBarType = new System.Windows.Forms.Label();
            this.DateTimePickerRequestIBHistDataEnd = new System.Windows.Forms.DateTimePicker();
            this.DateTimePickerRequestIBHistDataStart = new System.Windows.Forms.DateTimePicker();
            this.LbBarFreq = new System.Windows.Forms.Label();
            this.ComboBoxBarFreq = new System.Windows.Forms.ComboBox();
            this.ComboBoxBarType = new System.Windows.Forms.ComboBox();
            this.BtnVerifyHD = new System.Windows.Forms.Button();
            this.DateTimePickerQuandlEODEnd = new System.Windows.Forms.DateTimePicker();
            this.DateTimePickerQuandlEODStart = new System.Windows.Forms.DateTimePicker();
            this.GBoxQuandlEODMerger = new System.Windows.Forms.GroupBox();
            this.BtnQuandlEODFilesMerge = new System.Windows.Forms.Button();
            this.BtnQuandlEODFilesRemove = new System.Windows.Forms.Button();
            this.BtnQuandlEODFilesAdd = new System.Windows.Forms.Button();
            this.ListViewQuandlEODFiles = new System.Windows.Forms.ListView();
            this.CkBoxQuandlEODGetAll = new System.Windows.Forms.CheckBox();
            this.BtnBtnRequestQuandlEOD = new System.Windows.Forms.Button();
            this.tabFundamentals = new System.Windows.Forms.TabPage();
            this.GBoxExportFundamentalData = new System.Windows.Forms.GroupBox();
            this.CkBoxFundamentalExportAll = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DateTimePickerFundamentalDataEnd = new System.Windows.Forms.DateTimePicker();
            this.DateTimePickerFundamentalDataStart = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.ComboBoxFundamentalDataType = new System.Windows.Forms.ComboBox();
            this.BtnRequestFundamentalOwnership = new System.Windows.Forms.Button();
            this.BtnRequestFundamentalCalendar = new System.Windows.Forms.Button();
            this.BtnRequestFundamentalAnalystEstimates = new System.Windows.Forms.Button();
            this.BtnRequestFundamentalFinancialStatements = new System.Windows.Forms.Button();
            this.BtnCancelFundamental = new System.Windows.Forms.Button();
            this.BtnRequestFundamentalFinancialSummary = new System.Windows.Forms.Button();
            this.BtnRequestFundamentalCompanyOverview = new System.Windows.Forms.Button();
            this.tabWatchlist = new System.Windows.Forms.TabPage();
            this.LvSymbols = new System.Windows.Forms.ListView();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.ComboBoxExchange = new System.Windows.Forms.ComboBox();
            this.GBoxSymbol = new System.Windows.Forms.GroupBox();
            this.TextBoxSymbolIds = new System.Windows.Forms.TextBox();
            this.LbSymbolIdNumbers = new System.Windows.Forms.Label();
            this.TextBoxSymbolSummaryF = new System.Windows.Forms.TextBox();
            this.TextBoxSymbolSummaryB = new System.Windows.Forms.TextBox();
            this.LbFinancialSummary = new System.Windows.Forms.Label();
            this.LbBusinessSummary = new System.Windows.Forms.Label();
            this.TextBoxSymbolISIN = new System.Windows.Forms.TextBox();
            this.TextBoxSymbolFullName = new System.Windows.Forms.TextBox();
            this.LbSymbolFullName = new System.Windows.Forms.Label();
            this.LbSymbolISIN = new System.Windows.Forms.Label();
            this.BtnValidateSymbol = new System.Windows.Forms.Button();
            this.ComboBoxSecType = new System.Windows.Forms.ComboBox();
            this.LbSecType = new System.Windows.Forms.Label();
            this.LbExchange = new System.Windows.Forms.Label();
            this.LbSymbol = new System.Windows.Forms.Label();
            this.TextBoxSymbolTick = new System.Windows.Forms.TextBox();
            this.tabPageData = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.StatusGroupBox.SuspendLayout();
            this.GBoxConnection.SuspendLayout();
            this.TabControlMain.SuspendLayout();
            this.tabSymbols.SuspendLayout();
            this.tabAccount.SuspendLayout();
            this.tabOrder.SuspendLayout();
            this.tabMarketData.SuspendLayout();
            this.tabBarTables.SuspendLayout();
            this.TabControlHistoricalData.SuspendLayout();
            this.tabPageBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BarTableDataSourceGridView)).BeginInit();
            this.tabPageIB.SuspendLayout();
            this.tabPageQuandl.SuspendLayout();
            this.GBoxRequestIBHistData.SuspendLayout();
            this.GBoxQuandlEODMerger.SuspendLayout();
            this.tabFundamentals.SuspendLayout();
            this.GBoxExportFundamentalData.SuspendLayout();
            this.tabWatchlist.SuspendLayout();
            this.GBoxSymbol.SuspendLayout();
            this.tabPageData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnConnect
            // 
            this.BtnConnect.BackColor = System.Drawing.Color.Green;
            this.BtnConnect.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnConnect.ForeColor = System.Drawing.Color.White;
            this.BtnConnect.Location = new System.Drawing.Point(353, 13);
            this.BtnConnect.Name = "BtnConnect";
            this.BtnConnect.Size = new System.Drawing.Size(99, 34);
            this.BtnConnect.TabIndex = 0;
            this.BtnConnect.Text = "Connect";
            this.BtnConnect.UseVisualStyleBackColor = false;
            this.BtnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // BtnAccountSum
            // 
            this.BtnAccountSum.Location = new System.Drawing.Point(196, 345);
            this.BtnAccountSum.Name = "BtnAccountSum";
            this.BtnAccountSum.Size = new System.Drawing.Size(287, 28);
            this.BtnAccountSum.TabIndex = 2;
            this.BtnAccountSum.Text = "Account Summary";
            this.BtnAccountSum.UseVisualStyleBackColor = true;
            this.BtnAccountSum.Click += new System.EventHandler(this.BtnAccountSum_Click);
            // 
            // BtnAccountUpdate
            // 
            this.BtnAccountUpdate.Location = new System.Drawing.Point(196, 307);
            this.BtnAccountUpdate.Name = "BtnAccountUpdate";
            this.BtnAccountUpdate.Size = new System.Drawing.Size(287, 32);
            this.BtnAccountUpdate.TabIndex = 3;
            this.BtnAccountUpdate.Text = "Account Update U1xxx";
            this.BtnAccountUpdate.UseVisualStyleBackColor = true;
            this.BtnAccountUpdate.Click += new System.EventHandler(this.BtnAccountUpdate_Click);
            // 
            // BtnPostions
            // 
            this.BtnPostions.Location = new System.Drawing.Point(196, 379);
            this.BtnPostions.Name = "BtnPostions";
            this.BtnPostions.Size = new System.Drawing.Size(251, 33);
            this.BtnPostions.TabIndex = 4;
            this.BtnPostions.Text = "Postion";
            this.BtnPostions.UseVisualStyleBackColor = true;
            this.BtnPostions.Click += new System.EventHandler(this.BtnPostions_Click);
            // 
            // BtnRequestIBHistData
            // 
            this.BtnRequestIBHistData.Location = new System.Drawing.Point(17, 37);
            this.BtnRequestIBHistData.Name = "BtnRequestIBHistData";
            this.BtnRequestIBHistData.Size = new System.Drawing.Size(242, 30);
            this.BtnRequestIBHistData.TabIndex = 5;
            this.BtnRequestIBHistData.Text = "Request IB Historical Data";
            this.BtnRequestIBHistData.UseVisualStyleBackColor = true;
            this.BtnRequestIBHistData.Click += new System.EventHandler(this.BtnRequestIBHistData_Click);
            // 
            // BtnOrder
            // 
            this.BtnOrder.Location = new System.Drawing.Point(44, 91);
            this.BtnOrder.Name = "BtnOrder";
            this.BtnOrder.Size = new System.Drawing.Size(287, 37);
            this.BtnOrder.TabIndex = 6;
            this.BtnOrder.Text = "Order: AAPL";
            this.BtnOrder.UseVisualStyleBackColor = true;
            this.BtnOrder.Click += new System.EventHandler(this.BtnOrder_Click);
            // 
            // BtnGetSymbolSamples
            // 
            this.BtnGetSymbolSamples.Location = new System.Drawing.Point(128, 47);
            this.BtnGetSymbolSamples.Name = "BtnGetSymbolSamples";
            this.BtnGetSymbolSamples.Size = new System.Drawing.Size(91, 22);
            this.BtnGetSymbolSamples.TabIndex = 7;
            this.BtnGetSymbolSamples.Text = "Match Symbol";
            this.BtnGetSymbolSamples.UseVisualStyleBackColor = true;
            this.BtnGetSymbolSamples.Click += new System.EventHandler(this.BtnMatchSymbol_Click);
            // 
            // BoxContract
            // 
            this.BoxContract.Location = new System.Drawing.Point(22, 47);
            this.BoxContract.Name = "BoxContract";
            this.BoxContract.Size = new System.Drawing.Size(100, 22);
            this.BoxContract.TabIndex = 8;
            this.BoxContract.Text = "AAPL";
            // 
            // BtnScanAllFundamentals
            // 
            this.BtnScanAllFundamentals.Location = new System.Drawing.Point(49, 514);
            this.BtnScanAllFundamentals.Name = "BtnScanAllFundamentals";
            this.BtnScanAllFundamentals.Size = new System.Drawing.Size(196, 27);
            this.BtnScanAllFundamentals.TabIndex = 9;
            this.BtnScanAllFundamentals.Text = "Scan All Fundamentals";
            this.BtnScanAllFundamentals.UseVisualStyleBackColor = true;
            this.BtnScanAllFundamentals.Click += new System.EventHandler(this.BtnScanFundamentalStatements_Click);
            // 
            // BtnExportBarTableCSV
            // 
            this.BtnExportBarTableCSV.Location = new System.Drawing.Point(354, 48);
            this.BtnExportBarTableCSV.Name = "BtnExportBarTableCSV";
            this.BtnExportBarTableCSV.Size = new System.Drawing.Size(242, 25);
            this.BtnExportBarTableCSV.TabIndex = 10;
            this.BtnExportBarTableCSV.Text = "Export BarTable CSV";
            this.BtnExportBarTableCSV.UseVisualStyleBackColor = true;
            this.BtnExportBarTableCSV.Click += new System.EventHandler(this.BtnExportBarTableCSV_Click);
            // 
            // BtnScanFundamentalSummary
            // 
            this.BtnScanFundamentalSummary.Location = new System.Drawing.Point(49, 547);
            this.BtnScanFundamentalSummary.Name = "BtnScanFundamentalSummary";
            this.BtnScanFundamentalSummary.Size = new System.Drawing.Size(196, 30);
            this.BtnScanFundamentalSummary.TabIndex = 11;
            this.BtnScanFundamentalSummary.Text = "Scan Fundamental Summary";
            this.BtnScanFundamentalSummary.UseVisualStyleBackColor = true;
            this.BtnScanFundamentalSummary.Click += new System.EventHandler(this.BtnScanFundamentalSummary_Click);
            // 
            // BtnSaveSampleFundamental
            // 
            this.BtnSaveSampleFundamental.Location = new System.Drawing.Point(12, 147);
            this.BtnSaveSampleFundamental.Name = "BtnSaveSampleFundamental";
            this.BtnSaveSampleFundamental.Size = new System.Drawing.Size(242, 33);
            this.BtnSaveSampleFundamental.TabIndex = 12;
            this.BtnSaveSampleFundamental.Text = "Export Fundamental Data";
            this.BtnSaveSampleFundamental.UseVisualStyleBackColor = true;
            this.BtnSaveSampleFundamental.Click += new System.EventHandler(this.BtnSaveSampleFundamental_Click);
            // 
            // BtnGetAllFundamentals
            // 
            this.BtnGetAllFundamentals.Location = new System.Drawing.Point(11, 279);
            this.BtnGetAllFundamentals.Name = "BtnGetAllFundamentals";
            this.BtnGetAllFundamentals.Size = new System.Drawing.Size(350, 27);
            this.BtnGetAllFundamentals.TabIndex = 13;
            this.BtnGetAllFundamentals.Text = "Download All Fundamentals";
            this.BtnGetAllFundamentals.UseVisualStyleBackColor = true;
            this.BtnGetAllFundamentals.Click += new System.EventHandler(this.BtnDownloadAllFundamentals_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(1036, 249);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(101, 26);
            this.BtnStop.TabIndex = 14;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // BtnLoadEOD
            // 
            this.BtnLoadEOD.Location = new System.Drawing.Point(193, 36);
            this.BtnLoadEOD.Name = "BtnLoadEOD";
            this.BtnLoadEOD.Size = new System.Drawing.Size(211, 35);
            this.BtnLoadEOD.TabIndex = 15;
            this.BtnLoadEOD.Text = "Import Quandl EOD";
            this.BtnLoadEOD.UseVisualStyleBackColor = true;
            this.BtnLoadEOD.Click += new System.EventHandler(this.BtnLoadEOD_Click);
            // 
            // BtnRequestEquityInfo
            // 
            this.BtnRequestEquityInfo.Location = new System.Drawing.Point(22, 16);
            this.BtnRequestEquityInfo.Name = "BtnRequestEquityInfo";
            this.BtnRequestEquityInfo.Size = new System.Drawing.Size(197, 25);
            this.BtnRequestEquityInfo.TabIndex = 16;
            this.BtnRequestEquityInfo.Text = "Request Symbol Info";
            this.BtnRequestEquityInfo.UseVisualStyleBackColor = true;
            this.BtnRequestEquityInfo.Click += new System.EventHandler(this.BtnSymbolInfo_Click);
            // 
            // BtnImportFolderFundamentalData
            // 
            this.BtnImportFolderFundamentalData.Location = new System.Drawing.Point(11, 312);
            this.BtnImportFolderFundamentalData.Name = "BtnImportFolderFundamentalData";
            this.BtnImportFolderFundamentalData.Size = new System.Drawing.Size(350, 25);
            this.BtnImportFolderFundamentalData.TabIndex = 17;
            this.BtnImportFolderFundamentalData.Text = "Import Folder XML Fundamental Data";
            this.BtnImportFolderFundamentalData.UseVisualStyleBackColor = true;
            this.BtnImportFolderFundamentalData.Click += new System.EventHandler(this.BtnImportFolderFundamentalData_Click);
            // 
            // BtnHistoricalDataAdjust
            // 
            this.BtnHistoricalDataAdjust.Location = new System.Drawing.Point(354, 17);
            this.BtnHistoricalDataAdjust.Name = "BtnHistoricalDataAdjust";
            this.BtnHistoricalDataAdjust.Size = new System.Drawing.Size(68, 25);
            this.BtnHistoricalDataAdjust.TabIndex = 18;
            this.BtnHistoricalDataAdjust.Text = "Adjust";
            this.BtnHistoricalDataAdjust.UseVisualStyleBackColor = true;
            this.BtnHistoricalDataAdjust.Click += new System.EventHandler(this.BtnHistoricalDataAdjust_Click);
            // 
            // BtnHistoricalDataCounterAdjust
            // 
            this.BtnHistoricalDataCounterAdjust.Location = new System.Drawing.Point(428, 17);
            this.BtnHistoricalDataCounterAdjust.Name = "BtnHistoricalDataCounterAdjust";
            this.BtnHistoricalDataCounterAdjust.Size = new System.Drawing.Size(80, 25);
            this.BtnHistoricalDataCounterAdjust.TabIndex = 19;
            this.BtnHistoricalDataCounterAdjust.Text = "Counter Adj.";
            this.BtnHistoricalDataCounterAdjust.UseVisualStyleBackColor = true;
            this.BtnHistoricalDataCounterAdjust.Click += new System.EventHandler(this.BtnHistoricalDataCounterAdjust_Click);
            // 
            // StatusProgressBar1
            // 
            this.StatusProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusProgressBar1.Location = new System.Drawing.Point(6, 249);
            this.StatusProgressBar1.Name = "StatusProgressBar1";
            this.StatusProgressBar1.Size = new System.Drawing.Size(1024, 26);
            this.StatusProgressBar1.TabIndex = 20;
            // 
            // StatusGroupBox
            // 
            this.StatusGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusGroupBox.Controls.Add(this.messageBox);
            this.StatusGroupBox.Controls.Add(this.StatusProgressBar1);
            this.StatusGroupBox.Controls.Add(this.BtnStop);
            this.StatusGroupBox.Location = new System.Drawing.Point(12, 768);
            this.StatusGroupBox.Name = "StatusGroupBox";
            this.StatusGroupBox.Size = new System.Drawing.Size(1160, 281);
            this.StatusGroupBox.TabIndex = 23;
            this.StatusGroupBox.TabStop = false;
            this.StatusGroupBox.Text = "Status / Messages";
            // 
            // messageBox
            // 
            this.messageBox.AcceptsReturn = true;
            this.messageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.messageBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageBox.Location = new System.Drawing.Point(6, 21);
            this.messageBox.Multiline = true;
            this.messageBox.Name = "messageBox";
            this.messageBox.ReadOnly = true;
            this.messageBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messageBox.Size = new System.Drawing.Size(1148, 222);
            this.messageBox.TabIndex = 11;
            // 
            // buttonMarketDepth
            // 
            this.buttonMarketDepth.Location = new System.Drawing.Point(51, 29);
            this.buttonMarketDepth.Name = "buttonMarketDepth";
            this.buttonMarketDepth.Size = new System.Drawing.Size(162, 32);
            this.buttonMarketDepth.TabIndex = 24;
            this.buttonMarketDepth.Text = "Test Market Depth";
            this.buttonMarketDepth.UseVisualStyleBackColor = true;
            this.buttonMarketDepth.Click += new System.EventHandler(this.BtnMarketDepth_Click);
            // 
            // buttonMarketData
            // 
            this.buttonMarketData.Location = new System.Drawing.Point(66, 91);
            this.buttonMarketData.Name = "buttonMarketData";
            this.buttonMarketData.Size = new System.Drawing.Size(162, 32);
            this.buttonMarketData.TabIndex = 25;
            this.buttonMarketData.Text = "Test Market Data";
            this.buttonMarketData.UseVisualStyleBackColor = true;
            this.buttonMarketData.Click += new System.EventHandler(this.BtnMarketData_Click);
            // 
            // BtnMatchSymbols
            // 
            this.BtnMatchSymbols.Location = new System.Drawing.Point(22, 154);
            this.BtnMatchSymbols.Name = "BtnMatchSymbols";
            this.BtnMatchSymbols.Size = new System.Drawing.Size(197, 20);
            this.BtnMatchSymbols.TabIndex = 26;
            this.BtnMatchSymbols.Text = "Match Symbols";
            this.BtnMatchSymbols.UseVisualStyleBackColor = true;
            this.BtnMatchSymbols.Click += new System.EventHandler(this.BtnMatchSymbols_Click);
            // 
            // BtnTest
            // 
            this.BtnTest.Location = new System.Drawing.Point(175, 418);
            this.BtnTest.Name = "BtnTest";
            this.BtnTest.Size = new System.Drawing.Size(95, 34);
            this.BtnTest.TabIndex = 27;
            this.BtnTest.Text = "Test";
            this.BtnTest.UseVisualStyleBackColor = true;
            this.BtnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // BtnRescanUnknownSymbols
            // 
            this.BtnRescanUnknownSymbols.Location = new System.Drawing.Point(22, 320);
            this.BtnRescanUnknownSymbols.Name = "BtnRescanUnknownSymbols";
            this.BtnRescanUnknownSymbols.Size = new System.Drawing.Size(197, 32);
            this.BtnRescanUnknownSymbols.TabIndex = 28;
            this.BtnRescanUnknownSymbols.Text = "Rescan Unknown Symbols";
            this.BtnRescanUnknownSymbols.UseVisualStyleBackColor = true;
            this.BtnRescanUnknownSymbols.Click += new System.EventHandler(this.BtnRescanUnknownSymbols_Click);
            // 
            // BtnImportSymbols
            // 
            this.BtnImportSymbols.Location = new System.Drawing.Point(6, 709);
            this.BtnImportSymbols.Name = "BtnImportSymbols";
            this.BtnImportSymbols.Size = new System.Drawing.Size(193, 25);
            this.BtnImportSymbols.TabIndex = 29;
            this.BtnImportSymbols.Text = "Import Symbols";
            this.BtnImportSymbols.UseVisualStyleBackColor = true;
            this.BtnImportSymbols.Click += new System.EventHandler(this.BtnImportSymbols_Click);
            // 
            // BtnExportSymbols
            // 
            this.BtnExportSymbols.Location = new System.Drawing.Point(206, 709);
            this.BtnExportSymbols.Name = "BtnExportSymbols";
            this.BtnExportSymbols.Size = new System.Drawing.Size(193, 25);
            this.BtnExportSymbols.TabIndex = 30;
            this.BtnExportSymbols.Text = "Export Symbols";
            this.BtnExportSymbols.UseVisualStyleBackColor = true;
            this.BtnExportSymbols.Click += new System.EventHandler(this.BtnExportSymbols_Click);
            // 
            // GBoxConnection
            // 
            this.GBoxConnection.Controls.Add(this.LbClientId);
            this.GBoxConnection.Controls.Add(this.TextBoxClientId);
            this.GBoxConnection.Controls.Add(this.LbPort);
            this.GBoxConnection.Controls.Add(this.TextBoxHostPort);
            this.GBoxConnection.Controls.Add(this.LbHost);
            this.GBoxConnection.Controls.Add(this.TextBoxHostAddress);
            this.GBoxConnection.Controls.Add(this.BtnConnect);
            this.GBoxConnection.Location = new System.Drawing.Point(432, 13);
            this.GBoxConnection.Name = "GBoxConnection";
            this.GBoxConnection.Size = new System.Drawing.Size(458, 53);
            this.GBoxConnection.TabIndex = 31;
            this.GBoxConnection.TabStop = false;
            this.GBoxConnection.Text = "Interactive Brokers Connection - TWS API";
            // 
            // LbClientId
            // 
            this.LbClientId.AutoSize = true;
            this.LbClientId.Location = new System.Drawing.Point(238, 24);
            this.LbClientId.Name = "LbClientId";
            this.LbClientId.Size = new System.Drawing.Size(51, 13);
            this.LbClientId.TabIndex = 35;
            this.LbClientId.Text = "Client ID";
            // 
            // TextBoxClientId
            // 
            this.TextBoxClientId.Location = new System.Drawing.Point(295, 21);
            this.TextBoxClientId.Name = "TextBoxClientId";
            this.TextBoxClientId.Size = new System.Drawing.Size(40, 22);
            this.TextBoxClientId.TabIndex = 36;
            this.TextBoxClientId.Text = "180";
            this.TextBoxClientId.TextChanged += new System.EventHandler(this.TextBoxClientId_TextChanged);
            // 
            // LbPort
            // 
            this.LbPort.AutoSize = true;
            this.LbPort.Location = new System.Drawing.Point(149, 24);
            this.LbPort.Name = "LbPort";
            this.LbPort.Size = new System.Drawing.Size(28, 13);
            this.LbPort.TabIndex = 33;
            this.LbPort.Text = "Port";
            // 
            // TextBoxHostPort
            // 
            this.TextBoxHostPort.Location = new System.Drawing.Point(186, 21);
            this.TextBoxHostPort.Name = "TextBoxHostPort";
            this.TextBoxHostPort.Size = new System.Drawing.Size(46, 22);
            this.TextBoxHostPort.TabIndex = 34;
            this.TextBoxHostPort.Text = "15060";
            this.TextBoxHostPort.TextChanged += new System.EventHandler(this.TextBoxHostPort_TextChanged);
            // 
            // LbHost
            // 
            this.LbHost.AutoSize = true;
            this.LbHost.Location = new System.Drawing.Point(6, 24);
            this.LbHost.Name = "LbHost";
            this.LbHost.Size = new System.Drawing.Size(31, 13);
            this.LbHost.TabIndex = 32;
            this.LbHost.Text = "Host";
            // 
            // TextBoxHostAddress
            // 
            this.TextBoxHostAddress.Location = new System.Drawing.Point(43, 21);
            this.TextBoxHostAddress.Name = "TextBoxHostAddress";
            this.TextBoxHostAddress.Size = new System.Drawing.Size(100, 22);
            this.TextBoxHostAddress.TabIndex = 32;
            this.TextBoxHostAddress.Text = "127.0.0.1";
            this.TextBoxHostAddress.TextChanged += new System.EventHandler(this.TextBoxHostAddress_TextChanged);
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(9, 9);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(355, 345);
            this.listView1.TabIndex = 32;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // TabControlMain
            // 
            this.TabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControlMain.Controls.Add(this.tabSymbols);
            this.TabControlMain.Controls.Add(this.tabBarTables);
            this.TabControlMain.Controls.Add(this.tabAccount);
            this.TabControlMain.Controls.Add(this.tabOrder);
            this.TabControlMain.Controls.Add(this.tabMarketData);
            this.TabControlMain.Controls.Add(this.tabFundamentals);
            this.TabControlMain.Controls.Add(this.tabWatchlist);
            this.TabControlMain.Location = new System.Drawing.Point(432, 72);
            this.TabControlMain.Name = "TabControlMain";
            this.TabControlMain.SelectedIndex = 0;
            this.TabControlMain.Size = new System.Drawing.Size(740, 690);
            this.TabControlMain.TabIndex = 35;
            // 
            // tabSymbols
            // 
            this.tabSymbols.Controls.Add(this.BtnApplyETFList);
            this.tabSymbols.Controls.Add(this.BtnDownloadAllList);
            this.tabSymbols.Controls.Add(this.BtnRequestAllSymbolInfo);
            this.tabSymbols.Controls.Add(this.BtnMatchSymbolsEOD);
            this.tabSymbols.Controls.Add(this.BtnGetSymbolSamples);
            this.tabSymbols.Controls.Add(this.BoxContract);
            this.tabSymbols.Controls.Add(this.BtnRescanUnknownSymbols);
            this.tabSymbols.Controls.Add(this.BtnMatchSymbols);
            this.tabSymbols.Controls.Add(this.BtnRequestEquityInfo);
            this.tabSymbols.Location = new System.Drawing.Point(4, 22);
            this.tabSymbols.Name = "tabSymbols";
            this.tabSymbols.Padding = new System.Windows.Forms.Padding(3);
            this.tabSymbols.Size = new System.Drawing.Size(732, 664);
            this.tabSymbols.TabIndex = 0;
            this.tabSymbols.Text = "Symbols";
            this.tabSymbols.UseVisualStyleBackColor = true;
            // 
            // BtnApplyETFList
            // 
            this.BtnApplyETFList.Location = new System.Drawing.Point(22, 416);
            this.BtnApplyETFList.Name = "BtnApplyETFList";
            this.BtnApplyETFList.Size = new System.Drawing.Size(197, 32);
            this.BtnApplyETFList.TabIndex = 32;
            this.BtnApplyETFList.Text = "Apply ETF List / NASDAQ";
            this.BtnApplyETFList.UseVisualStyleBackColor = true;
            this.BtnApplyETFList.Click += new System.EventHandler(this.BtnApplyETFList_Click);
            // 
            // BtnDownloadAllList
            // 
            this.BtnDownloadAllList.Location = new System.Drawing.Point(22, 378);
            this.BtnDownloadAllList.Name = "BtnDownloadAllList";
            this.BtnDownloadAllList.Size = new System.Drawing.Size(197, 32);
            this.BtnDownloadAllList.TabIndex = 31;
            this.BtnDownloadAllList.Text = "Download All List / NASDAQ";
            this.BtnDownloadAllList.UseVisualStyleBackColor = true;
            this.BtnDownloadAllList.Click += new System.EventHandler(this.BtnDownloadAllList_Click);
            // 
            // BtnRequestAllSymbolInfo
            // 
            this.BtnRequestAllSymbolInfo.Location = new System.Drawing.Point(22, 236);
            this.BtnRequestAllSymbolInfo.Name = "BtnRequestAllSymbolInfo";
            this.BtnRequestAllSymbolInfo.Size = new System.Drawing.Size(309, 37);
            this.BtnRequestAllSymbolInfo.TabIndex = 30;
            this.BtnRequestAllSymbolInfo.Text = "Request All Symbol Info";
            this.BtnRequestAllSymbolInfo.UseVisualStyleBackColor = true;
            this.BtnRequestAllSymbolInfo.Click += new System.EventHandler(this.BtnRequestAllSymbolInfo_Click);
            // 
            // BtnMatchSymbolsEOD
            // 
            this.BtnMatchSymbolsEOD.Location = new System.Drawing.Point(22, 180);
            this.BtnMatchSymbolsEOD.Name = "BtnMatchSymbolsEOD";
            this.BtnMatchSymbolsEOD.Size = new System.Drawing.Size(197, 20);
            this.BtnMatchSymbolsEOD.TabIndex = 29;
            this.BtnMatchSymbolsEOD.Text = "Match Symbols EOD";
            this.BtnMatchSymbolsEOD.UseVisualStyleBackColor = true;
            this.BtnMatchSymbolsEOD.Click += new System.EventHandler(this.BtnMatchSymbolsEOD_Click);
            // 
            // tabAccount
            // 
            this.tabAccount.Controls.Add(this.BtnAccountUpdate);
            this.tabAccount.Controls.Add(this.BtnAccountSum);
            this.tabAccount.Controls.Add(this.BtnPostions);
            this.tabAccount.Location = new System.Drawing.Point(4, 22);
            this.tabAccount.Name = "tabAccount";
            this.tabAccount.Padding = new System.Windows.Forms.Padding(3);
            this.tabAccount.Size = new System.Drawing.Size(732, 664);
            this.tabAccount.TabIndex = 1;
            this.tabAccount.Text = "Account Info";
            this.tabAccount.UseVisualStyleBackColor = true;
            // 
            // tabOrder
            // 
            this.tabOrder.Controls.Add(this.BtnOrder);
            this.tabOrder.Location = new System.Drawing.Point(4, 22);
            this.tabOrder.Name = "tabOrder";
            this.tabOrder.Padding = new System.Windows.Forms.Padding(3);
            this.tabOrder.Size = new System.Drawing.Size(732, 664);
            this.tabOrder.TabIndex = 5;
            this.tabOrder.Text = "Order";
            this.tabOrder.UseVisualStyleBackColor = true;
            // 
            // tabMarketData
            // 
            this.tabMarketData.Controls.Add(this.buttonMarketDepth);
            this.tabMarketData.Controls.Add(this.buttonMarketData);
            this.tabMarketData.Location = new System.Drawing.Point(4, 22);
            this.tabMarketData.Name = "tabMarketData";
            this.tabMarketData.Padding = new System.Windows.Forms.Padding(3);
            this.tabMarketData.Size = new System.Drawing.Size(732, 664);
            this.tabMarketData.TabIndex = 3;
            this.tabMarketData.Text = "Market Data";
            this.tabMarketData.UseVisualStyleBackColor = true;
            // 
            // tabBarTables
            // 
            this.tabBarTables.Controls.Add(this.TabControlHistoricalData);
            this.tabBarTables.Controls.Add(this.GBoxRequestIBHistData);
            this.tabBarTables.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabBarTables.Location = new System.Drawing.Point(4, 22);
            this.tabBarTables.Name = "tabBarTables";
            this.tabBarTables.Padding = new System.Windows.Forms.Padding(3);
            this.tabBarTables.Size = new System.Drawing.Size(732, 664);
            this.tabBarTables.TabIndex = 2;
            this.tabBarTables.Text = "BarTables";
            this.tabBarTables.UseVisualStyleBackColor = true;
            // 
            // TabControlHistoricalData
            // 
            this.TabControlHistoricalData.Controls.Add(this.tabPageBasic);
            this.TabControlHistoricalData.Controls.Add(this.tabPageData);
            this.TabControlHistoricalData.Controls.Add(this.tabPageIB);
            this.TabControlHistoricalData.Controls.Add(this.tabPageQuandl);
            this.TabControlHistoricalData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControlHistoricalData.Location = new System.Drawing.Point(3, 86);
            this.TabControlHistoricalData.Name = "TabControlHistoricalData";
            this.TabControlHistoricalData.SelectedIndex = 0;
            this.TabControlHistoricalData.Size = new System.Drawing.Size(726, 575);
            this.TabControlHistoricalData.TabIndex = 66;
            // 
            // tabPageBasic
            // 
            this.tabPageBasic.Controls.Add(this.BtnInitBt);
            this.tabPageBasic.Controls.Add(this.BtnSaveAllBarTable);
            this.tabPageBasic.Controls.Add(this.CkBoxAdjustDividend);
            this.tabPageBasic.Controls.Add(this.BtnHistDataApplyAdj);
            this.tabPageBasic.Controls.Add(this.BtnExportBarTableCSV);
            this.tabPageBasic.Controls.Add(this.BtnHistoricalDataCounterAdjust);
            this.tabPageBasic.Controls.Add(this.BtnHistoricalDataAdjust);
            this.tabPageBasic.Location = new System.Drawing.Point(4, 22);
            this.tabPageBasic.Name = "tabPageBasic";
            this.tabPageBasic.Size = new System.Drawing.Size(718, 549);
            this.tabPageBasic.TabIndex = 2;
            this.tabPageBasic.Text = "Basic";
            this.tabPageBasic.UseVisualStyleBackColor = true;
            // 
            // BtnInitBt
            // 
            this.BtnInitBt.Location = new System.Drawing.Point(536, 194);
            this.BtnInitBt.Name = "BtnInitBt";
            this.BtnInitBt.Size = new System.Drawing.Size(139, 30);
            this.BtnInitBt.TabIndex = 68;
            this.BtnInitBt.Text = "Initialize";
            this.BtnInitBt.UseVisualStyleBackColor = true;
            // 
            // BtnSaveAllBarTable
            // 
            this.BtnSaveAllBarTable.Location = new System.Drawing.Point(17, 37);
            this.BtnSaveAllBarTable.Name = "BtnSaveAllBarTable";
            this.BtnSaveAllBarTable.Size = new System.Drawing.Size(242, 30);
            this.BtnSaveAllBarTable.TabIndex = 63;
            this.BtnSaveAllBarTable.Text = "Save All BarTable";
            this.BtnSaveAllBarTable.UseVisualStyleBackColor = true;
            this.BtnSaveAllBarTable.Click += new System.EventHandler(this.BtnSaveAllBarTable_Click);
            // 
            // CkBoxAdjustDividend
            // 
            this.CkBoxAdjustDividend.AutoSize = true;
            this.CkBoxAdjustDividend.Location = new System.Drawing.Point(281, 22);
            this.CkBoxAdjustDividend.Name = "CkBoxAdjustDividend";
            this.CkBoxAdjustDividend.Size = new System.Drawing.Size(72, 17);
            this.CkBoxAdjustDividend.TabIndex = 62;
            this.CkBoxAdjustDividend.Text = "Dividend";
            this.CkBoxAdjustDividend.UseVisualStyleBackColor = true;
            // 
            // BtnHistDataApplyAdj
            // 
            this.BtnHistDataApplyAdj.Location = new System.Drawing.Point(514, 17);
            this.BtnHistDataApplyAdj.Name = "BtnHistDataApplyAdj";
            this.BtnHistDataApplyAdj.Size = new System.Drawing.Size(161, 30);
            this.BtnHistDataApplyAdj.TabIndex = 54;
            this.BtnHistDataApplyAdj.Text = "Apply Dividend / Split";
            this.BtnHistDataApplyAdj.UseVisualStyleBackColor = true;
            this.BtnHistDataApplyAdj.Click += new System.EventHandler(this.BtnHistDataApplyAdj_Click);
            // 
            // BarTableDataSourceGridView
            // 
            this.BarTableDataSourceGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BarTableDataSourceGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.BarTableDataSourceGridView.Location = new System.Drawing.Point(0, 0);
            this.BarTableDataSourceGridView.Name = "BarTableDataSourceGridView";
            this.BarTableDataSourceGridView.Size = new System.Drawing.Size(718, 108);
            this.BarTableDataSourceGridView.TabIndex = 64;
            // 
            // tabPageIB
            // 
            this.tabPageIB.Controls.Add(this.BtnRequestIBHistData);
            this.tabPageIB.Location = new System.Drawing.Point(4, 22);
            this.tabPageIB.Name = "tabPageIB";
            this.tabPageIB.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIB.Size = new System.Drawing.Size(718, 549);
            this.tabPageIB.TabIndex = 0;
            this.tabPageIB.Text = "IB/TWS";
            this.tabPageIB.UseVisualStyleBackColor = true;
            // 
            // tabPageQuandl
            // 
            this.tabPageQuandl.Controls.Add(this.DateTimePickerQuandlEODEnd);
            this.tabPageQuandl.Controls.Add(this.DateTimePickerQuandlEODStart);
            this.tabPageQuandl.Controls.Add(this.BtnVerifyHD);
            this.tabPageQuandl.Controls.Add(this.BtnLoadEOD);
            this.tabPageQuandl.Controls.Add(this.BtnBtnRequestQuandlEOD);
            this.tabPageQuandl.Controls.Add(this.CkBoxQuandlEODGetAll);
            this.tabPageQuandl.Controls.Add(this.GBoxQuandlEODMerger);
            this.tabPageQuandl.Location = new System.Drawing.Point(4, 22);
            this.tabPageQuandl.Name = "tabPageQuandl";
            this.tabPageQuandl.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageQuandl.Size = new System.Drawing.Size(718, 549);
            this.tabPageQuandl.TabIndex = 1;
            this.tabPageQuandl.Text = "Quandl";
            this.tabPageQuandl.UseVisualStyleBackColor = true;
            // 
            // GBoxRequestIBHistData
            // 
            this.GBoxRequestIBHistData.Controls.Add(this.CkBoxIBHistDataRTHOnly);
            this.GBoxRequestIBHistData.Controls.Add(this.LbHistDataEnd);
            this.GBoxRequestIBHistData.Controls.Add(this.LbHistDataStart);
            this.GBoxRequestIBHistData.Controls.Add(this.BtnRefreshBarTableInfoUIs);
            this.GBoxRequestIBHistData.Controls.Add(this.LbBarType);
            this.GBoxRequestIBHistData.Controls.Add(this.DateTimePickerRequestIBHistDataEnd);
            this.GBoxRequestIBHistData.Controls.Add(this.DateTimePickerRequestIBHistDataStart);
            this.GBoxRequestIBHistData.Controls.Add(this.LbBarFreq);
            this.GBoxRequestIBHistData.Controls.Add(this.ComboBoxBarFreq);
            this.GBoxRequestIBHistData.Controls.Add(this.ComboBoxBarType);
            this.GBoxRequestIBHistData.Dock = System.Windows.Forms.DockStyle.Top;
            this.GBoxRequestIBHistData.Location = new System.Drawing.Point(3, 3);
            this.GBoxRequestIBHistData.Name = "GBoxRequestIBHistData";
            this.GBoxRequestIBHistData.Size = new System.Drawing.Size(726, 83);
            this.GBoxRequestIBHistData.TabIndex = 53;
            this.GBoxRequestIBHistData.TabStop = false;
            this.GBoxRequestIBHistData.Text = "Request IB Historical Data";
            // 
            // CkBoxIBHistDataRTHOnly
            // 
            this.CkBoxIBHistDataRTHOnly.AutoSize = true;
            this.CkBoxIBHistDataRTHOnly.Location = new System.Drawing.Point(441, 23);
            this.CkBoxIBHistDataRTHOnly.Name = "CkBoxIBHistDataRTHOnly";
            this.CkBoxIBHistDataRTHOnly.Size = new System.Drawing.Size(71, 17);
            this.CkBoxIBHistDataRTHOnly.TabIndex = 61;
            this.CkBoxIBHistDataRTHOnly.Text = "RTH only";
            this.CkBoxIBHistDataRTHOnly.UseVisualStyleBackColor = true;
            // 
            // LbHistDataEnd
            // 
            this.LbHistDataEnd.AutoSize = true;
            this.LbHistDataEnd.Location = new System.Drawing.Point(185, 52);
            this.LbHistDataEnd.Name = "LbHistDataEnd";
            this.LbHistDataEnd.Size = new System.Drawing.Size(27, 13);
            this.LbHistDataEnd.TabIndex = 54;
            this.LbHistDataEnd.Text = "End";
            // 
            // LbHistDataStart
            // 
            this.LbHistDataStart.AutoSize = true;
            this.LbHistDataStart.Location = new System.Drawing.Point(181, 25);
            this.LbHistDataStart.Name = "LbHistDataStart";
            this.LbHistDataStart.Size = new System.Drawing.Size(31, 13);
            this.LbHistDataStart.TabIndex = 53;
            this.LbHistDataStart.Text = "Start";
            // 
            // BtnRefreshBarTableInfoUIs
            // 
            this.BtnRefreshBarTableInfoUIs.Location = new System.Drawing.Point(540, 18);
            this.BtnRefreshBarTableInfoUIs.Name = "BtnRefreshBarTableInfoUIs";
            this.BtnRefreshBarTableInfoUIs.Size = new System.Drawing.Size(80, 25);
            this.BtnRefreshBarTableInfoUIs.TabIndex = 65;
            this.BtnRefreshBarTableInfoUIs.Text = "Refresh";
            this.BtnRefreshBarTableInfoUIs.UseVisualStyleBackColor = true;
            this.BtnRefreshBarTableInfoUIs.Click += new System.EventHandler(this.BtnRefreshBarTableInfoUIs_Click);
            // 
            // LbBarType
            // 
            this.LbBarType.AutoSize = true;
            this.LbBarType.Location = new System.Drawing.Point(6, 51);
            this.LbBarType.Name = "LbBarType";
            this.LbBarType.Size = new System.Drawing.Size(49, 13);
            this.LbBarType.TabIndex = 51;
            this.LbBarType.Text = "Bar Type";
            // 
            // DateTimePickerRequestIBHistDataEnd
            // 
            this.DateTimePickerRequestIBHistDataEnd.CustomFormat = "ddd MM/dd/yyyy HH:mm:ss";
            this.DateTimePickerRequestIBHistDataEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTimePickerRequestIBHistDataEnd.Location = new System.Drawing.Point(218, 47);
            this.DateTimePickerRequestIBHistDataEnd.Name = "DateTimePickerRequestIBHistDataEnd";
            this.DateTimePickerRequestIBHistDataEnd.Size = new System.Drawing.Size(178, 22);
            this.DateTimePickerRequestIBHistDataEnd.TabIndex = 51;
            // 
            // DateTimePickerRequestIBHistDataStart
            // 
            this.DateTimePickerRequestIBHistDataStart.CustomFormat = "ddd MM/dd/yyyy HH:mm:ss";
            this.DateTimePickerRequestIBHistDataStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTimePickerRequestIBHistDataStart.Location = new System.Drawing.Point(218, 20);
            this.DateTimePickerRequestIBHistDataStart.Name = "DateTimePickerRequestIBHistDataStart";
            this.DateTimePickerRequestIBHistDataStart.Size = new System.Drawing.Size(178, 22);
            this.DateTimePickerRequestIBHistDataStart.TabIndex = 52;
            // 
            // LbBarFreq
            // 
            this.LbBarFreq.AutoSize = true;
            this.LbBarFreq.Location = new System.Drawing.Point(6, 24);
            this.LbBarFreq.Name = "LbBarFreq";
            this.LbBarFreq.Size = new System.Drawing.Size(53, 13);
            this.LbBarFreq.TabIndex = 49;
            this.LbBarFreq.Text = "Bar Freq.";
            // 
            // ComboBoxBarFreq
            // 
            this.ComboBoxBarFreq.FormattingEnabled = true;
            this.ComboBoxBarFreq.Location = new System.Drawing.Point(61, 21);
            this.ComboBoxBarFreq.Name = "ComboBoxBarFreq";
            this.ComboBoxBarFreq.Size = new System.Drawing.Size(100, 21);
            this.ComboBoxBarFreq.TabIndex = 49;
            // 
            // ComboBoxBarType
            // 
            this.ComboBoxBarType.FormattingEnabled = true;
            this.ComboBoxBarType.Location = new System.Drawing.Point(61, 48);
            this.ComboBoxBarType.Name = "ComboBoxBarType";
            this.ComboBoxBarType.Size = new System.Drawing.Size(100, 21);
            this.ComboBoxBarType.TabIndex = 50;
            // 
            // BtnVerifyHD
            // 
            this.BtnVerifyHD.Location = new System.Drawing.Point(477, 72);
            this.BtnVerifyHD.Name = "BtnVerifyHD";
            this.BtnVerifyHD.Size = new System.Drawing.Size(139, 30);
            this.BtnVerifyHD.TabIndex = 67;
            this.BtnVerifyHD.Text = "Verify HD";
            this.BtnVerifyHD.UseVisualStyleBackColor = true;
            this.BtnVerifyHD.Click += new System.EventHandler(this.BtnVerifyHD_Click);
            // 
            // DateTimePickerQuandlEODEnd
            // 
            this.DateTimePickerQuandlEODEnd.Location = new System.Drawing.Point(230, 205);
            this.DateTimePickerQuandlEODEnd.Name = "DateTimePickerQuandlEODEnd";
            this.DateTimePickerQuandlEODEnd.Size = new System.Drawing.Size(208, 22);
            this.DateTimePickerQuandlEODEnd.TabIndex = 65;
            // 
            // DateTimePickerQuandlEODStart
            // 
            this.DateTimePickerQuandlEODStart.Location = new System.Drawing.Point(230, 177);
            this.DateTimePickerQuandlEODStart.Name = "DateTimePickerQuandlEODStart";
            this.DateTimePickerQuandlEODStart.Size = new System.Drawing.Size(208, 22);
            this.DateTimePickerQuandlEODStart.TabIndex = 66;
            // 
            // GBoxQuandlEODMerger
            // 
            this.GBoxQuandlEODMerger.Controls.Add(this.BtnQuandlEODFilesMerge);
            this.GBoxQuandlEODMerger.Controls.Add(this.BtnQuandlEODFilesRemove);
            this.GBoxQuandlEODMerger.Controls.Add(this.BtnQuandlEODFilesAdd);
            this.GBoxQuandlEODMerger.Controls.Add(this.ListViewQuandlEODFiles);
            this.GBoxQuandlEODMerger.Location = new System.Drawing.Point(10, 247);
            this.GBoxQuandlEODMerger.Name = "GBoxQuandlEODMerger";
            this.GBoxQuandlEODMerger.Size = new System.Drawing.Size(696, 287);
            this.GBoxQuandlEODMerger.TabIndex = 64;
            this.GBoxQuandlEODMerger.TabStop = false;
            this.GBoxQuandlEODMerger.Text = "Merge Files";
            // 
            // BtnQuandlEODFilesMerge
            // 
            this.BtnQuandlEODFilesMerge.Location = new System.Drawing.Point(572, 253);
            this.BtnQuandlEODFilesMerge.Name = "BtnQuandlEODFilesMerge";
            this.BtnQuandlEODFilesMerge.Size = new System.Drawing.Size(113, 23);
            this.BtnQuandlEODFilesMerge.TabIndex = 53;
            this.BtnQuandlEODFilesMerge.Text = "Merge";
            this.BtnQuandlEODFilesMerge.UseVisualStyleBackColor = true;
            this.BtnQuandlEODFilesMerge.Click += new System.EventHandler(this.BtnQuandlEODFilesMerge_Click);
            // 
            // BtnQuandlEODFilesRemove
            // 
            this.BtnQuandlEODFilesRemove.Location = new System.Drawing.Point(91, 253);
            this.BtnQuandlEODFilesRemove.Name = "BtnQuandlEODFilesRemove";
            this.BtnQuandlEODFilesRemove.Size = new System.Drawing.Size(75, 23);
            this.BtnQuandlEODFilesRemove.TabIndex = 52;
            this.BtnQuandlEODFilesRemove.Text = "Remove";
            this.BtnQuandlEODFilesRemove.UseVisualStyleBackColor = true;
            this.BtnQuandlEODFilesRemove.Click += new System.EventHandler(this.BtnQuandlEODFilesRemove_Click);
            // 
            // BtnQuandlEODFilesAdd
            // 
            this.BtnQuandlEODFilesAdd.Location = new System.Drawing.Point(10, 253);
            this.BtnQuandlEODFilesAdd.Name = "BtnQuandlEODFilesAdd";
            this.BtnQuandlEODFilesAdd.Size = new System.Drawing.Size(75, 23);
            this.BtnQuandlEODFilesAdd.TabIndex = 51;
            this.BtnQuandlEODFilesAdd.Text = "Add";
            this.BtnQuandlEODFilesAdd.UseVisualStyleBackColor = true;
            this.BtnQuandlEODFilesAdd.Click += new System.EventHandler(this.BtnQuandlEODFilesAdd_Click);
            // 
            // ListViewQuandlEODFiles
            // 
            this.ListViewQuandlEODFiles.Location = new System.Drawing.Point(10, 21);
            this.ListViewQuandlEODFiles.Name = "ListViewQuandlEODFiles";
            this.ListViewQuandlEODFiles.Size = new System.Drawing.Size(675, 226);
            this.ListViewQuandlEODFiles.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ListViewQuandlEODFiles.TabIndex = 50;
            this.ListViewQuandlEODFiles.UseCompatibleStateImageBehavior = false;
            // 
            // CkBoxQuandlEODGetAll
            // 
            this.CkBoxQuandlEODGetAll.AutoSize = true;
            this.CkBoxQuandlEODGetAll.Location = new System.Drawing.Point(184, 131);
            this.CkBoxQuandlEODGetAll.Name = "CkBoxQuandlEODGetAll";
            this.CkBoxQuandlEODGetAll.Size = new System.Drawing.Size(60, 17);
            this.CkBoxQuandlEODGetAll.TabIndex = 62;
            this.CkBoxQuandlEODGetAll.Text = "Get All";
            this.CkBoxQuandlEODGetAll.UseVisualStyleBackColor = true;
            // 
            // BtnBtnRequestQuandlEOD
            // 
            this.BtnBtnRequestQuandlEOD.Location = new System.Drawing.Point(253, 123);
            this.BtnBtnRequestQuandlEOD.Name = "BtnBtnRequestQuandlEOD";
            this.BtnBtnRequestQuandlEOD.Size = new System.Drawing.Size(139, 30);
            this.BtnBtnRequestQuandlEOD.TabIndex = 63;
            this.BtnBtnRequestQuandlEOD.Text = "Request Quandl EOD";
            this.BtnBtnRequestQuandlEOD.UseVisualStyleBackColor = true;
            this.BtnBtnRequestQuandlEOD.Click += new System.EventHandler(this.BtnBtnRequestQuandlEOD_Click);
            // 
            // tabFundamentals
            // 
            this.tabFundamentals.Controls.Add(this.GBoxExportFundamentalData);
            this.tabFundamentals.Controls.Add(this.BtnRequestFundamentalOwnership);
            this.tabFundamentals.Controls.Add(this.BtnRequestFundamentalCalendar);
            this.tabFundamentals.Controls.Add(this.BtnRequestFundamentalAnalystEstimates);
            this.tabFundamentals.Controls.Add(this.BtnRequestFundamentalFinancialStatements);
            this.tabFundamentals.Controls.Add(this.BtnCancelFundamental);
            this.tabFundamentals.Controls.Add(this.BtnRequestFundamentalFinancialSummary);
            this.tabFundamentals.Controls.Add(this.BtnRequestFundamentalCompanyOverview);
            this.tabFundamentals.Controls.Add(this.BtnGetAllFundamentals);
            this.tabFundamentals.Controls.Add(this.BtnImportFolderFundamentalData);
            this.tabFundamentals.Controls.Add(this.BtnScanAllFundamentals);
            this.tabFundamentals.Controls.Add(this.BtnScanFundamentalSummary);
            this.tabFundamentals.Location = new System.Drawing.Point(4, 22);
            this.tabFundamentals.Name = "tabFundamentals";
            this.tabFundamentals.Padding = new System.Windows.Forms.Padding(3);
            this.tabFundamentals.Size = new System.Drawing.Size(732, 664);
            this.tabFundamentals.TabIndex = 4;
            this.tabFundamentals.Text = "Fundamentals";
            this.tabFundamentals.UseVisualStyleBackColor = true;
            // 
            // GBoxExportFundamentalData
            // 
            this.GBoxExportFundamentalData.Controls.Add(this.CkBoxFundamentalExportAll);
            this.GBoxExportFundamentalData.Controls.Add(this.label1);
            this.GBoxExportFundamentalData.Controls.Add(this.label2);
            this.GBoxExportFundamentalData.Controls.Add(this.DateTimePickerFundamentalDataEnd);
            this.GBoxExportFundamentalData.Controls.Add(this.DateTimePickerFundamentalDataStart);
            this.GBoxExportFundamentalData.Controls.Add(this.label4);
            this.GBoxExportFundamentalData.Controls.Add(this.ComboBoxFundamentalDataType);
            this.GBoxExportFundamentalData.Controls.Add(this.BtnSaveSampleFundamental);
            this.GBoxExportFundamentalData.Location = new System.Drawing.Point(264, 16);
            this.GBoxExportFundamentalData.Name = "GBoxExportFundamentalData";
            this.GBoxExportFundamentalData.Size = new System.Drawing.Size(268, 193);
            this.GBoxExportFundamentalData.TabIndex = 54;
            this.GBoxExportFundamentalData.TabStop = false;
            this.GBoxExportFundamentalData.Text = "Export Fundamental Data";
            // 
            // CkBoxFundamentalExportAll
            // 
            this.CkBoxFundamentalExportAll.AutoSize = true;
            this.CkBoxFundamentalExportAll.Checked = true;
            this.CkBoxFundamentalExportAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CkBoxFundamentalExportAll.Location = new System.Drawing.Point(16, 115);
            this.CkBoxFundamentalExportAll.Name = "CkBoxFundamentalExportAll";
            this.CkBoxFundamentalExportAll.Size = new System.Drawing.Size(75, 17);
            this.CkBoxFundamentalExportAll.TabIndex = 62;
            this.CkBoxFundamentalExportAll.Text = "Export All";
            this.CkBoxFundamentalExportAll.UseVisualStyleBackColor = true;
            this.CkBoxFundamentalExportAll.CheckedChanged += new System.EventHandler(this.CkBoxFundamentalExportAll_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 54;
            this.label1.Text = "End";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 53;
            this.label2.Text = "Start";
            // 
            // DateTimePickerFundamentalDataEnd
            // 
            this.DateTimePickerFundamentalDataEnd.Location = new System.Drawing.Point(46, 79);
            this.DateTimePickerFundamentalDataEnd.Name = "DateTimePickerFundamentalDataEnd";
            this.DateTimePickerFundamentalDataEnd.Size = new System.Drawing.Size(208, 22);
            this.DateTimePickerFundamentalDataEnd.TabIndex = 51;
            // 
            // DateTimePickerFundamentalDataStart
            // 
            this.DateTimePickerFundamentalDataStart.Location = new System.Drawing.Point(46, 51);
            this.DateTimePickerFundamentalDataStart.Name = "DateTimePickerFundamentalDataStart";
            this.DateTimePickerFundamentalDataStart.Size = new System.Drawing.Size(208, 22);
            this.DateTimePickerFundamentalDataStart.TabIndex = 52;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 49;
            this.label4.Text = "Type";
            // 
            // ComboBoxFundamentalDataType
            // 
            this.ComboBoxFundamentalDataType.FormattingEnabled = true;
            this.ComboBoxFundamentalDataType.Location = new System.Drawing.Point(46, 20);
            this.ComboBoxFundamentalDataType.Name = "ComboBoxFundamentalDataType";
            this.ComboBoxFundamentalDataType.Size = new System.Drawing.Size(208, 21);
            this.ComboBoxFundamentalDataType.TabIndex = 49;
            // 
            // BtnRequestFundamentalOwnership
            // 
            this.BtnRequestFundamentalOwnership.Location = new System.Drawing.Point(11, 171);
            this.BtnRequestFundamentalOwnership.Name = "BtnRequestFundamentalOwnership";
            this.BtnRequestFundamentalOwnership.Size = new System.Drawing.Size(234, 25);
            this.BtnRequestFundamentalOwnership.TabIndex = 38;
            this.BtnRequestFundamentalOwnership.Text = "Request Ownership";
            this.BtnRequestFundamentalOwnership.UseVisualStyleBackColor = true;
            this.BtnRequestFundamentalOwnership.Click += new System.EventHandler(this.BtnRequestFundamentalOwnership_Click);
            // 
            // BtnRequestFundamentalCalendar
            // 
            this.BtnRequestFundamentalCalendar.Location = new System.Drawing.Point(11, 140);
            this.BtnRequestFundamentalCalendar.Name = "BtnRequestFundamentalCalendar";
            this.BtnRequestFundamentalCalendar.Size = new System.Drawing.Size(234, 25);
            this.BtnRequestFundamentalCalendar.TabIndex = 37;
            this.BtnRequestFundamentalCalendar.Text = "Request Calendar";
            this.BtnRequestFundamentalCalendar.UseVisualStyleBackColor = true;
            this.BtnRequestFundamentalCalendar.Click += new System.EventHandler(this.BtnRequestFundamentalCalendar_Click);
            // 
            // BtnRequestFundamentalAnalystEstimates
            // 
            this.BtnRequestFundamentalAnalystEstimates.Location = new System.Drawing.Point(11, 109);
            this.BtnRequestFundamentalAnalystEstimates.Name = "BtnRequestFundamentalAnalystEstimates";
            this.BtnRequestFundamentalAnalystEstimates.Size = new System.Drawing.Size(234, 25);
            this.BtnRequestFundamentalAnalystEstimates.TabIndex = 36;
            this.BtnRequestFundamentalAnalystEstimates.Text = "Request Analyst Estimates";
            this.BtnRequestFundamentalAnalystEstimates.UseVisualStyleBackColor = true;
            this.BtnRequestFundamentalAnalystEstimates.Click += new System.EventHandler(this.BtnRequestFundamentalAnalystEstimates_Click);
            // 
            // BtnRequestFundamentalFinancialStatements
            // 
            this.BtnRequestFundamentalFinancialStatements.Location = new System.Drawing.Point(11, 78);
            this.BtnRequestFundamentalFinancialStatements.Name = "BtnRequestFundamentalFinancialStatements";
            this.BtnRequestFundamentalFinancialStatements.Size = new System.Drawing.Size(234, 25);
            this.BtnRequestFundamentalFinancialStatements.TabIndex = 35;
            this.BtnRequestFundamentalFinancialStatements.Text = "Request Financial Statements";
            this.BtnRequestFundamentalFinancialStatements.UseVisualStyleBackColor = true;
            this.BtnRequestFundamentalFinancialStatements.Click += new System.EventHandler(this.BtnRequestFundamentalFinancialStatements_Click);
            // 
            // BtnCancelFundamental
            // 
            this.BtnCancelFundamental.BackColor = System.Drawing.Color.Red;
            this.BtnCancelFundamental.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancelFundamental.ForeColor = System.Drawing.Color.White;
            this.BtnCancelFundamental.Location = new System.Drawing.Point(11, 206);
            this.BtnCancelFundamental.Name = "BtnCancelFundamental";
            this.BtnCancelFundamental.Size = new System.Drawing.Size(234, 38);
            this.BtnCancelFundamental.TabIndex = 34;
            this.BtnCancelFundamental.Text = "Clear All Fundamental Requests";
            this.BtnCancelFundamental.UseVisualStyleBackColor = false;
            this.BtnCancelFundamental.Click += new System.EventHandler(this.BtnCancelFundamental_Click);
            // 
            // BtnRequestFundamentalFinancialSummary
            // 
            this.BtnRequestFundamentalFinancialSummary.Location = new System.Drawing.Point(11, 47);
            this.BtnRequestFundamentalFinancialSummary.Name = "BtnRequestFundamentalFinancialSummary";
            this.BtnRequestFundamentalFinancialSummary.Size = new System.Drawing.Size(234, 25);
            this.BtnRequestFundamentalFinancialSummary.TabIndex = 33;
            this.BtnRequestFundamentalFinancialSummary.Text = "Request Financial Summary";
            this.BtnRequestFundamentalFinancialSummary.UseVisualStyleBackColor = true;
            this.BtnRequestFundamentalFinancialSummary.Click += new System.EventHandler(this.BtnRequestFundamentalFinancialSummary_Click);
            // 
            // BtnRequestFundamentalCompanyOverview
            // 
            this.BtnRequestFundamentalCompanyOverview.Location = new System.Drawing.Point(11, 16);
            this.BtnRequestFundamentalCompanyOverview.Name = "BtnRequestFundamentalCompanyOverview";
            this.BtnRequestFundamentalCompanyOverview.Size = new System.Drawing.Size(234, 25);
            this.BtnRequestFundamentalCompanyOverview.TabIndex = 32;
            this.BtnRequestFundamentalCompanyOverview.Text = "Request Company Overview";
            this.BtnRequestFundamentalCompanyOverview.UseVisualStyleBackColor = true;
            this.BtnRequestFundamentalCompanyOverview.Click += new System.EventHandler(this.BtnRequestFundamentalCompanyOverview_Click);
            // 
            // tabWatchlist
            // 
            this.tabWatchlist.Controls.Add(this.LvSymbols);
            this.tabWatchlist.Controls.Add(this.listBox1);
            this.tabWatchlist.Controls.Add(this.listView1);
            this.tabWatchlist.Controls.Add(this.BtnTest);
            this.tabWatchlist.Location = new System.Drawing.Point(4, 22);
            this.tabWatchlist.Name = "tabWatchlist";
            this.tabWatchlist.Size = new System.Drawing.Size(732, 664);
            this.tabWatchlist.TabIndex = 6;
            this.tabWatchlist.Text = "Watchlist";
            this.tabWatchlist.UseVisualStyleBackColor = true;
            // 
            // LvSymbols
            // 
            this.LvSymbols.Location = new System.Drawing.Point(370, 9);
            this.LvSymbols.Name = "LvSymbols";
            this.LvSymbols.Size = new System.Drawing.Size(343, 633);
            this.LvSymbols.TabIndex = 31;
            this.LvSymbols.UseCompatibleStateImageBehavior = false;
            this.LvSymbols.View = System.Windows.Forms.View.List;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(57, 482);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 95);
            this.listBox1.TabIndex = 33;
            // 
            // ComboBoxExchange
            // 
            this.ComboBoxExchange.FormattingEnabled = true;
            this.ComboBoxExchange.Location = new System.Drawing.Point(68, 47);
            this.ComboBoxExchange.Name = "ComboBoxExchange";
            this.ComboBoxExchange.Size = new System.Drawing.Size(121, 21);
            this.ComboBoxExchange.TabIndex = 21;
            // 
            // GBoxSymbol
            // 
            this.GBoxSymbol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.GBoxSymbol.Controls.Add(this.TextBoxSymbolIds);
            this.GBoxSymbol.Controls.Add(this.LbSymbolIdNumbers);
            this.GBoxSymbol.Controls.Add(this.TextBoxSymbolSummaryF);
            this.GBoxSymbol.Controls.Add(this.BtnExportSymbols);
            this.GBoxSymbol.Controls.Add(this.BtnImportSymbols);
            this.GBoxSymbol.Controls.Add(this.TextBoxSymbolSummaryB);
            this.GBoxSymbol.Controls.Add(this.LbFinancialSummary);
            this.GBoxSymbol.Controls.Add(this.LbBusinessSummary);
            this.GBoxSymbol.Controls.Add(this.TextBoxSymbolISIN);
            this.GBoxSymbol.Controls.Add(this.TextBoxSymbolFullName);
            this.GBoxSymbol.Controls.Add(this.LbSymbolFullName);
            this.GBoxSymbol.Controls.Add(this.LbSymbolISIN);
            this.GBoxSymbol.Controls.Add(this.BtnValidateSymbol);
            this.GBoxSymbol.Controls.Add(this.ComboBoxSecType);
            this.GBoxSymbol.Controls.Add(this.LbSecType);
            this.GBoxSymbol.Controls.Add(this.ComboBoxExchange);
            this.GBoxSymbol.Controls.Add(this.LbExchange);
            this.GBoxSymbol.Controls.Add(this.LbSymbol);
            this.GBoxSymbol.Controls.Add(this.TextBoxSymbolTick);
            this.GBoxSymbol.Location = new System.Drawing.Point(12, 12);
            this.GBoxSymbol.Name = "GBoxSymbol";
            this.GBoxSymbol.Size = new System.Drawing.Size(414, 750);
            this.GBoxSymbol.TabIndex = 36;
            this.GBoxSymbol.TabStop = false;
            this.GBoxSymbol.Text = "Symbol Picker";
            // 
            // TextBoxSymbolIds
            // 
            this.TextBoxSymbolIds.AcceptsReturn = true;
            this.TextBoxSymbolIds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxSymbolIds.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxSymbolIds.Location = new System.Drawing.Point(6, 236);
            this.TextBoxSymbolIds.Multiline = true;
            this.TextBoxSymbolIds.Name = "TextBoxSymbolIds";
            this.TextBoxSymbolIds.ReadOnly = true;
            this.TextBoxSymbolIds.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxSymbolIds.Size = new System.Drawing.Size(393, 74);
            this.TextBoxSymbolIds.TabIndex = 50;
            // 
            // LbSymbolIdNumbers
            // 
            this.LbSymbolIdNumbers.AutoSize = true;
            this.LbSymbolIdNumbers.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbSymbolIdNumbers.Location = new System.Drawing.Point(6, 220);
            this.LbSymbolIdNumbers.Name = "LbSymbolIdNumbers";
            this.LbSymbolIdNumbers.Size = new System.Drawing.Size(69, 13);
            this.LbSymbolIdNumbers.TabIndex = 49;
            this.LbSymbolIdNumbers.Text = "ID Numbers";
            // 
            // TextBoxSymbolSummaryF
            // 
            this.TextBoxSymbolSummaryF.AcceptsReturn = true;
            this.TextBoxSymbolSummaryF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxSymbolSummaryF.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxSymbolSummaryF.Location = new System.Drawing.Point(6, 552);
            this.TextBoxSymbolSummaryF.Multiline = true;
            this.TextBoxSymbolSummaryF.Name = "TextBoxSymbolSummaryF";
            this.TextBoxSymbolSummaryF.ReadOnly = true;
            this.TextBoxSymbolSummaryF.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxSymbolSummaryF.Size = new System.Drawing.Size(393, 149);
            this.TextBoxSymbolSummaryF.TabIndex = 48;
            // 
            // TextBoxSymbolSummaryB
            // 
            this.TextBoxSymbolSummaryB.AcceptsReturn = true;
            this.TextBoxSymbolSummaryB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxSymbolSummaryB.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxSymbolSummaryB.Location = new System.Drawing.Point(6, 329);
            this.TextBoxSymbolSummaryB.Multiline = true;
            this.TextBoxSymbolSummaryB.Name = "TextBoxSymbolSummaryB";
            this.TextBoxSymbolSummaryB.ReadOnly = true;
            this.TextBoxSymbolSummaryB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxSymbolSummaryB.Size = new System.Drawing.Size(393, 202);
            this.TextBoxSymbolSummaryB.TabIndex = 47;
            // 
            // LbFinancialSummary
            // 
            this.LbFinancialSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LbFinancialSummary.AutoSize = true;
            this.LbFinancialSummary.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbFinancialSummary.Location = new System.Drawing.Point(6, 536);
            this.LbFinancialSummary.Name = "LbFinancialSummary";
            this.LbFinancialSummary.Size = new System.Drawing.Size(105, 13);
            this.LbFinancialSummary.TabIndex = 45;
            this.LbFinancialSummary.Text = "Financial Summary";
            // 
            // LbBusinessSummary
            // 
            this.LbBusinessSummary.AutoSize = true;
            this.LbBusinessSummary.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbBusinessSummary.Location = new System.Drawing.Point(6, 313);
            this.LbBusinessSummary.Name = "LbBusinessSummary";
            this.LbBusinessSummary.Size = new System.Drawing.Size(104, 13);
            this.LbBusinessSummary.TabIndex = 43;
            this.LbBusinessSummary.Text = "Business Summary";
            // 
            // TextBoxSymbolISIN
            // 
            this.TextBoxSymbolISIN.Location = new System.Drawing.Point(55, 183);
            this.TextBoxSymbolISIN.Name = "TextBoxSymbolISIN";
            this.TextBoxSymbolISIN.ReadOnly = true;
            this.TextBoxSymbolISIN.Size = new System.Drawing.Size(134, 22);
            this.TextBoxSymbolISIN.TabIndex = 42;
            // 
            // TextBoxSymbolFullName
            // 
            this.TextBoxSymbolFullName.Location = new System.Drawing.Point(55, 155);
            this.TextBoxSymbolFullName.Name = "TextBoxSymbolFullName";
            this.TextBoxSymbolFullName.ReadOnly = true;
            this.TextBoxSymbolFullName.Size = new System.Drawing.Size(344, 22);
            this.TextBoxSymbolFullName.TabIndex = 41;
            // 
            // LbSymbolFullName
            // 
            this.LbSymbolFullName.AutoSize = true;
            this.LbSymbolFullName.Location = new System.Drawing.Point(10, 158);
            this.LbSymbolFullName.Name = "LbSymbolFullName";
            this.LbSymbolFullName.Size = new System.Drawing.Size(39, 13);
            this.LbSymbolFullName.TabIndex = 40;
            this.LbSymbolFullName.Text = "Name:";
            // 
            // LbSymbolISIN
            // 
            this.LbSymbolISIN.AutoSize = true;
            this.LbSymbolISIN.Location = new System.Drawing.Point(19, 186);
            this.LbSymbolISIN.Name = "LbSymbolISIN";
            this.LbSymbolISIN.Size = new System.Drawing.Size(30, 13);
            this.LbSymbolISIN.TabIndex = 39;
            this.LbSymbolISIN.Text = "ISIN:";
            // 
            // BtnValidateSymbol
            // 
            this.BtnValidateSymbol.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnValidateSymbol.Location = new System.Drawing.Point(13, 108);
            this.BtnValidateSymbol.Name = "BtnValidateSymbol";
            this.BtnValidateSymbol.Size = new System.Drawing.Size(390, 34);
            this.BtnValidateSymbol.TabIndex = 28;
            this.BtnValidateSymbol.Text = "Validate >>>";
            this.BtnValidateSymbol.UseVisualStyleBackColor = true;
            this.BtnValidateSymbol.Click += new System.EventHandler(this.BtnValidateSymbol_Click);
            // 
            // ComboBoxSecType
            // 
            this.ComboBoxSecType.FormattingEnabled = true;
            this.ComboBoxSecType.Location = new System.Drawing.Point(68, 74);
            this.ComboBoxSecType.Name = "ComboBoxSecType";
            this.ComboBoxSecType.Size = new System.Drawing.Size(121, 21);
            this.ComboBoxSecType.TabIndex = 37;
            // 
            // LbSecType
            // 
            this.LbSecType.AutoSize = true;
            this.LbSecType.Location = new System.Drawing.Point(10, 77);
            this.LbSecType.Name = "LbSecType";
            this.LbSecType.Size = new System.Drawing.Size(52, 13);
            this.LbSecType.TabIndex = 38;
            this.LbSecType.Text = "Sec. Type";
            // 
            // LbExchange
            // 
            this.LbExchange.AutoSize = true;
            this.LbExchange.Location = new System.Drawing.Point(6, 50);
            this.LbExchange.Name = "LbExchange";
            this.LbExchange.Size = new System.Drawing.Size(56, 13);
            this.LbExchange.TabIndex = 33;
            this.LbExchange.Text = "Exchange";
            // 
            // LbSymbol
            // 
            this.LbSymbol.AutoSize = true;
            this.LbSymbol.Location = new System.Drawing.Point(26, 24);
            this.LbSymbol.Name = "LbSymbol";
            this.LbSymbol.Size = new System.Drawing.Size(36, 13);
            this.LbSymbol.TabIndex = 32;
            this.LbSymbol.Text = "Name";
            // 
            // TextBoxSymbolTick
            // 
            this.TextBoxSymbolTick.Location = new System.Drawing.Point(68, 21);
            this.TextBoxSymbolTick.Name = "TextBoxSymbolTick";
            this.TextBoxSymbolTick.Size = new System.Drawing.Size(121, 22);
            this.TextBoxSymbolTick.TabIndex = 32;
            this.TextBoxSymbolTick.Text = "AAPL";
            // 
            // tabPageData
            // 
            this.tabPageData.Controls.Add(this.dataGridView1);
            this.tabPageData.Controls.Add(this.BarTableDataSourceGridView);
            this.tabPageData.Location = new System.Drawing.Point(4, 22);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Size = new System.Drawing.Size(718, 549);
            this.tabPageData.TabIndex = 3;
            this.tabPageData.Text = "Data";
            this.tabPageData.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 108);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(718, 441);
            this.dataGridView1.TabIndex = 65;
            // 
            // DataUtility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 1061);
            this.Controls.Add(this.GBoxSymbol);
            this.Controls.Add(this.TabControlMain);
            this.Controls.Add(this.GBoxConnection);
            this.Controls.Add(this.StatusGroupBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(900, 1100);
            this.Name = "DataUtility";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Pacmio Data Utility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataUtility_FormClosing);
            this.StatusGroupBox.ResumeLayout(false);
            this.StatusGroupBox.PerformLayout();
            this.GBoxConnection.ResumeLayout(false);
            this.GBoxConnection.PerformLayout();
            this.TabControlMain.ResumeLayout(false);
            this.tabSymbols.ResumeLayout(false);
            this.tabSymbols.PerformLayout();
            this.tabAccount.ResumeLayout(false);
            this.tabOrder.ResumeLayout(false);
            this.tabMarketData.ResumeLayout(false);
            this.tabBarTables.ResumeLayout(false);
            this.TabControlHistoricalData.ResumeLayout(false);
            this.tabPageBasic.ResumeLayout(false);
            this.tabPageBasic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BarTableDataSourceGridView)).EndInit();
            this.tabPageIB.ResumeLayout(false);
            this.tabPageQuandl.ResumeLayout(false);
            this.tabPageQuandl.PerformLayout();
            this.GBoxRequestIBHistData.ResumeLayout(false);
            this.GBoxRequestIBHistData.PerformLayout();
            this.GBoxQuandlEODMerger.ResumeLayout(false);
            this.tabFundamentals.ResumeLayout(false);
            this.GBoxExportFundamentalData.ResumeLayout(false);
            this.GBoxExportFundamentalData.PerformLayout();
            this.tabWatchlist.ResumeLayout(false);
            this.GBoxSymbol.ResumeLayout(false);
            this.GBoxSymbol.PerformLayout();
            this.tabPageData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnConnect;
        private System.Windows.Forms.Button BtnAccountSum;
        private System.Windows.Forms.Button BtnAccountUpdate;
        private System.Windows.Forms.Button BtnPostions;
        private System.Windows.Forms.Button BtnRequestIBHistData;
        private System.Windows.Forms.Button BtnOrder;
        private System.Windows.Forms.Button BtnGetSymbolSamples;
        private System.Windows.Forms.TextBox BoxContract;
        private System.Windows.Forms.Button BtnScanAllFundamentals;
        private System.Windows.Forms.Button BtnExportBarTableCSV;
        private System.Windows.Forms.Button BtnScanFundamentalSummary;
        private System.Windows.Forms.Button BtnSaveSampleFundamental;
        private System.Windows.Forms.Button BtnGetAllFundamentals;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Button BtnLoadEOD;
        private System.Windows.Forms.Button BtnRequestEquityInfo;
        private System.Windows.Forms.Button BtnImportFolderFundamentalData;
        private System.Windows.Forms.Button BtnHistoricalDataAdjust;
        private System.Windows.Forms.Button BtnHistoricalDataCounterAdjust;
        private System.Windows.Forms.ProgressBar StatusProgressBar1;
        private System.Windows.Forms.GroupBox StatusGroupBox;
        private System.Windows.Forms.Button buttonMarketDepth;
        private System.Windows.Forms.Button buttonMarketData;
        private System.Windows.Forms.Button BtnMatchSymbols;
        private System.Windows.Forms.Button BtnTest;
        private System.Windows.Forms.Button BtnRescanUnknownSymbols;
        private System.Windows.Forms.Button BtnImportSymbols;
        private System.Windows.Forms.Button BtnExportSymbols;
        private System.Windows.Forms.GroupBox GBoxConnection;
        private System.Windows.Forms.Label LbClientId;
        private System.Windows.Forms.TextBox TextBoxClientId;
        private System.Windows.Forms.Label LbPort;
        private System.Windows.Forms.TextBox TextBoxHostPort;
        private System.Windows.Forms.Label LbHost;
        private System.Windows.Forms.TextBox TextBoxHostAddress;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TabControl TabControlMain;
        private System.Windows.Forms.TabPage tabSymbols;
        private System.Windows.Forms.TabPage tabAccount;
        private System.Windows.Forms.TabPage tabMarketData;
        private System.Windows.Forms.TabPage tabBarTables;
        private System.Windows.Forms.TabPage tabFundamentals;
        private System.Windows.Forms.TabPage tabOrder;
        private System.Windows.Forms.TabPage tabWatchlist;
        private System.Windows.Forms.ListView LvSymbols;
        private System.Windows.Forms.ComboBox ComboBoxExchange;
        private System.Windows.Forms.GroupBox GBoxSymbol;
        private System.Windows.Forms.Button BtnValidateSymbol;
        private System.Windows.Forms.ComboBox ComboBoxSecType;
        private System.Windows.Forms.Label LbSecType;
        private System.Windows.Forms.Label LbExchange;
        private System.Windows.Forms.Label LbSymbol;
        private System.Windows.Forms.TextBox TextBoxSymbolTick;
        private System.Windows.Forms.Label LbSymbolISIN;
        private System.Windows.Forms.Label LbSymbolFullName;
        private System.Windows.Forms.TextBox TextBoxSymbolISIN;
        private System.Windows.Forms.TextBox TextBoxSymbolFullName;
        private System.Windows.Forms.Label LbFinancialSummary;
        private System.Windows.Forms.Label LbBusinessSummary;
        private System.Windows.Forms.TextBox messageBox;
        private System.Windows.Forms.TextBox TextBoxSymbolSummaryF;
        private System.Windows.Forms.TextBox TextBoxSymbolSummaryB;
        private System.Windows.Forms.ComboBox ComboBoxBarFreq;
        private System.Windows.Forms.ComboBox ComboBoxBarType;
        private System.Windows.Forms.DateTimePicker DateTimePickerRequestIBHistDataEnd;
        private System.Windows.Forms.GroupBox GBoxRequestIBHistData;
        private System.Windows.Forms.Label LbBarType;
        private System.Windows.Forms.DateTimePicker DateTimePickerRequestIBHistDataStart;
        private System.Windows.Forms.Label LbBarFreq;
        private System.Windows.Forms.CheckBox CkBoxIBHistDataRTHOnly;
        private System.Windows.Forms.Label LbHistDataEnd;
        private System.Windows.Forms.Label LbHistDataStart;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListView ListViewQuandlEODFiles;
        private System.Windows.Forms.Button BtnRequestFundamentalCompanyOverview;
        private System.Windows.Forms.Button BtnRequestFundamentalFinancialSummary;
        private System.Windows.Forms.Button BtnCancelFundamental;
        private System.Windows.Forms.Button BtnRequestFundamentalOwnership;
        private System.Windows.Forms.Button BtnRequestFundamentalCalendar;
        private System.Windows.Forms.Button BtnRequestFundamentalAnalystEstimates;
        private System.Windows.Forms.Button BtnRequestFundamentalFinancialStatements;
        private System.Windows.Forms.GroupBox GBoxExportFundamentalData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DateTimePickerFundamentalDataEnd;
        private System.Windows.Forms.DateTimePicker DateTimePickerFundamentalDataStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ComboBoxFundamentalDataType;
        private System.Windows.Forms.CheckBox CkBoxFundamentalExportAll;
        private System.Windows.Forms.Button BtnHistDataApplyAdj;
        private System.Windows.Forms.CheckBox CkBoxAdjustDividend;
        private System.Windows.Forms.Button BtnBtnRequestQuandlEOD;
        private System.Windows.Forms.CheckBox CkBoxQuandlEODGetAll;
        private System.Windows.Forms.GroupBox GBoxQuandlEODMerger;
        private System.Windows.Forms.Button BtnQuandlEODFilesRemove;
        private System.Windows.Forms.Button BtnQuandlEODFilesAdd;
        private System.Windows.Forms.Button BtnQuandlEODFilesMerge;
        private System.Windows.Forms.DateTimePicker DateTimePickerQuandlEODEnd;
        private System.Windows.Forms.DateTimePicker DateTimePickerQuandlEODStart;
        private System.Windows.Forms.Button BtnVerifyHD;
        private System.Windows.Forms.TextBox TextBoxSymbolIds;
        private System.Windows.Forms.Label LbSymbolIdNumbers;
        private System.Windows.Forms.Button BtnMatchSymbolsEOD;
        private System.Windows.Forms.Button BtnRequestAllSymbolInfo;
        private System.Windows.Forms.Button BtnDownloadAllList;
        private System.Windows.Forms.Button BtnApplyETFList;
        private System.Windows.Forms.Button BtnSaveAllBarTable;
        private System.Windows.Forms.DataGridView BarTableDataSourceGridView;
        private System.Windows.Forms.Button BtnRefreshBarTableInfoUIs;
        private System.Windows.Forms.TabControl TabControlHistoricalData;
        private System.Windows.Forms.TabPage tabPageBasic;
        private System.Windows.Forms.TabPage tabPageIB;
        private System.Windows.Forms.TabPage tabPageQuandl;
        private System.Windows.Forms.Button BtnInitBt;
        private System.Windows.Forms.TabPage tabPageData;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}