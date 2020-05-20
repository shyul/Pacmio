/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - shyu.lee@gmail.com
/// 
/// Interactive Brokers API Client - Customized and Optimized
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;
using Xu.WindowsNativeMethods;
using IBClient_Old;

namespace Pacmio.Utility
{
    public partial class DataUtility : Form
    {
        #region Initialize

        public static Color ActiveColor => DWMAPI.GetWindowColorizationColor(true);

        public float ScaleFactor => GUI.ScalingFactor();

        public int ShowFormMsg => Program.SHOW_PACMIO;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == ShowFormMsg)
            {
                if (WindowState == FormWindowState.Minimized)
                    WindowState = FormWindowState.Normal;

                Activate();
            }

            base.WndProc(ref m);
        }

        public DataUtility()
        {
            //AllocConsole();
            InitializeComponent();
            InitServices();
            /*
            LvSymbols.BeginUpdate();
            LvSymbols.Columns.Clear();
            LvSymbols.Columns.Add("Name");
            LvSymbols.Columns[0].Width = 200;

            LvSymbols.Groups.Clear();
            LvSymbols.Groups.Add(new ListViewGroup("Symbols", "Symbols"));

            var items = Symbols.Values.Select(n => new ListViewItem { Text = n.ToString(), Group = LvSymbols.Groups[0] }).ToArray();

            LvSymbols.Items.Clear();

            for(int i = 1000; i < 1050; i++)
            {
                LvSymbols.Items.Add(items[i]);
            }

            LvSymbols.EndUpdate();*/
        }

        public static Progress<int> progress;
        public static CancellationTokenSource cts = new CancellationTokenSource();

        public void InitServices()
        {
            progress = new Progress<int>(percent =>
            {
                StatusProgressBar1.Value = percent;
            });

            ComboBoxExchange.Items.AddRange(typeof(Exchange).GetEnumNames());
            ComboBoxExchange.SelectedIndex = ComboBoxExchange.FindStringExact(Exchange.NASDAQ.ToString());

            ComboBoxSecType.Items.AddRange(typeof(SecurityType).GetEnumNames());
            ComboBoxSecType.SelectedIndex = ComboBoxSecType.FindStringExact(SecurityType.STOCK.ToString());

            ComboBoxBarFreq.Items.AddRange(typeof(BarFreq).GetEnumNames());
            ComboBoxBarFreq.SelectedIndex = ComboBoxBarFreq.FindStringExact(BarFreq.Daily.ToString());

            ComboBoxBarType.Items.AddRange(typeof(TickerType).GetEnumNames());
            ComboBoxBarType.SelectedIndex = ComboBoxBarType.FindStringExact(TickerType.Trades.ToString());

            BarTableDataSourceGridView.ColumnCount = 3;
            BarTableDataSourceGridView.Columns[0].Name = "Start time";
            BarTableDataSourceGridView.Columns[1].Name = "End time";
            BarTableDataSourceGridView.Columns[2].Name = "Data source";
            BarTableDataSourceGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            ComboBoxFundamentalDataType.Items.AddRange(typeof(FundamentalDataType).GetEnumNames());
            ComboBoxFundamentalDataType.SelectedIndex = ComboBoxFundamentalDataType.FindStringExact(FundamentalDataType.Dividend.ToString());
            CkBoxFundamentalExportAll_CheckedChanged(this, null);

            DateTimePickerRequestIBHistDataStart.Value = DateTime.Now.AddDays(-5);
            DateTimePickerRequestIBHistDataEnd.Value = DateTime.Now;

            DateTimePickerQuandlEODStart.Value = DateTime.Now.AddDays(-20);
            DateTimePickerQuandlEODEnd.Value = DateTime.Now;

            ListViewQuandlEODFiles.BeginUpdate();
            ListViewQuandlEODFiles.View = View.List;
            ListViewQuandlEODFiles.Columns.Clear();
            ListViewQuandlEODFiles.Columns.Add("File Name");
            ListViewQuandlEODFiles.Columns[0].Width = 674;
            ListViewQuandlEODFiles.Groups.Clear();
            ListViewQuandlEODFiles.Groups.Add(new ListViewGroup("EOD Files", "EOD Files"));
            ListViewQuandlEODFiles.Items.Clear();
            ListViewQuandlEODFiles.EndUpdate();

            TextBoxHostAddress.Text = Root.Settings.IBServerAddress;
            TextBoxHostPort.Text = Root.Settings.IBServerPort.ToString();
            TextBoxClientId.Text = Root.Settings.IBClientId.ToString();

            IBClient.OnConnected += Connection;
        }

        private void Connection(ConnectionStatus status, DateTime time, string msg)
        {
            switch (status)
            {
                case (ConnectionStatus.Disconnected):
                    Invoke(new Action(() =>
                    {
                        BtnConnect.Enabled = true;
                        BtnConnect.Text = "Connect";
                        BtnConnect.BackColor = Color.Green;
                        BtnConnect.Click -= BtnDisconnect_Click;
                        BtnConnect.Click += BtnConnect_Click;
                        TextBoxHostAddress.Enabled = true;
                        TextBoxHostPort.Enabled = true;
                        TextBoxClientId.Enabled = true;
                    }));
                    break;

                case (ConnectionStatus.Connecting):
                    Invoke(new Action(() =>
                    {
                        BtnConnect.Enabled = false;
                        BtnConnect.Text = "Connecting";
                        TextBoxHostAddress.Enabled = false;
                        TextBoxHostPort.Enabled = false;
                        TextBoxClientId.Enabled = false;
                    }));
                    break;

                case (ConnectionStatus.Connected):
                    Invoke(new Action(() =>
                    {
                        BtnConnect.Enabled = true;
                        BtnConnect.Text = "Disconnect";
                        BtnConnect.BackColor = Color.Red;
                        BtnConnect.Click -= BtnConnect_Click;
                        BtnConnect.Click += BtnDisconnect_Click;
                    }));
                    break;

                case (ConnectionStatus.Disconnecting):
                    Invoke(new Action(() =>
                    {
                        BtnConnect.Enabled = false;
                        BtnConnect.Text = "Disconnecting";
                        TextBoxHostAddress.Enabled = false;
                        TextBoxHostPort.Enabled = false;
                        TextBoxClientId.Enabled = false;
                    }));
                    break;
            }
        }

        private void TextBoxHostAddress_TextChanged(object sender, EventArgs e)
        {
            Root.Settings.IBServerAddress = TextBoxHostAddress.Text;
            TextBoxHostAddress.Text = Root.Settings.IBServerAddress;
        }

        private void TextBoxHostPort_TextChanged(object sender, EventArgs e)
        {
            Root.Settings.IBServerPort = TextBoxHostPort.Text.ToInt32(15060);
            TextBoxHostPort.Text = Root.Settings.IBServerPort.ToString();
        }

        private void TextBoxClientId_TextChanged(object sender, EventArgs e)
        {
            Root.Settings.IBClientId = TextBoxClientId.Text.ToInt32(180);
            TextBoxClientId.Text = Root.Settings.IBClientId.ToString();
        }

        //public static void IBClientStart() => IBClient.Connect(Settings.IBServerAddress, Settings.IBServerPort, Settings.IBClientId, Settings.IBTimeout);

        //public static void IBClientStop() => IBClient.Disconnect();

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            BtnConnect.Enabled = false;
            //Root.IBClientStart();
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            BtnConnect.Enabled = false;
            //Root.IBClientStop();
        }

        private void DataUtility_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Root.IBClientStop();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            cts.Cancel();
        }

        #endregion Initialize

        #region Symbol

        #endregion Symbol

        #region Account

        private void BtnAccountSum_Click(object sender, EventArgs e)
        {
            IBClient.SendReq_AccountSummary();
        }

        private void BtnAccountUpdate_Click(object sender, EventArgs e)
        {
            //IBClient.RequestAccountUpdate();
            IBClient.SendReq_AccountData("DU162256", true);
        }

        private void BtnPostions_Click(object sender, EventArgs e)
        {
            IBClient.SendReq_Positions();
        }

        #endregion Account

        #region Symbol

        private static string SymbolCSV = Root.ResourcePath + "Symbols.csv";

        private List<SymbolInfo> ValidSymbol()
        {
            string TickName = TextBoxSymbolTick.Text.Trim().ToUpper();
            TextBoxSymbolTick.Text = TickName;

            //LbSymbolFullName.Text = "Name: ";
            //LbSymbolISIN.Text = "ISIN: ";

            (bool IsSymbolValid, SymbolInfo Symbol) = SymbolsList.Get(TickName, ComboBoxExchange.Text.ParseEnum<Exchange>(), ComboBoxSecType.Text.ParseEnum<SecurityType>());

            if (IsSymbolValid)
            {
                ShowSymbolInfo(Symbol);
                return new List<SymbolInfo>() { Symbol };
            }
            else
            {
                List<SymbolInfo> SymbolList = SymbolsList.GetList(TickName);
                if (SymbolList.Count > 0)
                {
                    ComboBoxExchange.SelectedIndex = ComboBoxExchange.FindStringExact(SymbolList[0].Exchange.ToString());
                    ComboBoxSecType.SelectedIndex = ComboBoxSecType.FindStringExact(SymbolList[0].Type.ToString());

                    ShowSymbolInfo(SymbolList[0]);
                }
                return SymbolList;
            }
        }

        private void ShowSymbolInfo(SymbolInfo si)
        {
            TextBoxSymbolFullName.Text = si.FullName;
            TextBoxSymbolISIN.Text = si.ISIN;

            (bool valid, BusinessInfo bi) = si.GetBusinessInfo();

            if (valid)
            {
                TextBoxSymbolSummaryB.Text = bi.BusinessSummary;
                TextBoxSymbolSummaryF.Text = bi.FinancialSummary;

                string IdString = "CUSIP:" + bi.CUSIP + ", ";

                foreach (string idname in bi.IDs.Keys)
                {
                    IdString += idname + ":" + bi.IDs[idname] + ", ";
                }

                TextBoxSymbolIds.Text = IdString;
            }
        }

        private void BtnValidateSymbol_Click(object sender, EventArgs e)
        {
            ValidSymbol();
        }

        private void BtnMatchSymbol_Click(object sender, EventArgs e)
        {
            IBClient.Request_MatchSymbol(BoxContract.Text, ExchangeInfo.US_Exchanges, true);
        }

        private void BtnSymbolInfo_Click(object sender, EventArgs e)
        {
            var list = ValidSymbol();

            if (list.Count > 0)
            {
                IBClient.Request_SymbolInfo(list[0]);
            }
        }

        private void BtnMatchSymbols_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";
            cts = new CancellationTokenSource();

            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                Task m = new Task(() => {

                    /*
                    string sampleFileName = @"E:\Symbols_EOD.csv";

                    List<string> sampleList = new List<string>();

                    string line;
                    string[] values;

                    using (StreamReader strSampleSymbol = new StreamReader(sampleFileName))
                    {
                        while (!strSampleSymbol.EndOfStream)
                        {
                            line = strSampleSymbol.ReadLine();
                            values = line.Trim().Split(',');

                            if (values.Length > 0 && !UnknownItems.Contains(values[0]))
                            {
                                sampleList.Add(values[0]);
                                //Console.WriteLine("Invalid added: " + values[0]);
                            }
                        }
                    }

                    string[] samples = sampleList.ToArray();  */
                    /*= new string[] { "000", "600", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                    "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "XI" };*/

                    string all = File.ReadAllText(Root.OpenFile.FileName);

                    var samples = all.Split(',');
                    IBClient.Request_MatchSymbols(samples, ExchangeInfo.US_Exchanges, progress, cts);
                });
                m.Start();
            }
        }

        private void BtnMatchSymbolsEOD_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";
            cts = new CancellationTokenSource();
            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                Task m = new Task(() => {
                    var samples = Quandl.ImportSymbols(Root.OpenFile.FileName, progress, cts);
                    IBClient.Request_MatchSymbols(samples, ExchangeInfo.US_Exchanges, progress, cts);
                });
                m.Start();
            }
        }

        private void BtnRequestAllSymbolInfo_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();
            Task m = new Task(() => {
                IBClient.Request_SymbolInfo(progress, cts, false);
            });
            m.Start();
        }

        private void BtnRescanUnknownSymbols_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();
            Task m = new Task(() => {
                UnknownItems.Rescan(progress);
            });
            m.Start();
        }

        private void BtnExportSymbols_Click(object sender, EventArgs e)
        {
            Root.SaveFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.SaveFile.ShowDialog() == DialogResult.OK)
            {
                Task m = new Task(() => { SymbolsList.ExportCSV(Root.SaveFile.FileName, progress); });
                m.Start();
            }
        }

        private void BtnImportSymbols_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                Task m = new Task(() => { SymbolsList.ImportCSV(Root.OpenFile.FileName, progress); });
                m.Start();
            }
        }

        private void BtnDownloadAllList_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                cts = new CancellationTokenSource();
                Task m = new Task(() => { Nasdaq.ApplyFullNameList(Root.OpenFile.FileName, progress, cts); });
                m.Start();
            }
        }

        private void BtnApplyETFList_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                cts = new CancellationTokenSource();
                Task m = new Task(() => { Nasdaq.ApplyETFList(Root.OpenFile.FileName, progress, cts); });
                m.Start();
            }
        }

        #endregion Symbol

        #region Order

        private void BtnOrder_Click(object sender, EventArgs e)
        {
            IBClient.PlaceStockOrder(new SymbolInfo("AMZN", Exchange.NASDAQ, SecurityType.STOCK),
                "DU162256", TradeSide.Long, OrderType.Limit, 5, 1282.54012, 0, true, true, "GTC",
                DateTime.Now.AddDays(10), DateTime.Now);
        }

        #endregion Order

        #region Market Data

        private void BtnMarketDepth_Click(object sender, EventArgs e)
        {
            var list = ValidSymbol();

            if (list.Count > 0)
                IBClient.SendReq_MarketDepth(list[0]);
        }

        private void BtnMarketData_Click(object sender, EventArgs e)
        {
            var list = ValidSymbol();

            if (list.Count > 0)
                IBClient.SendReq_MarketData(list[0]);
        }

        #endregion Market Data

        #region Historical Data

        private void BtnRefreshBarTableInfoUIs_Click(object sender, EventArgs e)
        {
            var list = ValidSymbol();

            if (list.Count > 0)
            {
                BarTable bt = BarTableList.Initialize(list[0], ComboBoxBarFreq.Text.ParseEnum<BarFreq>(), ComboBoxBarType.Text.ParseEnum<TickerType>());

                BarTableDataSourceGridView.Rows.Clear();
                foreach (var item in bt.DataSourceSegments)
                {
                    string[] row = new string[] { item.Key.Start.ToString(), item.Key.Stop.ToString(), item.Value.ToString() };
                    BarTableDataSourceGridView.Rows.Add(row);
                }
            }
        }

        private void BtnBtnRequestQuandlEOD_Click(object sender, EventArgs e)
        {
            BtnBtnRequestQuandlEOD.Enabled = false;

            var list = ValidSymbol();

            if (list.Count > 0)
            {
                BarTable bt = list[0].Initialize(ComboBoxBarFreq.Text.ParseEnum<BarFreq>(), ComboBoxBarType.Text.ParseEnum<TickerType>());
                Period pd = new Period(DateTimePickerQuandlEODStart.Value, DateTimePickerQuandlEODEnd.Value);

                Log.Print(pd.ToString());

                Quandl.Request(bt, pd, CkBoxQuandlEODGetAll.Checked);
            }

            BtnBtnRequestQuandlEOD.Enabled = true;
        }


        private void BtnRequestIBHistData_Click(object sender, EventArgs e)
        {
            BtnRequestIBHistData.Enabled = false;

            //if (IBClient.Connected)
            //{
            var list = ValidSymbol();

            if (list.Count > 0)
            {
                BarTable bt = BarTableList.Initialize(list[0], ComboBoxBarFreq.Text.ParseEnum<BarFreq>(), ComboBoxBarType.Text.ParseEnum<TickerType>());
                Period pd = new Period(DateTimePickerRequestIBHistDataStart.Value, DateTimePickerRequestIBHistDataEnd.Value);

                //BarDataSet.Update(list[0], )
                IBClient.Request_HistoricalData(bt, pd, CkBoxIBHistDataRTHOnly.Checked);
            }
            //}



            BtnRequestIBHistData.Enabled = true;
        }

        private void BtnHistDataApplyAdj_Click(object sender, EventArgs e)
        {
            BtnHistDataApplyAdj.Enabled = false;

            var list = ValidSymbol();

            if (list.Count > 0)
            {
                BarTable bt = BarTableList.Initialize(list[0], ComboBoxBarFreq.Text.ParseEnum<BarFreq>(), ComboBoxBarType.Text.ParseEnum<TickerType>());
                //bt.LoadHistoricalBarFile();
                bt.ApplyDividendSplit();
            }

            BtnHistDataApplyAdj.Enabled = true;
        }

        private void BtnHistoricalDataAdjust_Click(object sender, EventArgs e)
        {
            BtnHistoricalDataAdjust.Enabled = false;

            var list = ValidSymbol();

            if (list.Count > 0)
            {
                BarTable bt = BarTableList.Initialize(list[0], ComboBoxBarFreq.Text.ParseEnum<BarFreq>(), ComboBoxBarType.Text.ParseEnum<TickerType>());
                bt.Adjust(CkBoxAdjustDividend.Checked);
            }

            BtnHistoricalDataAdjust.Enabled = true;
        }

        private void BtnHistoricalDataCounterAdjust_Click(object sender, EventArgs e)
        {
            BtnHistoricalDataCounterAdjust.Enabled = false;

            var list = ValidSymbol();

            if (list.Count > 0)
            {
                BarTable bt = BarTableList.Initialize(list[0], ComboBoxBarFreq.Text.ParseEnum<BarFreq>(), ComboBoxBarType.Text.ParseEnum<TickerType>());
                bt.Adjust(CkBoxAdjustDividend.Checked, true);
            }

            BtnHistoricalDataCounterAdjust.Enabled = true;
        }

        private void BtnExportBarTableCSV_Click(object sender, EventArgs e)
        {
            BtnExportBarTableCSV.Enabled = false;

            var list = ValidSymbol();

            if (list.Count > 0)
            {
                BarTable bt = BarTableList.Initialize(list[0], ComboBoxBarFreq.Text.ParseEnum<BarFreq>(), ComboBoxBarType.Text.ParseEnum<TickerType>());
                bt.SaveFS();

                bt.ExportCSV(Root.SaveFile.FileName);
            }

            BtnExportBarTableCSV.Enabled = true;
        }

        #endregion Historical Data

        #region Quandl Historical Data

        public HashSet<string> EODFiles = new HashSet<string>();

        private void BtnQuandlEODFilesAdd_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";
            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                bool hasAdded = EODFiles.CheckAdd(Root.OpenFile.FileName);
                if (hasAdded)
                {
                    ListViewQuandlEODFiles.BeginUpdate();
                    ListViewQuandlEODFiles.Items.Add(new ListViewItem
                    {
                        Text = Path.GetFileName(Root.OpenFile.FileName),
                        Tag = Root.OpenFile.FileName,
                        Group = ListViewQuandlEODFiles.Groups[0]
                    });
                    ListViewQuandlEODFiles.EndUpdate();
                }

            }

            //Log.Print(Util.ICollectionToString(EODFiles));
        }

        private void BtnQuandlEODFilesRemove_Click(object sender, EventArgs e)
        {
            if (ListViewQuandlEODFiles.SelectedItems.Count > 0)
            {
                foreach (ListViewItem eachItem in ListViewQuandlEODFiles.SelectedItems)
                {
                    EODFiles.CheckRemove(eachItem.Tag.ToString());
                    ListViewQuandlEODFiles.Items.Remove(eachItem);
                }
            }

            //Log.Print(Util.ICollectionToString(EODFiles));
        }

        private void BtnQuandlEODFilesMerge_Click(object sender, EventArgs e)
        {
            Root.SaveFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.SaveFile.ShowDialog() == DialogResult.OK)
            {
                cts = new CancellationTokenSource();
                Task m = new Task(() => { Quandl.MergeEODFiles(EODFiles, Root.SaveFile.FileName, progress, cts); });
                m.Start();
            }
        }

        private void BtnLoadEOD_Click(object sender, EventArgs e)
        {
            Root.OpenFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.OpenFile.ShowDialog() == DialogResult.OK)
            {
                cts = new CancellationTokenSource();
                Task m = new Task(() => { Quandl.ImportEOD(Root.OpenFile.FileName, progress, cts); });
                m.Start();
            }
        }

        #endregion Quandl Historical Data

        #region Fundamental

        private void BtnRequestFundamentalCompanyOverview_Click(object sender, EventArgs e)
        {
            var list = ValidSymbol();

            if (list.Count > 0)
            {
                IBClient.Request_FundamentalData(list[0], IBClient.FundamentalRequestType.CompanyOverview);
            }
        }

        private void BtnRequestFundamentalFinancialSummary_Click(object sender, EventArgs e)
        {
            var list = ValidSymbol();

            if (list.Count > 0)
            {
                IBClient.Request_FundamentalData(list[0], IBClient.FundamentalRequestType.FinancialSummary);
            }
        }

        private void BtnRequestFundamentalFinancialStatements_Click(object sender, EventArgs e)
        {
            var list = ValidSymbol();

            if (list.Count > 0)
            {
                IBClient.Request_FundamentalData(list[0], IBClient.FundamentalRequestType.FinancialStatements);
            }
        }

        private void BtnRequestFundamentalAnalystEstimates_Click(object sender, EventArgs e)
        {
            var list = ValidSymbol();

            if (list.Count > 0)
            {
                IBClient.Request_FundamentalData(list[0], IBClient.FundamentalRequestType.AnalystEstimates);
            }
        }

        private void BtnRequestFundamentalCalendar_Click(object sender, EventArgs e)
        {
            var list = ValidSymbol();

            if (list.Count > 0)
            {
                IBClient.Request_FundamentalData(list[0], IBClient.FundamentalRequestType.Calendar);
            }
        }

        private void BtnRequestFundamentalOwnership_Click(object sender, EventArgs e)
        {
            var list = ValidSymbol();

            if (list.Count > 0)
            {
                IBClient.Request_FundamentalData(list[0], IBClient.FundamentalRequestType.Ownership);
            }
        }

        private void BtnCancelFundamental_Click(object sender, EventArgs e)
        {
            IBClient.Cancel_FundamentalData();
        }

        private void BtnDownloadAllFundamentals_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                cts = new CancellationTokenSource();
                fbd.Description = "Download All Fundamentals Folder";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    Task m = new Task(() => { IBClient.DownloadAllFundamentalData(fbd.SelectedPath, ExchangeInfo.US_Exchanges, progress, cts); });
                    m.Start();
                }
            }
        }

        private void BtnImportFolderFundamentalData_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                cts = new CancellationTokenSource();
                fbd.Description = "Import All Fundamentals Folder";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    Task m = new Task(() => { IBClient.ImportFolderFundamentalData(fbd.SelectedPath, progress, cts); });
                    m.Start();
                }
            }
        }

        private void BtnScanFundamentalStatements_Click(object sender, EventArgs e)
        {
            Task m = new Task(() => { IBClient.Test_ScanFundamentalStatements(progress); });
            m.Start();
        }

        private void BtnScanFundamentalSummary_Click(object sender, EventArgs e)
        {
            Task m = new Task(() => { IBClient.Test_ScanFundamentalSummary(progress); });
            m.Start();
        }


        private void CkBoxFundamentalExportAll_CheckedChanged(object sender, EventArgs e)
        {
            if (CkBoxFundamentalExportAll.Checked)
            {
                ComboBoxFundamentalDataType.Enabled = false;
                DateTimePickerFundamentalDataStart.Enabled = false;
                DateTimePickerFundamentalDataEnd.Enabled = false;
            }
            else
            {
                ComboBoxFundamentalDataType.Enabled = true;
                DateTimePickerFundamentalDataStart.Enabled = true;
                DateTimePickerFundamentalDataEnd.Enabled = true;
            }
        }

        private void BtnSaveSampleFundamental_Click(object sender, EventArgs e)
        {
            var list = ValidSymbol();

            if (list.Count > 0)
            {
                (bool valid, BusinessInfo bi) = list[0].GetBusinessInfo();

                if (valid)
                {
                    if (!CkBoxFundamentalExportAll.Checked)
                    {
                        FundamentalDataType type = ComboBoxFundamentalDataType.Text.ParseEnum<FundamentalDataType>();
                        Period pd = new Period(DateTimePickerFundamentalDataStart.Value, DateTimePickerFundamentalDataEnd.Value);
                        bi.ExportFundamentalData(bi.GetFundamentalData(type, new Frequency(TimeUnit.Days), pd));
                    }
                    else
                    {
                        bi.ExportFundamentalData(bi.GetFundamentalData());
                    }
                }
            }
        }

        #endregion Fundamental

        #region Watchlist

        #endregion Watchlist

        #region Scans

        #endregion Scans

        private void BtnTest_Click(object sender, EventArgs e)
        {
            //Nasdaq.CleanUp();
            //IBClient.PrintRequestIds();
            //IBClient.M_reqHistoricalHeadTime();

            //IBClient.SendReq_TickByTickData(IBClient.TickByTickType.Last);

            //IBClient.M_reqHistoricalTicks();
            //foreach (TimeZoneInfo z in TimeZoneInfo.GetSystemTimeZones())
            //Console.WriteLine(z.Id);
            //SymbolList.GetSymbolInfo(SymbolList.GetSymbol("AAPL", Exchange.NASDAQ, SecurityType.STOCK));
        }

        private void BtnVerifyHD_Click(object sender, EventArgs e)
        {
            BtnVerifyHD.Enabled = false;

            var list = ValidSymbol();

            if (list.Count > 0)
            {
                BarTable bt = BarTableList.Initialize(list[0], ComboBoxBarFreq.Text.ParseEnum<BarFreq>(), ComboBoxBarType.Text.ParseEnum<TickerType>());
                Log.Print(bt.Symbol.ToString() + ": " + bt.Count.ToString());

                StringBuilder sb = new StringBuilder();
                sb.Append("Rows|" + bt.Count.ToString() + "|RowsEnd,Segments|");
                foreach (Period pd in bt.DataSourcePeriods)
                {
                    sb.Append(pd.Start.ToString() + "|" + pd.Stop.ToString() + "|" + bt.DataSourceSegments[pd].ToString() + "|");
                }
                sb.Append("SegmentsEnd");
                Log.Print(sb.ToString());
            }

            BtnVerifyHD.Enabled = true;
        }

        private void BtnSaveAllBarTable_Click(object sender, EventArgs e)
        {
            BarTableList.Save();
        }



























        /*
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        */

        /*
        public static void Clear(this EventHandler eh)
        {
            foreach (Delegate d in eh.GetInvocationList())
            {
                eh -= (EventHandler)d;
            }
        }
        */

        /*
        public class EnumComboBox<T> : ComboBox where T : struct
        {
            public EnumComboBox(T en)
            {
                Items.AddRange(typeof(T).GetEnumNames());
            }

            public T Selected => Text.EnumParse<T>();
        }*/
    }
}
