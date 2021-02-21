﻿using Pacmio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;


namespace TestClient
{
    public partial class MainForm : Form
    {
        public static AccountInfo TestAccount => AccountPositionManager.GetAccountById("DU332281");

        public BarFreq BarFreq => SelectHistoricalDataBarFreq.Text.ParseEnum<BarFreq>();

        public BarType BarType => SelectHistoricalDataBarType.Text.ParseEnum<BarType>();

        public Period HistoricalPeriod
        {
            get
            {
                return (CheckBoxChartToCurrent.Checked) ? new Period(DateTimePickerHistoricalDataStart.Value, true) :
                    new Period(DateTimePickerHistoricalDataStart.Value, DateTimePickerHistoricalDataStop.Value);
            }
            set
            {
                this.Invoke(() =>
                {
                    try
                    {
                        DateTimePickerHistoricalDataStart.Value = value.Start;
                        DateTimePickerHistoricalDataStop.Value = value.Stop;
                    }
                    catch (Exception e) when (e is ArgumentOutOfRangeException)
                    {

                    }
                });
                /*
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        DateTimePickerHistoricalDataStart.Value = value.Start;
                        DateTimePickerHistoricalDataStop.Value = value.Stop;
                    });
                }
                else
                {
                    DateTimePickerHistoricalDataStart.Value = value.Start;
                    DateTimePickerHistoricalDataStop.Value = value.Stop;
                }*/
            }
        }

        int MainProgBarValue = 0;

        public static MarketDataGridView MarketDataGridView { get; } = new MarketDataGridView("Market Data");

        public MainForm()
        {
            InitializeComponent();

            OrderTest.LiveAccount = AccountPositionManager.GetOrCreateAccountById("DU332281");
            /*
            ContractTest.InitializeTable(GridViewContractSearchResult);
            OrderTest.InitializeTable(GridViewAllOrders);
            TradeTest.InitializeTable(GridViewTradeTable);

            */

            AccountPositionManager.UpdatedHandler += AccountUpdatedHandler;
            WatchListManager.Add(MarketDataGridView);

            TextBoxIPAddress.Text = Root.Settings.IBServerAddress;
            UpdateAccountList();
            ToggleConnect();

            Root.OnNetConnectedHandler += NetClientOnConnectedHandler;

            TradeManager.UpdatedHandler += TradeTableHandler;

            Progress = new Progress<float>(percent =>
            {
                //Console.WriteLine("Progress Reported: " + percent.ToString("0.##") + "%");
                int pct = percent.ToInt32();
                if (pct >= 0 && pct <= 100 && MainProgBarValue != pct)
                {
                    MainProgBar.Value = MainProgBarValue = pct;
                    Console.WriteLine("Progress Reported: " + MainProgBarValue.ToString("0.##") + "%");
                }
            });

            Detailed_Progress = new Progress<float>(p =>
            {
                int val = p.ToInt32();
                if (val > 100) val = 100;
                else if (val < 0) val = 0;
                DownloadBarTableDetialedProgressBar.Value = val;
            });

            SelectBoxSingleContractExchange.Items.Add<Exchange>();
            //SelectSecurityType.Items.Add<ContractType>();
            SelectHistoricalDataBarFreq.Items.Add<BarFreq>();
            SelectHistoricalDataBarType.Items.Add<BarType>();

            ComboxBoxOrderSettingType.Items.Add<OrderType>();
            ComboBoxOrderSettingTIF.Items.Add<OrderTimeInForce>();

            if (CheckBoxChartToCurrent.Checked)
            {
                DateTimePickerHistoricalDataStop.Enabled = false;
                DateTimePickerHistoricalDataStop.Value = DateTime.Now;
            }
            else
                DateTimePickerHistoricalDataStop.Enabled = true;

            DateTime time = DateTime.Now;

            DateTimePickerHistoricalDataStop.Value = time.AddHours(3);

            while (time.DayOfWeek == DayOfWeek.Saturday || time.DayOfWeek == DayOfWeek.Sunday)
            {
                time = time.AddDays(-1);
            }

            DateTimePickerHistoricalDataStart.Value = time.Date;

            TestFreqAlign tfa = new TestFreqAlign();
            TestMultiPeriodDataSource tmpds = new TestMultiPeriodDataSource();
            //tfa.Show();
            //tmpds.Show();

            //if (DateTime.Now.Hour > 17)
            //LoadValidSymbolHistoricalDataChart();
        }

        private void BtnHistoricalDataConfigDailyFull_Click(object sender, EventArgs e)
        {
            SelectHistoricalDataBarFreq.Text = "Daily";
            DateTimePickerHistoricalDataStart.Value = new DateTime(1900, 1, 1);
        }

