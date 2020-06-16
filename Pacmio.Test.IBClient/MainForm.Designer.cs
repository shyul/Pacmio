using System.Threading;
using Pacmio;

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
                int timeout = Root.IBClient.Timeout + 2000;
                int j = 0;
                if (Root.IBClient.Connected)
                {
                    Root.IBClient.Disconnect.Start();
                    while (Root.IBClient.Connected)
                    {
                        Thread.Sleep(1);
                        j++;
                        if (j > timeout) break;
                    }
                }
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
            this.BtnMatchSymbols = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.BtnApplyTradeLogToChart = new System.Windows.Forms.Button();
            this.BtnArmLiveTrade = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.TextBoxRunAllSimulationInitialAccountValue = new System.Windows.Forms.TextBox();
            this.BtnRunAllSimulation = new System.Windows.Forms.Button();
            this.BtnTestScalping = new System.Windows.Forms.Button();
            this.BtnCloseChart = new System.Windows.Forms.Button();
            this.ListViewAllCharts = new System.Windows.Forms.ListView();
            this.BtnAlignCharts = new System.Windows.Forms.Button();
            this.BtnTestRealTimeBars = new System.Windows.Forms.Button();
            this.BtnRequestHistoricalTicks = new System.Windows.Forms.Button();
            this.BtnChartsUpdateAll = new System.Windows.Forms.Button();
            this.BtnRequestHistoricalData = new System.Windows.Forms.Button();
            this.tabContract = new System.Windows.Forms.TabPage();
            this.GroupBoxContractSearchResult = new System.Windows.Forms.GroupBox();
            this.GridViewContractSearchResult = new System.Windows.Forms.DataGridView();
            this.BtnValidUSSymbol = new System.Windows.Forms.Button();
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
            this.tabMarketQuote = new System.Windows.Forms.TabPage();
            this.BtnMarketDataTestFormShow = new System.Windows.Forms.Button();
            this.BtnMarketQuoteAddMultiContracts = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.BtnMarketQuoteAddContract = new System.Windows.Forms.Button();
            this.tabMarketDepth = new System.Windows.Forms.TabPage();
            this.mktDepthExchangesGrid_MDT = new System.Windows.Forms.DataGridView();
            this.mktDepthExchangesColumn_Exchange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mktDepthExchangesColumn_SecType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mktDepthExchangesColumn_ListingExch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mktDepthExchangesColumn_ServiceDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mktDepthExchangesColumn_AggGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnRequestMarketDepth = new System.Windows.Forms.Button();
            this.tabScan = new System.Windows.Forms.TabPage();
            this.BtnCancelAllScanner = new System.Windows.Forms.Button();
            this.BtnRequestScannerParameter = new System.Windows.Forms.Button();
            this.scannerGrid = new System.Windows.Forms.DataGridView();
            this.scanRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scanContract = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scanDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scanBenchmark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scanProjection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scanLegStr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnRequestScanner = new System.Windows.Forms.Button();
            this.tabOrder = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.TextBoxOrderId = new System.Windows.Forms.TextBox();
            this.TestMassOrder = new System.Windows.Forms.Button();
            this.BtnOrderBraket = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.DateTimePickerOrderDate = new System.Windows.Forms.DateTimePicker();
            this.CheckBoxOrderWhatIf = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.BtnGetCompletedOrders = new System.Windows.Forms.Button();
            this.TextBoxOrderStopPrice = new System.Windows.Forms.TextBox();
            this.TextBoxOrderLimitPrice = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ComboBoxOrderTimeInForce = new System.Windows.Forms.ComboBox();
            this.ComboxBoxOrderType = new System.Windows.Forms.ComboBox();
            this.TextBoxOrderQuantity = new System.Windows.Forms.TextBox();
            this.BtnGetOpenOrders = new System.Windows.Forms.Button();
            this.BtnModifyOrder = new System.Windows.Forms.Button();
            this.BtnOrder = new System.Windows.Forms.Button();
            this.BtnGlobalCancel = new System.Windows.Forms.Button();
            this.BtnCancelOrder = new System.Windows.Forms.Button();
            this.liveOrdersGroup = new System.Windows.Forms.GroupBox();
            this.GridViewAllOrders = new System.Windows.Forms.DataGridView();
            this.tabTrade = new System.Windows.Forms.TabPage();
            this.BtnExportExecTradeLog = new System.Windows.Forms.Button();
            this.BtnPositionCloseSelected = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnRequestExecData = new System.Windows.Forms.Button();
            this.groupBoxPositions = new System.Windows.Forms.GroupBox();
            this.PositionsGrid = new System.Windows.Forms.DataGridView();
            this.positionContract = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.positionAccount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.positionPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.positionAvgCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PositionCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnRequestPostion = new System.Windows.Forms.Button();
            this.executionsGroup = new System.Windows.Forms.GroupBox();
            this.GridViewTradeTable = new System.Windows.Forms.DataGridView();
            this.tabAccount = new System.Windows.Forms.TabPage();
            this.BtnSubscribePnL = new System.Windows.Forms.Button();
            this.BtnRequestPnL = new System.Windows.Forms.Button();
            this.TreeViewAccount = new System.Windows.Forms.TreeView();
            this.tabFileData = new System.Windows.Forms.TabPage();
            this.BtnDownloadBarTable = new System.Windows.Forms.Button();
            this.BtnReDownloadBarTable = new System.Windows.Forms.Button();
            this.GroupBoxQuandlTool = new System.Windows.Forms.GroupBox();
            this.BtmImportQuandlBlob = new System.Windows.Forms.Button();
            this.BtnAddQuandlFile = new System.Windows.Forms.Button();
            this.BtnMergeQuandlFile = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.ListViewQuandlFileMerge = new System.Windows.Forms.ListView();
            this.BtnTestMassiveSample = new System.Windows.Forms.Button();
            this.BtnExtractSymbols = new System.Windows.Forms.Button();
            this.BtnDownloadMultiTables = new System.Windows.Forms.Button();
            this.BtnExportContracts = new System.Windows.Forms.Button();
            this.BtnImportContracts = new System.Windows.Forms.Button();
            this.BtnTestSymbolsToCheck = new System.Windows.Forms.Button();
            this.DownloadBarTableDetialedProgressBar = new System.Windows.Forms.ProgressBar();
            this.TextBoxMultiContracts = new System.Windows.Forms.RichTextBox();
            this.BtnFindDuplicate = new System.Windows.Forms.Button();
            this.CheckBoxChartToCurrent = new System.Windows.Forms.CheckBox();
            this.LabelBarFreq = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DateTimePickerHistoricalDataStop = new System.Windows.Forms.DateTimePicker();
            this.SelectHistoricalDataBarType = new System.Windows.Forms.ComboBox();
            this.SelectHistoricalDataBarFreq = new System.Windows.Forms.ComboBox();
            this.DateTimePickerHistoricalDataStart = new System.Windows.Forms.DateTimePicker();
            this.ListBoxAccount = new System.Windows.Forms.ListBox();
            this.ib_banner = new System.Windows.Forms.PictureBox();
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
            this.BtnMarketDataTestFormHide = new System.Windows.Forms.Button();
            this.MainTab.SuspendLayout();
            this.tabHistoricalData.SuspendLayout();
            this.tabContract.SuspendLayout();
            this.GroupBoxContractSearchResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewContractSearchResult)).BeginInit();
            this.GroupBoxContractInfo.SuspendLayout();
            this.tabMarketQuote.SuspendLayout();
            this.tabMarketDepth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mktDepthExchangesGrid_MDT)).BeginInit();
            this.tabScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scannerGrid)).BeginInit();
            this.tabOrder.SuspendLayout();
            this.liveOrdersGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewAllOrders)).BeginInit();
            this.tabTrade.SuspendLayout();
            this.groupBoxPositions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PositionsGrid)).BeginInit();
            this.executionsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewTradeTable)).BeginInit();
            this.tabAccount.SuspendLayout();
            this.tabFileData.SuspendLayout();
            this.GroupBoxQuandlTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ib_banner)).BeginInit();
            this.GroupBoxSingleContract.SuspendLayout();
            this.GroupBoxBarTableSetting.SuspendLayout();
            this.GroupBoxMultiContracts.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConnect.Location = new System.Drawing.Point(117, 1167);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(80, 36);
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
            this.MainTab.Controls.Add(this.tabMarketQuote);
            this.MainTab.Controls.Add(this.tabMarketDepth);
            this.MainTab.Controls.Add(this.tabScan);
            this.MainTab.Controls.Add(this.tabOrder);
            this.MainTab.Controls.Add(this.tabTrade);
            this.MainTab.Controls.Add(this.tabAccount);
            this.MainTab.Controls.Add(this.tabFileData);
            this.MainTab.Location = new System.Drawing.Point(12, 190);
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(1560, 969);
            this.MainTab.TabIndex = 3;
            // 
            // tabHistoricalData
            // 
            this.tabHistoricalData.Controls.Add(this.BtnMatchSymbols);
            this.tabHistoricalData.Controls.Add(this.label12);
            this.tabHistoricalData.Controls.Add(this.BtnApplyTradeLogToChart);
            this.tabHistoricalData.Controls.Add(this.BtnArmLiveTrade);
            this.tabHistoricalData.Controls.Add(this.label10);
            this.tabHistoricalData.Controls.Add(this.TextBoxRunAllSimulationInitialAccountValue);
            this.tabHistoricalData.Controls.Add(this.BtnRunAllSimulation);
            this.tabHistoricalData.Controls.Add(this.BtnTestScalping);
            this.tabHistoricalData.Controls.Add(this.BtnCloseChart);
            this.tabHistoricalData.Controls.Add(this.ListViewAllCharts);
            this.tabHistoricalData.Controls.Add(this.BtnAlignCharts);
            this.tabHistoricalData.Controls.Add(this.BtnTestRealTimeBars);
            this.tabHistoricalData.Controls.Add(this.BtnRequestHistoricalTicks);
            this.tabHistoricalData.Controls.Add(this.BtnChartsUpdateAll);
            this.tabHistoricalData.Controls.Add(this.BtnRequestHistoricalData);
            this.tabHistoricalData.Location = new System.Drawing.Point(4, 22);
            this.tabHistoricalData.Name = "tabHistoricalData";
            this.tabHistoricalData.Padding = new System.Windows.Forms.Padding(3);
            this.tabHistoricalData.Size = new System.Drawing.Size(1552, 943);
            this.tabHistoricalData.TabIndex = 1;
            this.tabHistoricalData.Text = "Historical Data";
            this.tabHistoricalData.UseVisualStyleBackColor = true;
            // 
            // BtnMatchSymbols
            // 
            this.BtnMatchSymbols.Location = new System.Drawing.Point(500, 788);
            this.BtnMatchSymbols.Name = "BtnMatchSymbols";
            this.BtnMatchSymbols.Size = new System.Drawing.Size(289, 23);
            this.BtnMatchSymbols.TabIndex = 49;
            this.BtnMatchSymbols.Text = "Match Symbols";
            this.BtnMatchSymbols.UseVisualStyleBackColor = true;
            this.BtnMatchSymbols.Click += new System.EventHandler(this.BtnMatchSymbols_Click);
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
            this.BtnApplyTradeLogToChart.Location = new System.Drawing.Point(14, 364);
            this.BtnApplyTradeLogToChart.Name = "BtnApplyTradeLogToChart";
            this.BtnApplyTradeLogToChart.Size = new System.Drawing.Size(230, 23);
            this.BtnApplyTradeLogToChart.TabIndex = 43;
            this.BtnApplyTradeLogToChart.Text = "Apply Trade Log";
            this.BtnApplyTradeLogToChart.UseVisualStyleBackColor = true;
            this.BtnApplyTradeLogToChart.Click += new System.EventHandler(this.BtnApplyTradeLogToChart_Click);
            // 
            // BtnArmLiveTrade
            // 
            this.BtnArmLiveTrade.Location = new System.Drawing.Point(559, 233);
            this.BtnArmLiveTrade.Name = "BtnArmLiveTrade";
            this.BtnArmLiveTrade.Size = new System.Drawing.Size(230, 23);
            this.BtnArmLiveTrade.TabIndex = 42;
            this.BtnArmLiveTrade.Text = "Arm Live Trade";
            this.BtnArmLiveTrade.UseVisualStyleBackColor = true;
            this.BtnArmLiveTrade.Click += new System.EventHandler(this.BtnArmLiveTrade_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(566, 144);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 13);
            this.label10.TabIndex = 41;
            this.label10.Text = "Initial Value";
            // 
            // TextBoxRunAllSimulationInitialAccountValue
            // 
            this.TextBoxRunAllSimulationInitialAccountValue.Location = new System.Drawing.Point(639, 141);
            this.TextBoxRunAllSimulationInitialAccountValue.Name = "TextBoxRunAllSimulationInitialAccountValue";
            this.TextBoxRunAllSimulationInitialAccountValue.Size = new System.Drawing.Size(150, 22);
            this.TextBoxRunAllSimulationInitialAccountValue.TabIndex = 40;
            this.TextBoxRunAllSimulationInitialAccountValue.Text = "100000";
            // 
            // BtnRunAllSimulation
            // 
            this.BtnRunAllSimulation.Location = new System.Drawing.Point(559, 169);
            this.BtnRunAllSimulation.Name = "BtnRunAllSimulation";
            this.BtnRunAllSimulation.Size = new System.Drawing.Size(230, 23);
            this.BtnRunAllSimulation.TabIndex = 39;
            this.BtnRunAllSimulation.Text = "Run All Simulation";
            this.BtnRunAllSimulation.UseVisualStyleBackColor = true;
            this.BtnRunAllSimulation.Click += new System.EventHandler(this.BtnRunAllSimulation_Click);
            // 
            // BtnTestScalping
            // 
            this.BtnTestScalping.Location = new System.Drawing.Point(559, 74);
            this.BtnTestScalping.Name = "BtnTestScalping";
            this.BtnTestScalping.Size = new System.Drawing.Size(230, 23);
            this.BtnTestScalping.TabIndex = 38;
            this.BtnTestScalping.Text = "Setup Scalping Simulation";
            this.BtnTestScalping.UseVisualStyleBackColor = true;
            this.BtnTestScalping.Click += new System.EventHandler(this.BtnTestScalping_Click);
            // 
            // BtnCloseChart
            // 
            this.BtnCloseChart.Location = new System.Drawing.Point(14, 105);
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
            this.BtnAlignCharts.Location = new System.Drawing.Point(14, 332);
            this.BtnAlignCharts.Name = "BtnAlignCharts";
            this.BtnAlignCharts.Size = new System.Drawing.Size(230, 26);
            this.BtnAlignCharts.TabIndex = 34;
            this.BtnAlignCharts.Text = "Align All Charts";
            this.BtnAlignCharts.UseVisualStyleBackColor = true;
            this.BtnAlignCharts.Click += new System.EventHandler(this.BtnAlignCharts_Click);
            // 
            // BtnTestRealTimeBars
            // 
            this.BtnTestRealTimeBars.Location = new System.Drawing.Point(14, 229);
            this.BtnTestRealTimeBars.Name = "BtnTestRealTimeBars";
            this.BtnTestRealTimeBars.Size = new System.Drawing.Size(230, 23);
            this.BtnTestRealTimeBars.TabIndex = 9;
            this.BtnTestRealTimeBars.Text = "Test Realtime Bars";
            this.BtnTestRealTimeBars.UseVisualStyleBackColor = true;
            this.BtnTestRealTimeBars.Click += new System.EventHandler(this.BtnTestRealTimeBars_Click);
            // 
            // BtnRequestHistoricalTicks
            // 
            this.BtnRequestHistoricalTicks.Location = new System.Drawing.Point(14, 258);
            this.BtnRequestHistoricalTicks.Name = "BtnRequestHistoricalTicks";
            this.BtnRequestHistoricalTicks.Size = new System.Drawing.Size(230, 23);
            this.BtnRequestHistoricalTicks.TabIndex = 3;
            this.BtnRequestHistoricalTicks.Text = "Historical Ticks";
            this.BtnRequestHistoricalTicks.UseVisualStyleBackColor = true;
            this.BtnRequestHistoricalTicks.Click += new System.EventHandler(this.BtnRequestHistoricalTicks_Click);
            // 
            // BtnChartsUpdateAll
            // 
            this.BtnChartsUpdateAll.Location = new System.Drawing.Point(14, 160);
            this.BtnChartsUpdateAll.Name = "BtnChartsUpdateAll";
            this.BtnChartsUpdateAll.Size = new System.Drawing.Size(230, 23);
            this.BtnChartsUpdateAll.TabIndex = 2;
            this.BtnChartsUpdateAll.Text = "Update All To Date w/ Tick";
            this.BtnChartsUpdateAll.UseVisualStyleBackColor = true;
            this.BtnChartsUpdateAll.Click += new System.EventHandler(this.BtnChartsUpdateAll_Click);
            // 
            // BtnRequestHistoricalData
            // 
            this.BtnRequestHistoricalData.Location = new System.Drawing.Point(14, 75);
            this.BtnRequestHistoricalData.Name = "BtnRequestHistoricalData";
            this.BtnRequestHistoricalData.Size = new System.Drawing.Size(230, 23);
            this.BtnRequestHistoricalData.TabIndex = 0;
            this.BtnRequestHistoricalData.Text = "Historical Chart";
            this.BtnRequestHistoricalData.UseVisualStyleBackColor = true;
            this.BtnRequestHistoricalData.Click += new System.EventHandler(this.BtnRequestHistoricalData_Click);
            // 
            // tabContract
            // 
            this.tabContract.Controls.Add(this.GroupBoxContractSearchResult);
            this.tabContract.Controls.Add(this.BtnValidUSSymbol);
            this.tabContract.Controls.Add(this.GroupBoxContractInfo);
            this.tabContract.Controls.Add(this.BtnGetContractInfo);
            this.tabContract.Controls.Add(this.TextBoxSearchSymbol);
            this.tabContract.Controls.Add(this.BtnSearchSymbol);
            this.tabContract.Location = new System.Drawing.Point(4, 22);
            this.tabContract.Name = "tabContract";
            this.tabContract.Size = new System.Drawing.Size(1552, 943);
            this.tabContract.TabIndex = 2;
            this.tabContract.Text = "Contract";
            this.tabContract.UseVisualStyleBackColor = true;
            // 
            // GroupBoxContractSearchResult
            // 
            this.GroupBoxContractSearchResult.Controls.Add(this.GridViewContractSearchResult);
            this.GroupBoxContractSearchResult.Location = new System.Drawing.Point(398, 3);
            this.GroupBoxContractSearchResult.Name = "GroupBoxContractSearchResult";
            this.GroupBoxContractSearchResult.Size = new System.Drawing.Size(706, 937);
            this.GroupBoxContractSearchResult.TabIndex = 65;
            this.GroupBoxContractSearchResult.TabStop = false;
            this.GroupBoxContractSearchResult.Text = "Search Result";
            // 
            // GridViewContractSearchResult
            // 
            this.GridViewContractSearchResult.AllowUserToAddRows = false;
            this.GridViewContractSearchResult.AllowUserToDeleteRows = false;
            this.GridViewContractSearchResult.AllowUserToOrderColumns = true;
            this.GridViewContractSearchResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GridViewContractSearchResult.Location = new System.Drawing.Point(6, 21);
            this.GridViewContractSearchResult.Name = "GridViewContractSearchResult";
            this.GridViewContractSearchResult.ReadOnly = true;
            this.GridViewContractSearchResult.RowHeadersWidth = 20;
            this.GridViewContractSearchResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridViewContractSearchResult.Size = new System.Drawing.Size(694, 910);
            this.GridViewContractSearchResult.TabIndex = 2;
            // 
            // BtnValidUSSymbol
            // 
            this.BtnValidUSSymbol.Location = new System.Drawing.Point(17, 91);
            this.BtnValidUSSymbol.Name = "BtnValidUSSymbol";
            this.BtnValidUSSymbol.Size = new System.Drawing.Size(260, 22);
            this.BtnValidUSSymbol.TabIndex = 63;
            this.BtnValidUSSymbol.Text = "Valid US";
            this.BtnValidUSSymbol.UseVisualStyleBackColor = true;
            this.BtnValidUSSymbol.Click += new System.EventHandler(this.BtnValidUSSymbol_Click);
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
            this.GroupBoxContractInfo.Location = new System.Drawing.Point(1110, 3);
            this.GroupBoxContractInfo.Name = "GroupBoxContractInfo";
            this.GroupBoxContractInfo.Size = new System.Drawing.Size(439, 937);
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
            this.TextBoxSymbolFullName.Size = new System.Drawing.Size(360, 22);
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
            this.TextBoxSymbolIds.Size = new System.Drawing.Size(420, 150);
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
            this.TextBoxSymbolSummaryF.Location = new System.Drawing.Point(8, 472);
            this.TextBoxSymbolSummaryF.Multiline = true;
            this.TextBoxSymbolSummaryF.Name = "TextBoxSymbolSummaryF";
            this.TextBoxSymbolSummaryF.ReadOnly = true;
            this.TextBoxSymbolSummaryF.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxSymbolSummaryF.Size = new System.Drawing.Size(420, 288);
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
            this.LbFinancialSummary.Location = new System.Drawing.Point(8, 456);
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
            this.TextBoxSymbolSummaryB.Size = new System.Drawing.Size(420, 176);
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
            // tabMarketQuote
            // 
            this.tabMarketQuote.Controls.Add(this.BtnMarketDataTestFormHide);
            this.tabMarketQuote.Controls.Add(this.BtnMarketDataTestFormShow);
            this.tabMarketQuote.Controls.Add(this.BtnMarketQuoteAddMultiContracts);
            this.tabMarketQuote.Controls.Add(this.button4);
            this.tabMarketQuote.Controls.Add(this.BtnMarketQuoteAddContract);
            this.tabMarketQuote.Location = new System.Drawing.Point(4, 22);
            this.tabMarketQuote.Name = "tabMarketQuote";
            this.tabMarketQuote.Size = new System.Drawing.Size(1552, 943);
            this.tabMarketQuote.TabIndex = 3;
            this.tabMarketQuote.Text = "Market Quote";
            this.tabMarketQuote.UseVisualStyleBackColor = true;
            // 
            // BtnMarketDataTestFormShow
            // 
            this.BtnMarketDataTestFormShow.Location = new System.Drawing.Point(6, 6);
            this.BtnMarketDataTestFormShow.Name = "BtnMarketDataTestFormShow";
            this.BtnMarketDataTestFormShow.Size = new System.Drawing.Size(100, 23);
            this.BtnMarketDataTestFormShow.TabIndex = 5;
            this.BtnMarketDataTestFormShow.Text = "Show Form";
            this.BtnMarketDataTestFormShow.UseVisualStyleBackColor = true;
            this.BtnMarketDataTestFormShow.Click += new System.EventHandler(this.BtnMarketDataTestFormShow_Click);
            // 
            // BtnMarketQuoteAddMultiContracts
            // 
            this.BtnMarketQuoteAddMultiContracts.Location = new System.Drawing.Point(166, 93);
            this.BtnMarketQuoteAddMultiContracts.Name = "BtnMarketQuoteAddMultiContracts";
            this.BtnMarketQuoteAddMultiContracts.Size = new System.Drawing.Size(133, 23);
            this.BtnMarketQuoteAddMultiContracts.TabIndex = 4;
            this.BtnMarketQuoteAddMultiContracts.Text = "Add Multi Contracts";
            this.BtnMarketQuoteAddMultiContracts.UseVisualStyleBackColor = true;
            this.BtnMarketQuoteAddMultiContracts.Click += new System.EventHandler(this.BtnAddMarketQuoteTest_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(6, 93);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Remove Tick";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // BtnMarketQuoteAddContract
            // 
            this.BtnMarketQuoteAddContract.Location = new System.Drawing.Point(166, 64);
            this.BtnMarketQuoteAddContract.Name = "BtnMarketQuoteAddContract";
            this.BtnMarketQuoteAddContract.Size = new System.Drawing.Size(133, 23);
            this.BtnMarketQuoteAddContract.TabIndex = 2;
            this.BtnMarketQuoteAddContract.Text = "Add Contract";
            this.BtnMarketQuoteAddContract.UseVisualStyleBackColor = true;
            this.BtnMarketQuoteAddContract.Click += new System.EventHandler(this.BtnAddMarketQuote_Click);
            // 
            // tabMarketDepth
            // 
            this.tabMarketDepth.Controls.Add(this.mktDepthExchangesGrid_MDT);
            this.tabMarketDepth.Controls.Add(this.BtnRequestMarketDepth);
            this.tabMarketDepth.Location = new System.Drawing.Point(4, 22);
            this.tabMarketDepth.Name = "tabMarketDepth";
            this.tabMarketDepth.Size = new System.Drawing.Size(1552, 943);
            this.tabMarketDepth.TabIndex = 7;
            this.tabMarketDepth.Text = "Market Depth";
            this.tabMarketDepth.UseVisualStyleBackColor = true;
            // 
            // mktDepthExchangesGrid_MDT
            // 
            this.mktDepthExchangesGrid_MDT.AllowUserToAddRows = false;
            this.mktDepthExchangesGrid_MDT.AllowUserToDeleteRows = false;
            this.mktDepthExchangesGrid_MDT.AllowUserToOrderColumns = true;
            this.mktDepthExchangesGrid_MDT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.mktDepthExchangesGrid_MDT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mktDepthExchangesGrid_MDT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mktDepthExchangesColumn_Exchange,
            this.mktDepthExchangesColumn_SecType,
            this.mktDepthExchangesColumn_ListingExch,
            this.mktDepthExchangesColumn_ServiceDataType,
            this.mktDepthExchangesColumn_AggGroup});
            this.mktDepthExchangesGrid_MDT.Location = new System.Drawing.Point(6, 35);
            this.mktDepthExchangesGrid_MDT.Name = "mktDepthExchangesGrid_MDT";
            this.mktDepthExchangesGrid_MDT.ReadOnly = true;
            this.mktDepthExchangesGrid_MDT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mktDepthExchangesGrid_MDT.Size = new System.Drawing.Size(574, 902);
            this.mktDepthExchangesGrid_MDT.TabIndex = 6;
            // 
            // mktDepthExchangesColumn_Exchange
            // 
            this.mktDepthExchangesColumn_Exchange.HeaderText = "Exchange";
            this.mktDepthExchangesColumn_Exchange.Name = "mktDepthExchangesColumn_Exchange";
            this.mktDepthExchangesColumn_Exchange.ReadOnly = true;
            // 
            // mktDepthExchangesColumn_SecType
            // 
            this.mktDepthExchangesColumn_SecType.HeaderText = "SecType";
            this.mktDepthExchangesColumn_SecType.Name = "mktDepthExchangesColumn_SecType";
            this.mktDepthExchangesColumn_SecType.ReadOnly = true;
            // 
            // mktDepthExchangesColumn_ListingExch
            // 
            this.mktDepthExchangesColumn_ListingExch.HeaderText = "ListingExch";
            this.mktDepthExchangesColumn_ListingExch.Name = "mktDepthExchangesColumn_ListingExch";
            this.mktDepthExchangesColumn_ListingExch.ReadOnly = true;
            // 
            // mktDepthExchangesColumn_ServiceDataType
            // 
            this.mktDepthExchangesColumn_ServiceDataType.HeaderText = "ServiceDataType";
            this.mktDepthExchangesColumn_ServiceDataType.Name = "mktDepthExchangesColumn_ServiceDataType";
            this.mktDepthExchangesColumn_ServiceDataType.ReadOnly = true;
            // 
            // mktDepthExchangesColumn_AggGroup
            // 
            this.mktDepthExchangesColumn_AggGroup.HeaderText = "AggGroup";
            this.mktDepthExchangesColumn_AggGroup.Name = "mktDepthExchangesColumn_AggGroup";
            this.mktDepthExchangesColumn_AggGroup.ReadOnly = true;
            // 
            // BtnRequestMarketDepth
            // 
            this.BtnRequestMarketDepth.Location = new System.Drawing.Point(6, 6);
            this.BtnRequestMarketDepth.Name = "BtnRequestMarketDepth";
            this.BtnRequestMarketDepth.Size = new System.Drawing.Size(100, 23);
            this.BtnRequestMarketDepth.TabIndex = 5;
            this.BtnRequestMarketDepth.Text = "Request";
            this.BtnRequestMarketDepth.UseVisualStyleBackColor = true;
            this.BtnRequestMarketDepth.Click += new System.EventHandler(this.BtnRequestMarketDepth_Click);
            // 
            // tabScan
            // 
            this.tabScan.Controls.Add(this.BtnCancelAllScanner);
            this.tabScan.Controls.Add(this.BtnRequestScannerParameter);
            this.tabScan.Controls.Add(this.scannerGrid);
            this.tabScan.Controls.Add(this.BtnRequestScanner);
            this.tabScan.Location = new System.Drawing.Point(4, 22);
            this.tabScan.Name = "tabScan";
            this.tabScan.Size = new System.Drawing.Size(1552, 943);
            this.tabScan.TabIndex = 5;
            this.tabScan.Text = "Scanner";
            this.tabScan.UseVisualStyleBackColor = true;
            // 
            // BtnCancelAllScanner
            // 
            this.BtnCancelAllScanner.Location = new System.Drawing.Point(440, 6);
            this.BtnCancelAllScanner.Name = "BtnCancelAllScanner";
            this.BtnCancelAllScanner.Size = new System.Drawing.Size(162, 23);
            this.BtnCancelAllScanner.TabIndex = 9;
            this.BtnCancelAllScanner.Text = "Cancel All Scanner";
            this.BtnCancelAllScanner.UseVisualStyleBackColor = true;
            this.BtnCancelAllScanner.Click += new System.EventHandler(this.BtnCancelAllScanner_Click);
            // 
            // BtnRequestScannerParameter
            // 
            this.BtnRequestScannerParameter.Location = new System.Drawing.Point(204, 6);
            this.BtnRequestScannerParameter.Name = "BtnRequestScannerParameter";
            this.BtnRequestScannerParameter.Size = new System.Drawing.Size(162, 23);
            this.BtnRequestScannerParameter.TabIndex = 8;
            this.BtnRequestScannerParameter.Text = "Request Parameter";
            this.BtnRequestScannerParameter.UseVisualStyleBackColor = true;
            this.BtnRequestScannerParameter.Click += new System.EventHandler(this.BtnRequestScannerParameter_Click);
            // 
            // scannerGrid
            // 
            this.scannerGrid.AllowUserToAddRows = false;
            this.scannerGrid.AllowUserToDeleteRows = false;
            this.scannerGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scannerGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scannerGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.scanRank,
            this.scanContract,
            this.scanDistance,
            this.scanBenchmark,
            this.scanProjection,
            this.scanLegStr});
            this.scannerGrid.Location = new System.Drawing.Point(6, 35);
            this.scannerGrid.Name = "scannerGrid";
            this.scannerGrid.ReadOnly = true;
            this.scannerGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.scannerGrid.Size = new System.Drawing.Size(1543, 902);
            this.scannerGrid.TabIndex = 7;
            // 
            // scanRank
            // 
            this.scanRank.HeaderText = "Rank";
            this.scanRank.Name = "scanRank";
            this.scanRank.ReadOnly = true;
            // 
            // scanContract
            // 
            this.scanContract.HeaderText = "Contract";
            this.scanContract.Name = "scanContract";
            this.scanContract.ReadOnly = true;
            this.scanContract.Width = 200;
            // 
            // scanDistance
            // 
            this.scanDistance.HeaderText = "Distance";
            this.scanDistance.Name = "scanDistance";
            this.scanDistance.ReadOnly = true;
            // 
            // scanBenchmark
            // 
            this.scanBenchmark.HeaderText = "Benchmark";
            this.scanBenchmark.Name = "scanBenchmark";
            this.scanBenchmark.ReadOnly = true;
            // 
            // scanProjection
            // 
            this.scanProjection.HeaderText = "Projection";
            this.scanProjection.Name = "scanProjection";
            this.scanProjection.ReadOnly = true;
            // 
            // scanLegStr
            // 
            this.scanLegStr.HeaderText = "Legs";
            this.scanLegStr.Name = "scanLegStr";
            this.scanLegStr.ReadOnly = true;
            // 
            // BtnRequestScanner
            // 
            this.BtnRequestScanner.Location = new System.Drawing.Point(6, 6);
            this.BtnRequestScanner.Name = "BtnRequestScanner";
            this.BtnRequestScanner.Size = new System.Drawing.Size(162, 23);
            this.BtnRequestScanner.TabIndex = 6;
            this.BtnRequestScanner.Text = "Request Scanner";
            this.BtnRequestScanner.UseVisualStyleBackColor = true;
            this.BtnRequestScanner.Click += new System.EventHandler(this.BtnRequestScanner_Click);
            // 
            // tabOrder
            // 
            this.tabOrder.Controls.Add(this.label11);
            this.tabOrder.Controls.Add(this.TextBoxOrderId);
            this.tabOrder.Controls.Add(this.TestMassOrder);
            this.tabOrder.Controls.Add(this.BtnOrderBraket);
            this.tabOrder.Controls.Add(this.label9);
            this.tabOrder.Controls.Add(this.DateTimePickerOrderDate);
            this.tabOrder.Controls.Add(this.CheckBoxOrderWhatIf);
            this.tabOrder.Controls.Add(this.label8);
            this.tabOrder.Controls.Add(this.label7);
            this.tabOrder.Controls.Add(this.BtnGetCompletedOrders);
            this.tabOrder.Controls.Add(this.TextBoxOrderStopPrice);
            this.tabOrder.Controls.Add(this.TextBoxOrderLimitPrice);
            this.tabOrder.Controls.Add(this.label6);
            this.tabOrder.Controls.Add(this.label5);
            this.tabOrder.Controls.Add(this.label4);
            this.tabOrder.Controls.Add(this.ComboBoxOrderTimeInForce);
            this.tabOrder.Controls.Add(this.ComboxBoxOrderType);
            this.tabOrder.Controls.Add(this.TextBoxOrderQuantity);
            this.tabOrder.Controls.Add(this.BtnGetOpenOrders);
            this.tabOrder.Controls.Add(this.BtnModifyOrder);
            this.tabOrder.Controls.Add(this.BtnOrder);
            this.tabOrder.Controls.Add(this.BtnGlobalCancel);
            this.tabOrder.Controls.Add(this.BtnCancelOrder);
            this.tabOrder.Controls.Add(this.liveOrdersGroup);
            this.tabOrder.Location = new System.Drawing.Point(4, 22);
            this.tabOrder.Name = "tabOrder";
            this.tabOrder.Size = new System.Drawing.Size(1552, 943);
            this.tabOrder.TabIndex = 8;
            this.tabOrder.Text = "Order";
            this.tabOrder.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 443);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 13);
            this.label11.TabIndex = 81;
            this.label11.Text = "Order Id";
            // 
            // TextBoxOrderId
            // 
            this.TextBoxOrderId.Location = new System.Drawing.Point(70, 440);
            this.TextBoxOrderId.Name = "TextBoxOrderId";
            this.TextBoxOrderId.Size = new System.Drawing.Size(95, 22);
            this.TextBoxOrderId.TabIndex = 80;
            this.TextBoxOrderId.Text = "100";
            // 
            // TestMassOrder
            // 
            this.TestMassOrder.Location = new System.Drawing.Point(17, 335);
            this.TestMassOrder.Name = "TestMassOrder";
            this.TestMassOrder.Size = new System.Drawing.Size(128, 23);
            this.TestMassOrder.TabIndex = 12;
            this.TestMassOrder.Text = "Test Mass Order";
            this.TestMassOrder.UseVisualStyleBackColor = true;
            this.TestMassOrder.Click += new System.EventHandler(this.TestMassOrder_Click);
            // 
            // BtnOrderBraket
            // 
            this.BtnOrderBraket.Location = new System.Drawing.Point(3, 279);
            this.BtnOrderBraket.Name = "BtnOrderBraket";
            this.BtnOrderBraket.Size = new System.Drawing.Size(162, 23);
            this.BtnOrderBraket.TabIndex = 79;
            this.BtnOrderBraket.Text = "Braket Order";
            this.BtnOrderBraket.UseVisualStyleBackColor = true;
            this.BtnOrderBraket.Click += new System.EventHandler(this.BtnOrderBraket_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 165);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 78;
            this.label9.Text = "GTD";
            // 
            // DateTimePickerOrderDate
            // 
            this.DateTimePickerOrderDate.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.DateTimePickerOrderDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTimePickerOrderDate.Location = new System.Drawing.Point(41, 160);
            this.DateTimePickerOrderDate.Name = "DateTimePickerOrderDate";
            this.DateTimePickerOrderDate.Size = new System.Drawing.Size(124, 22);
            this.DateTimePickerOrderDate.TabIndex = 77;
            this.DateTimePickerOrderDate.Value = new System.DateTime(2020, 3, 20, 17, 0, 0, 0);
            // 
            // CheckBoxOrderWhatIf
            // 
            this.CheckBoxOrderWhatIf.AutoSize = true;
            this.CheckBoxOrderWhatIf.Location = new System.Drawing.Point(48, 208);
            this.CheckBoxOrderWhatIf.Name = "CheckBoxOrderWhatIf";
            this.CheckBoxOrderWhatIf.Size = new System.Drawing.Size(64, 17);
            this.CheckBoxOrderWhatIf.TabIndex = 76;
            this.CheckBoxOrderWhatIf.Text = "What If";
            this.CheckBoxOrderWhatIf.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 108);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 75;
            this.label8.Text = "Stop";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 74;
            this.label7.Text = "Limit";
            // 
            // BtnGetCompletedOrders
            // 
            this.BtnGetCompletedOrders.Location = new System.Drawing.Point(3, 626);
            this.BtnGetCompletedOrders.Name = "BtnGetCompletedOrders";
            this.BtnGetCompletedOrders.Size = new System.Drawing.Size(162, 23);
            this.BtnGetCompletedOrders.TabIndex = 9;
            this.BtnGetCompletedOrders.Text = "Get Completed Orders";
            this.BtnGetCompletedOrders.UseVisualStyleBackColor = true;
            this.BtnGetCompletedOrders.Click += new System.EventHandler(this.BtnGetCompletedOrders_Click);
            // 
            // TextBoxOrderStopPrice
            // 
            this.TextBoxOrderStopPrice.Location = new System.Drawing.Point(48, 105);
            this.TextBoxOrderStopPrice.Name = "TextBoxOrderStopPrice";
            this.TextBoxOrderStopPrice.Size = new System.Drawing.Size(117, 22);
            this.TextBoxOrderStopPrice.TabIndex = 73;
            this.TextBoxOrderStopPrice.Text = "100";
            // 
            // TextBoxOrderLimitPrice
            // 
            this.TextBoxOrderLimitPrice.Location = new System.Drawing.Point(48, 77);
            this.TextBoxOrderLimitPrice.Name = "TextBoxOrderLimitPrice";
            this.TextBoxOrderLimitPrice.Size = new System.Drawing.Size(117, 22);
            this.TextBoxOrderLimitPrice.TabIndex = 72;
            this.TextBoxOrderLimitPrice.Text = "100";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 136);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 13);
            this.label6.TabIndex = 71;
            this.label6.Text = "TIF";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 70;
            this.label5.Text = "Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 69;
            this.label4.Text = "Quantity";
            // 
            // ComboBoxOrderTimeInForce
            // 
            this.ComboBoxOrderTimeInForce.FormattingEnabled = true;
            this.ComboBoxOrderTimeInForce.Location = new System.Drawing.Point(48, 133);
            this.ComboBoxOrderTimeInForce.Name = "ComboBoxOrderTimeInForce";
            this.ComboBoxOrderTimeInForce.Size = new System.Drawing.Size(117, 21);
            this.ComboBoxOrderTimeInForce.TabIndex = 68;
            this.ComboBoxOrderTimeInForce.Text = "Day";
            // 
            // ComboxBoxOrderType
            // 
            this.ComboxBoxOrderType.FormattingEnabled = true;
            this.ComboxBoxOrderType.Location = new System.Drawing.Point(48, 50);
            this.ComboxBoxOrderType.Name = "ComboxBoxOrderType";
            this.ComboxBoxOrderType.Size = new System.Drawing.Size(117, 21);
            this.ComboxBoxOrderType.TabIndex = 67;
            this.ComboxBoxOrderType.Text = " Market";
            // 
            // TextBoxOrderQuantity
            // 
            this.TextBoxOrderQuantity.Location = new System.Drawing.Point(70, 22);
            this.TextBoxOrderQuantity.Name = "TextBoxOrderQuantity";
            this.TextBoxOrderQuantity.Size = new System.Drawing.Size(95, 22);
            this.TextBoxOrderQuantity.TabIndex = 66;
            this.TextBoxOrderQuantity.Text = "100";
            // 
            // BtnGetOpenOrders
            // 
            this.BtnGetOpenOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnGetOpenOrders.Location = new System.Drawing.Point(3, 706);
            this.BtnGetOpenOrders.Name = "BtnGetOpenOrders";
            this.BtnGetOpenOrders.Size = new System.Drawing.Size(162, 23);
            this.BtnGetOpenOrders.TabIndex = 10;
            this.BtnGetOpenOrders.Text = "Get Open Orders";
            this.BtnGetOpenOrders.UseVisualStyleBackColor = true;
            this.BtnGetOpenOrders.Click += new System.EventHandler(this.BtnGetOpenOrders_Click);
            // 
            // BtnModifyOrder
            // 
            this.BtnModifyOrder.Location = new System.Drawing.Point(3, 468);
            this.BtnModifyOrder.Name = "BtnModifyOrder";
            this.BtnModifyOrder.Size = new System.Drawing.Size(162, 23);
            this.BtnModifyOrder.TabIndex = 7;
            this.BtnModifyOrder.Text = "Modify Order";
            this.BtnModifyOrder.UseVisualStyleBackColor = true;
            this.BtnModifyOrder.Click += new System.EventHandler(this.BtnModifyOrder_Click);
            // 
            // BtnOrder
            // 
            this.BtnOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.BtnOrder.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnOrder.Location = new System.Drawing.Point(3, 244);
            this.BtnOrder.Name = "BtnOrder";
            this.BtnOrder.Size = new System.Drawing.Size(162, 29);
            this.BtnOrder.TabIndex = 5;
            this.BtnOrder.Text = "Order";
            this.BtnOrder.UseVisualStyleBackColor = false;
            this.BtnOrder.Click += new System.EventHandler(this.BtnOrder_Click);
            // 
            // BtnGlobalCancel
            // 
            this.BtnGlobalCancel.Location = new System.Drawing.Point(3, 526);
            this.BtnGlobalCancel.Name = "BtnGlobalCancel";
            this.BtnGlobalCancel.Size = new System.Drawing.Size(162, 23);
            this.BtnGlobalCancel.TabIndex = 6;
            this.BtnGlobalCancel.Text = "Global Cancel";
            this.BtnGlobalCancel.UseVisualStyleBackColor = true;
            this.BtnGlobalCancel.Click += new System.EventHandler(this.BtnGlobalCancel_Click);
            // 
            // BtnCancelOrder
            // 
            this.BtnCancelOrder.Location = new System.Drawing.Point(3, 497);
            this.BtnCancelOrder.Name = "BtnCancelOrder";
            this.BtnCancelOrder.Size = new System.Drawing.Size(162, 23);
            this.BtnCancelOrder.TabIndex = 8;
            this.BtnCancelOrder.Text = "Cancel Order";
            this.BtnCancelOrder.UseVisualStyleBackColor = true;
            // 
            // liveOrdersGroup
            // 
            this.liveOrdersGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.liveOrdersGroup.Controls.Add(this.GridViewAllOrders);
            this.liveOrdersGroup.Location = new System.Drawing.Point(171, 3);
            this.liveOrdersGroup.Name = "liveOrdersGroup";
            this.liveOrdersGroup.Size = new System.Drawing.Size(1378, 937);
            this.liveOrdersGroup.TabIndex = 2;
            this.liveOrdersGroup.TabStop = false;
            this.liveOrdersGroup.Text = "Live Orders - double click to modify.";
            // 
            // GridViewAllOrders
            // 
            this.GridViewAllOrders.AllowUserToAddRows = false;
            this.GridViewAllOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GridViewAllOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewAllOrders.Location = new System.Drawing.Point(6, 19);
            this.GridViewAllOrders.Name = "GridViewAllOrders";
            this.GridViewAllOrders.ReadOnly = true;
            this.GridViewAllOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridViewAllOrders.Size = new System.Drawing.Size(1366, 912);
            this.GridViewAllOrders.TabIndex = 0;
            this.GridViewAllOrders.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewAllOrders_RowEnter);
            // 
            // tabTrade
            // 
            this.tabTrade.Controls.Add(this.BtnExportExecTradeLog);
            this.tabTrade.Controls.Add(this.BtnPositionCloseSelected);
            this.tabTrade.Controls.Add(this.button1);
            this.tabTrade.Controls.Add(this.BtnRequestExecData);
            this.tabTrade.Controls.Add(this.groupBoxPositions);
            this.tabTrade.Controls.Add(this.BtnRequestPostion);
            this.tabTrade.Controls.Add(this.executionsGroup);
            this.tabTrade.Location = new System.Drawing.Point(4, 22);
            this.tabTrade.Name = "tabTrade";
            this.tabTrade.Size = new System.Drawing.Size(1552, 943);
            this.tabTrade.TabIndex = 4;
            this.tabTrade.Text = "Trades";
            this.tabTrade.UseVisualStyleBackColor = true;
            // 
            // BtnExportExecTradeLog
            // 
            this.BtnExportExecTradeLog.Location = new System.Drawing.Point(9, 48);
            this.BtnExportExecTradeLog.Name = "BtnExportExecTradeLog";
            this.BtnExportExecTradeLog.Size = new System.Drawing.Size(167, 23);
            this.BtnExportExecTradeLog.TabIndex = 80;
            this.BtnExportExecTradeLog.Text = "Export To TradeLog";
            this.BtnExportExecTradeLog.UseVisualStyleBackColor = true;
            this.BtnExportExecTradeLog.Click += new System.EventHandler(this.BtnExportExecTradeLog_Click);
            // 
            // BtnPositionCloseSelected
            // 
            this.BtnPositionCloseSelected.Location = new System.Drawing.Point(9, 584);
            this.BtnPositionCloseSelected.Name = "BtnPositionCloseSelected";
            this.BtnPositionCloseSelected.Size = new System.Drawing.Size(167, 23);
            this.BtnPositionCloseSelected.TabIndex = 79;
            this.BtnPositionCloseSelected.Text = "Close Selected";
            this.BtnPositionCloseSelected.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 640);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 23);
            this.button1.TabIndex = 77;
            this.button1.Text = "Close All";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BtnCloseAllPosition_Click);
            // 
            // BtnRequestExecData
            // 
            this.BtnRequestExecData.Location = new System.Drawing.Point(9, 19);
            this.BtnRequestExecData.Name = "BtnRequestExecData";
            this.BtnRequestExecData.Size = new System.Drawing.Size(167, 23);
            this.BtnRequestExecData.TabIndex = 78;
            this.BtnRequestExecData.Text = "Request Exec Data";
            this.BtnRequestExecData.UseVisualStyleBackColor = true;
            this.BtnRequestExecData.Click += new System.EventHandler(this.BtnRequestExecData_Click);
            // 
            // groupBoxPositions
            // 
            this.groupBoxPositions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPositions.Controls.Add(this.PositionsGrid);
            this.groupBoxPositions.Location = new System.Drawing.Point(193, 643);
            this.groupBoxPositions.Name = "groupBoxPositions";
            this.groupBoxPositions.Size = new System.Drawing.Size(1356, 297);
            this.groupBoxPositions.TabIndex = 11;
            this.groupBoxPositions.TabStop = false;
            this.groupBoxPositions.Text = "Positions";
            // 
            // PositionsGrid
            // 
            this.PositionsGrid.AllowUserToAddRows = false;
            this.PositionsGrid.AllowUserToDeleteRows = false;
            this.PositionsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PositionsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PositionsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.positionContract,
            this.positionAccount,
            this.positionPosition,
            this.positionAvgCost,
            this.PositionCost});
            this.PositionsGrid.Location = new System.Drawing.Point(6, 21);
            this.PositionsGrid.Name = "PositionsGrid";
            this.PositionsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PositionsGrid.Size = new System.Drawing.Size(1344, 270);
            this.PositionsGrid.TabIndex = 6;
            // 
            // positionContract
            // 
            this.positionContract.HeaderText = "Contract";
            this.positionContract.Name = "positionContract";
            this.positionContract.ReadOnly = true;
            this.positionContract.Width = 150;
            // 
            // positionAccount
            // 
            this.positionAccount.HeaderText = "Account";
            this.positionAccount.Name = "positionAccount";
            this.positionAccount.ReadOnly = true;
            // 
            // positionPosition
            // 
            this.positionPosition.HeaderText = "Position";
            this.positionPosition.Name = "positionPosition";
            this.positionPosition.ReadOnly = true;
            this.positionPosition.Width = 80;
            // 
            // positionAvgCost
            // 
            this.positionAvgCost.HeaderText = "Average Cost";
            this.positionAvgCost.Name = "positionAvgCost";
            this.positionAvgCost.ReadOnly = true;
            // 
            // PositionCost
            // 
            this.PositionCost.HeaderText = "Position Cost";
            this.PositionCost.Name = "PositionCost";
            this.PositionCost.ReadOnly = true;
            // 
            // BtnRequestPostion
            // 
            this.BtnRequestPostion.Location = new System.Drawing.Point(9, 555);
            this.BtnRequestPostion.Name = "BtnRequestPostion";
            this.BtnRequestPostion.Size = new System.Drawing.Size(167, 23);
            this.BtnRequestPostion.TabIndex = 7;
            this.BtnRequestPostion.Text = "Request Postion";
            this.BtnRequestPostion.UseVisualStyleBackColor = true;
            this.BtnRequestPostion.Click += new System.EventHandler(this.BtnRequestPostion_Click);
            // 
            // executionsGroup
            // 
            this.executionsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.executionsGroup.Controls.Add(this.GridViewTradeTable);
            this.executionsGroup.Location = new System.Drawing.Point(196, 0);
            this.executionsGroup.Name = "executionsGroup";
            this.executionsGroup.Size = new System.Drawing.Size(1356, 637);
            this.executionsGroup.TabIndex = 4;
            this.executionsGroup.TabStop = false;
            this.executionsGroup.Text = "Trade Log (Executions)";
            // 
            // GridViewTradeTable
            // 
            this.GridViewTradeTable.AllowUserToAddRows = false;
            this.GridViewTradeTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GridViewTradeTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewTradeTable.Location = new System.Drawing.Point(6, 19);
            this.GridViewTradeTable.Name = "GridViewTradeTable";
            this.GridViewTradeTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridViewTradeTable.Size = new System.Drawing.Size(1344, 612);
            this.GridViewTradeTable.TabIndex = 0;
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
            this.tabAccount.Size = new System.Drawing.Size(1552, 943);
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
            this.TreeViewAccount.Size = new System.Drawing.Size(516, 902);
            this.TreeViewAccount.TabIndex = 5;
            // 
            // tabFileData
            // 
            this.tabFileData.Controls.Add(this.BtnDownloadBarTable);
            this.tabFileData.Controls.Add(this.BtnReDownloadBarTable);
            this.tabFileData.Controls.Add(this.GroupBoxQuandlTool);
            this.tabFileData.Controls.Add(this.BtnTestMassiveSample);
            this.tabFileData.Controls.Add(this.BtnExtractSymbols);
            this.tabFileData.Controls.Add(this.BtnDownloadMultiTables);
            this.tabFileData.Controls.Add(this.BtnExportContracts);
            this.tabFileData.Controls.Add(this.BtnImportContracts);
            this.tabFileData.Location = new System.Drawing.Point(4, 22);
            this.tabFileData.Name = "tabFileData";
            this.tabFileData.Size = new System.Drawing.Size(1552, 943);
            this.tabFileData.TabIndex = 6;
            this.tabFileData.Text = "File / Data";
            this.tabFileData.UseVisualStyleBackColor = true;
            // 
            // BtnDownloadBarTable
            // 
            this.BtnDownloadBarTable.Location = new System.Drawing.Point(32, 69);
            this.BtnDownloadBarTable.Name = "BtnDownloadBarTable";
            this.BtnDownloadBarTable.Size = new System.Drawing.Size(160, 23);
            this.BtnDownloadBarTable.TabIndex = 54;
            this.BtnDownloadBarTable.Text = "Download BarTable";
            this.BtnDownloadBarTable.UseVisualStyleBackColor = true;
            // 
            // BtnReDownloadBarTable
            // 
            this.BtnReDownloadBarTable.Location = new System.Drawing.Point(32, 98);
            this.BtnReDownloadBarTable.Name = "BtnReDownloadBarTable";
            this.BtnReDownloadBarTable.Size = new System.Drawing.Size(160, 23);
            this.BtnReDownloadBarTable.TabIndex = 53;
            this.BtnReDownloadBarTable.Text = "Re-Download BarTable";
            this.BtnReDownloadBarTable.UseVisualStyleBackColor = true;
            // 
            // GroupBoxQuandlTool
            // 
            this.GroupBoxQuandlTool.Controls.Add(this.BtmImportQuandlBlob);
            this.GroupBoxQuandlTool.Controls.Add(this.BtnAddQuandlFile);
            this.GroupBoxQuandlTool.Controls.Add(this.BtnMergeQuandlFile);
            this.GroupBoxQuandlTool.Controls.Add(this.label13);
            this.GroupBoxQuandlTool.Controls.Add(this.ListViewQuandlFileMerge);
            this.GroupBoxQuandlTool.Location = new System.Drawing.Point(987, 98);
            this.GroupBoxQuandlTool.Name = "GroupBoxQuandlTool";
            this.GroupBoxQuandlTool.Size = new System.Drawing.Size(500, 354);
            this.GroupBoxQuandlTool.TabIndex = 52;
            this.GroupBoxQuandlTool.TabStop = false;
            this.GroupBoxQuandlTool.Text = "Quandl Tool";
            // 
            // BtmImportQuandlBlob
            // 
            this.BtmImportQuandlBlob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtmImportQuandlBlob.Location = new System.Drawing.Point(14, 260);
            this.BtmImportQuandlBlob.Name = "BtmImportQuandlBlob";
            this.BtmImportQuandlBlob.Size = new System.Drawing.Size(173, 57);
            this.BtmImportQuandlBlob.TabIndex = 7;
            this.BtmImportQuandlBlob.Text = "Import Quandl";
            this.BtmImportQuandlBlob.UseVisualStyleBackColor = true;
            this.BtmImportQuandlBlob.Click += new System.EventHandler(this.BtmImportQuandlBlob_Click);
            // 
            // BtnAddQuandlFile
            // 
            this.BtnAddQuandlFile.Location = new System.Drawing.Point(14, 177);
            this.BtnAddQuandlFile.Name = "BtnAddQuandlFile";
            this.BtnAddQuandlFile.Size = new System.Drawing.Size(173, 23);
            this.BtnAddQuandlFile.TabIndex = 47;
            this.BtnAddQuandlFile.Text = "Add Quandl File";
            this.BtnAddQuandlFile.UseVisualStyleBackColor = true;
            this.BtnAddQuandlFile.Click += new System.EventHandler(this.BtnAddQuandlFile_Click);
            // 
            // BtnMergeQuandlFile
            // 
            this.BtnMergeQuandlFile.Location = new System.Drawing.Point(14, 206);
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
            this.label13.Location = new System.Drawing.Point(238, 58);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 13);
            this.label13.TabIndex = 46;
            this.label13.Text = "Quandl File List";
            // 
            // ListViewQuandlFileMerge
            // 
            this.ListViewQuandlFileMerge.HideSelection = false;
            this.ListViewQuandlFileMerge.Location = new System.Drawing.Point(202, 74);
            this.ListViewQuandlFileMerge.Name = "ListViewQuandlFileMerge";
            this.ListViewQuandlFileMerge.Size = new System.Drawing.Size(279, 238);
            this.ListViewQuandlFileMerge.TabIndex = 45;
            this.ListViewQuandlFileMerge.UseCompatibleStateImageBehavior = false;
            this.ListViewQuandlFileMerge.View = System.Windows.Forms.View.List;
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
            // BtnExtractSymbols
            // 
            this.BtnExtractSymbols.Location = new System.Drawing.Point(701, 447);
            this.BtnExtractSymbols.Name = "BtnExtractSymbols";
            this.BtnExtractSymbols.Size = new System.Drawing.Size(173, 23);
            this.BtnExtractSymbols.TabIndex = 51;
            this.BtnExtractSymbols.Text = "Extract Symbols";
            this.BtnExtractSymbols.UseVisualStyleBackColor = true;
            this.BtnExtractSymbols.Click += new System.EventHandler(this.BtnExtractSymbols_Click);
            // 
            // BtnDownloadMultiTables
            // 
            this.BtnDownloadMultiTables.Location = new System.Drawing.Point(32, 127);
            this.BtnDownloadMultiTables.Name = "BtnDownloadMultiTables";
            this.BtnDownloadMultiTables.Size = new System.Drawing.Size(160, 23);
            this.BtnDownloadMultiTables.TabIndex = 10;
            this.BtnDownloadMultiTables.Text = "Download Multi Tables";
            this.BtnDownloadMultiTables.UseVisualStyleBackColor = true;
            this.BtnDownloadMultiTables.Click += new System.EventHandler(this.BtnDownloadTables_Click);
            // 
            // BtnExportContracts
            // 
            this.BtnExportContracts.Location = new System.Drawing.Point(644, 531);
            this.BtnExportContracts.Name = "BtnExportContracts";
            this.BtnExportContracts.Size = new System.Drawing.Size(120, 23);
            this.BtnExportContracts.TabIndex = 6;
            this.BtnExportContracts.Text = "Export Contracts";
            this.BtnExportContracts.UseVisualStyleBackColor = true;
            this.BtnExportContracts.Click += new System.EventHandler(this.BtnExportSymbols_Click);
            // 
            // BtnImportContracts
            // 
            this.BtnImportContracts.Location = new System.Drawing.Point(644, 502);
            this.BtnImportContracts.Name = "BtnImportContracts";
            this.BtnImportContracts.Size = new System.Drawing.Size(120, 23);
            this.BtnImportContracts.TabIndex = 5;
            this.BtnImportContracts.Text = "Import Contracts";
            this.BtnImportContracts.UseVisualStyleBackColor = true;
            this.BtnImportContracts.Click += new System.EventHandler(this.BtnImportSymbols_Click);
            // 
            // BtnTestSymbolsToCheck
            // 
            this.BtnTestSymbolsToCheck.Location = new System.Drawing.Point(596, 21);
            this.BtnTestSymbolsToCheck.Name = "BtnTestSymbolsToCheck";
            this.BtnTestSymbolsToCheck.Size = new System.Drawing.Size(167, 23);
            this.BtnTestSymbolsToCheck.TabIndex = 53;
            this.BtnTestSymbolsToCheck.Text = "Format Symbols To Check Only";
            this.BtnTestSymbolsToCheck.UseVisualStyleBackColor = true;
            this.BtnTestSymbolsToCheck.Click += new System.EventHandler(this.BtnTestSymbolsToCheck_Click);
            // 
            // DownloadBarTableDetialedProgressBar
            // 
            this.DownloadBarTableDetialedProgressBar.Location = new System.Drawing.Point(473, 1167);
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
            this.TextBoxMultiContracts.Size = new System.Drawing.Size(584, 143);
            this.TextBoxMultiContracts.TabIndex = 9;
            this.TextBoxMultiContracts.Text = resources.GetString("TextBoxMultiContracts.Text");
            // 
            // BtnFindDuplicate
            // 
            this.BtnFindDuplicate.Location = new System.Drawing.Point(596, 50);
            this.BtnFindDuplicate.Name = "BtnFindDuplicate";
            this.BtnFindDuplicate.Size = new System.Drawing.Size(167, 23);
            this.BtnFindDuplicate.TabIndex = 52;
            this.BtnFindDuplicate.Text = "Find Duplicate Symbols";
            this.BtnFindDuplicate.UseVisualStyleBackColor = true;
            this.BtnFindDuplicate.Click += new System.EventHandler(this.BtnFindDuplicate_Click);
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
            this.ListBoxAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListBoxAccount.FormattingEnabled = true;
            this.ListBoxAccount.Location = new System.Drawing.Point(1479, 63);
            this.ListBoxAccount.Name = "ListBoxAccount";
            this.ListBoxAccount.Size = new System.Drawing.Size(89, 121);
            this.ListBoxAccount.TabIndex = 4;
            // 
            // ib_banner
            // 
            this.ib_banner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ib_banner.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ib_banner.Image = global::TestClient.Properties.Resources.LogoIcon;
            this.ib_banner.Location = new System.Drawing.Point(1335, 12);
            this.ib_banner.Name = "ib_banner";
            this.ib_banner.Size = new System.Drawing.Size(237, 36);
            this.ib_banner.TabIndex = 10;
            this.ib_banner.TabStop = false;
            // 
            // LbStatus
            // 
            this.LbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LbStatus.AutoSize = true;
            this.LbStatus.Location = new System.Drawing.Point(403, 1179);
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
            this.TextBoxSingleContractName.Size = new System.Drawing.Size(220, 22);
            this.TextBoxSingleContractName.TabIndex = 18;
            this.TextBoxSingleContractName.Text = "SPY";
            this.TextBoxSingleContractName.TextChanged += new System.EventHandler(this.TbSymbolName_TextChanged);
            // 
            // MainProgBar
            // 
            this.MainProgBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MainProgBar.Location = new System.Drawing.Point(473, 1188);
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
            this.BtnMasterCancel.Location = new System.Drawing.Point(12, 1167);
            this.BtnMasterCancel.Name = "BtnMasterCancel";
            this.BtnMasterCancel.Size = new System.Drawing.Size(99, 36);
            this.BtnMasterCancel.TabIndex = 38;
            this.BtnMasterCancel.Text = "Master Cancel";
            this.BtnMasterCancel.UseVisualStyleBackColor = false;
            this.BtnMasterCancel.Click += new System.EventHandler(this.BtnMasterCancel_Click);
            // 
            // TextBoxIPAddress
            // 
            this.TextBoxIPAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TextBoxIPAddress.Location = new System.Drawing.Point(212, 1176);
            this.TextBoxIPAddress.Name = "TextBoxIPAddress";
            this.TextBoxIPAddress.Size = new System.Drawing.Size(150, 22);
            this.TextBoxIPAddress.TabIndex = 65;
            this.TextBoxIPAddress.Text = "192.168.18.7";
            // 
            // GroupBoxSingleContract
            // 
            this.GroupBoxSingleContract.Controls.Add(this.label14);
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
            this.GroupBoxMultiContracts.Controls.Add(this.BtnTestSymbolsToCheck);
            this.GroupBoxMultiContracts.Controls.Add(this.BtnFindDuplicate);
            this.GroupBoxMultiContracts.Location = new System.Drawing.Point(498, 14);
            this.GroupBoxMultiContracts.Name = "GroupBoxMultiContracts";
            this.GroupBoxMultiContracts.Size = new System.Drawing.Size(774, 170);
            this.GroupBoxMultiContracts.TabIndex = 69;
            this.GroupBoxMultiContracts.TabStop = false;
            this.GroupBoxMultiContracts.Text = "Multi Contracts";
            // 
            // BtnMarketDataTestFormHide
            // 
            this.BtnMarketDataTestFormHide.Location = new System.Drawing.Point(112, 6);
            this.BtnMarketDataTestFormHide.Name = "BtnMarketDataTestFormHide";
            this.BtnMarketDataTestFormHide.Size = new System.Drawing.Size(100, 23);
            this.BtnMarketDataTestFormHide.TabIndex = 6;
            this.BtnMarketDataTestFormHide.Text = "Hide Form";
            this.BtnMarketDataTestFormHide.UseVisualStyleBackColor = true;
            this.BtnMarketDataTestFormHide.Click += new System.EventHandler(this.BtnMarketDataTestFormHide_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 1215);
            this.Controls.Add(this.DownloadBarTableDetialedProgressBar);
            this.Controls.Add(this.GroupBoxMultiContracts);
            this.Controls.Add(this.GroupBoxBarTableSetting);
            this.Controls.Add(this.GroupBoxSingleContract);
            this.Controls.Add(this.TextBoxIPAddress);
            this.Controls.Add(this.BtnMasterCancel);
            this.Controls.Add(this.MainProgBar);
            this.Controls.Add(this.ListBoxAccount);
            this.Controls.Add(this.LbStatus);
            this.Controls.Add(this.ib_banner);
            this.Controls.Add(this.MainTab);
            this.Controls.Add(this.btnConnect);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1600, 1000);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test IB Client";
            this.MainTab.ResumeLayout(false);
            this.tabHistoricalData.ResumeLayout(false);
            this.tabHistoricalData.PerformLayout();
            this.tabContract.ResumeLayout(false);
            this.tabContract.PerformLayout();
            this.GroupBoxContractSearchResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewContractSearchResult)).EndInit();
            this.GroupBoxContractInfo.ResumeLayout(false);
            this.GroupBoxContractInfo.PerformLayout();
            this.tabMarketQuote.ResumeLayout(false);
            this.tabMarketDepth.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mktDepthExchangesGrid_MDT)).EndInit();
            this.tabScan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scannerGrid)).EndInit();
            this.tabOrder.ResumeLayout(false);
            this.tabOrder.PerformLayout();
            this.liveOrdersGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewAllOrders)).EndInit();
            this.tabTrade.ResumeLayout(false);
            this.groupBoxPositions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PositionsGrid)).EndInit();
            this.executionsGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridViewTradeTable)).EndInit();
            this.tabAccount.ResumeLayout(false);
            this.tabFileData.ResumeLayout(false);
            this.GroupBoxQuandlTool.ResumeLayout(false);
            this.GroupBoxQuandlTool.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ib_banner)).EndInit();
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
        private System.Windows.Forms.TabPage tabTrade;
        private System.Windows.Forms.TabPage tabMarketQuote;
        private System.Windows.Forms.Label LbStatus;
        private System.Windows.Forms.CheckBox CheckBoxSingleContractUseSmart;
        private System.Windows.Forms.ComboBox SelectBoxSingleContractExchange;
        private System.Windows.Forms.Label LabelSingleContractExchange;
        private System.Windows.Forms.ComboBox SelectBoxSingleContractSecurityType;
        private System.Windows.Forms.Label LabelSingleContractType;
        private System.Windows.Forms.Label LabelSingleContractName;
        private System.Windows.Forms.TextBox TextBoxSingleContractName;
        private System.Windows.Forms.TabPage tabScan;
        private System.Windows.Forms.ListBox ListBoxAccount;
        private System.Windows.Forms.TreeView TreeViewAccount;
        private System.Windows.Forms.Button BtnRequestHistoricalData;
        private System.Windows.Forms.Button BtnRequestHistoricalTicks;
        private System.Windows.Forms.Button BtnChartsUpdateAll;
        private System.Windows.Forms.DateTimePicker DateTimePickerHistoricalDataStart;
        private System.Windows.Forms.ComboBox SelectHistoricalDataBarType;
        private System.Windows.Forms.ComboBox SelectHistoricalDataBarFreq;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button BtnMarketQuoteAddContract;
        private System.Windows.Forms.DateTimePicker DateTimePickerHistoricalDataStop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelBarFreq;
        private System.Windows.Forms.ProgressBar MainProgBar;
        private System.Windows.Forms.DataGridView PositionsGrid;
        private System.Windows.Forms.Button BtnRequestPostion;
        private System.Windows.Forms.GroupBox executionsGroup;
        private System.Windows.Forms.DataGridView GridViewTradeTable;
        private System.Windows.Forms.GroupBox liveOrdersGroup;
        private System.Windows.Forms.DataGridView GridViewAllOrders;
        private System.Windows.Forms.Button BtnModifyOrder;
        private System.Windows.Forms.Button BtnGlobalCancel;
        private System.Windows.Forms.Button BtnOrder;
        private System.Windows.Forms.Button BtnCancelOrder;
        private System.Windows.Forms.Button BtnGetCompletedOrders;
        private System.Windows.Forms.TabPage tabFileData;
        private System.Windows.Forms.DataGridView scannerGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn scanRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn scanContract;
        private System.Windows.Forms.DataGridViewTextBoxColumn scanDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn scanBenchmark;
        private System.Windows.Forms.DataGridViewTextBoxColumn scanProjection;
        private System.Windows.Forms.DataGridViewTextBoxColumn scanLegStr;
        private System.Windows.Forms.Button BtnRequestScanner;
        private System.Windows.Forms.TabPage tabMarketDepth;
        private System.Windows.Forms.DataGridView mktDepthExchangesGrid_MDT;
        private System.Windows.Forms.DataGridViewTextBoxColumn mktDepthExchangesColumn_Exchange;
        private System.Windows.Forms.DataGridViewTextBoxColumn mktDepthExchangesColumn_SecType;
        private System.Windows.Forms.DataGridViewTextBoxColumn mktDepthExchangesColumn_ListingExch;
        private System.Windows.Forms.DataGridViewTextBoxColumn mktDepthExchangesColumn_ServiceDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn mktDepthExchangesColumn_AggGroup;
        private System.Windows.Forms.Button BtnRequestMarketDepth;
        private System.Windows.Forms.Button BtnGetOpenOrders;
        private System.Windows.Forms.DataGridViewTextBoxColumn positionContract;
        private System.Windows.Forms.DataGridViewTextBoxColumn positionAccount;
        private System.Windows.Forms.DataGridViewTextBoxColumn positionPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn positionAvgCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionCost;
        private System.Windows.Forms.Button BtnMarketQuoteAddMultiContracts;
        private System.Windows.Forms.Button BtnExportContracts;
        private System.Windows.Forms.Button BtnImportContracts;
        private System.Windows.Forms.DataGridView GridViewContractSearchResult;
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
        private System.Windows.Forms.Button BtnTestScalping;
        private System.Windows.Forms.TextBox TextBoxIPAddress;
        private System.Windows.Forms.GroupBox groupBoxPositions;
        private System.Windows.Forms.TabPage tabOrder;
        private System.Windows.Forms.TextBox TextBoxOrderQuantity;
        private System.Windows.Forms.ComboBox ComboxBoxOrderType;
        private System.Windows.Forms.ComboBox ComboBoxOrderTimeInForce;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TextBoxOrderStopPrice;
        private System.Windows.Forms.TextBox TextBoxOrderLimitPrice;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox CheckBoxOrderWhatIf;
        private System.Windows.Forms.Button TestMassOrder;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BtnRequestExecData;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker DateTimePickerOrderDate;
        private System.Windows.Forms.Button BtnPositionCloseSelected;
        private System.Windows.Forms.Button BtnRunAllSimulation;
        private System.Windows.Forms.Button BtnOrderBraket;
        private System.Windows.Forms.TextBox TextBoxRunAllSimulationInitialAccountValue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button BtnArmLiveTrade;
        private System.Windows.Forms.Button BtnExportExecTradeLog;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TextBoxOrderId;
        private System.Windows.Forms.Button BtnApplyTradeLogToChart;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button BtnAddQuandlFile;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ListView ListViewQuandlFileMerge;
        private System.Windows.Forms.Button BtnMergeQuandlFile;
        private System.Windows.Forms.Button BtnMatchSymbols;
        private System.Windows.Forms.Button BtnExtractSymbols;
        private System.Windows.Forms.Button BtnFindDuplicate;
        private System.Windows.Forms.Button BtnTestSymbolsToCheck;
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
        private System.Windows.Forms.GroupBox GroupBoxContractSearchResult;
        private System.Windows.Forms.GroupBox GroupBoxQuandlTool;
        private System.Windows.Forms.Button BtnReDownloadBarTable;
        private System.Windows.Forms.Button BtnDownloadBarTable;
        private System.Windows.Forms.Button BtnMarketDataTestFormShow;
        private System.Windows.Forms.Button BtnMarketDataTestFormHide;
    }
}