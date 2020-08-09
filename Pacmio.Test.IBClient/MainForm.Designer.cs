using System.Threading;
using Pacmio;
using Pacmio.IB;

namespace TestClient
{
    partial class MainForm
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
                Root.IBClientStop();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnAccountSummary = new System.Windows.Forms.Button();
            this.MainTab = new System.Windows.Forms.TabControl();
            this.tabHistoricalData = new System.Windows.Forms.TabPage();
            this.BtnHistoricalDataContractSet8 = new System.Windows.Forms.Button();
            this.BtnHistoricalDataContractSet7 = new System.Windows.Forms.Button();
            this.BtnHistoricalDataContractSet6 = new System.Windows.Forms.Button();
            this.BtnHistoricalDataContractSet5 = new System.Windows.Forms.Button();
            this.BtnHistoricalDataContractSet4 = new System.Windows.Forms.Button();
            this.BtnHistoricalDataContractSet3 = new System.Windows.Forms.Button();
            this.BtnHistoricalDataContractSet2 = new System.Windows.Forms.Button();
            this.BtnHistoricalDataContractSet1 = new System.Windows.Forms.Button();
            this.BtnHistoricalDataConfigMinuteLastWeek = new System.Windows.Forms.Button();
            this.BtnHistoricalDataConfigDailyFull = new System.Windows.Forms.Button();
            this.BtnLoadMultiHistoricalChart = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.BtnApplyTradeLogToChart = new System.Windows.Forms.Button();
            this.BtnCloseChart = new System.Windows.Forms.Button();
            this.ListViewAllCharts = new System.Windows.Forms.ListView();
            this.BtnAlignCharts = new System.Windows.Forms.Button();
            this.BtnTestRealTimeBars = new System.Windows.Forms.Button();
            this.BtnRequestHistoricalTicks = new System.Windows.Forms.Button();
            this.BtnChartsUpdateAll = new System.Windows.Forms.Button();
            this.BtnLoadHistoricalChart = new System.Windows.Forms.Button();
            this.tabContract = new System.Windows.Forms.TabPage();
            this.GroupBoxContractInfo = new System.Windows.Forms.GroupBox();
            this.TextBoxSymbolFullName = new System.Windows.Forms.TextBox();
            this.LbSymbolISIN = new System.Windows.Forms.Label();
            this.TextBoxSymbolIds = new System.Windows.Forms.TextBox();
            this.LbSymbolFullName = new System.Windows.Forms.Label();
            this.LbSymbolIdNumbers = new System.Windows.Forms.Label();
            this.TextBoxSymbolISIN = new System.Windows.Forms.TextBox();
            this.TextBoxSymbolSummaryF = new System.Windows.Forms.TextBox();
            this.LbBusinessSummary = new System.Windows.Forms.Label();
            this.LbFinancialSummary = new System.Windows.Forms.Label();
            this.TextBoxSymbolSummaryB = new System.Windows.Forms.TextBox();
            this.BtnGetContractInfo = new System.Windows.Forms.Button();
            this.TextBoxSearchSymbol = new System.Windows.Forms.TextBox();
            this.BtnSearchSymbol = new System.Windows.Forms.Button();
            this.tabMarketData = new System.Windows.Forms.TabPage();
            this.BtnMarketDataSyncTicks = new System.Windows.Forms.Button();
            this.BtnCancelAllScanner = new System.Windows.Forms.Button();
            this.BtnRequestMarketDepth = new System.Windows.Forms.Button();
            this.LabelWatchListName = new System.Windows.Forms.Label();
            this.BtnRequestScannerParameter = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.LabelGenericTickList = new System.Windows.Forms.Label();
            this.BtnRequestScanner = new System.Windows.Forms.Button();
            this.TextBoxGenericTickList = new System.Windows.Forms.TextBox();
            this.BtnMarketDataAddMultiContracts = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.BtnMarketDataAddContract = new System.Windows.Forms.Button();
            this.tabOrder = new System.Windows.Forms.TabPage();
            this.BtnTestMassiveOrder = new System.Windows.Forms.Button();
            this.GroupBoxOrderSetting = new System.Windows.Forms.GroupBox();
            this.BtnOrderSettingApplyCurrentPrice = new System.Windows.Forms.Button();
            this.TextBoxOrderSettingQuantity = new System.Windows.Forms.TextBox();
            this.ComboxBoxOrderSettingType = new System.Windows.Forms.ComboBox();
            this.ComboBoxOrderSettingTIF = new System.Windows.Forms.ComboBox();
            this.LabelOrderSettingQuantity = new System.Windows.Forms.Label();
            this.BtnOrderSettingPlaceMultiOrder = new System.Windows.Forms.Button();
            this.BtnModifyOrder = new System.Windows.Forms.Button();
            this.BtnGlobalCancel = new System.Windows.Forms.Button();
            this.LabelOrderSettingType = new System.Windows.Forms.Label();
            this.BtnCancelOrder = new System.Windows.Forms.Button();
            this.LabelOrderSettingTIF = new System.Windows.Forms.Label();
            this.BtnOrderSettingOrderBraket = new System.Windows.Forms.Button();
            this.TextBoxOrderSettingLimitPrice = new System.Windows.Forms.TextBox();
            this.CheckBoxOrderWhatIf = new System.Windows.Forms.CheckBox();
            this.LabelOrderSettingGTD = new System.Windows.Forms.Label();
            this.TextBoxOrderSettingStopPrice = new System.Windows.Forms.TextBox();
            this.DateTimePickerOrderSettingGTD = new System.Windows.Forms.DateTimePicker();
            this.LabelOrderSettingLimitPrice = new System.Windows.Forms.Label();
            this.BtnOrderSettingPlaceOrder = new System.Windows.Forms.Button();
            this.LabelOrderSettingStopPrice = new System.Windows.Forms.Label();
            this.GroupBoxPositions = new System.Windows.Forms.GroupBox();
            this.BtnPositionCloseSelected = new System.Windows.Forms.Button();
            this.BtnRequestPostion = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.executionsGroup = new System.Windows.Forms.GroupBox();
            this.BtnExportExecTradeLog = new System.Windows.Forms.Button();
            this.BtnRequestExecData = new System.Windows.Forms.Button();
            this.ib_banner = new System.Windows.Forms.PictureBox();
            this.BtnGetCompletedOrders = new System.Windows.Forms.Button();
            this.BtnGetOpenOrders = new System.Windows.Forms.Button();
            this.tabSimulation = new System.Windows.Forms.TabPage();
            this.BtnSetupSimulation = new System.Windows.Forms.Button();
            this.TextBoxRunAllSimulationInitialAccountValue = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.BtnRunAllSimulation = new System.Windows.Forms.Button();
            this.BtnArmLiveTrade = new System.Windows.Forms.Button();
            this.tabAccount = new System.Windows.Forms.TabPage();
            this.BtnSubscribePnL = new System.Windows.Forms.Button();
            this.BtnRequestPnL = new System.Windows.Forms.Button();
            this.TreeViewAccount = new System.Windows.Forms.TreeView();
            this.tabFileData = new System.Windows.Forms.TabPage();
            this.BtnApplyDefaultDownloadPeriod = new System.Windows.Forms.Button();
            this.BtnMatchSymbols = new System.Windows.Forms.Button();
            this.BtnImportNasdaq = new System.Windows.Forms.Button();
            this.BtnUpdateContracts = new System.Windows.Forms.Button();
            this.BtnImportSymbols = new System.Windows.Forms.Button();
            this.BtnDownloadBarTable = new System.Windows.Forms.Button();
            this.BtnReDownloadBarTable = new System.Windows.Forms.Button();
            this.BtnCleanUpDuplicateStock = new System.Windows.Forms.Button();
            this.GroupBoxQuandlTool = new System.Windows.Forms.GroupBox();
            this.BtmImportQuandlBlob = new System.Windows.Forms.Button();
            this.BtnAddQuandlFile = new System.Windows.Forms.Button();
            this.BtnMergeQuandlFile = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.ListViewQuandlFileMerge = new System.Windows.Forms.ListView();
            this.BtnExtractSymbols = new System.Windows.Forms.Button();
            this.BtnTestMassiveSample = new System.Windows.Forms.Button();
            this.BtnDownloadMultiTables = new System.Windows.Forms.Button();
            this.BtnExportContracts = new System.Windows.Forms.Button();
            this.BtnImportContracts = new System.Windows.Forms.Button();
            this.BtnValidUSSymbol = new System.Windows.Forms.Button();
            this.BtnMarketDataFormHide = new System.Windows.Forms.Button();
            this.BtnMarketDataFormShow = new System.Windows.Forms.Button();
            this.BtnFormatSymbolsList = new System.Windows.Forms.Button();
            this.DownloadBarTableDetialedProgressBar = new System.Windows.Forms.ProgressBar();
            this.TextBoxMultiContracts = new System.Windows.Forms.RichTextBox();
            this.CheckBoxChartToCurrent = new System.Windows.Forms.CheckBox();
            this.LabelBarFreq = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DateTimePickerHistoricalDataStop = new System.Windows.Forms.DateTimePicker();
            this.SelectHistoricalDataBarType = new System.Windows.Forms.ComboBox();
            this.SelectHistoricalDataBarFreq = new System.Windows.Forms.ComboBox();
            this.DateTimePickerHistoricalDataStart = new System.Windows.Forms.DateTimePicker();
            this.ListBoxAccount = new System.Windows.Forms.ListBox();
            this.LbStatus = new System.Windows.Forms.Label();
            this.CheckBoxSingleContractUseSmart = new System.Windows.Forms.CheckBox();
            this.SelectBoxSingleContractExchange = new System.Windows.Forms.ComboBox();
            this.LabelSingleContractExchange = new System.Windows.Forms.Label();
            this.SelectBoxSingleContractSecurityType = new System.Windows.Forms.ComboBox();
            this.LabelSingleContractType = new System.Windows.Forms.Label();
            this.LabelSingleContractName = new System.Windows.Forms.Label();
            this.TextBoxSingleContractName = new System.Windows.Forms.TextBox();
            this.MainProgBar = new System.Windows.Forms.ProgressBar();
            this.RequestPnL = new System.Windows.Forms.Button();
            this.BtnMasterCancel = new System.Windows.Forms.Button();
            this.TextBoxIPAddress = new System.Windows.Forms.TextBox();
            this.GroupBoxSingleContract = new System.Windows.Forms.GroupBox();
            this.TextBoxValidCountryCode = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LabelSingleContractStrike = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.TextBoxSingleContractStrike = new System.Windows.Forms.TextBox();
            this.DateTimePickerSingleContractExpire = new System.Windows.Forms.DateTimePicker();
            this.LabelSingleContractExpire = new System.Windows.Forms.Label();
            this.LabelBarType = new System.Windows.Forms.Label();
            this.GroupBoxBarTableSetting = new System.Windows.Forms.GroupBox();
            this.GroupBoxMultiContracts = new System.Windows.Forms.GroupBox();
            this.MainTab.SuspendLayout();
            this.tabHistoricalData.SuspendLayout();
            this.tabContract.SuspendLayout();
            this.GroupBoxContractInfo.SuspendLayout();
            this.tabMarketData.SuspendLayout();
            this.tabOrder.SuspendLayout();
            this.GroupBoxOrderSetting.SuspendLayout();
            this.GroupBoxPositions.SuspendLayout();
            this.executionsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ib_banner)).BeginInit();
            this.tabSimulation.SuspendLayout();
            this.tabAccount.SuspendLayout();
            this.tabFileData.SuspendLayout();
            this.GroupBoxQuandlTool.SuspendLayout();
            this.GroupBoxSingleContract.SuspendLayout();
            this.GroupBoxBarTableSetting.SuspendLayout();
            this.GroupBoxMultiContracts.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConnect.Location = new System.Drawing.Point(16, 803);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 40);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // btnAccountSummary
            // 
            this.btnAccountSummary.Location = new System.Drawing.Point(6, 6);
            this.btnAccountSummary.Name = "btnAccountSummary";
            this.btnAccountSummary.Size = new System.Drawing.Size(170, 23);
            this.btnAccountSummary.TabIndex = 2;
            this.btnAccountSummary.Text = "Request Account Summary";
            this.btnAccountSummary.UseVisualStyleBackColor = true;
            this.btnAccountSummary.Click += new System.EventHandler(this.BtnAccountSummary_Click);
            // 
            // MainTab
            // 
            this.MainTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainTab.Controls.Add(this.tabHistoricalData);
            this.MainTab.Controls.Add(this.tabContract);
            this.MainTab.Controls.Add(this.tabMarketData);
            this.MainTab.Controls.Add(this.tabOrder);
            this.MainTab.Controls.Add(this.tabSimulation);
            this.MainTab.Controls.Add(this.tabAccount);
            this.MainTab.Controls.Add(this.tabFileData);
            this.MainTab.Location = new System.Drawing.Point(12, 190);
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(960, 510);
            this.MainTab.TabIndex = 3;
            // 
            // tabHistoricalData
            // 
            this.tabHistoricalData.Controls.Add(this.BtnHistoricalDataContractSet8);
            this.tabHistoricalData.Controls.Add(this.BtnHistoricalDataContractSet7);
            this.tabHistoricalData.Controls.Add(this.BtnHistoricalDataContractSet6);
            this.tabHistoricalData.Controls.Add(this.BtnHistoricalDataContractSet5);
            this.tabHistoricalData.Controls.Add(this.BtnHistoricalDataContractSet4);
            this.tabHistoricalData.Controls.Add(this.BtnHistoricalDataContractSet3);
            this.tabHistoricalData.Controls.Add(this.BtnHistoricalDataContractSet2);
            this.tabHistoricalData.Controls.Add(this.BtnHistoricalDataContractSet1);
            this.tabHistoricalData.Controls.Add(this.BtnHistoricalDataConfigMinuteLastWeek);
            this.tabHistoricalData.Controls.Add(this.BtnHistoricalDataConfigDailyFull);
            this.tabHistoricalData.Controls.Add(this.BtnLoadMultiHistoricalChart);
            this.tabHistoricalData.Controls.Add(this.label12);
            this.tabHistoricalData.Controls.Add(this.BtnApplyTradeLogToChart);
            this.tabHistoricalData.Controls.Add(this.BtnCloseChart);
            this.tabHistoricalData.Controls.Add(this.ListViewAllCharts);
            this.tabHistoricalData.Controls.Add(this.BtnAlignCharts);
            this.tabHistoricalData.Controls.Add(this.BtnTestRealTimeBars);
            this.tabHistoricalData.Controls.Add(this.BtnRequestHistoricalTicks);
            this.tabHistoricalData.Controls.Add(this.BtnChartsUpdateAll);
            this.tabHistoricalData.Controls.Add(this.BtnLoadHistoricalChart);
            this.tabHistoricalData.Location = new System.Drawing.Point(4, 22);
            this.tabHistoricalData.Name = "tabHistoricalData";
            this.tabHistoricalData.Padding = new System.Windows.Forms.Padding(3);
            this.tabHistoricalData.Size = new System.Drawing.Size(952, 484);
            this.tabHistoricalData.TabIndex = 1;
            this.tabHistoricalData.Text = "Historical Data";
            this.tabHistoricalData.UseVisualStyleBackColor = true;
            // 
            // BtnHistoricalDataContractSet8
            // 
            this.BtnHistoricalDataContractSet8.Location = new System.Drawing.Point(364, 35);
            this.BtnHistoricalDataContractSet8.Name = "BtnHistoricalDataContractSet8";
            this.BtnHistoricalDataContractSet8.Size = new System.Drawing.Size(45, 23);
            this.BtnHistoricalDataContractSet8.TabIndex = 56;
            this.BtnHistoricalDataContractSet8.Text = "FB";
            this.BtnHistoricalDataContractSet8.UseVisualStyleBackColor = true;
            this.BtnHistoricalDataContractSet8.Click += new System.EventHandler(this.BtnHistoricalDataContractSet8_Click);
            // 
            // BtnHistoricalDataContractSet7
            // 
            this.BtnHistoricalDataContractSet7.Location = new System.Drawing.Point(313, 35);
            this.BtnHistoricalDataContractSet7.Name = "BtnHistoricalDataContractSet7";
            this.BtnHistoricalDataContractSet7.Size = new System.Drawing.Size(45, 23);
            this.BtnHistoricalDataContractSet7.TabIndex = 55;
            this.BtnHistoricalDataContractSet7.Text = "AAPL";
            this.BtnHistoricalDataContractSet7.UseVisualStyleBackColor = true;
            this.BtnHistoricalDataContractSet7.Click += new System.EventHandler(this.BtnHistoricalDataContractSet7_Click);
            // 
            // BtnHistoricalDataContractSet6
            // 
            this.BtnHistoricalDataContractSet6.Location = new System.Drawing.Point(262, 35);
            this.BtnHistoricalDataContractSet6.Name = "BtnHistoricalDataContractSet6";
            this.BtnHistoricalDataContractSet6.Size = new System.Drawing.Size(45, 23);
            this.BtnHistoricalDataContractSet6.TabIndex = 54;
            this.BtnHistoricalDataContractSet6.Text = "TSLA";
            this.BtnHistoricalDataContractSet6.UseVisualStyleBackColor = true;
            this.BtnHistoricalDataContractSet6.Click += new System.EventHandler(this.BtnHistoricalDataContractSet6_Click);
            // 
            // BtnHistoricalDataContractSet5
            // 
            this.BtnHistoricalDataContractSet5.Location = new System.Drawing.Point(211, 35);
            this.BtnHistoricalDataContractSet5.Name = "BtnHistoricalDataContractSet5";
            this.BtnHistoricalDataContractSet5.Size = new System.Drawing.Size(45, 23);
            this.BtnHistoricalDataContractSet5.TabIndex = 53;
            this.BtnHistoricalDataContractSet5.Text = "GLD";
            this.BtnHistoricalDataContractSet5.UseVisualStyleBackColor = true;
            this.BtnHistoricalDataContractSet5.Click += new System.EventHandler(this.BtnHistoricalDataContractSet5_Click);
            // 
            // BtnHistoricalDataContractSet4
            // 
            this.BtnHistoricalDataContractSet4.Location = new System.Drawing.Point(160, 35);
            this.BtnHistoricalDataContractSet4.Name = "BtnHistoricalDataContractSet4";
            this.BtnHistoricalDataContractSet4.Size = new System.Drawing.Size(45, 23);
            this.BtnHistoricalDataContractSet4.TabIndex = 51;
            this.BtnHistoricalDataContractSet4.Text = "IWM";
            this.BtnHistoricalDataContractSet4.UseVisualStyleBackColor = true;
            this.BtnHistoricalDataContractSet4.Click += new System.EventHandler(this.BtnHistoricalDataContractSet4_Click);
            // 
            // BtnHistoricalDataContractSet3
            // 
            this.BtnHistoricalDataContractSet3.Location = new System.Drawing.Point(109, 35);
            this.BtnHistoricalDataContractSet3.Name = "BtnHistoricalDataContractSet3";
            this.BtnHistoricalDataContractSet3.Size = new System.Drawing.Size(45, 23);
            this.BtnHistoricalDataContractSet3.TabIndex = 50;
            this.BtnHistoricalDataContractSet3.Text = "VXX";
            this.BtnHistoricalDataContractSet3.UseVisualStyleBackColor = true;
            this.BtnHistoricalDataContractSet3.Click += new System.EventHandler(this.BtnHistoricalDataContractSet3_Click);
            // 
            // BtnHistoricalDataContractSet2
            // 
            this.BtnHistoricalDataContractSet2.Location = new System.Drawing.Point(58, 35);
            this.BtnHistoricalDataContractSet2.Name = "BtnHistoricalDataContractSet2";
            this.BtnHistoricalDataContractSet2.Size = new System.Drawing.Size(45, 23);
            this.BtnHistoricalDataContractSet2.TabIndex = 49;
            this.BtnHistoricalDataContractSet2.Text = "SPY";
            this.BtnHistoricalDataContractSet2.UseVisualStyleBackColor = true;
            this.BtnHistoricalDataContractSet2.Click += new System.EventHandler(this.BtnHistoricalDataContractSet2_Click);
            // 
            // BtnHistoricalDataContractSet1
            // 
            this.BtnHistoricalDataContractSet1.Location = new System.Drawing.Point(7, 35);
            this.BtnHistoricalDataContractSet1.Name = "BtnHistoricalDataContractSet1";
            this.BtnHistoricalDataContractSet1.Size = new System.Drawing.Size(45, 23);
            this.BtnHistoricalDataContractSet1.TabIndex = 48;
            this.BtnHistoricalDataContractSet1.Text = "QQQ";
            this.BtnHistoricalDataContractSet1.UseVisualStyleBackColor = true;
            this.BtnHistoricalDataContractSet1.Click += new System.EventHandler(this.BtnHistoricalDataContractSet1_Click);
            // 
            // BtnHistoricalDataConfigMinuteLastWeek
            // 
            this.BtnHistoricalDataConfigMinuteLastWeek.Location = new System.Drawing.Point(211, 6);
            this.BtnHistoricalDataConfigMinuteLastWeek.Name = "BtnHistoricalDataConfigMinuteLastWeek";
            this.BtnHistoricalDataConfigMinuteLastWeek.Size = new System.Drawing.Size(198, 23);
            this.BtnHistoricalDataConfigMinuteLastWeek.TabIndex = 47;
            this.BtnHistoricalDataConfigMinuteLastWeek.Text = "1 Minute / Last Week";
            this.BtnHistoricalDataConfigMinuteLastWeek.UseVisualStyleBackColor = true;
            this.BtnHistoricalDataConfigMinuteLastWeek.Click += new System.EventHandler(this.BtnHistoricalDataConfigMinuteLastWeek_Click);
            // 
            // BtnHistoricalDataConfigDailyFull
            // 
            this.BtnHistoricalDataConfigDailyFull.Location = new System.Drawing.Point(7, 6);
            this.BtnHistoricalDataConfigDailyFull.Name = "BtnHistoricalDataConfigDailyFull";
            this.BtnHistoricalDataConfigDailyFull.Size = new System.Drawing.Size(198, 23);
            this.BtnHistoricalDataConfigDailyFull.TabIndex = 46;
            this.BtnHistoricalDataConfigDailyFull.Text = "Daily / Full";
            this.BtnHistoricalDataConfigDailyFull.UseVisualStyleBackColor = true;
            this.BtnHistoricalDataConfigDailyFull.Click += new System.EventHandler(this.BtnHistoricalDataConfigDailyFull_Click);
            // 
            // BtnLoadMultiHistoricalChart
            // 
            this.BtnLoadMultiHistoricalChart.Location = new System.Drawing.Point(7, 108);
            this.BtnLoadMultiHistoricalChart.Name = "BtnLoadMultiHistoricalChart";
            this.BtnLoadMultiHistoricalChart.Size = new System.Drawing.Size(230, 23);
            this.BtnLoadMultiHistoricalChart.TabIndex = 45;
            this.BtnLoadMultiHistoricalChart.Text = "Show Multi Historical Chart";
            this.BtnLoadMultiHistoricalChart.UseVisualStyleBackColor = true;
            this.BtnLoadMultiHistoricalChart.Click += new System.EventHandler(this.BtnLoadMultiHistoricalChart_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(261, 84);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 13);
            this.label12.TabIndex = 44;
            this.label12.Text = "Chart List";
            // 
            // BtnApplyTradeLogToChart
            // 
            this.BtnApplyTradeLogToChart.Location = new System.Drawing.Point(695, 88);
            this.BtnApplyTradeLogToChart.Name = "BtnApplyTradeLogToChart";
            this.BtnApplyTradeLogToChart.Size = new System.Drawing.Size(230, 23);
            this.BtnApplyTradeLogToChart.TabIndex = 43;
            this.BtnApplyTradeLogToChart.Text = "Apply Trade Log";
            this.BtnApplyTradeLogToChart.UseVisualStyleBackColor = true;
            this.BtnApplyTradeLogToChart.Click += new System.EventHandler(this.BtnApplyTradeLogToChart_Click);
            // 
            // BtnCloseChart
            // 
            this.BtnCloseChart.Location = new System.Drawing.Point(7, 166);
            this.BtnCloseChart.Name = "BtnCloseChart";
            this.BtnCloseChart.Size = new System.Drawing.Size(230, 23);
            this.BtnCloseChart.TabIndex = 37;
            this.BtnCloseChart.Text = "Close Chart";
            this.BtnCloseChart.UseVisualStyleBackColor = true;
            // 
            // ListViewAllCharts
            // 
            this.ListViewAllCharts.HideSelection = false;
            this.ListViewAllCharts.Location = new System.Drawing.Point(264, 100);
            this.ListViewAllCharts.Name = "ListViewAllCharts";
            this.ListViewAllCharts.Size = new System.Drawing.Size(279, 238);
            this.ListViewAllCharts.TabIndex = 36;
            this.ListViewAllCharts.UseCompatibleStateImageBehavior = false;
            // 
            // BtnAlignCharts
            // 
            this.BtnAlignCharts.Location = new System.Drawing.Point(7, 250);
            this.BtnAlignCharts.Name = "BtnAlignCharts";
            this.BtnAlignCharts.Size = new System.Drawing.Size(230, 26);
            this.BtnAlignCharts.TabIndex = 34;
            this.BtnAlignCharts.Text = "Align All Charts";
            this.BtnAlignCharts.UseVisualStyleBackColor = true;
            this.BtnAlignCharts.Click += new System.EventHandler(this.BtnAlignCharts_Click);
            // 
            // BtnTestRealTimeBars
            // 
            this.BtnTestRealTimeBars.Location = new System.Drawing.Point(695, 29);
            this.BtnTestRealTimeBars.Name = "BtnTestRealTimeBars";
            this.BtnTestRealTimeBars.Size = new System.Drawing.Size(230, 23);
            this.BtnTestRealTimeBars.TabIndex = 9;
            this.BtnTestRealTimeBars.Text = "Test Realtime Bars";
            this.BtnTestRealTimeBars.UseVisualStyleBackColor = true;
            this.BtnTestRealTimeBars.Click += new System.EventHandler(this.BtnTestRealTimeBars_Click);
            // 
            // BtnRequestHistoricalTicks
            // 
            this.BtnRequestHistoricalTicks.Location = new System.Drawing.Point(695, 58);
            this.BtnRequestHistoricalTicks.Name = "BtnRequestHistoricalTicks";
            this.BtnRequestHistoricalTicks.Size = new System.Drawing.Size(230, 23);
            this.BtnRequestHistoricalTicks.TabIndex = 3;
            this.BtnRequestHistoricalTicks.Text = "Historical Ticks";
            this.BtnRequestHistoricalTicks.UseVisualStyleBackColor = true;
            this.BtnRequestHistoricalTicks.Click += new System.EventHandler(this.BtnRequestHistoricalTicks_Click);
            // 
            // BtnChartsUpdateAll
            // 
            this.BtnChartsUpdateAll.Location = new System.Drawing.Point(7, 221);
            this.BtnChartsUpdateAll.Name = "BtnChartsUpdateAll";
            this.BtnChartsUpdateAll.Size = new System.Drawing.Size(230, 23);
            this.BtnChartsUpdateAll.TabIndex = 2;
            this.BtnChartsUpdateAll.Text = "Update All To Date w/ Tick";
            this.BtnChartsUpdateAll.UseVisualStyleBackColor = true;
            this.BtnChartsUpdateAll.Click += new System.EventHandler(this.BtnChartsUpdateAll_Click);
            // 
            // BtnLoadHistoricalChart
            // 
            this.BtnLoadHistoricalChart.BackColor = System.Drawing.Color.YellowGreen;
            this.BtnLoadHistoricalChart.Location = new System.Drawing.Point(7, 79);
            this.BtnLoadHistoricalChart.Name = "BtnLoadHistoricalChart";
            this.BtnLoadHistoricalChart.Size = new System.Drawing.Size(230, 23);
            this.BtnLoadHistoricalChart.TabIndex = 0;
            this.BtnLoadHistoricalChart.Text = "Show Historical Chart";
            this.BtnLoadHistoricalChart.UseVisualStyleBackColor = false;
            this.BtnLoadHistoricalChart.Click += new System.EventHandler(this.BtnLoadHistoricalChart_Click);
            // 
            // tabContract
            // 
            this.tabContract.Controls.Add(this.GroupBoxContractInfo);
            this.tabContract.Controls.Add(this.BtnGetContractInfo);
            this.tabContract.Controls.Add(this.TextBoxSearchSymbol);
            this.tabContract.Controls.Add(this.BtnSearchSymbol);
            this.tabContract.Location = new System.Drawing.Point(4, 22);
            this.tabContract.Name = "tabContract";
            this.tabContract.Size = new System.Drawing.Size(952, 484);
            this.tabContract.TabIndex = 2;
            this.tabContract.Text = "Contract";
            this.tabContract.UseVisualStyleBackColor = true;
            // 
            // GroupBoxContractInfo
            // 
            this.GroupBoxContractInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxContractInfo.Controls.Add(this.TextBoxSymbolFullName);
            this.GroupBoxContractInfo.Controls.Add(this.LbSymbolISIN);
            this.GroupBoxContractInfo.Controls.Add(this.TextBoxSymbolIds);
            this.GroupBoxContractInfo.Controls.Add(this.LbSymbolFullName);
            this.GroupBoxContractInfo.Controls.Add(this.LbSymbolIdNumbers);
            this.GroupBoxContractInfo.Controls.Add(this.TextBoxSymbolISIN);
            this.GroupBoxContractInfo.Controls.Add(this.TextBoxSymbolSummaryF);
            this.GroupBoxContractInfo.Controls.Add(this.LbBusinessSummary);
            this.GroupBoxContractInfo.Controls.Add(this.LbFinancialSummary);
            this.GroupBoxContractInfo.Controls.Add(this.TextBoxSymbolSummaryB);
            this.GroupBoxContractInfo.Location = new System.Drawing.Point(398, 3);
            this.GroupBoxContractInfo.Name = "GroupBoxContractInfo";
            this.GroupBoxContractInfo.Size = new System.Drawing.Size(551, 478);
            this.GroupBoxContractInfo.TabIndex = 63;
            this.GroupBoxContractInfo.TabStop = false;
            this.GroupBoxContractInfo.Text = "Contract Information";
            // 
            // TextBoxSymbolFullName
            // 
            this.TextBoxSymbolFullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSymbolFullName.Location = new System.Drawing.Point(57, 21);
            this.TextBoxSymbolFullName.Name = "TextBoxSymbolFullName";
            this.TextBoxSymbolFullName.ReadOnly = true;
            this.TextBoxSymbolFullName.Size = new System.Drawing.Size(472, 22);
            this.TextBoxSymbolFullName.TabIndex = 54;
            // 
            // LbSymbolISIN
            // 
            this.LbSymbolISIN.AutoSize = true;
            this.LbSymbolISIN.Location = new System.Drawing.Point(21, 52);
            this.LbSymbolISIN.Name = "LbSymbolISIN";
            this.LbSymbolISIN.Size = new System.Drawing.Size(30, 13);
            this.LbSymbolISIN.TabIndex = 52;
            this.LbSymbolISIN.Text = "ISIN:";
            // 
            // TextBoxSymbolIds
            // 
            this.TextBoxSymbolIds.AcceptsReturn = true;
            this.TextBoxSymbolIds.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSymbolIds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxSymbolIds.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxSymbolIds.Location = new System.Drawing.Point(8, 104);
            this.TextBoxSymbolIds.Multiline = true;
            this.TextBoxSymbolIds.Name = "TextBoxSymbolIds";
            this.TextBoxSymbolIds.ReadOnly = true;
            this.TextBoxSymbolIds.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxSymbolIds.Size = new System.Drawing.Size(532, 150);
            this.TextBoxSymbolIds.TabIndex = 61;
            // 
            // LbSymbolFullName
            // 
            this.LbSymbolFullName.AutoSize = true;
            this.LbSymbolFullName.Location = new System.Drawing.Point(12, 24);
            this.LbSymbolFullName.Name = "LbSymbolFullName";
            this.LbSymbolFullName.Size = new System.Drawing.Size(39, 13);
            this.LbSymbolFullName.TabIndex = 53;
            this.LbSymbolFullName.Text = "Name:";
            // 
            // LbSymbolIdNumbers
            // 
            this.LbSymbolIdNumbers.AutoSize = true;
            this.LbSymbolIdNumbers.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbSymbolIdNumbers.Location = new System.Drawing.Point(8, 88);
            this.LbSymbolIdNumbers.Name = "LbSymbolIdNumbers";
            this.LbSymbolIdNumbers.Size = new System.Drawing.Size(69, 13);
            this.LbSymbolIdNumbers.TabIndex = 60;
            this.LbSymbolIdNumbers.Text = "ID Numbers";
            // 
            // TextBoxSymbolISIN
            // 
            this.TextBoxSymbolISIN.Location = new System.Drawing.Point(57, 49);
            this.TextBoxSymbolISIN.Name = "TextBoxSymbolISIN";
            this.TextBoxSymbolISIN.ReadOnly = true;
            this.TextBoxSymbolISIN.Size = new System.Drawing.Size(134, 22);
            this.TextBoxSymbolISIN.TabIndex = 55;
            // 
            // TextBoxSymbolSummaryF
            // 
            this.TextBoxSymbolSummaryF.AcceptsReturn = true;
            this.TextBoxSymbolSummaryF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSymbolSummaryF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxSymbolSummaryF.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxSymbolSummaryF.Location = new System.Drawing.Point(8, 413);
            this.TextBoxSymbolSummaryF.Multiline = true;
            this.TextBoxSymbolSummaryF.Name = "TextBoxSymbolSummaryF";
            this.TextBoxSymbolSummaryF.ReadOnly = true;
            this.TextBoxSymbolSummaryF.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxSymbolSummaryF.Size = new System.Drawing.Size(532, 288);
            this.TextBoxSymbolSummaryF.TabIndex = 59;
            // 
            // LbBusinessSummary
            // 
            this.LbBusinessSummary.AutoSize = true;
            this.LbBusinessSummary.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbBusinessSummary.Location = new System.Drawing.Point(8, 257);
            this.LbBusinessSummary.Name = "LbBusinessSummary";
            this.LbBusinessSummary.Size = new System.Drawing.Size(104, 13);
            this.LbBusinessSummary.TabIndex = 56;
            this.LbBusinessSummary.Text = "Business Summary";
            // 
            // LbFinancialSummary
            // 
            this.LbFinancialSummary.AutoSize = true;
            this.LbFinancialSummary.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbFinancialSummary.Location = new System.Drawing.Point(8, 397);
            this.LbFinancialSummary.Name = "LbFinancialSummary";
            this.LbFinancialSummary.Size = new System.Drawing.Size(105, 13);
            this.LbFinancialSummary.TabIndex = 57;
            this.LbFinancialSummary.Text = "Financial Summary";
            // 
            // TextBoxSymbolSummaryB
            // 
            this.TextBoxSymbolSummaryB.AcceptsReturn = true;
            this.TextBoxSymbolSummaryB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSymbolSummaryB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxSymbolSummaryB.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxSymbolSummaryB.Location = new System.Drawing.Point(8, 273);
            this.TextBoxSymbolSummaryB.Multiline = true;
            this.TextBoxSymbolSummaryB.Name = "TextBoxSymbolSummaryB";
            this.TextBoxSymbolSummaryB.ReadOnly = true;
            this.TextBoxSymbolSummaryB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxSymbolSummaryB.Size = new System.Drawing.Size(532, 120);
            this.TextBoxSymbolSummaryB.TabIndex = 58;
            // 
            // BtnGetContractInfo
            // 
            this.BtnGetContractInfo.Location = new System.Drawing.Point(17, 119);
            this.BtnGetContractInfo.Name = "BtnGetContractInfo";
            this.BtnGetContractInfo.Size = new System.Drawing.Size(260, 22);
            this.BtnGetContractInfo.TabIndex = 64;
            this.BtnGetContractInfo.Text = "Get Info";
            this.BtnGetContractInfo.UseVisualStyleBackColor = true;
            this.BtnGetContractInfo.Click += new System.EventHandler(this.BtnGetContractInfo_Click);
            // 
            // TextBoxSearchSymbol
            // 
            this.TextBoxSearchSymbol.Location = new System.Drawing.Point(17, 18);
            this.TextBoxSearchSymbol.Name = "TextBoxSearchSymbol";
            this.TextBoxSearchSymbol.Size = new System.Drawing.Size(375, 22);
            this.TextBoxSearchSymbol.TabIndex = 62;
            // 
            // BtnSearchSymbol
            // 
            this.BtnSearchSymbol.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSearchSymbol.Location = new System.Drawing.Point(17, 46);
            this.BtnSearchSymbol.Name = "BtnSearchSymbol";
            this.BtnSearchSymbol.Size = new System.Drawing.Size(375, 34);
            this.BtnSearchSymbol.TabIndex = 51;
            this.BtnSearchSymbol.Text = "Search >>>";
            this.BtnSearchSymbol.UseVisualStyleBackColor = true;
            this.BtnSearchSymbol.Click += new System.EventHandler(this.BtnSearchSymbol_Click);
            // 
            // tabMarketData
            // 
            this.tabMarketData.Controls.Add(this.BtnMarketDataSyncTicks);
            this.tabMarketData.Controls.Add(this.BtnCancelAllScanner);
            this.tabMarketData.Controls.Add(this.BtnRequestMarketDepth);
            this.tabMarketData.Controls.Add(this.LabelWatchListName);
            this.tabMarketData.Controls.Add(this.BtnRequestScannerParameter);
            this.tabMarketData.Controls.Add(this.textBox2);
            this.tabMarketData.Controls.Add(this.LabelGenericTickList);
            this.tabMarketData.Controls.Add(this.BtnRequestScanner);
            this.tabMarketData.Controls.Add(this.TextBoxGenericTickList);
            this.tabMarketData.Controls.Add(this.BtnMarketDataAddMultiContracts);
            this.tabMarketData.Controls.Add(this.button4);
            this.tabMarketData.Controls.Add(this.BtnMarketDataAddContract);
            this.tabMarketData.Location = new System.Drawing.Point(4, 22);
            this.tabMarketData.Name = "tabMarketData";
            this.tabMarketData.Size = new System.Drawing.Size(952, 484);
            this.tabMarketData.TabIndex = 3;
            this.tabMarketData.Text = "Market Data";
            this.tabMarketData.UseVisualStyleBackColor = true;
            // 
            // BtnMarketDataSyncTicks
            // 
            this.BtnMarketDataSyncTicks.Location = new System.Drawing.Point(82, 82);
            this.BtnMarketDataSyncTicks.Name = "BtnMarketDataSyncTicks";
            this.BtnMarketDataSyncTicks.Size = new System.Drawing.Size(100, 23);
            this.BtnMarketDataSyncTicks.TabIndex = 50;
            this.BtnMarketDataSyncTicks.Text = "Sync Ticks";
            this.BtnMarketDataSyncTicks.UseVisualStyleBackColor = true;
            this.BtnMarketDataSyncTicks.Click += new System.EventHandler(this.BtnMarketDataSyncTicks_Click);
            // 
            // BtnCancelAllScanner
            // 
            this.BtnCancelAllScanner.Location = new System.Drawing.Point(506, 274);
            this.BtnCancelAllScanner.Name = "BtnCancelAllScanner";
            this.BtnCancelAllScanner.Size = new System.Drawing.Size(162, 23);
            this.BtnCancelAllScanner.TabIndex = 9;
            this.BtnCancelAllScanner.Text = "Cancel All Scanner";
            this.BtnCancelAllScanner.UseVisualStyleBackColor = true;
            this.BtnCancelAllScanner.Click += new System.EventHandler(this.BtnCancelAllScanner_Click);
            // 
            // BtnRequestMarketDepth
            // 
            this.BtnRequestMarketDepth.Location = new System.Drawing.Point(262, 70);
            this.BtnRequestMarketDepth.Name = "BtnRequestMarketDepth";
            this.BtnRequestMarketDepth.Size = new System.Drawing.Size(133, 23);
            this.BtnRequestMarketDepth.TabIndex = 5;
            this.BtnRequestMarketDepth.Text = "Request Market Depth";
            this.BtnRequestMarketDepth.UseVisualStyleBackColor = true;
            this.BtnRequestMarketDepth.Click += new System.EventHandler(this.BtnRequestMarketDepth_Click);
            // 
            // LabelWatchListName
            // 
            this.LabelWatchListName.AutoSize = true;
            this.LabelWatchListName.Location = new System.Drawing.Point(44, 44);
            this.LabelWatchListName.Name = "LabelWatchListName";
            this.LabelWatchListName.Size = new System.Drawing.Size(59, 13);
            this.LabelWatchListName.TabIndex = 49;
            this.LabelWatchListName.Text = "List Name:";
            // 
            // BtnRequestScannerParameter
            // 
            this.BtnRequestScannerParameter.Location = new System.Drawing.Point(506, 245);
            this.BtnRequestScannerParameter.Name = "BtnRequestScannerParameter";
            this.BtnRequestScannerParameter.Size = new System.Drawing.Size(162, 23);
            this.BtnRequestScannerParameter.TabIndex = 8;
            this.BtnRequestScannerParameter.Text = "Request Parameter";
            this.BtnRequestScannerParameter.UseVisualStyleBackColor = true;
            this.BtnRequestScannerParameter.Click += new System.EventHandler(this.BtnRequestScannerParameter_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(109, 41);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(135, 22);
            this.textBox2.TabIndex = 48;
            this.textBox2.Text = "Market Data";
            // 
            // LabelGenericTickList
            // 
            this.LabelGenericTickList.AutoSize = true;
            this.LabelGenericTickList.Location = new System.Drawing.Point(11, 16);
            this.LabelGenericTickList.Name = "LabelGenericTickList";
            this.LabelGenericTickList.Size = new System.Drawing.Size(92, 13);
            this.LabelGenericTickList.TabIndex = 47;
            this.LabelGenericTickList.Text = "Generic Tick List:";
            // 
            // BtnRequestScanner
            // 
            this.BtnRequestScanner.Location = new System.Drawing.Point(506, 216);
            this.BtnRequestScanner.Name = "BtnRequestScanner";
            this.BtnRequestScanner.Size = new System.Drawing.Size(162, 23);
            this.BtnRequestScanner.TabIndex = 6;
            this.BtnRequestScanner.Text = "Request Scanner";
            this.BtnRequestScanner.UseVisualStyleBackColor = true;
            this.BtnRequestScanner.Click += new System.EventHandler(this.BtnRequestScanner_Click);
            // 
            // TextBoxGenericTickList
            // 
            this.TextBoxGenericTickList.Location = new System.Drawing.Point(109, 13);
            this.TextBoxGenericTickList.Name = "TextBoxGenericTickList";
            this.TextBoxGenericTickList.Size = new System.Drawing.Size(135, 22);
            this.TextBoxGenericTickList.TabIndex = 19;
            this.TextBoxGenericTickList.Text = "236,375";
            // 
            // BtnMarketDataAddMultiContracts
            // 
            this.BtnMarketDataAddMultiContracts.Location = new System.Drawing.Point(262, 41);
            this.BtnMarketDataAddMultiContracts.Name = "BtnMarketDataAddMultiContracts";
            this.BtnMarketDataAddMultiContracts.Size = new System.Drawing.Size(133, 23);
            this.BtnMarketDataAddMultiContracts.TabIndex = 4;
            this.BtnMarketDataAddMultiContracts.Text = "Add Multi Contracts";
            this.BtnMarketDataAddMultiContracts.UseVisualStyleBackColor = true;
            this.BtnMarketDataAddMultiContracts.Click += new System.EventHandler(this.BtnMarketDataAddMultiContracts_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(273, 99);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Remove Tick";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // BtnMarketDataAddContract
            // 
            this.BtnMarketDataAddContract.Location = new System.Drawing.Point(262, 13);
            this.BtnMarketDataAddContract.Name = "BtnMarketDataAddContract";
            this.BtnMarketDataAddContract.Size = new System.Drawing.Size(133, 23);
            this.BtnMarketDataAddContract.TabIndex = 2;
            this.BtnMarketDataAddContract.Text = "Add Contract";
            this.BtnMarketDataAddContract.UseVisualStyleBackColor = true;
            this.BtnMarketDataAddContract.Click += new System.EventHandler(this.BtnMarketDataAddContract_Click);
            // 
            // tabOrder
            // 
            this.tabOrder.Controls.Add(this.BtnTestMassiveOrder);
            this.tabOrder.Controls.Add(this.GroupBoxOrderSetting);
            this.tabOrder.Controls.Add(this.GroupBoxPositions);
            this.tabOrder.Controls.Add(this.executionsGroup);
            this.tabOrder.Controls.Add(this.ib_banner);
            this.tabOrder.Controls.Add(this.BtnGetCompletedOrders);
            this.tabOrder.Controls.Add(this.BtnGetOpenOrders);
            this.tabOrder.Location = new System.Drawing.Point(4, 22);
            this.tabOrder.Name = "tabOrder";
            this.tabOrder.Size = new System.Drawing.Size(952, 484);
            this.tabOrder.TabIndex = 8;
            this.tabOrder.Text = "Order";
            this.tabOrder.UseVisualStyleBackColor = true;
            // 
            // BtnTestMassiveOrder
            // 
            this.BtnTestMassiveOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.BtnTestMassiveOrder.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnTestMassiveOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnTestMassiveOrder.Location = new System.Drawing.Point(253, 33);
            this.BtnTestMassiveOrder.Name = "BtnTestMassiveOrder";
            this.BtnTestMassiveOrder.Size = new System.Drawing.Size(183, 29);
            this.BtnTestMassiveOrder.TabIndex = 83;
            this.BtnTestMassiveOrder.Text = "Test Massive Order";
            this.BtnTestMassiveOrder.UseVisualStyleBackColor = false;
            this.BtnTestMassiveOrder.Click += new System.EventHandler(this.BtnTestMassiveOrder_Click);
            // 
            // GroupBoxOrderSetting
            // 
            this.GroupBoxOrderSetting.Controls.Add(this.BtnOrderSettingApplyCurrentPrice);
            this.GroupBoxOrderSetting.Controls.Add(this.TextBoxOrderSettingQuantity);
            this.GroupBoxOrderSetting.Controls.Add(this.ComboxBoxOrderSettingType);
            this.GroupBoxOrderSetting.Controls.Add(this.ComboBoxOrderSettingTIF);
            this.GroupBoxOrderSetting.Controls.Add(this.LabelOrderSettingQuantity);
            this.GroupBoxOrderSetting.Controls.Add(this.BtnOrderSettingPlaceMultiOrder);
            this.GroupBoxOrderSetting.Controls.Add(this.BtnModifyOrder);
            this.GroupBoxOrderSetting.Controls.Add(this.BtnGlobalCancel);
            this.GroupBoxOrderSetting.Controls.Add(this.LabelOrderSettingType);
            this.GroupBoxOrderSetting.Controls.Add(this.BtnCancelOrder);
            this.GroupBoxOrderSetting.Controls.Add(this.LabelOrderSettingTIF);
            this.GroupBoxOrderSetting.Controls.Add(this.BtnOrderSettingOrderBraket);
            this.GroupBoxOrderSetting.Controls.Add(this.TextBoxOrderSettingLimitPrice);
            this.GroupBoxOrderSetting.Controls.Add(this.CheckBoxOrderWhatIf);
            this.GroupBoxOrderSetting.Controls.Add(this.LabelOrderSettingGTD);
            this.GroupBoxOrderSetting.Controls.Add(this.TextBoxOrderSettingStopPrice);
            this.GroupBoxOrderSetting.Controls.Add(this.DateTimePickerOrderSettingGTD);
            this.GroupBoxOrderSetting.Controls.Add(this.LabelOrderSettingLimitPrice);
            this.GroupBoxOrderSetting.Controls.Add(this.BtnOrderSettingPlaceOrder);
            this.GroupBoxOrderSetting.Controls.Add(this.LabelOrderSettingStopPrice);
            this.GroupBoxOrderSetting.Location = new System.Drawing.Point(7, 12);
            this.GroupBoxOrderSetting.Name = "GroupBoxOrderSetting";
            this.GroupBoxOrderSetting.Size = new System.Drawing.Size(210, 465);
            this.GroupBoxOrderSetting.TabIndex = 82;
            this.GroupBoxOrderSetting.TabStop = false;
            this.GroupBoxOrderSetting.Text = "Order Setting";
            // 
            // BtnOrderSettingApplyCurrentPrice
            // 
            this.BtnOrderSettingApplyCurrentPrice.Location = new System.Drawing.Point(13, 213);
            this.BtnOrderSettingApplyCurrentPrice.Name = "BtnOrderSettingApplyCurrentPrice";
            this.BtnOrderSettingApplyCurrentPrice.Size = new System.Drawing.Size(183, 23);
            this.BtnOrderSettingApplyCurrentPrice.TabIndex = 84;
            this.BtnOrderSettingApplyCurrentPrice.Text = "Apply Current Price";
            this.BtnOrderSettingApplyCurrentPrice.UseVisualStyleBackColor = true;
            // 
            // TextBoxOrderSettingQuantity
            // 
            this.TextBoxOrderSettingQuantity.Location = new System.Drawing.Point(69, 21);
            this.TextBoxOrderSettingQuantity.Name = "TextBoxOrderSettingQuantity";
            this.TextBoxOrderSettingQuantity.Size = new System.Drawing.Size(126, 22);
            this.TextBoxOrderSettingQuantity.TabIndex = 66;
            this.TextBoxOrderSettingQuantity.Text = "100";
            // 
            // ComboxBoxOrderSettingType
            // 
            this.ComboxBoxOrderSettingType.FormattingEnabled = true;
            this.ComboxBoxOrderSettingType.Location = new System.Drawing.Point(54, 49);
            this.ComboxBoxOrderSettingType.Name = "ComboxBoxOrderSettingType";
            this.ComboxBoxOrderSettingType.Size = new System.Drawing.Size(141, 21);
            this.ComboxBoxOrderSettingType.TabIndex = 67;
            this.ComboxBoxOrderSettingType.Text = " Market";
            // 
            // ComboBoxOrderSettingTIF
            // 
            this.ComboBoxOrderSettingTIF.FormattingEnabled = true;
            this.ComboBoxOrderSettingTIF.Location = new System.Drawing.Point(54, 132);
            this.ComboBoxOrderSettingTIF.Name = "ComboBoxOrderSettingTIF";
            this.ComboBoxOrderSettingTIF.Size = new System.Drawing.Size(141, 21);
            this.ComboBoxOrderSettingTIF.TabIndex = 68;
            this.ComboBoxOrderSettingTIF.Text = "Day";
            // 
            // LabelOrderSettingQuantity
            // 
            this.LabelOrderSettingQuantity.AutoSize = true;
            this.LabelOrderSettingQuantity.Location = new System.Drawing.Point(9, 25);
            this.LabelOrderSettingQuantity.Name = "LabelOrderSettingQuantity";
            this.LabelOrderSettingQuantity.Size = new System.Drawing.Size(54, 13);
            this.LabelOrderSettingQuantity.TabIndex = 69;
            this.LabelOrderSettingQuantity.Text = "Quantity:";
            // 
            // BtnOrderSettingPlaceMultiOrder
            // 
            this.BtnOrderSettingPlaceMultiOrder.Location = new System.Drawing.Point(13, 306);
            this.BtnOrderSettingPlaceMultiOrder.Name = "BtnOrderSettingPlaceMultiOrder";
            this.BtnOrderSettingPlaceMultiOrder.Size = new System.Drawing.Size(183, 23);
            this.BtnOrderSettingPlaceMultiOrder.TabIndex = 12;
            this.BtnOrderSettingPlaceMultiOrder.Text = "Multi Order";
            this.BtnOrderSettingPlaceMultiOrder.UseVisualStyleBackColor = true;
            this.BtnOrderSettingPlaceMultiOrder.Click += new System.EventHandler(this.TestMassOrder_Click);
            // 
            // BtnModifyOrder
            // 
            this.BtnModifyOrder.Location = new System.Drawing.Point(13, 370);
            this.BtnModifyOrder.Name = "BtnModifyOrder";
            this.BtnModifyOrder.Size = new System.Drawing.Size(182, 23);
            this.BtnModifyOrder.TabIndex = 7;
            this.BtnModifyOrder.Text = "Modify Order";
            this.BtnModifyOrder.UseVisualStyleBackColor = true;
            this.BtnModifyOrder.Click += new System.EventHandler(this.BtnModifyOrder_Click);
            // 
            // BtnGlobalCancel
            // 
            this.BtnGlobalCancel.Location = new System.Drawing.Point(13, 428);
            this.BtnGlobalCancel.Name = "BtnGlobalCancel";
            this.BtnGlobalCancel.Size = new System.Drawing.Size(182, 23);
            this.BtnGlobalCancel.TabIndex = 6;
            this.BtnGlobalCancel.Text = "Global Cancel";
            this.BtnGlobalCancel.UseVisualStyleBackColor = true;
            this.BtnGlobalCancel.Click += new System.EventHandler(this.BtnGlobalCancel_Click);
            // 
            // LabelOrderSettingType
            // 
            this.LabelOrderSettingType.AutoSize = true;
            this.LabelOrderSettingType.Location = new System.Drawing.Point(15, 52);
            this.LabelOrderSettingType.Name = "LabelOrderSettingType";
            this.LabelOrderSettingType.Size = new System.Drawing.Size(33, 13);
            this.LabelOrderSettingType.TabIndex = 70;
            this.LabelOrderSettingType.Text = "Type:";
            // 
            // BtnCancelOrder
            // 
            this.BtnCancelOrder.Location = new System.Drawing.Point(13, 399);
            this.BtnCancelOrder.Name = "BtnCancelOrder";
            this.BtnCancelOrder.Size = new System.Drawing.Size(182, 23);
            this.BtnCancelOrder.TabIndex = 8;
            this.BtnCancelOrder.Text = "Cancel Order";
            this.BtnCancelOrder.UseVisualStyleBackColor = true;
            // 
            // LabelOrderSettingTIF
            // 
            this.LabelOrderSettingTIF.AutoSize = true;
            this.LabelOrderSettingTIF.Location = new System.Drawing.Point(23, 135);
            this.LabelOrderSettingTIF.Name = "LabelOrderSettingTIF";
            this.LabelOrderSettingTIF.Size = new System.Drawing.Size(25, 13);
            this.LabelOrderSettingTIF.TabIndex = 71;
            this.LabelOrderSettingTIF.Text = "TIF:";
            // 
            // BtnOrderSettingOrderBraket
            // 
            this.BtnOrderSettingOrderBraket.Location = new System.Drawing.Point(13, 277);
            this.BtnOrderSettingOrderBraket.Name = "BtnOrderSettingOrderBraket";
            this.BtnOrderSettingOrderBraket.Size = new System.Drawing.Size(183, 23);
            this.BtnOrderSettingOrderBraket.TabIndex = 79;
            this.BtnOrderSettingOrderBraket.Text = "Braket Order";
            this.BtnOrderSettingOrderBraket.UseVisualStyleBackColor = true;
            this.BtnOrderSettingOrderBraket.Click += new System.EventHandler(this.BtnOrderBraket_Click);
            // 
            // TextBoxOrderSettingLimitPrice
            // 
            this.TextBoxOrderSettingLimitPrice.Location = new System.Drawing.Point(54, 76);
            this.TextBoxOrderSettingLimitPrice.Name = "TextBoxOrderSettingLimitPrice";
            this.TextBoxOrderSettingLimitPrice.Size = new System.Drawing.Size(141, 22);
            this.TextBoxOrderSettingLimitPrice.TabIndex = 72;
            this.TextBoxOrderSettingLimitPrice.Text = "100";
            // 
            // CheckBoxOrderWhatIf
            // 
            this.CheckBoxOrderWhatIf.AutoSize = true;
            this.CheckBoxOrderWhatIf.Location = new System.Drawing.Point(69, 187);
            this.CheckBoxOrderWhatIf.Name = "CheckBoxOrderWhatIf";
            this.CheckBoxOrderWhatIf.Size = new System.Drawing.Size(64, 17);
            this.CheckBoxOrderWhatIf.TabIndex = 76;
            this.CheckBoxOrderWhatIf.Text = "What If";
            this.CheckBoxOrderWhatIf.UseVisualStyleBackColor = true;
            // 
            // LabelOrderSettingGTD
            // 
            this.LabelOrderSettingGTD.AutoSize = true;
            this.LabelOrderSettingGTD.Location = new System.Drawing.Point(16, 163);
            this.LabelOrderSettingGTD.Name = "LabelOrderSettingGTD";
            this.LabelOrderSettingGTD.Size = new System.Drawing.Size(32, 13);
            this.LabelOrderSettingGTD.TabIndex = 78;
            this.LabelOrderSettingGTD.Text = "GTD:";
            // 
            // TextBoxOrderSettingStopPrice
            // 
            this.TextBoxOrderSettingStopPrice.Location = new System.Drawing.Point(54, 104);
            this.TextBoxOrderSettingStopPrice.Name = "TextBoxOrderSettingStopPrice";
            this.TextBoxOrderSettingStopPrice.Size = new System.Drawing.Size(141, 22);
            this.TextBoxOrderSettingStopPrice.TabIndex = 73;
            this.TextBoxOrderSettingStopPrice.Text = "100";
            // 
            // DateTimePickerOrderSettingGTD
            // 
            this.DateTimePickerOrderSettingGTD.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.DateTimePickerOrderSettingGTD.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTimePickerOrderSettingGTD.Location = new System.Drawing.Point(54, 159);
            this.DateTimePickerOrderSettingGTD.Name = "DateTimePickerOrderSettingGTD";
            this.DateTimePickerOrderSettingGTD.Size = new System.Drawing.Size(141, 22);
            this.DateTimePickerOrderSettingGTD.TabIndex = 77;
            this.DateTimePickerOrderSettingGTD.Value = new System.DateTime(2020, 3, 20, 17, 0, 0, 0);
            // 
            // LabelOrderSettingLimitPrice
            // 
            this.LabelOrderSettingLimitPrice.AutoSize = true;
            this.LabelOrderSettingLimitPrice.Location = new System.Drawing.Point(14, 79);
            this.LabelOrderSettingLimitPrice.Name = "LabelOrderSettingLimitPrice";
            this.LabelOrderSettingLimitPrice.Size = new System.Drawing.Size(34, 13);
            this.LabelOrderSettingLimitPrice.TabIndex = 74;
            this.LabelOrderSettingLimitPrice.Text = "Limit:";
            // 
            // BtnOrderSettingPlaceOrder
            // 
            this.BtnOrderSettingPlaceOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.BtnOrderSettingPlaceOrder.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnOrderSettingPlaceOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnOrderSettingPlaceOrder.Location = new System.Drawing.Point(13, 242);
            this.BtnOrderSettingPlaceOrder.Name = "BtnOrderSettingPlaceOrder";
            this.BtnOrderSettingPlaceOrder.Size = new System.Drawing.Size(183, 29);
            this.BtnOrderSettingPlaceOrder.TabIndex = 5;
            this.BtnOrderSettingPlaceOrder.Text = "Order";
            this.BtnOrderSettingPlaceOrder.UseVisualStyleBackColor = false;
            this.BtnOrderSettingPlaceOrder.Click += new System.EventHandler(this.BtnOrder_Click);
            // 
            // LabelOrderSettingStopPrice
            // 
            this.LabelOrderSettingStopPrice.AutoSize = true;
            this.LabelOrderSettingStopPrice.Location = new System.Drawing.Point(14, 107);
            this.LabelOrderSettingStopPrice.Name = "LabelOrderSettingStopPrice";
            this.LabelOrderSettingStopPrice.Size = new System.Drawing.Size(34, 13);
            this.LabelOrderSettingStopPrice.TabIndex = 75;
            this.LabelOrderSettingStopPrice.Text = "Stop:";
            // 
            // GroupBoxPositions
            // 
            this.GroupBoxPositions.Controls.Add(this.BtnPositionCloseSelected);
            this.GroupBoxPositions.Controls.Add(this.BtnRequestPostion);
            this.GroupBoxPositions.Controls.Add(this.button1);
            this.GroupBoxPositions.Location = new System.Drawing.Point(632, 225);
            this.GroupBoxPositions.Name = "GroupBoxPositions";
            this.GroupBoxPositions.Size = new System.Drawing.Size(248, 168);
            this.GroupBoxPositions.TabIndex = 11;
            this.GroupBoxPositions.TabStop = false;
            this.GroupBoxPositions.Text = "Positions";
            // 
            // BtnPositionCloseSelected
            // 
            this.BtnPositionCloseSelected.Location = new System.Drawing.Point(20, 61);
            this.BtnPositionCloseSelected.Name = "BtnPositionCloseSelected";
            this.BtnPositionCloseSelected.Size = new System.Drawing.Size(167, 23);
            this.BtnPositionCloseSelected.TabIndex = 79;
            this.BtnPositionCloseSelected.Text = "Close Selected";
            this.BtnPositionCloseSelected.UseVisualStyleBackColor = true;
            // 
            // BtnRequestPostion
            // 
            this.BtnRequestPostion.Location = new System.Drawing.Point(20, 32);
            this.BtnRequestPostion.Name = "BtnRequestPostion";
            this.BtnRequestPostion.Size = new System.Drawing.Size(167, 23);
            this.BtnRequestPostion.TabIndex = 7;
            this.BtnRequestPostion.Text = "Request Postion";
            this.BtnRequestPostion.UseVisualStyleBackColor = true;
            this.BtnRequestPostion.Click += new System.EventHandler(this.BtnRequestPostion_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 117);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 23);
            this.button1.TabIndex = 77;
            this.button1.Text = "Close All";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BtnCloseAllPosition_Click);
            // 
            // executionsGroup
            // 
            this.executionsGroup.Controls.Add(this.BtnExportExecTradeLog);
            this.executionsGroup.Controls.Add(this.BtnRequestExecData);
            this.executionsGroup.Location = new System.Drawing.Point(348, 287);
            this.executionsGroup.Name = "executionsGroup";
            this.executionsGroup.Size = new System.Drawing.Size(248, 136);
            this.executionsGroup.TabIndex = 4;
            this.executionsGroup.TabStop = false;
            this.executionsGroup.Text = "Trade Log (Executions)";
            // 
            // BtnExportExecTradeLog
            // 
            this.BtnExportExecTradeLog.Location = new System.Drawing.Point(20, 67);
            this.BtnExportExecTradeLog.Name = "BtnExportExecTradeLog";
            this.BtnExportExecTradeLog.Size = new System.Drawing.Size(167, 23);
            this.BtnExportExecTradeLog.TabIndex = 80;
            this.BtnExportExecTradeLog.Text = "Export To TradeLog";
            this.BtnExportExecTradeLog.UseVisualStyleBackColor = true;
            this.BtnExportExecTradeLog.Click += new System.EventHandler(this.BtnExportExecTradeLog_Click);
            // 
            // BtnRequestExecData
            // 
            this.BtnRequestExecData.Location = new System.Drawing.Point(20, 38);
            this.BtnRequestExecData.Name = "BtnRequestExecData";
            this.BtnRequestExecData.Size = new System.Drawing.Size(167, 23);
            this.BtnRequestExecData.TabIndex = 78;
            this.BtnRequestExecData.Text = "Request Exec Data";
            this.BtnRequestExecData.UseVisualStyleBackColor = true;
            this.BtnRequestExecData.Click += new System.EventHandler(this.BtnRequestExecData_Click);
            // 
            // ib_banner
            // 
            this.ib_banner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ib_banner.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ib_banner.Image = global::TestClient.Properties.Resources.LogoIcon;
            this.ib_banner.Location = new System.Drawing.Point(696, 427);
            this.ib_banner.Name = "ib_banner";
            this.ib_banner.Size = new System.Drawing.Size(237, 36);
            this.ib_banner.TabIndex = 10;
            this.ib_banner.TabStop = false;
            // 
            // BtnGetCompletedOrders
            // 
            this.BtnGetCompletedOrders.Location = new System.Drawing.Point(460, 108);
            this.BtnGetCompletedOrders.Name = "BtnGetCompletedOrders";
            this.BtnGetCompletedOrders.Size = new System.Drawing.Size(162, 23);
            this.BtnGetCompletedOrders.TabIndex = 9;
            this.BtnGetCompletedOrders.Text = "Get Completed Orders";
            this.BtnGetCompletedOrders.UseVisualStyleBackColor = true;
            this.BtnGetCompletedOrders.Click += new System.EventHandler(this.BtnGetCompletedOrders_Click);
            // 
            // BtnGetOpenOrders
            // 
            this.BtnGetOpenOrders.Location = new System.Drawing.Point(458, 137);
            this.BtnGetOpenOrders.Name = "BtnGetOpenOrders";
            this.BtnGetOpenOrders.Size = new System.Drawing.Size(162, 23);
            this.BtnGetOpenOrders.TabIndex = 10;
            this.BtnGetOpenOrders.Text = "Get Open Orders";
            this.BtnGetOpenOrders.UseVisualStyleBackColor = true;
            this.BtnGetOpenOrders.Click += new System.EventHandler(this.BtnGetOpenOrders_Click);
            // 
            // tabSimulation
            // 
            this.tabSimulation.Controls.Add(this.BtnSetupSimulation);
            this.tabSimulation.Controls.Add(this.TextBoxRunAllSimulationInitialAccountValue);
            this.tabSimulation.Controls.Add(this.label10);
            this.tabSimulation.Controls.Add(this.BtnRunAllSimulation);
            this.tabSimulation.Controls.Add(this.BtnArmLiveTrade);
            this.tabSimulation.Location = new System.Drawing.Point(4, 22);
            this.tabSimulation.Name = "tabSimulation";
            this.tabSimulation.Size = new System.Drawing.Size(952, 484);
            this.tabSimulation.TabIndex = 9;
            this.tabSimulation.Text = "Simulation";
            this.tabSimulation.UseVisualStyleBackColor = true;
            // 
            // BtnSetupSimulation
            // 
            this.BtnSetupSimulation.Location = new System.Drawing.Point(19, 15);
            this.BtnSetupSimulation.Name = "BtnSetupSimulation";
            this.BtnSetupSimulation.Size = new System.Drawing.Size(159, 23);
            this.BtnSetupSimulation.TabIndex = 38;
            this.BtnSetupSimulation.Text = "Setup Simulation";
            this.BtnSetupSimulation.UseVisualStyleBackColor = true;
            this.BtnSetupSimulation.Click += new System.EventHandler(this.BtnSetupSimulation_Click);
            // 
            // TextBoxRunAllSimulationInitialAccountValue
            // 
            this.TextBoxRunAllSimulationInitialAccountValue.Location = new System.Drawing.Point(363, 135);
            this.TextBoxRunAllSimulationInitialAccountValue.Name = "TextBoxRunAllSimulationInitialAccountValue";
            this.TextBoxRunAllSimulationInitialAccountValue.Size = new System.Drawing.Size(150, 22);
            this.TextBoxRunAllSimulationInitialAccountValue.TabIndex = 40;
            this.TextBoxRunAllSimulationInitialAccountValue.Text = "100000";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(280, 138);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 13);
            this.label10.TabIndex = 41;
            this.label10.Text = "Initial Value";
            // 
            // BtnRunAllSimulation
            // 
            this.BtnRunAllSimulation.Location = new System.Drawing.Point(283, 172);
            this.BtnRunAllSimulation.Name = "BtnRunAllSimulation";
            this.BtnRunAllSimulation.Size = new System.Drawing.Size(230, 23);
            this.BtnRunAllSimulation.TabIndex = 39;
            this.BtnRunAllSimulation.Text = "Run All Simulation";
            this.BtnRunAllSimulation.UseVisualStyleBackColor = true;
            this.BtnRunAllSimulation.Click += new System.EventHandler(this.BtnRunAllSimulation_Click);
            // 
            // BtnArmLiveTrade
            // 
            this.BtnArmLiveTrade.Location = new System.Drawing.Point(283, 210);
            this.BtnArmLiveTrade.Name = "BtnArmLiveTrade";
            this.BtnArmLiveTrade.Size = new System.Drawing.Size(230, 23);
            this.BtnArmLiveTrade.TabIndex = 42;
            this.BtnArmLiveTrade.Text = "Arm Live Trade";
            this.BtnArmLiveTrade.UseVisualStyleBackColor = true;
            this.BtnArmLiveTrade.Click += new System.EventHandler(this.BtnArmLiveTrade_Click);
            // 
            // tabAccount
            // 
            this.tabAccount.Controls.Add(this.BtnSubscribePnL);
            this.tabAccount.Controls.Add(this.BtnRequestPnL);
            this.tabAccount.Controls.Add(this.TreeViewAccount);
            this.tabAccount.Controls.Add(this.btnAccountSummary);
            this.tabAccount.Location = new System.Drawing.Point(4, 22);
            this.tabAccount.Name = "tabAccount";
            this.tabAccount.Padding = new System.Windows.Forms.Padding(3);
            this.tabAccount.Size = new System.Drawing.Size(952, 484);
            this.tabAccount.TabIndex = 0;
            this.tabAccount.Text = "Account";
            this.tabAccount.UseVisualStyleBackColor = true;
            // 
            // BtnSubscribePnL
            // 
            this.BtnSubscribePnL.Location = new System.Drawing.Point(299, 6);
            this.BtnSubscribePnL.Name = "BtnSubscribePnL";
            this.BtnSubscribePnL.Size = new System.Drawing.Size(90, 23);
            this.BtnSubscribePnL.TabIndex = 9;
            this.BtnSubscribePnL.Text = "Subscribe PnL";
            this.BtnSubscribePnL.UseVisualStyleBackColor = true;
            this.BtnSubscribePnL.Click += new System.EventHandler(this.BtnSubscribePnL_Click);
            // 
            // BtnRequestPnL
            // 
            this.BtnRequestPnL.Location = new System.Drawing.Point(204, 6);
            this.BtnRequestPnL.Name = "BtnRequestPnL";
            this.BtnRequestPnL.Size = new System.Drawing.Size(90, 23);
            this.BtnRequestPnL.TabIndex = 8;
            this.BtnRequestPnL.Text = "Request PnL";
            this.BtnRequestPnL.UseVisualStyleBackColor = true;
            this.BtnRequestPnL.Click += new System.EventHandler(this.BtnRequestPnL_Click);
            // 
            // TreeViewAccount
            // 
            this.TreeViewAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeViewAccount.Location = new System.Drawing.Point(6, 35);
            this.TreeViewAccount.Name = "TreeViewAccount";
            this.TreeViewAccount.Size = new System.Drawing.Size(516, 443);
            this.TreeViewAccount.TabIndex = 5;
            // 
            // tabFileData
            // 
            this.tabFileData.Controls.Add(this.BtnApplyDefaultDownloadPeriod);
            this.tabFileData.Controls.Add(this.BtnMatchSymbols);
            this.tabFileData.Controls.Add(this.BtnImportNasdaq);
            this.tabFileData.Controls.Add(this.BtnUpdateContracts);
            this.tabFileData.Controls.Add(this.BtnImportSymbols);
            this.tabFileData.Controls.Add(this.BtnDownloadBarTable);
            this.tabFileData.Controls.Add(this.BtnReDownloadBarTable);
            this.tabFileData.Controls.Add(this.BtnCleanUpDuplicateStock);
            this.tabFileData.Controls.Add(this.GroupBoxQuandlTool);
            this.tabFileData.Controls.Add(this.BtnTestMassiveSample);
            this.tabFileData.Controls.Add(this.BtnDownloadMultiTables);
            this.tabFileData.Controls.Add(this.BtnExportContracts);
            this.tabFileData.Controls.Add(this.BtnImportContracts);
            this.tabFileData.Location = new System.Drawing.Point(4, 22);
            this.tabFileData.Name = "tabFileData";
            this.tabFileData.Size = new System.Drawing.Size(952, 484);
            this.tabFileData.TabIndex = 6;
            this.tabFileData.Text = "File / Data";
            this.tabFileData.UseVisualStyleBackColor = true;
            // 
            // BtnApplyDefaultDownloadPeriod
            // 
            this.BtnApplyDefaultDownloadPeriod.Location = new System.Drawing.Point(31, 34);
            this.BtnApplyDefaultDownloadPeriod.Name = "BtnApplyDefaultDownloadPeriod";
            this.BtnApplyDefaultDownloadPeriod.Size = new System.Drawing.Size(160, 23);
            this.BtnApplyDefaultDownloadPeriod.TabIndex = 58;
            this.BtnApplyDefaultDownloadPeriod.Text = "Default Download Setting";
            this.BtnApplyDefaultDownloadPeriod.UseVisualStyleBackColor = true;
            this.BtnApplyDefaultDownloadPeriod.Click += new System.EventHandler(this.BtnApplyDefaultDownloadPeriod_Click);
            // 
            // BtnMatchSymbols
            // 
            this.BtnMatchSymbols.Location = new System.Drawing.Point(799, 289);
            this.BtnMatchSymbols.Name = "BtnMatchSymbols";
            this.BtnMatchSymbols.Size = new System.Drawing.Size(120, 23);
            this.BtnMatchSymbols.TabIndex = 49;
            this.BtnMatchSymbols.Text = "Match Symbols";
            this.BtnMatchSymbols.UseVisualStyleBackColor = true;
            this.BtnMatchSymbols.Click += new System.EventHandler(this.BtnMatchSymbols_Click);
            // 
            // BtnImportNasdaq
            // 
            this.BtnImportNasdaq.Location = new System.Drawing.Point(673, 260);
            this.BtnImportNasdaq.Name = "BtnImportNasdaq";
            this.BtnImportNasdaq.Size = new System.Drawing.Size(120, 23);
            this.BtnImportNasdaq.TabIndex = 57;
            this.BtnImportNasdaq.Text = "Import Nasdaq";
            this.BtnImportNasdaq.UseVisualStyleBackColor = true;
            this.BtnImportNasdaq.Click += new System.EventHandler(this.BtnImportNasdaq_Click);
            // 
            // BtnUpdateContracts
            // 
            this.BtnUpdateContracts.Location = new System.Drawing.Point(799, 260);
            this.BtnUpdateContracts.Name = "BtnUpdateContracts";
            this.BtnUpdateContracts.Size = new System.Drawing.Size(120, 23);
            this.BtnUpdateContracts.TabIndex = 56;
            this.BtnUpdateContracts.Text = "Update Contracts";
            this.BtnUpdateContracts.UseVisualStyleBackColor = true;
            this.BtnUpdateContracts.Click += new System.EventHandler(this.BtnUpdateContracts_Click);
            // 
            // BtnImportSymbols
            // 
            this.BtnImportSymbols.Location = new System.Drawing.Point(673, 289);
            this.BtnImportSymbols.Name = "BtnImportSymbols";
            this.BtnImportSymbols.Size = new System.Drawing.Size(120, 23);
            this.BtnImportSymbols.TabIndex = 55;
            this.BtnImportSymbols.Text = "Import Symbols";
            this.BtnImportSymbols.UseVisualStyleBackColor = true;
            this.BtnImportSymbols.Click += new System.EventHandler(this.BtnImportSymbols_Click);
            // 
            // BtnDownloadBarTable
            // 
            this.BtnDownloadBarTable.Location = new System.Drawing.Point(31, 79);
            this.BtnDownloadBarTable.Name = "BtnDownloadBarTable";
            this.BtnDownloadBarTable.Size = new System.Drawing.Size(160, 23);
            this.BtnDownloadBarTable.TabIndex = 54;
            this.BtnDownloadBarTable.Text = "Download BarTable";
            this.BtnDownloadBarTable.UseVisualStyleBackColor = true;
            this.BtnDownloadBarTable.Click += new System.EventHandler(this.BtnDownloadBarTable_Click);
            // 
            // BtnReDownloadBarTable
            // 
            this.BtnReDownloadBarTable.Location = new System.Drawing.Point(31, 108);
            this.BtnReDownloadBarTable.Name = "BtnReDownloadBarTable";
            this.BtnReDownloadBarTable.Size = new System.Drawing.Size(160, 23);
            this.BtnReDownloadBarTable.TabIndex = 53;
            this.BtnReDownloadBarTable.Text = "Re-Download BarTable";
            this.BtnReDownloadBarTable.UseVisualStyleBackColor = true;
            this.BtnReDownloadBarTable.Click += new System.EventHandler(this.BtnReDownloadBarTable_Click);
            // 
            // BtnCleanUpDuplicateStock
            // 
            this.BtnCleanUpDuplicateStock.Location = new System.Drawing.Point(750, 172);
            this.BtnCleanUpDuplicateStock.Name = "BtnCleanUpDuplicateStock";
            this.BtnCleanUpDuplicateStock.Size = new System.Drawing.Size(198, 23);
            this.BtnCleanUpDuplicateStock.TabIndex = 52;
            this.BtnCleanUpDuplicateStock.Text = "Clean Up Duplicate Stock";
            this.BtnCleanUpDuplicateStock.UseVisualStyleBackColor = true;
            this.BtnCleanUpDuplicateStock.Click += new System.EventHandler(this.BtnCleanUpDuplicateStock_Click);
            // 
            // GroupBoxQuandlTool
            // 
            this.GroupBoxQuandlTool.Controls.Add(this.BtmImportQuandlBlob);
            this.GroupBoxQuandlTool.Controls.Add(this.BtnAddQuandlFile);
            this.GroupBoxQuandlTool.Controls.Add(this.BtnMergeQuandlFile);
            this.GroupBoxQuandlTool.Controls.Add(this.label13);
            this.GroupBoxQuandlTool.Controls.Add(this.ListViewQuandlFileMerge);
            this.GroupBoxQuandlTool.Controls.Add(this.BtnExtractSymbols);
            this.GroupBoxQuandlTool.Location = new System.Drawing.Point(270, 13);
            this.GroupBoxQuandlTool.Name = "GroupBoxQuandlTool";
            this.GroupBoxQuandlTool.Size = new System.Drawing.Size(474, 190);
            this.GroupBoxQuandlTool.TabIndex = 52;
            this.GroupBoxQuandlTool.TabStop = false;
            this.GroupBoxQuandlTool.Text = "Quandl Tool";
            // 
            // BtmImportQuandlBlob
            // 
            this.BtmImportQuandlBlob.Location = new System.Drawing.Point(6, 125);
            this.BtmImportQuandlBlob.Name = "BtmImportQuandlBlob";
            this.BtmImportQuandlBlob.Size = new System.Drawing.Size(173, 57);
            this.BtmImportQuandlBlob.TabIndex = 7;
            this.BtmImportQuandlBlob.Text = "Import Quandl";
            this.BtmImportQuandlBlob.UseVisualStyleBackColor = true;
            this.BtmImportQuandlBlob.Click += new System.EventHandler(this.BtmImportQuandlBlob_Click);
            // 
            // BtnAddQuandlFile
            // 
            this.BtnAddQuandlFile.Location = new System.Drawing.Point(6, 37);
            this.BtnAddQuandlFile.Name = "BtnAddQuandlFile";
            this.BtnAddQuandlFile.Size = new System.Drawing.Size(173, 23);
            this.BtnAddQuandlFile.TabIndex = 47;
            this.BtnAddQuandlFile.Text = "Add Quandl File";
            this.BtnAddQuandlFile.UseVisualStyleBackColor = true;
            this.BtnAddQuandlFile.Click += new System.EventHandler(this.BtnAddQuandlFile_Click);
            // 
            // BtnMergeQuandlFile
            // 
            this.BtnMergeQuandlFile.Location = new System.Drawing.Point(6, 66);
            this.BtnMergeQuandlFile.Name = "BtnMergeQuandlFile";
            this.BtnMergeQuandlFile.Size = new System.Drawing.Size(173, 23);
            this.BtnMergeQuandlFile.TabIndex = 48;
            this.BtnMergeQuandlFile.Text = "Merge Quandl File";
            this.BtnMergeQuandlFile.UseVisualStyleBackColor = true;
            this.BtnMergeQuandlFile.Click += new System.EventHandler(this.BtnMergeQuandlFile_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(185, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 13);
            this.label13.TabIndex = 46;
            this.label13.Text = "Quandl File List";
            // 
            // ListViewQuandlFileMerge
            // 
            this.ListViewQuandlFileMerge.HideSelection = false;
            this.ListViewQuandlFileMerge.Location = new System.Drawing.Point(185, 37);
            this.ListViewQuandlFileMerge.Name = "ListViewQuandlFileMerge";
            this.ListViewQuandlFileMerge.Size = new System.Drawing.Size(279, 145);
            this.ListViewQuandlFileMerge.TabIndex = 45;
            this.ListViewQuandlFileMerge.UseCompatibleStateImageBehavior = false;
            this.ListViewQuandlFileMerge.View = System.Windows.Forms.View.List;
            // 
            // BtnExtractSymbols
            // 
            this.BtnExtractSymbols.Location = new System.Drawing.Point(6, 95);
            this.BtnExtractSymbols.Name = "BtnExtractSymbols";
            this.BtnExtractSymbols.Size = new System.Drawing.Size(173, 23);
            this.BtnExtractSymbols.TabIndex = 51;
            this.BtnExtractSymbols.Text = "Extract Symbols";
            this.BtnExtractSymbols.UseVisualStyleBackColor = true;
            this.BtnExtractSymbols.Click += new System.EventHandler(this.BtnExtractSymbols_Click);
            // 
            // BtnTestMassiveSample
            // 
            this.BtnTestMassiveSample.Location = new System.Drawing.Point(19, 988);
            this.BtnTestMassiveSample.Name = "BtnTestMassiveSample";
            this.BtnTestMassiveSample.Size = new System.Drawing.Size(146, 23);
            this.BtnTestMassiveSample.TabIndex = 8;
            this.BtnTestMassiveSample.Text = "Test Massive Samples";
            this.BtnTestMassiveSample.UseVisualStyleBackColor = true;
            this.BtnTestMassiveSample.Click += new System.EventHandler(this.BtnTestMassiveSamples_Click);
            // 
            // BtnDownloadMultiTables
            // 
            this.BtnDownloadMultiTables.Location = new System.Drawing.Point(31, 155);
            this.BtnDownloadMultiTables.Name = "BtnDownloadMultiTables";
            this.BtnDownloadMultiTables.Size = new System.Drawing.Size(160, 23);
            this.BtnDownloadMultiTables.TabIndex = 10;
            this.BtnDownloadMultiTables.Text = "Download Multi Tables";
            this.BtnDownloadMultiTables.UseVisualStyleBackColor = true;
            this.BtnDownloadMultiTables.Click += new System.EventHandler(this.BtnDownloadMultiTables_Click);
            // 
            // BtnExportContracts
            // 
            this.BtnExportContracts.Location = new System.Drawing.Point(547, 289);
            this.BtnExportContracts.Name = "BtnExportContracts";
            this.BtnExportContracts.Size = new System.Drawing.Size(120, 23);
            this.BtnExportContracts.TabIndex = 6;
            this.BtnExportContracts.Text = "Export Contracts";
            this.BtnExportContracts.UseVisualStyleBackColor = true;
            this.BtnExportContracts.Click += new System.EventHandler(this.BtnExportSymbols_Click);
            // 
            // BtnImportContracts
            // 
            this.BtnImportContracts.Location = new System.Drawing.Point(547, 260);
            this.BtnImportContracts.Name = "BtnImportContracts";
            this.BtnImportContracts.Size = new System.Drawing.Size(120, 23);
            this.BtnImportContracts.TabIndex = 5;
            this.BtnImportContracts.Text = "Import Contracts";
            this.BtnImportContracts.UseVisualStyleBackColor = true;
            this.BtnImportContracts.Click += new System.EventHandler(this.BtnImportContracts_Click);
            // 
            // BtnValidUSSymbol
            // 
            this.BtnValidUSSymbol.Location = new System.Drawing.Point(191, 21);
            this.BtnValidUSSymbol.Name = "BtnValidUSSymbol";
            this.BtnValidUSSymbol.Size = new System.Drawing.Size(47, 22);
            this.BtnValidUSSymbol.TabIndex = 63;
            this.BtnValidUSSymbol.Text = "Valid";
            this.BtnValidUSSymbol.UseVisualStyleBackColor = true;
            this.BtnValidUSSymbol.Click += new System.EventHandler(this.BtnValidUSSymbol_Click);
            // 
            // BtnMarketDataFormHide
            // 
            this.BtnMarketDataFormHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnMarketDataFormHide.Location = new System.Drawing.Point(125, 718);
            this.BtnMarketDataFormHide.Name = "BtnMarketDataFormHide";
            this.BtnMarketDataFormHide.Size = new System.Drawing.Size(100, 23);
            this.BtnMarketDataFormHide.TabIndex = 6;
            this.BtnMarketDataFormHide.Text = "Hide Form";
            this.BtnMarketDataFormHide.UseVisualStyleBackColor = true;
            this.BtnMarketDataFormHide.Click += new System.EventHandler(this.BtnMarketDataFormHide_Click);
            // 
            // BtnMarketDataFormShow
            // 
            this.BtnMarketDataFormShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnMarketDataFormShow.Location = new System.Drawing.Point(16, 718);
            this.BtnMarketDataFormShow.Name = "BtnMarketDataFormShow";
            this.BtnMarketDataFormShow.Size = new System.Drawing.Size(100, 23);
            this.BtnMarketDataFormShow.TabIndex = 5;
            this.BtnMarketDataFormShow.Text = "Show Form";
            this.BtnMarketDataFormShow.UseVisualStyleBackColor = true;
            this.BtnMarketDataFormShow.Click += new System.EventHandler(this.BtnMarketDataFormShow_Click);
            // 
            // BtnFormatSymbolsList
            // 
            this.BtnFormatSymbolsList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnFormatSymbolsList.Location = new System.Drawing.Point(410, 21);
            this.BtnFormatSymbolsList.Name = "BtnFormatSymbolsList";
            this.BtnFormatSymbolsList.Size = new System.Drawing.Size(58, 55);
            this.BtnFormatSymbolsList.TabIndex = 53;
            this.BtnFormatSymbolsList.Text = "Format Symbols List";
            this.BtnFormatSymbolsList.UseVisualStyleBackColor = true;
            this.BtnFormatSymbolsList.Click += new System.EventHandler(this.BtnFormatSymbolsList_Click);
            // 
            // DownloadBarTableDetialedProgressBar
            // 
            this.DownloadBarTableDetialedProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DownloadBarTableDetialedProgressBar.Location = new System.Drawing.Point(125, 757);
            this.DownloadBarTableDetialedProgressBar.Name = "DownloadBarTableDetialedProgressBar";
            this.DownloadBarTableDetialedProgressBar.Size = new System.Drawing.Size(487, 15);
            this.DownloadBarTableDetialedProgressBar.TabIndex = 11;
            // 
            // TextBoxMultiContracts
            // 
            this.TextBoxMultiContracts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxMultiContracts.Location = new System.Drawing.Point(6, 21);
            this.TextBoxMultiContracts.Name = "TextBoxMultiContracts";
            this.TextBoxMultiContracts.Size = new System.Drawing.Size(398, 143);
            this.TextBoxMultiContracts.TabIndex = 9;
            this.TextBoxMultiContracts.Text = resources.GetString("TextBoxMultiContracts.Text");
            // 
            // CheckBoxChartToCurrent
            // 
            this.CheckBoxChartToCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckBoxChartToCurrent.AutoSize = true;
            this.CheckBoxChartToCurrent.Checked = true;
            this.CheckBoxChartToCurrent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxChartToCurrent.Location = new System.Drawing.Point(47, 76);
            this.CheckBoxChartToCurrent.Name = "CheckBoxChartToCurrent";
            this.CheckBoxChartToCurrent.Size = new System.Drawing.Size(65, 17);
            this.CheckBoxChartToCurrent.TabIndex = 35;
            this.CheckBoxChartToCurrent.Text = "Current";
            this.CheckBoxChartToCurrent.UseVisualStyleBackColor = true;
            this.CheckBoxChartToCurrent.CheckedChanged += new System.EventHandler(this.CheckBoxChartToCurrent_CheckedChanged);
            // 
            // LabelBarFreq
            // 
            this.LabelBarFreq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelBarFreq.AutoSize = true;
            this.LabelBarFreq.Location = new System.Drawing.Point(7, 117);
            this.LabelBarFreq.Name = "LabelBarFreq";
            this.LabelBarFreq.Size = new System.Drawing.Size(52, 13);
            this.LabelBarFreq.TabIndex = 27;
            this.LabelBarFreq.Text = "Bar Freq:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Stop:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Start:";
            // 
            // DateTimePickerHistoricalDataStop
            // 
            this.DateTimePickerHistoricalDataStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DateTimePickerHistoricalDataStop.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.DateTimePickerHistoricalDataStop.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTimePickerHistoricalDataStop.Location = new System.Drawing.Point(47, 49);
            this.DateTimePickerHistoricalDataStop.Name = "DateTimePickerHistoricalDataStop";
            this.DateTimePickerHistoricalDataStop.Size = new System.Drawing.Size(140, 22);
            this.DateTimePickerHistoricalDataStop.TabIndex = 7;
            this.DateTimePickerHistoricalDataStop.Value = new System.DateTime(1999, 1, 22, 12, 0, 0, 0);
            // 
            // SelectHistoricalDataBarType
            // 
            this.SelectHistoricalDataBarType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectHistoricalDataBarType.FormattingEnabled = true;
            this.SelectHistoricalDataBarType.Location = new System.Drawing.Point(65, 141);
            this.SelectHistoricalDataBarType.Name = "SelectHistoricalDataBarType";
            this.SelectHistoricalDataBarType.Size = new System.Drawing.Size(122, 21);
            this.SelectHistoricalDataBarType.TabIndex = 25;
            this.SelectHistoricalDataBarType.Text = "Trades";
            // 
            // SelectHistoricalDataBarFreq
            // 
            this.SelectHistoricalDataBarFreq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectHistoricalDataBarFreq.FormattingEnabled = true;
            this.SelectHistoricalDataBarFreq.Location = new System.Drawing.Point(65, 114);
            this.SelectHistoricalDataBarFreq.Name = "SelectHistoricalDataBarFreq";
            this.SelectHistoricalDataBarFreq.Size = new System.Drawing.Size(122, 21);
            this.SelectHistoricalDataBarFreq.TabIndex = 22;
            this.SelectHistoricalDataBarFreq.Text = "Minute";
            // 
            // DateTimePickerHistoricalDataStart
            // 
            this.DateTimePickerHistoricalDataStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DateTimePickerHistoricalDataStart.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.DateTimePickerHistoricalDataStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTimePickerHistoricalDataStart.Location = new System.Drawing.Point(47, 21);
            this.DateTimePickerHistoricalDataStart.Name = "DateTimePickerHistoricalDataStart";
            this.DateTimePickerHistoricalDataStart.Size = new System.Drawing.Size(140, 22);
            this.DateTimePickerHistoricalDataStart.TabIndex = 5;
            this.DateTimePickerHistoricalDataStart.Value = new System.DateTime(1999, 1, 22, 12, 0, 0, 0);
            // 
            // ListBoxAccount
            // 
            this.ListBoxAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ListBoxAccount.FormattingEnabled = true;
            this.ListBoxAccount.Location = new System.Drawing.Point(88, 718);
            this.ListBoxAccount.Name = "ListBoxAccount";
            this.ListBoxAccount.Size = new System.Drawing.Size(0, 121);
            this.ListBoxAccount.TabIndex = 4;
            // 
            // LbStatus
            // 
            this.LbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LbStatus.AutoSize = true;
            this.LbStatus.Location = new System.Drawing.Point(122, 805);
            this.LbStatus.Name = "LbStatus";
            this.LbStatus.Size = new System.Drawing.Size(42, 13);
            this.LbStatus.TabIndex = 11;
            this.LbStatus.Text = "Status:";
            // 
            // CheckBoxSingleContractUseSmart
            // 
            this.CheckBoxSingleContractUseSmart.AutoSize = true;
            this.CheckBoxSingleContractUseSmart.Checked = true;
            this.CheckBoxSingleContractUseSmart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxSingleContractUseSmart.Location = new System.Drawing.Point(190, 52);
            this.CheckBoxSingleContractUseSmart.Name = "CheckBoxSingleContractUseSmart";
            this.CheckBoxSingleContractUseSmart.Size = new System.Drawing.Size(74, 17);
            this.CheckBoxSingleContractUseSmart.TabIndex = 24;
            this.CheckBoxSingleContractUseSmart.Text = "UseSmart";
            this.CheckBoxSingleContractUseSmart.UseVisualStyleBackColor = true;
            // 
            // SelectBoxSingleContractExchange
            // 
            this.SelectBoxSingleContractExchange.Enabled = false;
            this.SelectBoxSingleContractExchange.FormattingEnabled = true;
            this.SelectBoxSingleContractExchange.Location = new System.Drawing.Point(72, 49);
            this.SelectBoxSingleContractExchange.Name = "SelectBoxSingleContractExchange";
            this.SelectBoxSingleContractExchange.Size = new System.Drawing.Size(110, 21);
            this.SelectBoxSingleContractExchange.TabIndex = 23;
            this.SelectBoxSingleContractExchange.Text = "NYSE";
            // 
            // LabelSingleContractExchange
            // 
            this.LabelSingleContractExchange.AutoSize = true;
            this.LabelSingleContractExchange.Location = new System.Drawing.Point(7, 52);
            this.LabelSingleContractExchange.Name = "LabelSingleContractExchange";
            this.LabelSingleContractExchange.Size = new System.Drawing.Size(59, 13);
            this.LabelSingleContractExchange.TabIndex = 22;
            this.LabelSingleContractExchange.Text = "Exchange:";
            // 
            // SelectBoxSingleContractSecurityType
            // 
            this.SelectBoxSingleContractSecurityType.Enabled = false;
            this.SelectBoxSingleContractSecurityType.FormattingEnabled = true;
            this.SelectBoxSingleContractSecurityType.Location = new System.Drawing.Point(72, 76);
            this.SelectBoxSingleContractSecurityType.Name = "SelectBoxSingleContractSecurityType";
            this.SelectBoxSingleContractSecurityType.Size = new System.Drawing.Size(110, 21);
            this.SelectBoxSingleContractSecurityType.TabIndex = 21;
            this.SelectBoxSingleContractSecurityType.Text = "STOCK";
            // 
            // LabelSingleContractType
            // 
            this.LabelSingleContractType.AutoSize = true;
            this.LabelSingleContractType.Location = new System.Drawing.Point(33, 80);
            this.LabelSingleContractType.Name = "LabelSingleContractType";
            this.LabelSingleContractType.Size = new System.Drawing.Size(33, 13);
            this.LabelSingleContractType.TabIndex = 20;
            this.LabelSingleContractType.Text = "Type:";
            // 
            // LabelSingleContractName
            // 
            this.LabelSingleContractName.AutoSize = true;
            this.LabelSingleContractName.Location = new System.Drawing.Point(6, 24);
            this.LabelSingleContractName.Name = "LabelSingleContractName";
            this.LabelSingleContractName.Size = new System.Drawing.Size(39, 13);
            this.LabelSingleContractName.TabIndex = 19;
            this.LabelSingleContractName.Text = "Name:";
            // 
            // TextBoxSingleContractName
            // 
            this.TextBoxSingleContractName.Location = new System.Drawing.Point(50, 21);
            this.TextBoxSingleContractName.Name = "TextBoxSingleContractName";
            this.TextBoxSingleContractName.Size = new System.Drawing.Size(132, 22);
            this.TextBoxSingleContractName.TabIndex = 18;
            this.TextBoxSingleContractName.Text = "QQQ";
            this.TextBoxSingleContractName.TextChanged += new System.EventHandler(this.TbSymbolName_TextChanged);
            // 
            // MainProgBar
            // 
            this.MainProgBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MainProgBar.Location = new System.Drawing.Point(125, 782);
            this.MainProgBar.Name = "MainProgBar";
            this.MainProgBar.Size = new System.Drawing.Size(487, 15);
            this.MainProgBar.TabIndex = 25;
            // 
            // RequestPnL
            // 
            this.RequestPnL.Location = new System.Drawing.Point(204, 6);
            this.RequestPnL.Name = "RequestPnL";
            this.RequestPnL.Size = new System.Drawing.Size(90, 23);
            this.RequestPnL.TabIndex = 8;
            this.RequestPnL.Text = "Request PnL";
            this.RequestPnL.UseVisualStyleBackColor = true;
            this.RequestPnL.Click += new System.EventHandler(this.BtnRequestPnL_Click);
            // 
            // BtnMasterCancel
            // 
            this.BtnMasterCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnMasterCancel.BackColor = System.Drawing.Color.DarkRed;
            this.BtnMasterCancel.ForeColor = System.Drawing.Color.White;
            this.BtnMasterCancel.Location = new System.Drawing.Point(16, 757);
            this.BtnMasterCancel.Name = "BtnMasterCancel";
            this.BtnMasterCancel.Size = new System.Drawing.Size(100, 40);
            this.BtnMasterCancel.TabIndex = 38;
            this.BtnMasterCancel.Text = "Master Cancel";
            this.BtnMasterCancel.UseVisualStyleBackColor = false;
            this.BtnMasterCancel.Click += new System.EventHandler(this.BtnMasterCancel_Click);
            // 
            // TextBoxIPAddress
            // 
            this.TextBoxIPAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TextBoxIPAddress.Location = new System.Drawing.Point(125, 821);
            this.TextBoxIPAddress.Name = "TextBoxIPAddress";
            this.TextBoxIPAddress.Size = new System.Drawing.Size(150, 22);
            this.TextBoxIPAddress.TabIndex = 65;
            this.TextBoxIPAddress.Text = "192.168.18.7";
            // 
            // GroupBoxSingleContract
            // 
            this.GroupBoxSingleContract.Controls.Add(this.TextBoxValidCountryCode);
            this.GroupBoxSingleContract.Controls.Add(this.label14);
            this.GroupBoxSingleContract.Controls.Add(this.BtnValidUSSymbol);
            this.GroupBoxSingleContract.Controls.Add(this.TextBoxSingleContractName);
            this.GroupBoxSingleContract.Controls.Add(this.LabelSingleContractName);
            this.GroupBoxSingleContract.Controls.Add(this.textBox1);
            this.GroupBoxSingleContract.Controls.Add(this.LabelSingleContractType);
            this.GroupBoxSingleContract.Controls.Add(this.SelectBoxSingleContractSecurityType);
            this.GroupBoxSingleContract.Controls.Add(this.label3);
            this.GroupBoxSingleContract.Controls.Add(this.LabelSingleContractExchange);
            this.GroupBoxSingleContract.Controls.Add(this.SelectBoxSingleContractExchange);
            this.GroupBoxSingleContract.Controls.Add(this.LabelSingleContractStrike);
            this.GroupBoxSingleContract.Controls.Add(this.comboBox1);
            this.GroupBoxSingleContract.Controls.Add(this.CheckBoxSingleContractUseSmart);
            this.GroupBoxSingleContract.Controls.Add(this.TextBoxSingleContractStrike);
            this.GroupBoxSingleContract.Controls.Add(this.DateTimePickerSingleContractExpire);
            this.GroupBoxSingleContract.Controls.Add(this.LabelSingleContractExpire);
            this.GroupBoxSingleContract.Location = new System.Drawing.Point(12, 14);
            this.GroupBoxSingleContract.Name = "GroupBoxSingleContract";
            this.GroupBoxSingleContract.Size = new System.Drawing.Size(281, 170);
            this.GroupBoxSingleContract.TabIndex = 66;
            this.GroupBoxSingleContract.TabStop = false;
            this.GroupBoxSingleContract.Text = "Single Contract";
            // 
            // TextBoxValidCountryCode
            // 
            this.TextBoxValidCountryCode.Location = new System.Drawing.Point(244, 21);
            this.TextBoxValidCountryCode.Name = "TextBoxValidCountryCode";
            this.TextBoxValidCountryCode.Size = new System.Drawing.Size(26, 22);
            this.TextBoxValidCountryCode.TabIndex = 64;
            this.TextBoxValidCountryCode.Text = "US";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(149, 144);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(37, 13);
            this.label14.TabIndex = 32;
            this.label14.Text = "Mlplr:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(191, 140);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(79, 22);
            this.textBox1.TabIndex = 31;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Side:";
            // 
            // LabelSingleContractStrike
            // 
            this.LabelSingleContractStrike.AutoSize = true;
            this.LabelSingleContractStrike.Location = new System.Drawing.Point(146, 118);
            this.LabelSingleContractStrike.Name = "LabelSingleContractStrike";
            this.LabelSingleContractStrike.Size = new System.Drawing.Size(39, 13);
            this.LabelSingleContractStrike.TabIndex = 27;
            this.LabelSingleContractStrike.Text = "Strike:";
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "CALL",
            "PUT"});
            this.comboBox1.Location = new System.Drawing.Point(46, 141);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(94, 21);
            this.comboBox1.TabIndex = 29;
            this.comboBox1.Text = "CALL";
            // 
            // TextBoxSingleContractStrike
            // 
            this.TextBoxSingleContractStrike.Location = new System.Drawing.Point(191, 113);
            this.TextBoxSingleContractStrike.Name = "TextBoxSingleContractStrike";
            this.TextBoxSingleContractStrike.Size = new System.Drawing.Size(79, 22);
            this.TextBoxSingleContractStrike.TabIndex = 25;
            // 
            // DateTimePickerSingleContractExpire
            // 
            this.DateTimePickerSingleContractExpire.CustomFormat = "MM/dd/yyyy";
            this.DateTimePickerSingleContractExpire.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTimePickerSingleContractExpire.Location = new System.Drawing.Point(46, 113);
            this.DateTimePickerSingleContractExpire.Name = "DateTimePickerSingleContractExpire";
            this.DateTimePickerSingleContractExpire.Size = new System.Drawing.Size(94, 22);
            this.DateTimePickerSingleContractExpire.TabIndex = 26;
            this.DateTimePickerSingleContractExpire.Value = new System.DateTime(1999, 1, 22, 12, 0, 0, 0);
            // 
            // LabelSingleContractExpire
            // 
            this.LabelSingleContractExpire.AutoSize = true;
            this.LabelSingleContractExpire.Location = new System.Drawing.Point(6, 118);
            this.LabelSingleContractExpire.Name = "LabelSingleContractExpire";
            this.LabelSingleContractExpire.Size = new System.Drawing.Size(34, 13);
            this.LabelSingleContractExpire.TabIndex = 28;
            this.LabelSingleContractExpire.Text = "Date:";
            // 
            // LabelBarType
            // 
            this.LabelBarType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelBarType.AutoSize = true;
            this.LabelBarType.Location = new System.Drawing.Point(7, 144);
            this.LabelBarType.Name = "LabelBarType";
            this.LabelBarType.Size = new System.Drawing.Size(52, 13);
            this.LabelBarType.TabIndex = 67;
            this.LabelBarType.Text = "Bar Type:";
            // 
            // GroupBoxBarTableSetting
            // 
            this.GroupBoxBarTableSetting.Controls.Add(this.DateTimePickerHistoricalDataStart);
            this.GroupBoxBarTableSetting.Controls.Add(this.LabelBarType);
            this.GroupBoxBarTableSetting.Controls.Add(this.label2);
            this.GroupBoxBarTableSetting.Controls.Add(this.DateTimePickerHistoricalDataStop);
            this.GroupBoxBarTableSetting.Controls.Add(this.label1);
            this.GroupBoxBarTableSetting.Controls.Add(this.SelectHistoricalDataBarFreq);
            this.GroupBoxBarTableSetting.Controls.Add(this.SelectHistoricalDataBarType);
            this.GroupBoxBarTableSetting.Controls.Add(this.LabelBarFreq);
            this.GroupBoxBarTableSetting.Controls.Add(this.CheckBoxChartToCurrent);
            this.GroupBoxBarTableSetting.Location = new System.Drawing.Point(299, 14);
            this.GroupBoxBarTableSetting.Name = "GroupBoxBarTableSetting";
            this.GroupBoxBarTableSetting.Size = new System.Drawing.Size(193, 170);
            this.GroupBoxBarTableSetting.TabIndex = 68;
            this.GroupBoxBarTableSetting.TabStop = false;
            this.GroupBoxBarTableSetting.Text = "BarTable Setting";
            // 
            // GroupBoxMultiContracts
            // 
            this.GroupBoxMultiContracts.Controls.Add(this.TextBoxMultiContracts);
            this.GroupBoxMultiContracts.Controls.Add(this.BtnFormatSymbolsList);
            this.GroupBoxMultiContracts.Location = new System.Drawing.Point(498, 14);
            this.GroupBoxMultiContracts.Name = "GroupBoxMultiContracts";
            this.GroupBoxMultiContracts.Size = new System.Drawing.Size(474, 170);
            this.GroupBoxMultiContracts.TabIndex = 69;
            this.GroupBoxMultiContracts.TabStop = false;
            this.GroupBoxMultiContracts.Text = "Multi Contracts";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 861);
            this.Controls.Add(this.DownloadBarTableDetialedProgressBar);
            this.Controls.Add(this.BtnMarketDataFormShow);
            this.Controls.Add(this.BtnMarketDataFormHide);
            this.Controls.Add(this.GroupBoxMultiContracts);
            this.Controls.Add(this.GroupBoxBarTableSetting);
            this.Controls.Add(this.GroupBoxSingleContract);
            this.Controls.Add(this.TextBoxIPAddress);
            this.Controls.Add(this.BtnMasterCancel);
            this.Controls.Add(this.MainProgBar);
            this.Controls.Add(this.ListBoxAccount);
            this.Controls.Add(this.LbStatus);
            this.Controls.Add(this.MainTab);
            this.Controls.Add(this.btnConnect);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1000, 900);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test IB Client";
            this.MainTab.ResumeLayout(false);
            this.tabHistoricalData.ResumeLayout(false);
            this.tabHistoricalData.PerformLayout();
            this.tabContract.ResumeLayout(false);
            this.tabContract.PerformLayout();
            this.GroupBoxContractInfo.ResumeLayout(false);
            this.GroupBoxContractInfo.PerformLayout();
            this.tabMarketData.ResumeLayout(false);
            this.tabMarketData.PerformLayout();
            this.tabOrder.ResumeLayout(false);
            this.GroupBoxOrderSetting.ResumeLayout(false);
            this.GroupBoxOrderSetting.PerformLayout();
            this.GroupBoxPositions.ResumeLayout(false);
            this.executionsGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ib_banner)).EndInit();
            this.tabSimulation.ResumeLayout(false);
            this.tabSimulation.PerformLayout();
            this.tabAccount.ResumeLayout(false);
            this.tabFileData.ResumeLayout(false);
            this.GroupBoxQuandlTool.ResumeLayout(false);
            this.GroupBoxQuandlTool.PerformLayout();
            this.GroupBoxSingleContract.ResumeLayout(false);
            this.GroupBoxSingleContract.PerformLayout();
            this.GroupBoxBarTableSetting.ResumeLayout(false);
            this.GroupBoxBarTableSetting.PerformLayout();
            this.GroupBoxMultiContracts.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnAccountSummary;
        private System.Windows.Forms.TabControl MainTab;
        private System.Windows.Forms.TabPage tabAccount;
        private System.Windows.Forms.TabPage tabHistoricalData;
        private System.Windows.Forms.PictureBox ib_banner;
        private System.Windows.Forms.TabPage tabMarketData;
        private System.Windows.Forms.Label LbStatus;
        private System.Windows.Forms.CheckBox CheckBoxSingleContractUseSmart;
        private System.Windows.Forms.ComboBox SelectBoxSingleContractExchange;
        private System.Windows.Forms.Label LabelSingleContractExchange;
        private System.Windows.Forms.ComboBox SelectBoxSingleContractSecurityType;
        private System.Windows.Forms.Label LabelSingleContractType;
        private System.Windows.Forms.Label LabelSingleContractName;
        private System.Windows.Forms.TextBox TextBoxSingleContractName;
        private System.Windows.Forms.ListBox ListBoxAccount;
        private System.Windows.Forms.TreeView TreeViewAccount;
        private System.Windows.Forms.Button BtnLoadHistoricalChart;
        private System.Windows.Forms.Button BtnRequestHistoricalTicks;
        private System.Windows.Forms.Button BtnChartsUpdateAll;
        private System.Windows.Forms.DateTimePicker DateTimePickerHistoricalDataStart;
        private System.Windows.Forms.ComboBox SelectHistoricalDataBarType;
        private System.Windows.Forms.ComboBox SelectHistoricalDataBarFreq;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button BtnMarketDataAddContract;
        private System.Windows.Forms.DateTimePicker DateTimePickerHistoricalDataStop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelBarFreq;
        private System.Windows.Forms.ProgressBar MainProgBar;
        private System.Windows.Forms.Button BtnRequestPostion;
        private System.Windows.Forms.GroupBox executionsGroup;
        private System.Windows.Forms.Button BtnModifyOrder;
        private System.Windows.Forms.Button BtnGlobalCancel;
        private System.Windows.Forms.Button BtnOrderSettingPlaceOrder;
        private System.Windows.Forms.Button BtnCancelOrder;
        private System.Windows.Forms.Button BtnGetCompletedOrders;
        private System.Windows.Forms.TabPage tabFileData;
        private System.Windows.Forms.Button BtnRequestScanner;
        private System.Windows.Forms.Button BtnRequestMarketDepth;
        private System.Windows.Forms.Button BtnGetOpenOrders;
        private System.Windows.Forms.Button BtnMarketDataAddMultiContracts;
        private System.Windows.Forms.Button BtnExportContracts;
        private System.Windows.Forms.Button BtnImportContracts;
        private System.Windows.Forms.TextBox TextBoxSymbolIds;
        private System.Windows.Forms.Label LbSymbolIdNumbers;
        private System.Windows.Forms.TextBox TextBoxSymbolSummaryF;
        private System.Windows.Forms.TextBox TextBoxSymbolSummaryB;
        private System.Windows.Forms.Label LbFinancialSummary;
        private System.Windows.Forms.Label LbBusinessSummary;
        private System.Windows.Forms.TextBox TextBoxSymbolISIN;
        private System.Windows.Forms.TextBox TextBoxSymbolFullName;
        private System.Windows.Forms.Label LbSymbolFullName;
        private System.Windows.Forms.Label LbSymbolISIN;
        private System.Windows.Forms.Button BtnSearchSymbol;
        private System.Windows.Forms.Button BtnRequestScannerParameter;
        private System.Windows.Forms.TextBox TextBoxSearchSymbol;
        private System.Windows.Forms.Button BtnValidUSSymbol;
        private System.Windows.Forms.Button BtmImportQuandlBlob;
        private System.Windows.Forms.Button BtnGetContractInfo;
        private System.Windows.Forms.Button BtnCancelAllScanner;
        private System.Windows.Forms.Button BtnTestMassiveSample;
        private System.Windows.Forms.Button BtnTestRealTimeBars;
        private System.Windows.Forms.Button BtnSubscribePnL;
        private System.Windows.Forms.Button BtnRequestPnL;
        private System.Windows.Forms.Button RequestPnL;
        private System.Windows.Forms.Button BtnAlignCharts;
        private System.Windows.Forms.CheckBox CheckBoxChartToCurrent;
        private System.Windows.Forms.RichTextBox TextBoxMultiContracts;
        private System.Windows.Forms.ListView ListViewAllCharts;
        private System.Windows.Forms.Button BtnCloseChart;
        private System.Windows.Forms.Button BtnMasterCancel;
        private System.Windows.Forms.Button BtnDownloadMultiTables;
        private System.Windows.Forms.ProgressBar DownloadBarTableDetialedProgressBar;
        private System.Windows.Forms.Button BtnSetupSimulation;
        private System.Windows.Forms.TextBox TextBoxIPAddress;
        private System.Windows.Forms.GroupBox GroupBoxPositions;
        private System.Windows.Forms.TabPage tabOrder;
        private System.Windows.Forms.TextBox TextBoxOrderSettingQuantity;
        private System.Windows.Forms.ComboBox ComboxBoxOrderSettingType;
        private System.Windows.Forms.ComboBox ComboBoxOrderSettingTIF;
        private System.Windows.Forms.Label LabelOrderSettingStopPrice;
        private System.Windows.Forms.Label LabelOrderSettingLimitPrice;
        private System.Windows.Forms.TextBox TextBoxOrderSettingStopPrice;
        private System.Windows.Forms.TextBox TextBoxOrderSettingLimitPrice;
        private System.Windows.Forms.Label LabelOrderSettingTIF;
        private System.Windows.Forms.Label LabelOrderSettingType;
        private System.Windows.Forms.Label LabelOrderSettingQuantity;
        private System.Windows.Forms.CheckBox CheckBoxOrderWhatIf;
        private System.Windows.Forms.Button BtnOrderSettingPlaceMultiOrder;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BtnRequestExecData;
        private System.Windows.Forms.Label LabelOrderSettingGTD;
        private System.Windows.Forms.DateTimePicker DateTimePickerOrderSettingGTD;
        private System.Windows.Forms.Button BtnPositionCloseSelected;
        private System.Windows.Forms.Button BtnRunAllSimulation;
        private System.Windows.Forms.Button BtnOrderSettingOrderBraket;
        private System.Windows.Forms.TextBox TextBoxRunAllSimulationInitialAccountValue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button BtnArmLiveTrade;
        private System.Windows.Forms.Button BtnExportExecTradeLog;
        private System.Windows.Forms.Button BtnApplyTradeLogToChart;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button BtnAddQuandlFile;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ListView ListViewQuandlFileMerge;
        private System.Windows.Forms.Button BtnMergeQuandlFile;
        private System.Windows.Forms.Button BtnMatchSymbols;
        private System.Windows.Forms.Button BtnExtractSymbols;
        private System.Windows.Forms.Button BtnCleanUpDuplicateStock;
        private System.Windows.Forms.Button BtnFormatSymbolsList;
        private System.Windows.Forms.GroupBox GroupBoxSingleContract;
        private System.Windows.Forms.Label LabelBarType;
        private System.Windows.Forms.GroupBox GroupBoxBarTableSetting;
        private System.Windows.Forms.GroupBox GroupBoxMultiContracts;
        private System.Windows.Forms.GroupBox GroupBoxContractInfo;
        private System.Windows.Forms.TabPage tabContract;
        private System.Windows.Forms.Label LabelSingleContractStrike;
        private System.Windows.Forms.DateTimePicker DateTimePickerSingleContractExpire;
        private System.Windows.Forms.TextBox TextBoxSingleContractStrike;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label LabelSingleContractExpire;
        private System.Windows.Forms.GroupBox GroupBoxQuandlTool;
        private System.Windows.Forms.Button BtnReDownloadBarTable;
        private System.Windows.Forms.Button BtnDownloadBarTable;
        private System.Windows.Forms.Button BtnMarketDataFormShow;
        private System.Windows.Forms.Button BtnMarketDataFormHide;
        private System.Windows.Forms.TextBox TextBoxGenericTickList;
        private System.Windows.Forms.Label LabelGenericTickList;
        private System.Windows.Forms.Label LabelWatchListName;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.GroupBox GroupBoxOrderSetting;
        private System.Windows.Forms.Button BtnOrderSettingApplyCurrentPrice;
        private System.Windows.Forms.TabPage tabSimulation;
        private System.Windows.Forms.Button BtnLoadMultiHistoricalChart;
        private System.Windows.Forms.Button BtnImportSymbols;
        private System.Windows.Forms.Button BtnUpdateContracts;
        private System.Windows.Forms.Button BtnImportNasdaq;
        private System.Windows.Forms.TextBox TextBoxValidCountryCode;
        private System.Windows.Forms.Button BtnMarketDataSyncTicks;
        private System.Windows.Forms.Button BtnApplyDefaultDownloadPeriod;
        private System.Windows.Forms.Button BtnTestMassiveOrder;
        private System.Windows.Forms.Button BtnHistoricalDataConfigMinuteLastWeek;
        private System.Windows.Forms.Button BtnHistoricalDataConfigDailyFull;
        private System.Windows.Forms.Button BtnHistoricalDataContractSet8;
        private System.Windows.Forms.Button BtnHistoricalDataContractSet7;
        private System.Windows.Forms.Button BtnHistoricalDataContractSet6;
        private System.Windows.Forms.Button BtnHistoricalDataContractSet5;
        private System.Windows.Forms.Button BtnHistoricalDataContractSet4;
        private System.Windows.Forms.Button BtnHistoricalDataContractSet3;
        private System.Windows.Forms.Button BtnHistoricalDataContractSet2;
        private System.Windows.Forms.Button BtnHistoricalDataContractSet1;
    }
}