        private void BtnHistoricalDataConfigMinuteLastWeek_Click(object sender, EventArgs e)
        {
            SelectHistoricalDataBarFreq.Text = "Minute";
            DateTime time = DateTime.Now.AddDays(-7);
            while (time.DayOfWeek != DayOfWeek.Sunday) time = time.AddDays(-1);
            time = time.Date;
            DateTimePickerHistoricalDataStart.Value = time;
            CheckBoxChartToCurrent.Checked = true;
        }

        private void BtnHistoricalDataContractSet1_Click(object sender, EventArgs e)
        {
            TextBoxSingleContractName.Text = BtnHistoricalDataContractSet1.Text;
            ValidateSymbol();
        }

        private void BtnHistoricalDataContractSet2_Click(object sender, EventArgs e)
        {
            TextBoxSingleContractName.Text = BtnHistoricalDataContractSet2.Text;
            ValidateSymbol();
        }

        private void BtnHistoricalDataContractSet3_Click(object sender, EventArgs e)
        {
            TextBoxSingleContractName.Text = BtnHistoricalDataContractSet3.Text;
            ValidateSymbol();
        }

        private void BtnHistoricalDataContractSet4_Click(object sender, EventArgs e)
        {
            TextBoxSingleContractName.Text = BtnHistoricalDataContractSet4.Text;
            ValidateSymbol();
        }

        private void BtnHistoricalDataContractSet5_Click(object sender, EventArgs e)
        {
            TextBoxSingleContractName.Text = BtnHistoricalDataContractSet5.Text;
            ValidateSymbol();
        }

        private void BtnHistoricalDataContractSet6_Click(object sender, EventArgs e)
        {
            TextBoxSingleContractName.Text = BtnHistoricalDataContractSet6.Text;
            ValidateSymbol();
        }

        private void BtnHistoricalDataContractSet7_Click(object sender, EventArgs e)
        {
            TextBoxSingleContractName.Text = BtnHistoricalDataContractSet7.Text;
            ValidateSymbol();
        }

        private void BtnHistoricalDataContractSet8_Click(object sender, EventArgs e)
        {
            TextBoxSingleContractName.Text = BtnHistoricalDataContractSet8.Text;
            ValidateSymbol();
        }

        private void BtnMasterCancel_Click(object sender, EventArgs e)
        {
            if (!(Cts is null))
            {
                Console.WriteLine("\n ##### Master Cancel !! \n");
                Cts.Cancel();
            }

        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            Root.Settings.IBServerAddress = TextBoxIPAddress.Text;

            btnConnect.Enabled = false;
            if (Root.NetConnected)
            {
                Root.NetClientStop();
            }
            else
            {
                Root.NetClientStart();

            }
            Thread.Sleep(100);
            btnConnect.Enabled = true;
        }

        #region Symbols

        private void TbSymbolName_TextChanged(object sender, EventArgs e)
        {
            TextBoxSingleContractName.ForeColor = Color.Orange;
        }



        private void BtnGetContractInfo_Click(object sender, EventArgs e)
        {
            string symbol = TextBoxSingleContractName.Text.ToUpper();
            Cts = new CancellationTokenSource();
            ContractManager.GetOrFetch(symbol, "US", true, Cts);
        }

        #endregion Symbols



        private void BtnAccountSummary_Click(object sender, EventArgs e)
        {
            AccountPositionManager.Request_AccountSummary();
        }


        private void BtnRequestPostion_Click(object sender, EventArgs e)
        {
            AccountPositionManager.Request_Position();
        }

        private void BtnGetOpenOrders_Click(object sender, EventArgs e)
        {
            OrderManager.Request_AllOpenOrders();
            //OrderManager.Request_OpenOrders();
        }

