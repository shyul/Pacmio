using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Xu;
using Xu.Chart;
using Pacmio;
using Pacmio.IB;
using System.Net.Http.Headers;
using System.IO;

namespace TestClient
{
    public partial class MainForm : Form
    {
        public BarFreq BarFreq => SelectHistoricalDataBarFreq.Text.ParseEnum<BarFreq>();

        public BarType BarType => SelectHistoricalDataBarType.Text.ParseEnum<BarType>();

        public Period HistoricalPeriod => (CheckBoxChartToCurrent.Checked) ? new Period(DateTimePickerHistoricalDataStart.Value, true) :
                        new Period(DateTimePickerHistoricalDataStart.Value, DateTimePickerHistoricalDataStop.Value);

        public MainForm()
        {
            InitializeComponent();

            OrderTest.LiveAccount = AccountManager.GetOrAdd("DU332281");
            /*
            ContractTest.InitializeTable(GridViewContractSearchResult);
            OrderTest.InitializeTable(GridViewAllOrders);
            TradeTest.InitializeTable(GridViewTradeTable);

            */

            AccountManager.UpdatedHandler += AccountUpdatedHandler;
            //AccountManager.UpdatedHandler += PositionUpdatedHandler;

            TextBoxIPAddress.Text = Root.Settings.IBServerAddress;
            UpdateAccountList();
            ToggleConnect();

            Root.OnIBConnectedHandler += IBClientOnConnectedHandler;

            TradeLogManager.UpdatedHandler += TradeTableHandler;

            Progress = new Progress<int>(percent =>
            {
                MainProgBar.Value = percent;
            });

            Detailed_Progress = new Progress<float>(p => {
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

            DateTimePickerHistoricalDataStop.Value = DateTime.Now.AddHours(3);
            DateTimePickerHistoricalDataStart.Value = DateTimePickerHistoricalDataStop.Value.Date;//   .AddDays(-);

            TestFreqAlign tfa = new TestFreqAlign();
            TestMultiPeriodDataSource tmpds = new TestMultiPeriodDataSource();
            //tfa.Show();
            //tmpds.Show();

            //if (DateTime.Now.Hour > 17)
            //LoadValidSymbolHistoricalDataChart();
        }

        private void BtnMasterCancel_Click(object sender, EventArgs e)
        {
            if (!(Cts is null))
                Cts.Cancel();
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            Root.Settings.IBServerAddress = TextBoxIPAddress.Text;

            btnConnect.Enabled = false;
            if (Root.IBConnected)
            {
                Root.IBClientStop();
            }
            else
            {
                Root.IBClientStart();

            }
            Thread.Sleep(100);
            btnConnect.Enabled = true;
        }

        #region Symbols

        private void TbSymbolName_TextChanged(object sender, EventArgs e) => TextBoxSingleContractName.ForeColor = Color.Orange;
        private void BtnValidUSSymbol_Click(object sender, EventArgs e)
        {
            ValidateSymbol();
        }
        private void BtnGetContractInfo_Click(object sender, EventArgs e)
        {
            string symbol = TextBoxSingleContractName.Text.ToUpper();
            ContractList.GetOrFetch(symbol, "US");
        }

        #endregion Symbols



        private void BtnAccountSummary_Click(object sender, EventArgs e)
        {
            AccountManager.Request_AccountSummary();
        }


        private void BtnRequestPostion_Click(object sender, EventArgs e)
        {
            AccountManager.Request_Postion();
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




                BarTable bt = ContractTest.ActiveContract.GetTable(BarFreq, BarType);//, pd);
                bt.Reset(HistoricalPeriod, null, null);
                ChartList.GetForm(bt, ChartList.SampleChartConfig());
                Root.Form.Show();
            }
        }

        private void BtnLoadMultiHistoricalChart_Click(object sender, EventArgs e)
        {

        }

        private void BtnAlignCharts_Click(object sender, EventArgs e) => ChartList.ResetAllChartsPointer();

        private void BtnChartsUpdateAll_Click(object sender, EventArgs e) => ChartList.UpdateAllCharts(HistoricalPeriod, Cts = new CancellationTokenSource(), null);


        #endregion Bar Chart

        #region Simulation

        private void BtnTestScalping_Click(object sender, EventArgs e)
        {
            Root.Form.Show();
        }

        private void BtnRunAllSimulation_Click(object sender, EventArgs e)
        {

        }

        #endregion Simulation

        private void BtnRequestHistoricalTicks_Click(object sender, EventArgs e)
        {
            if (!Root.IBConnected || !ValidateSymbol()) return;



            //Client.Request_HistoricalTick(ContractTest.ActiveContract, pd);
            //Client.SendRequest_HistoricalTick(ContractTest.ActiveContract, DateTime.Now.AddHours(-6));
        }





        private void BtnImportSymbols_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                Task m = new Task(() => { ContractList.ImportCSV(Root.OpenFile.FileName, Progress); });
                m.Start();
            }
        }

