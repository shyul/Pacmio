using Pacmio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;
using Pacmio.Analysis;


namespace TestClient
{
    public partial class MainForm : Form
    {
        public BarFreq BarFreq => SelectHistoricalDataBarFreq.Text.ParseEnum<BarFreq>();

        public PriceType DataType => SelectHistoricalDataBarType.Text.ParseEnum<PriceType>();

        public AccountDataAdapter AccountDataAdapter { get; }
        public OrderInfoGridView OrderInfoGridView { get; } = new OrderInfoGridView();

        public PositionInfoGridView PositionInfoGridView { get; } = new PositionInfoGridView();

        public WatchListDataAdapter WatchListDataAdapter { get; }

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
            }
        }

        private Task PercentTask { get; }

        float Percent { get; set; } = 0;

        private void ProgressWorker()
        {
            float percent = 0;

            while (true)
            {
                if (Percent != percent)
                {
                    percent = Percent;
                    int pct = percent.ToInt32();

                    if (pct < 0) pct = 0;
                    else if (pct > 100) pct = 100;

                    if (MainProgBar.Value != pct)
                    {
                        this?.Invoke(() =>
                        {
                            MainProgBar.Value = pct;
                        });
                    }

                    Console.WriteLine("Progress Reported: " + percent.ToString("0.##") + "%");
                }

                Thread.Sleep(30);
            }
        }

        private PositionUpdate PositionUpdate { get; }

        public MainForm()
        {
            InitializeComponent();

            TextBoxIPAddress.Text = Root.Settings.IBServerAddress;

            AccountDataAdapter = new AccountDataAdapter(TreeViewAccount, ListBoxAccount);
            WatchListDataAdapter = new WatchListDataAdapter(CheckedListBoxWatchLists);

            ToggleConnect();

            Root.OnNetConnectHandler += NetClientOnConnectHandler;

            Root.Form.AddForm(DockStyle.Fill, 0, OrderInfoGridView);
            Root.Form.AddForm(DockStyle.Fill, 0, PositionInfoGridView);

            PercentTask = new Task(() => ProgressWorker());
            PercentTask.Start();
            Progress = new Progress<float>(percent =>
            {
                //Console.WriteLine("++++++++++++++++++ Progress Reported: " + percent.ToString("0.##") + "%");
                Percent = percent;
                /*
                int pct = percent.ToInt32();
                if (pct >= 0 && pct <= 100 && MainProgBarValue != pct)
                {


                    MainProgBar.Value = MainProgBarValue = pct;
                    Console.WriteLine("Progress Reported: " + MainProgBarValue.ToString("0.##") + "%");
                }*/
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
            SelectHistoricalDataBarType.Items.Add<PriceType>();

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

            TestFreqAlign tfa = new();
            TestMultiPeriodDataSource tmpds = new();

            PositionUpdate = new PositionUpdate(new Button[] { BtnHistoricalDataContractSet1, BtnHistoricalDataContractSet2, BtnHistoricalDataContractSet3,
                BtnHistoricalDataContractSet4, BtnHistoricalDataContractSet5, BtnHistoricalDataContractSet6, BtnHistoricalDataContractSet7, BtnHistoricalDataContractSet8,
                BtnHistoricalDataContractSet9, BtnHistoricalDataContractSet10 });

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

        private void BtnHistoricalDataContractSet9_Click(object sender, EventArgs e)
        {
            TextBoxSingleContractName.Text = BtnHistoricalDataContractSet9.Text;
            ValidateSymbol();
        }

        private void BtnHistoricalDataContractSet10_Click(object sender, EventArgs e)
        {
            TextBoxSingleContractName.Text = BtnHistoricalDataContractSet10.Text;
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

        private void BtnExportFundamental_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                Period pd = HistoricalPeriod;
                Root.SaveFile.FileName = "FD_" + ContractTest.ActiveContract.Name;
                if (Root.SaveFile.ShowDialog() == DialogResult.OK)
                {
                    var fd = ContractTest.ActiveContract.GetOrCreateFundamentalData();
                    fd.ExportCSV(Root.SaveFile.FileName);
                }
            }
        }

        private void BtnExportBarTableData_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                BarFreq freq = BarFreq;
                PriceType type = DataType;
                Root.SaveFile.FileName = "BT_" + ContractTest.ActiveContract.Name;
                if (Root.SaveFile.ShowDialog() == DialogResult.OK)
                {
                    BarDataFile btd = BarDataFile.LoadFile((ContractTest.ActiveContract.Key, freq, type));
                    btd.ExportCSV(Root.SaveFile.FileName);
                }
            }
        }

        public BarTableGroup BarTableGroup { get; } = new BarTableGroup();

        private void BtnLoadHistoricalChart_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                BarFreq freq = BarFreq;
                PriceType type = DataType;
                Period pd = HistoricalPeriod;
                Contract c = ContractTest.ActiveContract;

                if (pd.IsCurrent) c.MarketData.Start();

                Cts = new CancellationTokenSource();
                MultiPeriod mp = new MultiPeriod();
                mp.Add(pd);

                Task.Run(() =>
                {
                    /*BarTable bt = freq < BarFreq.Daily ?
                    c.LoadBarTable(pd, freq, type, false) :
                    BarTableManager.GetOrCreateDailyBarTable(c, freq);*/

                    //var bt = c.LoadBarTable(freq, type, pd, false, Cts);

                    BarTableSet bts = new BarTableSet(c, false);
                    bts.SetPeriod(mp, Cts);
                    var bt = bts[freq, type];
                    BarChart bc = bt.GetChart(Pacmio.Analysis.TestReversal.BarAnalysisList);
                    //BarChart bc = bt.GetChart(Pacmio.Analysis.TestTrend.BarAnalysisSet);

                    HistoricalPeriod = bt.Period;
                }, Cts.Token);

                Root.Form.Show();
            }
        }

        private void BtnTestIndicators_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                BarFreq freq = BarFreq;
                PriceType type = DataType;
                Period pd = HistoricalPeriod;
                Contract c = ContractTest.ActiveContract;

                if (pd.IsCurrent) c.MarketData.Start();

                Cts = new CancellationTokenSource();
                MultiPeriod mp = new MultiPeriod();
                mp.Add(pd);

                Task.Run(() =>
                {
                    /*BarTable bt = freq < BarFreq.Daily ?
                    c.LoadBarTable(pd, freq, type, false) :
                    BarTableManager.GetOrCreateDailyBarTable(c, freq);*/

                    //var bt = c.LoadBarTable(freq, type, pd, false, Cts);

                    BarTableSet bts = BarTableGroup[c];
                    bts.SetPeriod(mp, Cts);
                    var bt = bts[freq, type];

                    BarChart bc = bt.GetChart(Pacmio.Analysis.TestOscillators.BarAnalysisList);

                    HistoricalPeriod = bt.Period;
                }, Cts.Token);

                Root.Form.Show();
            }
        }

        private void BtnTestNativeAnalysis_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                BarFreq freq = BarFreq;
                PriceType type = DataType;
                Period pd = HistoricalPeriod;
                Contract c = ContractTest.ActiveContract;

                if (pd.IsCurrent) c.MarketData.Start();

                MultiPeriod mp = new MultiPeriod();
                mp.Add(pd);
                mp.Add(new Period(new DateTime(2020, 9, 1), new DateTime(2020, 9, 10)));

                Cts = new CancellationTokenSource();

                Task.Run(() =>
                {
                    /*BarTable bt = freq < BarFreq.Daily ?
                    c.LoadBarTable(pd, freq, type, false) :
                    BarTableManager.GetOrCreateDailyBarTable(c, freq);*/

                    //var bt = c.LoadBarTable(freq, type, pd, false, Cts);

                    BarTableSet bts = BarTableGroup[c];
                    bts.SetPeriod(mp, Cts);
                    var bt = bts[freq, type];

                    BarChart bc = bt.GetChart(TestNative.BarAnalysisList);

                    HistoricalPeriod = bt.Period;
                }, Cts.Token);

                Root.Form.Show();
            }
        }

        private void BtnTestTimeFrame_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                BarFreq freq = BarFreq;
                PriceType type = DataType;
                Period pd = HistoricalPeriod;
                Contract c = ContractTest.ActiveContract;

                if (pd.IsCurrent) c.MarketData.Start();

                Cts = new CancellationTokenSource();
                MultiPeriod mp = new MultiPeriod();
                mp.Add(pd);

                Task.Run(() =>
                {
                    BarTableSet bts = BarTableGroup[c];
                    bts.SetPeriod(mp, Cts);
                    BarTable bt = bts[freq, type];
                    BarChart bc = bt.GetChart(TestNative.BarAnalysisListTimeFrame);

                    HistoricalPeriod = bt.Period;
                }, Cts.Token);

                Root.Form.Show();
            }
        }

        private void BtnTestPatternAnalysis_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                BarFreq freq = BarFreq;
                PriceType type = DataType;
                Period pd = HistoricalPeriod;
                Contract c = ContractTest.ActiveContract;

                if (pd.IsCurrent) c.MarketData.Start();

                Cts = new CancellationTokenSource();
                MultiPeriod mp = new MultiPeriod();
                mp.Add(pd);

                Task.Run(() =>
                {
                    /*
                    BarTable bt = freq < BarFreq.Daily ?
                    c.LoadBarTable(pd, freq, type, false) :
                    BarTableManager.GetOrCreateDailyBarTable(c, freq);*/

                    //var bt = c.LoadBarTable(freq, type, pd, false, Cts);
                    BarTableSet bts = BarTableGroup[c];
                    bts.SetPeriod(mp, Cts);
                    BarTable bt = bts[freq, type];
                    BarChart bc = bt.GetChart(Pacmio.Analysis.TestTrend.BarAnalysisSet);

                    HistoricalPeriod = bt.Period;
                }, Cts.Token);

                Root.Form.Show();
            }
        }

        private void BtnTestSignal_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                BarFreq freq = BarFreq;
                PriceType type = DataType;
                Period pd = HistoricalPeriod;
                Contract c = ContractTest.ActiveContract;

                if (pd.IsCurrent) c.MarketData.Start();

                Cts = new CancellationTokenSource();

                Task.Run(() =>
                {
          
                    BarTableSet bts = BarTableGroup[c];
                    bts.SetPeriod(pd, Cts);

                    BarTable bt = bts[freq, type];
                    //BarChart bc = bt.GetCharts(iset);

                    HistoricalPeriod = bt.Period;
                }, Cts.Token);

                Root.Form.Show();
            }
        }

        private void BtnRunScreener_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                Contract c = ContractTest.ActiveContract;
                Period pd = HistoricalPeriod;
                var filter = new GapFilter();

                //  if (Cts is null || Cts.Cancelled())
                Cts = new CancellationTokenSource();

                Task.Run(() =>
                {
                    var cList = new Contract[] { c };
                    Console.WriteLine("total number = " + cList.Count());
                    cList.Screen(new FilterAnalysis[] { filter }, pd, 16, Cts, Progress);
                    GC.Collect();
                }, Cts.Token);
            }
        }

        private void BtnLoadMultiBarTable_Click(object sender, EventArgs e)
        {
            string symbolText = TextBoxMultiContracts.Text;
            Period pd = HistoricalPeriod;
            var filter = new GapFilter();
            var strategy = new GapGoOrbStrategy();

            //if (Cts is null || Cts.Token.Di || Cts.Cancelled())
            Cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                var symbols = StaticWatchList.GetSymbolListFromCsv(ref symbolText);
                var cList = ContractManager.GetOrFetch(symbols, "US", Cts, null);
                Console.WriteLine("total number = " + cList.Count());


                //cList.Screen(new FilterAnalysis[] { filter }, pd, 16, Cts, Progress);
                cList.Evaluate(new Strategy[] { strategy }, pd, 16, Cts, Progress);


                GC.Collect();
            }, Cts.Token);

        }

        private void BtnLoadAllBarTable_Click(object sender, EventArgs e)
        {
            Period pd = HistoricalPeriod;
            var filter = new GapFilter();

            //if (Cts is null || Cts.Cancelled()) 
            Cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                var cList = ContractManager.TradeableNoETFList.ToList();
                Console.WriteLine("total number = " + cList.Count());
                cList.Screen(new FilterAnalysis[] { filter }, pd, 16, Cts, Progress);
                GC.Collect();
            }, Cts.Token);
        }

        private void BtnAlignCharts_Click(object sender, EventArgs e) => BarChartManager.PointerToEndAll();

        private void BtnChartsUpdateAll_Click(object sender, EventArgs e)
        {
            BarFreq freq = BarFreq;
            PriceType type = DataType;
            if (Cts is null || Cts.IsCancellationRequested) Cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                //BarTableTest.BarTableSet.UpdatePeriod(freq, type, HistoricalPeriod, Cts, Progress);
                //BarTableTest.BarTableSet.Calculate(BarTableTest.TestBarAnalysisSet, Cts, Progress);
                BarChartManager.PointerToEndAll();
            }, Cts.Token);
        }

        private void BtnDownloadBarTable_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                BarFreq freq = BarFreq;
                PriceType type = DataType;
                if (Cts is null || Cts.IsCancellationRequested) Cts = new CancellationTokenSource();
                Period pd = new(new DateTime(1000, 1, 1), DateTime.Now);

                Task.Run(() =>
                {
                    //BarTableTest.BarTableSet.AddContract(ContractTest.ActiveContract, BarFreq.Daily, type, ref pd, Cts);
                    pd = HistoricalPeriod;
                    // BarTableTest.BarTableSet.AddContract(ContractTest.ActiveContract, freq, type, ref pd, Cts);
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
            PriceType type = DataType;
            if (Cts is null || Cts.IsCancellationRequested) Cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                var symbols = StaticWatchList.GetSymbolListFromCsv(ref symbolText);
                var cList = ContractManager.GetOrFetch(symbols, "US", Cts = new CancellationTokenSource(), null);
                //BarTable.Download(cList, new List<(BarFreq freq, BarType type, Period period)>() { (freq, type, HistoricalPeriod) }, Cts, Progress);
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
            WatchListTest.AddIBMostActive();
        }

        private void BtnCancelAllScanner_Click(object sender, EventArgs e)
        {
            WatchListManager.Stop();
        }

        private void BtnRequestScannerParameter_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected) return;
            Pacmio.IB.Client.SendRequest_ScannerParameters();
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
                c.MarketData.Start();// (tickList);
            }

        }

        private void BtnGlobalCancel_Click(object sender, EventArgs e)
        {
            OrderManager.CancelAllOrders();
        }

        private void BtnRequestExecData_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected) return;
            ExecutionManager.RequestExecutionData();
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
                ExecutionManager.ExportTradeLog(Root.SaveFile.FileName);
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
            Task.Run(() => { ContractManager.RemoveDuplicateUSStock("US", Cts); });
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
                Task m = new(() => { Quandl.MergeEODFiles(list, Root.SaveFile.FileName, Cts, Progress); });
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
                    Task m = new(() =>
                    {
                        var list = Quandl.ImportSymbols(sourceFileName, Cts, Progress);



                        /*
                        foreach (var symbol in list)
                        {
                            ContractList.GetOrFetch(symbol, "US");
                        }*/


                        using (var fs = new FileStream(destFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        using (StreamWriter file = new(fs))
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
                Task m = new(() => { Quandl.ImportEOD(Root.OpenFile.FileName, Progress, Cts); });
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
                    using (StreamReader sr = new(fs))
                    {
                        List<string> symbollist = new();
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
            StaticWatchList wt = new("Test Static WatchList", ref symbolText);
            TextBoxMultiContracts.Text = symbolText;

            WatchListManager.Add(wt);
        }

        #endregion Contract Settings

        #region Market Data

        private void BtnMarketDataSyncTicks_Click(object sender, EventArgs e)
        {
            foreach (var md in Pacmio.IB.Client.ActiveMarketData)
            {
                if (md is MarketData sd)
                {
                    StaticWatchList wt = WatchListManager.Add(new StaticWatchList("Draft"));
                    wt.Add(sd.Contract);
                    WatchListGridViewManager.Add(new WatchListGridView(wt));
                }

            }
        }

        private void BtnMarketDataAddContract_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected || !ValidateSymbol()) return;

            if (ContractTest.ActiveContract is Stock s)
            {
                s.MarketData.Start(); //Request_MarketTicks(TextBoxGenericTickList.Text);
                StaticWatchList wt = WatchListManager.Add(new StaticWatchList("Draft"));
                wt.Add(s);
                WatchListGridViewManager.Add(new WatchListGridView(wt));
            }


            Root.Form?.Show();
        }

        private void BtnSnapshotContract_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected || !ValidateSymbol()) return;
            ContractTest.ActiveContract.MarketData.Snapshot(); //Request_MarketTicks(TextBoxGenericTickList.Text);

            if (ContractTest.ActiveContract is Stock s)
            {
                StaticWatchList wt = WatchListManager.Add(new StaticWatchList("Draft"));
                wt.Add(s);
                WatchListGridViewManager.Add(new WatchListGridView(wt));
            }

            Root.Form?.Show();
        }

        private void BtnMarketDataAddMultiContracts_Click(object sender, EventArgs e)
        {
            //string tickList = TextBoxGenericTickList.Text; // "236,mdoff,292";
            string symbolText = TextBoxMultiContracts.Text;
            StaticWatchList wt = WatchListManager.Add(new StaticWatchList("Default", ref symbolText));
            TextBoxMultiContracts.Text = symbolText;


            WatchListGridViewManager.Add(new WatchListGridView(wt));

            Task.Run(() => {
                foreach (var s in wt.Contracts.Take(60))
                {
                    //while (Pacmio.IB.Client.SubscriptionOverflow) { Thread.Sleep(10); }
                    Console.WriteLine("MarketQuote: " + s.MarketData.Start()); //c.Request_MarketTicks(tickList));
                }
            });

            Root.Form?.Show();
        }

        private void BtnMarketDataSnapshotMultiContracts_Click(object sender, EventArgs e)
        {
            string symbolText = TextBoxMultiContracts.Text;
            StaticWatchList wt = new("Default", ref symbolText);
            TextBoxMultiContracts.Text = symbolText;

            WatchListManager.Add(wt);
            WatchListGridViewManager.Add(new WatchListGridView(wt));

            Task.Run(() => {
                foreach (Contract c in wt.Contracts.Take(60))
                {
                    Console.WriteLine("MarketQuote Snapshot: " + c);
                    //c.MarketData.SnapshotTicks();
                    Pacmio.IB.Client.DataRequest_MarketData(c.MarketData);
                }
            });

            Root.Form?.Show();
        }

        private void BtnRequestMarketDepth_Click(object sender, EventArgs e)
        {
            if (!Root.NetConnected || !ValidateSymbol()) return;

            if (ContractTest.ActiveContract is Stock s)
            {
                MarketDepthGridView gv = s.EnableMarketDepthGridView();
                gv.ReadyToShow = true;
                Root.Form.AddForm(DockStyle.Fill, 0, gv);
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

                if (OrderTest.Account is AccountInfo ac)
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

            if (OrderTest.Account is AccountInfo ac)
                Task.Run(() =>
                {
                    var symbols = StaticWatchList.GetSymbolListFromCsv(ref symbolText);
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
                AccountInfo iba = OrderTest.Account;

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

            Pacmio.TIProData.TopListHandler tls = WatchListTest.AddTradeIdeasTopList();

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
            Pacmio.TIProData.AlertHandler tls = WatchListTest.AddTradeIdeasAlert();
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

        private void BtnRefreshOrderInfoGrid_Click(object sender, EventArgs e)
        {
            foreach (OrderInfo od in OrderManager.GetList())
            {
                Console.WriteLine("OrderInfo PermId = " + od.PermId + " | " + od.Contract + " | " + od.Status);
            }

            //OrderInfoGridView.Update(OrderManager.List);
        }

        private void BtnRequestFundamentalData_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                Pacmio.IB.Client.Fetch_FundamentalData(ContractTest.ActiveContract, FinancialDataRequestType.CompanyOverview, new CancellationTokenSource());


            }
        }

        private void BtnTestFundamentalXMLFile_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "XML file (*.xml) | *.xml";

            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = Root.OpenFile.FileName;


                Reuters.CompanyOverview.ReportSnapshot data = Serialization.DeserializeXML<Reuters.CompanyOverview.ReportSnapshot>(File.ReadAllText(fileName));
                foreach (var officer in data.Officers.Officer)
                {
                    Console.WriteLine(officer.FirstName + " " + officer.LastName);
                }
                foreach (var text in data.TextInfo.Text)
                {
                    //DateTime time = DateTime.ParseExact(text.LastModified, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture);
                    Console.WriteLine(text.Type + " | " + text.LastModifiedTime + " | " + text.Value);
                }

                /*
                Reuters.FinancialSummary.FinancialSummary data = Serialization.DeserializeXML<Reuters.FinancialSummary.FinancialSummary>(File.ReadAllText(fileName));
                foreach (var eps in data.EPSs.EPS.Where(n =>  n.ReportType == "A" && n.Period == "3M"))
                { 
                    Console.WriteLine(eps.AsofDate + " | " + eps.Value);
                }*/
            }
        }

        private void BtnTimeZoneTest_Click(object sender, EventArgs e)
        {
            DateTime endTime = TimeZoneInfo.ConvertTimeFromUtc(TimeZoneInfo.ConvertTimeToUtc(DateTime.Now, ContractTest.ActiveContract.TimeZone), TimeZoneInfo.Local);
            Console.WriteLine(endTime);
        }

        private void BtnUpdateContract_Click(object sender, EventArgs e)
        {
            ValidateSymbol();
            ContractManager.Fetch(ContractTest.ActiveContract);
        }

        private void BtnTestFlag_Click(object sender, EventArgs e)
        {
            if (ValidateSymbol())
            {
                BarFreq freq = BarFreq;
                PriceType type = DataType;
                Period pd = HistoricalPeriod;
                Contract c = ContractTest.ActiveContract;

                if (pd.IsCurrent) c.MarketData.Start();

                Cts = new CancellationTokenSource();

                Task.Run(() =>
                {
                    /*
                    BarTable bt = freq < BarFreq.Daily ?
                    c.LoadBarTable(pd, freq, type, false) :
                    BarTableManager.GetOrCreateDailyBarTable(c, freq);*/

                    var bt = c.LoadBarTable(freq, type, pd, false, Cts);
                    BarChart bc = bt.GetChart(TestFlag.BarAnalysisSet);

                    HistoricalPeriod = bt.Period;
                }, Cts.Token);

                Root.Form.Show();
            }
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