        private void CheckBoxChartToCurrent_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxChartToCurrent.Checked)
            {
                DateTimePickerHistoricalDataStop.Enabled = false;
                DateTimePickerHistoricalDataStop.Value = DateTime.Now.AddHours(10);
            }
            else
                DateTimePickerHistoricalDataStop.Enabled = true;
        }

        #region Bar Chart



        private void BtnLoadHistoricalChart_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                BarFreq freq = BarFreq;
                BarType type = BarType;
                Period pd = HistoricalPeriod;

                //Strategy tr = new TestStrategy(freq);

                Cts = new CancellationTokenSource();

                Task.Run(() =>
                {
                    BarTableTest.BarTableSet.AddChart(ContractTest.ActiveContract, BarTableTest.TestBarAnalysisSet, freq, type, ref pd, Cts);
                    HistoricalPeriod = pd;
                }, Cts.Token);

                Root.Form.Show();
            }
        }

        private void BtnLoadMultiHistoricalChart_Click(object sender, EventArgs e)
        {
            string symbolText = TextBoxMultiContracts.Text;
            BarFreq freq = BarFreq;
            BarType type = BarType;
            //IAnalysisSetting tr = new TestStrategy(freq);

            var symbols = ContractManager.GetSymbolList(ref symbolText);
            var cList = ContractManager.GetOrFetch(symbols, "US", Cts = new CancellationTokenSource(), null).Select(n => (n, BarTableTest.TestBarAnalysisSet));

            if (Cts is null || Cts.IsCancellationRequested) Cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                BarTableTest.BarTableSet.AddChart(cList, freq, type, HistoricalPeriod, Cts, Progress);
            }, Cts.Token);
            Root.Form.Show();
        }

        private void BtnAlignCharts_Click(object sender, EventArgs e) => BarChart.PointerToEndAll();

        private void BtnChartsUpdateAll_Click(object sender, EventArgs e)
        {
            BarFreq freq = BarFreq;
            BarType type = BarType;
            if (Cts is null || Cts.IsCancellationRequested) Cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                BarTableTest.BarTableSet.UpdatePeriod(freq, type, HistoricalPeriod, Cts, Progress);
                BarTableTest.BarTableSet.Calculate(BarTableTest.TestBarAnalysisSet, Cts, Progress);
                BarChart.PointerToEndAll();
            }, Cts.Token);
        }

        private void BtnDownloadBarTable_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                BarFreq freq = BarFreq;
                BarType type = BarType;
                if (Cts is null || Cts.IsCancellationRequested) Cts = new CancellationTokenSource();
                Period pd = new Period(new DateTime(1000, 1, 1), DateTime.Now);

                Task.Run(() =>
                {
                    BarTableTest.BarTableSet.AddContract(ContractTest.ActiveContract, BarFreq.Daily, type, ref pd, Cts);
                    pd = HistoricalPeriod;
                    BarTableTest.BarTableSet.AddContract(ContractTest.ActiveContract, freq, type, ref pd, Cts);
                    HistoricalPeriod = pd;
                    Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": Finished!");
                }, Cts.Token);
            }
        }

        private void BtnReDownloadBarTable_Click(object sender, EventArgs e)
        {

        }

        private void BtnDownloadMultiTables_Click(object sender, EventArgs e)
        {
            string symbolText = TextBoxMultiContracts.Text;

            BarFreq freq = BarFreq;
            BarType type = BarType;
            if (Cts is null || Cts.IsCancellationRequested) Cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                var symbols = ContractManager.GetSymbolList(ref symbolText);
                var cList = ContractManager.GetOrFetch(symbols, "US", Cts = new CancellationTokenSource(), null);
                BarTable.Download(cList, new List<(BarFreq freq, BarType type, Period period)>() { (freq, type, HistoricalPeriod) }, Cts, Progress);
                //BarTableTest.BarTableSet.AddContract(cList, BarFreq.Daily, type, new Period(new DateTime(1000, 1, 1), DateTime.Now), Cts, Progress);
                //BarTableTest.BarTableSet.AddContract(cList, freq, type, HistoricalPeriod, Cts, Progress);
            }, Cts.Token);
        }

        private void BtnApplyDefaultDownloadPeriod_Click(object sender, EventArgs e)
        {
            HistoricalPeriod = new Period(new DateTime(2019, 7, 1), DateTime.Now.AddDays(1));
            CheckBoxChartToCurrent.Checked = false;
        }

        #endregion Bar Chart

        #region Simulation

        private void BtnSetupSimulation_Click(object sender, EventArgs e)
        {
            Cts = new CancellationTokenSource();
            Task.Run(() =>
            {
                var list = ContractManager.Values.AsParallel().Where(n => n is Stock && !n.Name.Contains(' ') && (n.Exchange == Exchange.NASDAQ || n.Exchange == Exchange.NYSE) && n.Status == ContractStatus.Alive && !n.NameSuffix.Contains("ETF") && !n.NameSuffix.Contains("ETN") && !n.NameSuffix.Contains("ADR")).Select(n => (Stock)n);
                //StrategyManager.GetWatchList(list, Cts, Progress);
            }, Cts.Token);
        }

        private void BtnRunAllSimulation_Click(object sender, EventArgs e)
        {

        }

        #endregion Simulation

        private void BtnRequestHistoricalTicks_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected || !ValidateSymbol()) return;



            //Client.Request_HistoricalTick(ContractTest.ActiveContract, pd);
            //Client.SendRequest_HistoricalTick(ContractTest.ActiveContract, DateTime.Now.AddHours(-6));
        }
















        private void BtnSearchSymbol_Click(object sender, EventArgs e)
        {
            ContractTest.UpdateSymbolInfoTable(TextBoxSearchSymbol.Text);
        }







        private void BtnRequestScanner_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected) return;

            Pacmio.IB.WatchList wcl = new Pacmio.IB.WatchList("Most Active", 100)
            {
                ScanType = "MOST_ACTIVE",
                Price = (10, 100),
                VolumeMinimum = 1e7,
                MarketCap = (1e8, double.NaN),
                ExtraConfig = "stkTypes=inc:CORP",
            };

            //FilterOptions = "openGapPercAbove=1;priceAbove=5;priceBelow=50;avgVolumeAbove=10000;marketCapAbove1e6=100;marketCapBelow1e6=100000;stkTypes=inc:CORP;"
            //FilterOptions = "priceAbove=5;priceBelow=300;avgVolumeAbove=10000000;marketCapAbove1e6=10000;stkTypes=inc:CORP;"
            //FilterOptions = "openGapPercAbove=1;priceAbove=5;priceBelow=50;avgVolumeAbove=10000;marketCapAbove1e6=100;marketCapBelow1e6=100000;stkTypes=inc:CORP;"
            //FilterOptions = "openGapPercBelow=-1;priceAbove=5;priceBelow=50;avgVolumeAbove=10000;marketCapAbove1e6=100;marketCapBelow1e6=100000;stkTypes=inc:CORP;"
            //FilterOptions = "priceAbove=10;priceBelow=100;avgVolumeAbove=10000000;marketCapAbove1e6=5000;marketCapBelow1e6=20000;stkTypes=inc:CORP;"

            wcl = ScannerManager.Add(wcl);

            wcl.Start();


            //ScannerManager.GetOrAdd(info_MOST_ACTIVE);
            //ScannerList.GetOrAdd(info_TOP_OPEN_PERC_GAIN);
            //ScannerList.GetOrAdd(info_TOP_OPEN_PERC_LOSE);
        }

        private void BtnCancelAllScanner_Click(object sender, EventArgs e)
        {
            ScannerManager.Stop();
        }

        private void BtnRequestScannerParameter_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected) return;
            ScannerManager.Request_ScannerParameters();
        }



        private void BtnTestMassiveSamples_Click(object sender, EventArgs e)
        {
            string[] symbols = new string[] { "XLNX", "FB" ,"AAPL", "LULU", "GOOGL", "NFLX", "NATI", "TSLA",
                                            "EDU", "QQQ", "NIO", "KEYS", "A","DTSS","SINT", "HYG","SPY","NEAR",
                                            "TQQQ","BA","B","T", "ADI", "TXN", "INTC","NVDA","D","QBIO","JPM",
                                            "WFC","W", "GILD","ABBV","MSFT","AMGN","UPRO","ALXN", "IBM" };

            foreach (Contract c in ContractManager.GetOrFetch(symbols, "US", Cts = new CancellationTokenSource(), null))
            {
                Console.WriteLine("Request Realtime Bars: " + c.ToString());
                //c.Request_RealTimeBars();
            }
        }

        private void BtnTestRealTimeBars_Click(object sender, EventArgs e)
        {
            if (!ValidateSymbol()) return;
            //ContractTest.ActiveContract.Request_RealTimeBars();
        }

        private void BtnRequestPnL_Click(object sender, EventArgs e)
        {

        }

        private void BtnSubscribePnL_Click(object sender, EventArgs e)
        {

        }





        private void TestMassOrder_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected) return;
            AccountPositionManager.Request_AccountSummary();

            string[] symbols = new string[] { "XLNX", "TQQQ", "ET", "LULU", "BAC", "JPM" };
            var list = ContractManager.GetOrFetch(symbols, "US", null, null);
            string tickList = TextBoxGenericTickList.Text;
            foreach (Contract c in list)
            {
                c.MarketData.StartTicks();// (tickList);
            }

        }

        private void BtnGlobalCancel_Click(object sender, EventArgs e)
        {
            OrderManager.CancelAllOrders();
        }

        private void BtnRequestExecData_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected) return;
            TradeManager.RequestExecutionData();
        }

        private void BtnCloseAllPosition_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected) return;
            AccountPositionManager.EmergencyCloseAllPositions();
        }

        private void BtnArmLiveTrade_Click(object sender, EventArgs e)
        {

        }

        private void BtnExportExecTradeLog_Click(object sender, EventArgs e)
        {
            Root.SaveFile.Filter = "Transaction Log file (*.tlg) | *.tlg";

            if (Root.SaveFile.ShowDialog() == DialogResult.OK)
            {
                TradeManager.ExportTradeLog(Root.SaveFile.FileName);
            }
        }

        private void BtnApplyTradeLogToChart_Click(object sender, EventArgs e)
        {

            /*
            foreach (BarTable bt in ChartList.List.Select(n => n.BarTable))
            {
                Contract c = bt.Contract;
                var trades = TradeLogManager.Get(c);

                for (int i = 0; i < bt.Count; i++)
                {
                    Bar b = bt[i];
                    b.ResetPositionTrackingInfo();

                    if (i > 0)
                    {
                        Bar b_1 = bt[i - 1];
                        b.PositionQuantity = b_1.PositionQuantity;
                        b.PositionCostBasis = b_1.PositionCostBasis;

                        if (b.PositionQuantity > 0)
                        {
                            b.ActionType = TradeActionType.LongHold;
                        }
                        else if (b.PositionQuantity < 0)
                        {
                            b.ActionType = TradeActionType.ShortHold;
                        }
                        else
                        {
                            b.ActionType = TradeActionType.None;
                            b.PositionCostBasis = double.NaN;
                        }

                    }


                    var selectedTrades = trades.Where(ti => b.Period.Contains(ti.ExecuteTime)).OrderBy(ti => ti.ExecuteTime);

                    if (selectedTrades.Count() > 0)
                    {
                        double totalQuantity = selectedTrades.Select(n => n.Quantity).Sum();
                        double totalProceeds = selectedTrades.Select(n => n.Quantity * n.Price).Sum();
                        double averagePrice = totalProceeds / totalQuantity;

                        TradeInfo lastTrade = selectedTrades.Last();
                        if (lastTrade.LastLiquidity == LiquidityType.Added)
                        {
                            if (lastTrade.Quantity > 0) b.ActionType = TradeActionType.Long;
                            else if (lastTrade.Quantity < 0) b.ActionType = TradeActionType.Short;
                        }
                        else if (lastTrade.LastLiquidity == LiquidityType.Removed)
                        {
                            if (lastTrade.Quantity > 0) b.ActionType = TradeActionType.Cover;
                            else if (lastTrade.Quantity < 0) b.ActionType = TradeActionType.Sell;
                        }

                        b.PositionQuantity += totalQuantity;
                        b.PositionCostBasis = averagePrice;


                    }


                }



            }*/

        }

        public static Progress<float> Detailed_Progress;

        private void BtnCleanUpDuplicateStock_Click(object sender, EventArgs e)
        {
            Cts = new CancellationTokenSource();
            Task.Run(() => { ContractManager.RemoveDuplicateStock("US", Cts); });
        }

        #region Quandl Tools

        private Dictionary<string, DateTime> MergeEODFiles { get; } = new Dictionary<string, DateTime>();

        private void BtnAddQuandlFile_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = Root.OpenFile.FileName;
                ListViewQuandlFileMerge.Items.Add(fileName);
                MergeEODFiles.CheckAdd(fileName, File.GetLastWriteTime(fileName));
            }
        }

        private void BtnMergeQuandlFile_Click(object sender, EventArgs e)
        {
            if (Root.SaveFile.ShowDialog() == DialogResult.OK)
            {

                var list = MergeEODFiles.OrderByDescending(n => n.Value).Select(n => n.Key);

                Cts = new CancellationTokenSource();
                Task m = new Task(() => { Quandl.MergeEODFiles(list, Root.SaveFile.FileName, Cts, Progress); });
                m.Start();
            }
        }

        private void BtnExtractSymbols_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";
            Root.SaveFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                if (Root.SaveFile.ShowDialog() == DialogResult.OK)
                {
                    string sourceFileName = Root.OpenFile.FileName;
                    string destFileName = Root.SaveFile.FileName;

                    Cts = new CancellationTokenSource();
                    Task m = new Task(() =>
                    {
                        var list = Quandl.ImportSymbols(sourceFileName, Cts, Progress);



                        /*
                        foreach (var symbol in list)
                        {
                            ContractList.GetOrFetch(symbol, "US");
                        }*/


                        using (var fs = new FileStream(destFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        using (StreamWriter file = new StreamWriter(fs))
                        {
                            foreach (var symbol in list)
                            {

                                file.WriteLine(symbol);
                            }
                        }
                    });
                    m.Start();
                }
            }
        }



        private void BtmImportQuandlBlob_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                Cts = new CancellationTokenSource();
                Task m = new Task(() => { Quandl.ImportEOD(Root.OpenFile.FileName, Progress, Cts); });
                m.Start();
            }
        }

        #endregion Quandl Tools

        #region Contract Settings

        private void BtnValidUSSymbol_Click(object sender, EventArgs e) => ValidateSymbol();

        private void BtnImportSymbols_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";
            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                Cts = new CancellationTokenSource();
                Task.Run(() =>
                {
                    string sourceFileName = Root.OpenFile.FileName;
                    using (var fs = new FileStream(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        List<string> symbollist = new List<string>();
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] symbols = line.Split(',');
                            //Console.WriteLine(line + ": " + symbols.Length);
                            symbollist.AddRange(symbols);
                        }

                        ContractManager.GetOrFetch(symbollist, TextBoxValidCountryCode.Text, Cts, Progress);
                    }
                }, Cts.Token);
            }
        }

        private void BtnImportNasdaq_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.txt) | *.txt";
            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                Cts = new CancellationTokenSource();
                Task.Run(() =>
                {
                    string sourceFileName = Root.OpenFile.FileName;
                    ContractManager.ImportNasdaq(sourceFileName, Cts, Progress);
                }, Cts.Token);
            }
        }

        private void BtnMatchSymbols_Click(object sender, EventArgs e)
        {
            Cts = new CancellationTokenSource();
            Task.Run(() =>
            {
                ContractManager.UpdateContractData("US", n => n.Status != ContractStatus.Incomplete && (DateTime.Now - n.UpdateTime).Days > 7, Cts, Progress);
            }, Cts.Token);
        }

        private void BtnUpdateContracts_Click(object sender, EventArgs e)
        {
            Cts = new CancellationTokenSource();
            Task.Run(() =>
            {
                //ContractList.UpdateContractData("US", n => (DateTime.Now - n.UpdateTime).Minutes > 180, Cts, Progress);
                //ContractList.UpdateContractData("US", n => n.Status != ContractStatus.Incomplete && n is IBusiness ib && string.IsNullOrWhiteSpace(ib.Industry), Cts, Progress);
                ContractManager.UpdateContractData("US", n => n.Status != ContractStatus.Incomplete && n is IBusiness ib && ib.Industry is null, Cts, Progress);
            }, Cts.Token);
        }

        private void BtnImportContracts_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                Cts = new CancellationTokenSource();
                Task.Run(() =>
                {
                    ContractManager.ImportCSV(Root.OpenFile.FileName, Cts, Progress);
                }, Cts.Token);
            }
        }

        private void BtnExportSymbols_Click(object sender, EventArgs e)
        {
            Root.SaveFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.SaveFile.ShowDialog() == DialogResult.OK)
            {
                Cts = new CancellationTokenSource();
                Task.Run(() =>
                {
                    ContractManager.ExportCSV(Root.SaveFile.FileName, Cts, Progress);
                }, Cts.Token);
            }
        }

        private void BtnFormatSymbolsList_Click(object sender, EventArgs e)
        {
            string symbolText = TextBoxMultiContracts.Text;
            var SymbolList = ContractManager.GetSymbolList(ref symbolText);
            TextBoxMultiContracts.Text = symbolText;
            Console.WriteLine("SymbolList.Count() = " + SymbolList.Count);
            var list = ContractManager.GetList(SymbolList.Where(n => n.Length > 0), "US");

            HashSet<string> existingSymbols = new HashSet<string>();
            var existingSymbolsArray = list.Select(n => n.Name);

            foreach (string symbol in existingSymbolsArray)
            {
                existingSymbols.CheckAdd(symbol);
            }

            var non_existing_symbols = SymbolList.Where(n => !existingSymbols.Contains(n));

            foreach (string s in non_existing_symbols)
            {
                Console.WriteLine("Can't find: " + s);
            }

            //var list = ContractList.Values.Where(n => SymbolList.Contains(n.Name));
            //var list = ContractList.Values.Where(n => n.Name == "AAPL");
            Console.WriteLine("ContractList.Values.Count() = " + ContractManager.Values.Count());
            Console.WriteLine("list.Count() = " + list.Count());

            /*
            string[] symbolString = TextBoxSymbols.Text.CsvReadFields();
            HashSet<string> symbols = new HashSet<string>();

            foreach(string s in symbolString) 
            {
                string st = s.TrimCsvValueField();

                if (!string.IsNullOrWhiteSpace(st)) 
                {
                    symbols.CheckAdd("\"" + st + "\"");
                }
           
            }

            string rectified = symbols.ToString(", ");

            TextBoxSymbols.Text = rectified;*/
        }

        #endregion Contract Settings

        #region Market Data

        private void BtnMarketDataSyncTicks_Click(object sender, EventArgs e)
        {
            foreach (var md in Pacmio.IB.Client.ActiveMarketData)
            {
                if(md is StockData sd)
                MarketDataGridView.Add(sd);
            }
        }

        private void BtnMarketDataAddContract_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected || !ValidateSymbol()) return;
            ContractTest.ActiveContract.MarketData.StartTicks(); //Request_MarketTicks(TextBoxGenericTickList.Text);

            if (ContractTest.ActiveContract is Stock s)
                MarketDataGridView.Add(s.StockData);

            Root.Form?.Show();
        }

        private void BtnSnapshotContract_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected || !ValidateSymbol()) return;
            ContractTest.ActiveContract.MarketData.SnapshotTicks(); //Request_MarketTicks(TextBoxGenericTickList.Text);

            if (ContractTest.ActiveContract is Stock s)
                MarketDataGridView.Add(s.StockData);

            Root.Form?.Show();
        }

        private void BtnMarketDataAddMultiContracts_Click(object sender, EventArgs e)
        {
            string tickList = TextBoxGenericTickList.Text; // "236,mdoff,292";

            string symbolText = TextBoxMultiContracts.Text;
            var symbols = ContractManager.GetSymbolList(ref symbolText);

            var cList = ContractManager.GetOrFetch(symbols, "US", Cts = new CancellationTokenSource(), null);
            //MarketDataGridView GridView = new MarketDataGridView("Market Data", new MarketDataTable());

            foreach (Contract c in cList)
            {
                while (Pacmio.IB.Client.SubscriptionOverflow) { Thread.Sleep(10); }
                Console.WriteLine("MarketQuote: " + c.MarketData.StartTicks()); //c.Request_MarketTicks(tickList));

                if (c is Stock s)
                    MarketDataGridView.Add(s.StockData);
            }

            Root.Form?.Show();
        }

        private void BtnMarketDataSnapshotMultiContracts_Click(object sender, EventArgs e)
        {
            string symbolText = TextBoxMultiContracts.Text;
            var symbols = ContractManager.GetSymbolList(ref symbolText);

            var cList = ContractManager.GetOrFetch(symbols, "US", Cts = new CancellationTokenSource(), null);
            //MarketDataGridView GridView = new MarketDataGridView("Market Data", new MarketDataTable());

            Task.Run(() => {

                foreach (Contract c in cList)
                {
                    Console.WriteLine("MarketQuote Snapshot: " + c);
                    //c.MarketData.SnapshotTicks();

                    Pacmio.IB.Client.DataRequest_MarketData(c.MarketData);


                    if (c is Stock s)
                        MarketDataGridView.Add(s.StockData);
                }
            });



            Root.Form?.Show();
        }



        private void BtnRequestMarketDepth_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected || !ValidateSymbol()) return;

            if (ContractTest.ActiveContract is Stock s)
            {
                MarketDepthManager.Add(s);
                Console.WriteLine("MarketDepth: " + s);
            }
        }

        #endregion Market Data

        #region Order

        private void BtnOrder_Click(object sender, EventArgs e)
        {
            if (Root.NetConnected && ValidateSymbol())
            {
                OrderType orderType = ComboxBoxOrderSettingType.Text.ParseEnum<OrderType>();
                OrderTimeInForce tif = ComboBoxOrderSettingTIF.Text.ParseEnum<OrderTimeInForce>();

                if (TestAccount is AccountInfo ac)
                    ContractTest.ActiveContract.PlaceOrder(ac, TextBoxOrderSettingQuantity.Text.ToInt32(0),
                    TradeType.Entry, tif, DateTime.Now.AddSeconds(30), true, orderType, true,
                    TextBoxOrderSettingLimitPrice.Text.ToDouble(0),
                    TextBoxOrderSettingStopPrice.Text.ToDouble(0)); // TODO: CheckBoxOrderWhatIf.Checked);
            }
        }

        private void BtnTestMassiveOrder_Click(object sender, EventArgs e)
        {
            string symbolText = TextBoxMultiContracts.Text;
            OrderType orderType = ComboxBoxOrderSettingType.Text.ParseEnum<OrderType>();
            OrderTimeInForce tif = ComboBoxOrderSettingTIF.Text.ParseEnum<OrderTimeInForce>();
            if (Cts is null || Cts.IsCancellationRequested) Cts = new CancellationTokenSource();

            if (TestAccount is AccountInfo ac)
                Task.Run(() =>
                {
                    var symbols = ContractManager.GetSymbolList(ref symbolText);
                    var cList = ContractManager.GetOrFetch(symbols, "US", Cts = new CancellationTokenSource(), null);

                    foreach (Contract c in cList.Where(n => n.Status == ContractStatus.Alive))
                    {
                        c.PlaceOrder(ac, TextBoxOrderSettingQuantity.Text.ToInt32(0),
                            TradeType.Entry, tif, DateTime.Now.AddSeconds(30), true, orderType, true,
                            TextBoxOrderSettingLimitPrice.Text.ToDouble(0),
                            TextBoxOrderSettingStopPrice.Text.ToDouble(0)); // TODO: CheckBoxOrderWhatIf.Checked);
                    }
                }, Cts.Token);
        }

        private void BtnOrderBraket_Click(object sender, EventArgs e)
        {
            if (Root.NetConnected && ValidateSymbol())
            {
                AccountInfo iba = OrderTest.LiveAccount;

            }
        }

        private void GridViewAllOrders_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            /*
            int selectedRowCount = GridViewAllOrders.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
                for (int i = 0; i < selectedRowCount; i++)
                {
                    DataGridViewRow row = GridViewAllOrders.SelectedRows[i];
                    int orderId = (int)row.Cells.GetCellValueFromColumnHeader("PermId");
                    TextBoxOrderId.Text = orderId.ToString(); //Console.WriteLine("Row Selected: " + (int)orderId);

                    OrderInfo od = OrderManager.Get(TextBoxOrderId.Text.ToInt32());


                    TextBoxOrderSettingQuantity.Text = od.Quantity.ToString();
                    TextBoxOrderSettingStopPrice.Text = od.AuxPrice.ToString();
                    TextBoxOrderSettingLimitPrice.Text = od.LimitPrice.ToString();
                }
            */
        }

        private void BtnModifyOrder_Click(object sender, EventArgs e)
        {
            /*
            OrderInfo od = OrderManager.Get(TextBoxOrderId.Text.ToInt32());
            if (od is OrderInfo)
            {
                od.Quantity = TextBoxOrderSettingQuantity.Text.ToInt32();

                if (od.Type == OrderType.Stop)
                {
                    od.AuxPrice = TextBoxOrderSettingStopPrice.Text.ToInt32();
                }

                if (od.Type == OrderType.Limit)
                {
                    od.AuxPrice = TextBoxOrderSettingLimitPrice.Text.ToInt32();
                }

                Client.PlaceOrder(od, false, true);
            }*/
        }

        private void BtnGetCompletedOrders_Click(object sender, EventArgs e)
        {
            OrderManager.Request_CompleteOrders(false);
        }






        #endregion Order

        private void BtnMarketDataFormShow_Click(object sender, EventArgs e)
        {
            Root.Form?.Show();
        }

        private void BtnMarketDataFormHide_Click(object sender, EventArgs e)
        {
            Root.Form?.Hide();
        }



        //public static Pacmio.TIProData.TopListScanner tls { get; private set; }

        private void BtnRequestTIProScanner_Click(object sender, EventArgs e)
        {
            /*
            tls = new Pacmio.TIProData.TopListScanner("Low Price Gappers")
            {
                Price = (1.5, 25),
                Volume = (50e3, double.NaN),
                GapPercent = (5, -5),
                AverageTrueRange = (0.25, double.NaN),
                ExtraConfig = "form=1&sort=MaxGUP&omh=1&col_ver=1&show0=D_Symbol&show1=Price&show2=Float&show3=SFloat&show4=GUP&show5=TV&show6=EarningD&show7=Vol5&show8=STP&show9=RV&show10=D_Name&show11=RD&show12=FCP&show13=D_Sector&show14=",
            };*/

            Pacmio.TIProData.TopListHandler tls = ScannerManager.AddTradeIdeasTopList();

            tls.SortColumn = "MaxGUP";

            //tls.Exchanges.Clear();
            //tls.ExtraConfig = tls.ExtraConfig.TrimEnd('&') + "&XX=on&X_CAV=on&X_CAT=on&X_SMAL=on&X_OTCQX=on&X_OTCQB=on&X_ARCA=on";

            tls.Start();
            /*
            if (tls.Snapshot(new DateTime(2020, 09, 02, 06, 30, 00)) is ICollection<Contract> list)
            {
                Pacmio.IB.WatchList.PrintWatchList(list);
            }*/
        }

        private void BtnRequestTIProAlert_Click(object sender, EventArgs e)
        {
            Pacmio.TIProData.AlertHandler tls = ScannerManager.AddTradeIdeasAlert();
            tls.Start();
        }

        private void BtnSmartComponents_Click(object sender, EventArgs e)
        {
            Pacmio.IB.Client.SendRequest_SmartComponents("a60001");
        }

        private void BtnRequestMarketDepthExch_Click(object sender, EventArgs e)
        {
            Pacmio.IB.Client.SendRequest_MktDepthExchanges();
        }

        private void BtnRequestNewProvider_Click(object sender, EventArgs e)
        {
            Pacmio.IB.Client.SendRequest_NewsProviders();
        }

        /*
        Parse_TickNews: (0)"84"-(1)"1"-(2)"1596196993000"-(3)"BRFUPDN"-(4)"BRFUPDN$0d4cf703"-(5)"JMP Securities reiterated Facebook (FB) coverage with Mkt Outperform and target $305"
        Parse_TickNews: (0)"84"-(1)"1"-(2)"1596210397000"-(3)"BRFG"-(4)"BRFG$0d4d7115"-(5)"Facebook's blowout results highlight the resiliency of its business model"-(6)"K:1.00"
        Parse_TickNews: (0)"84"-(1)"1"-(2)"1596197004000"-(3)"BRFUPDN"-(4)"BRFUPDN$0d4cce59"-(5)"Barclays reiterated Facebook (FB) coverage with Overweight and target $285"
        Parse_TickNews: (0)"84"-(1)"1"-(2)"1596543523000"-(3)"BRFUPDN"-(4)"BRFUPDN$0d51773a"-(5)"Argus reiterated Facebook (FB) coverage with Buy and target $300"
        Parse_TickNews: (0)"84"-(1)"1"-(2)"1598358178000"-(3)"BRFUPDN"-(4)"BRFUPDN$0d6f3944"-(5)"UBS reiterated Facebook (FB) coverage with Buy and target $330"-(6)"K:1.00"
         
         */
        private void BtnRequestNewsArticle_Click(object sender, EventArgs e)
        {
            Pacmio.IB.Client.SendRequest_NewsArticle("BRFG", "BRFG$0d4d7115");
        }


    }
    public static class DataGridHelper
    {
        public static object GetCellValueFromColumnHeader(this DataGridViewCellCollection CellCollection, string HeaderText)
        {
            return CellCollection.Cast<DataGridViewCell>().First(c => c.OwningColumn.HeaderText == HeaderText).Value;
        }
    }
}