        private void BtnExportSymbols_Click(object sender, EventArgs e)
        {
            Root.SaveFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.SaveFile.ShowDialog() == DialogResult.OK)
            {
                Task m = new Task(() => { ContractList.ExportCSV(Root.SaveFile.FileName, Progress); });
                m.Start();
            }
        }








        private void BtnSearchSymbol_Click(object sender, EventArgs e)
        {
            ContractTest.UpdateSymbolInfoTable(TextBoxSearchSymbol.Text);
        }







        private void BtnRequestScanner_Click(object sender, EventArgs e)
        {
            if (!Root.IBConnected) return;
            ScannerInfo info_MOST_ACTIVE = new ScannerInfo()
            {
                //Code = "MOST_ACTIVE",
                Code = "MOST_ACTIVE",
                //FilterOptions = "openGapPercAbove=1;priceAbove=5;priceBelow=50;avgVolumeAbove=10000;marketCapAbove1e6=100;marketCapBelow1e6=100000;stkTypes=inc:CORP;"
                //FilterOptions = "priceAbove=5;priceBelow=300;avgVolumeAbove=10000000;marketCapAbove1e6=10000;stkTypes=inc:CORP;"
                FilterOptions = "priceAbove=10;priceBelow=100;avgVolumeAbove=10000000;marketCapAbove1e6=5000;marketCapBelow1e6=20000;stkTypes=inc:CORP;"
            };
            /*
            ScannerInfo info_TOP_OPEN_PERC_GAIN = new ScannerInfo()
            {
                //Code = "MOST_ACTIVE",
                Code = "TOP_OPEN_PERC_GAIN",
                FilterOptions = "openGapPercAbove=1;priceAbove=5;priceBelow=50;avgVolumeAbove=10000;marketCapAbove1e6=100;marketCapBelow1e6=100000;stkTypes=inc:CORP;"
            };
            ScannerInfo info_TOP_OPEN_PERC_LOSE = new ScannerInfo()
            {
                //Code = "MOST_ACTIVE",
                Code = "TOP_OPEN_PERC_LOSE",
                FilterOptions = "openGapPercBelow=-1;priceAbove=5;priceBelow=50;avgVolumeAbove=10000;marketCapAbove1e6=100;marketCapBelow1e6=100000;stkTypes=inc:CORP;"
            };*/

            ScannerManager.GetOrAdd(info_MOST_ACTIVE);
            //ScannerList.GetOrAdd(info_TOP_OPEN_PERC_GAIN);
            //ScannerList.GetOrAdd(info_TOP_OPEN_PERC_LOSE);
        }

        private void BtnCancelAllScanner_Click(object sender, EventArgs e)
        {
            ScannerManager.CancelAll();
        }

        private void BtnRequestScannerParameter_Click(object sender, EventArgs e)
        {
            if (!Root.IBConnected) return;
            ScannerManager.Request_ScannerParameters();
        }



        private void BtnTestMassiveSamples_Click(object sender, EventArgs e)
        {
            string[] symbols = new string[] { "XLNX", "FB" ,"AAPL", "LULU", "GOOGL", "NFLX", "NATI", "TSLA",
                                            "EDU", "QQQ", "NIO", "KEYS", "A","DTSS","SINT", "HYG","SPY","NEAR",
                                            "TQQQ","BA","B","T", "ADI", "TXN", "INTC","NVDA","D","QBIO","JPM",
                                            "WFC","W", "GILD","ABBV","MSFT","AMGN","UPRO","ALXN", "IBM" };

            foreach (Contract c in ContractList.GetOrFetch(symbols, "US", Cts = new CancellationTokenSource(), null))
            {
                Console.WriteLine("Request Realtime Bars: " + c.ToString());
                c.Request_RealTimeBars();
            }
        }

        private void BtnTestRealTimeBars_Click(object sender, EventArgs e)
        {
            if (!ValidateSymbol()) return;
            ContractTest.ActiveContract.Request_RealTimeBars();
        }

        private void BtnRequestPnL_Click(object sender, EventArgs e)
        {

        }

        private void BtnSubscribePnL_Click(object sender, EventArgs e)
        {

        }





        private void TestMassOrder_Click(object sender, EventArgs e)
        {
            if (!Root.IBConnected) return;
            AccountManager.Request_AccountSummary();

            string[] symbols = new string[] { "XLNX", "TQQQ", "ET", "LULU", "BAC", "JPM" };
            var list = ContractList.GetOrFetch(symbols, "US", null, null);
            string tickList = TextBoxGenericTickList.Text;
            foreach (Contract c in list)
            {
                c.Request_MarketTicks(tickList);
            }

        }

        private void BtnGlobalCancel_Click(object sender, EventArgs e)
        {
            OrderManager.CancelAllOrders();
        }

        private void BtnRequestExecData_Click(object sender, EventArgs e)
        {
            if (!Root.IBConnected) return;
            TradeLogManager.Request_Log();
        }

        private void BtnCloseAllPosition_Click(object sender, EventArgs e)
        {
            if (!Root.IBConnected) return;
            OrderManager.CloseAllPositions();
        }

        private void BtnArmLiveTrade_Click(object sender, EventArgs e)
        {

        }

        private void BtnExportExecTradeLog_Click(object sender, EventArgs e)
        {
            Root.SaveFile.Filter = "Transaction Log file (*.tlg) | *.tlg";

            if (Root.SaveFile.ShowDialog() == DialogResult.OK)
            {
                TradeLogManager.ExportTradeLog(Root.SaveFile.FileName);
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





        private void BtnMatchSymbols_Click(object sender, EventArgs e)
        {
            Cts = new CancellationTokenSource();
            Task m = new Task(() => { ContractList.UpdateContractData(Cts, Progress); });
            m.Start();
        }



        private void BtnDownloadTables_Click(object sender, EventArgs e)
        {
            DownloadBarTable.DownloadCancellationTokenSource = Cts = new CancellationTokenSource();
            DownloadBarTable.DownloadProgress = Progress;
            DownloadBarTable.DetailedProgress = Detailed_Progress;

            DownloadBarTable.Period = (CheckBoxChartToCurrent.Checked) ? new Period(DateTimePickerHistoricalDataStart.Value, true) :
                new Period(DateTimePickerHistoricalDataStart.Value, DateTimePickerHistoricalDataStop.Value);

            string symbolText = TextBoxMultiContracts.Text;
            DownloadBarTable.SymbolList.AddRange(ContractTools.GetSymbolList(ref symbolText));
            TextBoxMultiContracts.Text = symbolText;

            DownloadBarTable.BarFreqs.Add(BarFreq.Daily);
            DownloadBarTable.BarFreqs.Add(SelectHistoricalDataBarFreq.Text.ParseEnum<BarFreq>());
            //DownloadBarTable.BarFreqs.Add(BarFreq.HalfMinute);

            Task.Run(() => { DownloadBarTable.Worker(); });
        }

        public static Progress<float> Detailed_Progress;

        private void BtnCleanUpDuplicateStock_Click(object sender, EventArgs e)
        {
            var result = ContractTools.FindDuplicateStock("US");

            var toDelete = result.Where(n => n.Status != ContractStatus.Alive).ToList();

            foreach (Stock s in toDelete)
            {
                ContractList.Remove(s.Info);
                Console.WriteLine("Removing: " + s.Status + " | " + s.ToString());
            }

            foreach (Stock s in result)
            {
                Console.WriteLine(s.Status + " | " + s.ToString());
            }
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
            var list = MergeEODFiles.OrderByDescending(n => n.Value).Select(n => n.Key);

            Cts = new CancellationTokenSource();
            Task m = new Task(() => { Quandl.MergeEODFiles(list, "D:\\EOD_Merged.csv", Progress, Cts); });
            m.Start();
        }

        private void BtnExtractSymbols_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = Root.OpenFile.FileName;
                Cts = new CancellationTokenSource();
                Task m = new Task(() =>
                {
                    var list = Quandl.ImportSymbols(fileName, Progress, Cts);
                    /*
                    foreach (var symbol in list)
                    {
                        ContractList.GetOrFetch(symbol, "US");
                    }*/

                    /*
                    using (var fs = new FileStream("D:\\EOD_symbols.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter file = new StreamWriter(fs))
                    {
                        foreach (var symbol in list)
                        {
 
                            file.WriteLine(symbol);
                        }
                    }*/
                });
                m.Start();
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

        private void BtnTestSymbolsToCheck_Click(object sender, EventArgs e)
        {
            string symbolText = TextBoxMultiContracts.Text;
            var SymbolList = ContractTools.GetSymbolList(ref symbolText);
            TextBoxMultiContracts.Text = symbolText;
            Console.WriteLine("SymbolList.Count() = " + SymbolList.Count);
            var list = ContractList.GetList(SymbolList.Where(n => n.Length > 0), "US");

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
            Console.WriteLine("ContractList.Values.Count() = " + ContractList.Values.Count());
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

        private void BtnMarketDataFormShow_Click(object sender, EventArgs e)
        {
            Root.Form?.Show();
        }

        private void BtnMarketDataFormHide_Click(object sender, EventArgs e)
        {
            Root.Form?.Hide();
        }

        private void BtnMarketDataAddContract_Click(object sender, EventArgs e)
        {
            if (!Root.IBConnected || !ValidateSymbol()) return;
            ContractTest.ActiveContract.Request_MarketTicks(TextBoxGenericTickList.Text);
        }

        private void BtnMarketDataAddMultiContracts_Click(object sender, EventArgs e)
        {
            string tickList = TextBoxGenericTickList.Text; // "236,mdoff,292";
            /*
            string[] symbols = new string[] { "XLNX", "FB" ,"AAPL", "LULU", "GOOGL", "NFLX", "NATI", "TSLA",
                                            "EDU", "QQQ", "NIO", "KEYS", "A","DTSS","SINT", "HYG","SPY","NEAR",
                                            "TQQQ","BA","B","T", "ADI", "TXN", "INTC","NVDA","D","QBIO","JPM",
                                            "WFC","W", "GILD","ABBV","MSFT","AMGN","UPRO","ALXN" };*/


            string symbolText = TextBoxMultiContracts.Text;
            var symbols = ContractTools.GetSymbolList(ref symbolText);
            /*
            string[] symbols = new string[] { "CCL", "DAL", "UAL", "HAL", "PINS", "RCL", "MGM", "CARR", "PCG", "VIAC", "CTL", "LYFT", "KEY",
            "RF", "SYF", "MRVL", "WORK", "COG", "IMMU", "TLRY", "OSTK", "IO", "CHEF", "PLAY", "VVUS" };*/

            var cList = ContractList.GetOrFetch(symbols, "US", Cts = new CancellationTokenSource(), null);
            MarketDataGridView GridView = new MarketDataGridView("Market Data", new MarketDataTable());

            foreach (Contract c in cList)
            {
                Console.WriteLine("MarketQuote: " + c.Request_MarketTicks(tickList));

                GridView.MarketDataTable.Add(c.MarketData);


            }
            MarketDataManager.Add(GridView);
            Root.Form?.Show();
        }

        private void BtnRequestMarketDepth_Click(object sender, EventArgs e)
        {
            if (!Root.IBConnected || !ValidateSymbol()) return;

            if (ContractTest.ActiveContract is IMarketDepth imd)
            {

                Console.WriteLine("MarketDepth: " + imd.Request_MarketDepth());
            }
        }

        #endregion Market Data

        #region Order

        private void BtnOrder_Click(object sender, EventArgs e)
        {
            if (Root.IBConnected && ValidateSymbol())
            {
                OrderType orderType = ComboxBoxOrderSettingType.Text.ParseEnum<OrderType>();
                OrderTimeInForce tif = ComboBoxOrderSettingTIF.Text.ParseEnum<OrderTimeInForce>();

                OrderInfo od = new OrderInfo()
                {
                    Contract = ContractTest.ActiveContract,
                    Quantity = TextBoxOrderSettingQuantity.Text.ToInt32(0),
                    Type = orderType,
                    LimitPrice = TextBoxOrderSettingLimitPrice.Text.ToDouble(0),
                    AuxPrice = TextBoxOrderSettingStopPrice.Text.ToDouble(0),
                    TimeInForce = tif,
                    AccountCode = "DU332281",
                    OutsideRegularTradeHours = true,
                };

                if (tif == OrderTimeInForce.GoodUntilDate || tif == OrderTimeInForce.GoodAfterDate)
                {
                    od.EffectiveDateTime = DateTime.Now.AddSeconds(30);
                    DateTimePickerOrderSettingGTD.Value = od.EffectiveDateTime;
                }

                OrderManager.PlaceOrder(od); // TODO: CheckBoxOrderWhatIf.Checked);
            }
        }

        private void BtnOrderBraket_Click(object sender, EventArgs e)
        {
            if (Root.IBConnected && ValidateSymbol())
            {
                Account iba = OrderTest.LiveAccount;

                iba.EntryBraket(
                    ContractTest.ActiveContract,
                    TextBoxOrderSettingQuantity.Text.ToInt32(0),
                    TextBoxOrderSettingStopPrice.Text.ToDouble(0),
                    TextBoxOrderSettingLimitPrice.Text.ToDouble(0)
                );
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


    }
    public static class DataGridHelper
    {
        public static object GetCellValueFromColumnHeader(this DataGridViewCellCollection CellCollection, string HeaderText)
        {
            return CellCollection.Cast<DataGridViewCell>().First(c => c.OwningColumn.HeaderText == HeaderText).Value;
        }
    }
}
