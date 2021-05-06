using Pacmio;
using Pacmio.IB;
using System;
using System.ComponentModel;
using System.Drawing;
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
        public string Status => "Server Ver: " + Client.ServerVersion + " | Next Valid ID: " + Client.RequestId + " | Connect Time: " + Client.ConnectTime.ToString();

        public static CancellationTokenSource Cts { get; set; }

        public static IProgress<float> Progress { get; private set; }

        public bool ValidateSymbol()
        {
            string symbol = TextBoxSingleContractName.Text.ToUpper();
            var siList = ContractManager.GetOrFetch(symbol, "US");

            if (siList.Count() > 0)
            {
                ContractTest.ActiveContract = siList.First();
                TextBoxSingleContractName.Text = ContractTest.ActiveContract.Name;
                TextBoxSingleContractName.ForeColor = Color.Green;
                SelectBoxSingleContractSecurityType.Text = ContractTest.ActiveContract.TypeName.ToString();
                SelectBoxSingleContractExchange.Text = ContractTest.ActiveContract.Exchange.ToString();
                ContractTest.ActiveContract.Status = ContractStatus.Alive;
                return true;
            }
            else
            {
                TextBoxSingleContractName.ForeColor = Color.Red;
                SelectBoxSingleContractSecurityType.Text = string.Empty;
                SelectBoxSingleContractExchange.Text = string.Empty;
                return false;
            }
        }

        public void GetChart(BarAnalysisList bat, MultiPeriod mp)
        {
            if (ValidateSymbol())
            {
                BarFreq freq = BarFreq;
                PriceType type = DataType;
                Contract c = ContractTest.ActiveContract;
                BarTableSet bts = BarTableGroup[c];

                Cts = new CancellationTokenSource();

                Task.Run(() =>
                {
                    /*BarTable bt = freq < BarFreq.Daily ?
                    c.LoadBarTable(pd, freq, type, false) :
                    BarTableManager.GetOrCreateDailyBarTable(c, freq);*/

                    //var bt = c.LoadBarTable(freq, type, pd, false, Cts);

                    bts.SetPeriod(mp, Cts);
                    BarTable bt = bts[freq, type];
                    bt.CalculateRefresh(bat);
                    BarChart bc = bt.GetChart(bat);
                    HistoricalPeriod = bt.Period;
                }, Cts.Token);

                Root.Form.Show();
            }

        }

        public void GetChart(BarAnalysisList bat) => GetChart(bat, new MultiPeriod(HistoricalPeriod));

        public void GetChart(BarAnalysisSet bas) => GetChart(bas, new MultiPeriod(HistoricalPeriod));

        public void GetChart(BarAnalysisSet bas, MultiPeriod mp)
        {
            if (ValidateSymbol())
            {
                //BarFreq freq = BarFreq;
                //PriceType type = DataType;
                Contract c = ContractTest.ActiveContract;
                BarTableSet bts = BarTableGroup[c];
                //MultiPeriod mp = ;

                Cts = new CancellationTokenSource();

                Task.Run(() =>
                {
                    bts.SetPeriod(mp, Cts);
                    bts.CalculateRefresh(bas);
                    bts.GetChart(bas);
                }, Cts.Token);

                Root.Form.Show();
            }

        }

        public void GetChart(Strategy s, MultiPeriod mp)
        {
            if (ValidateSymbol())
            {
                //BarFreq freq = BarFreq;
                //PriceType type = DataType;
                Contract c = ContractTest.ActiveContract;
                BarTableSet bts = BarTableGroup[c];
                //MultiPeriod mp = ;

                Cts = new CancellationTokenSource();

                Task.Run(() =>
                {
                    bts[s.Filter.BarFreq, s.Filter.PriceType].CalculateRefresh(s.Filter.BarAnalysisList);
                    bts.SetPeriod(mp, Cts);
   
                    bts.CalculateRefresh(s.AnalysisSet);
                    bts.GetChart(s.AnalysisSet);
                }, Cts.Token);

                Root.Form.Show();
            }

        }

        public void NetClientOnConnectHandler(ConnectionStatus status, DateTime time, string msg)
        {
            Task.Run(() =>
            {
                this?.Invoke(() =>
                {
                    ToggleConnect();
                });
            });

        }

        /*
        public void AccountUpdateHandler(int status, DateTime time, string msg)
        {
            Task.Run(() =>
            {
                this?.Invoke(() =>
                {
                    UpdateAccountList();
                    tabAccount.Invalidate(true);
                });
            });

        }

        public void UpdateAccountList()
        {
            ListBoxAccount.Items.Clear();
            TreeViewAccount.Nodes.Clear();
            foreach (var a in AccountPositionManager.List)
            {
                ListBoxAccount.Items.Add(a.AccountId);


                string dayTradingString = "[ ";
                foreach (int d in a.DayTradesRemaining)
                {
                    if (d == -1)
                    {
                        dayTradingString = "[ No DT Limit";
                        break;
                    }
                    else
                        dayTradingString += d + ", ";
                }
                dayTradingString = dayTradingString.Trim().TrimEnd(',') + " ]";

                TreeNode tr = new TreeNode(a.AccountId + ": " + a.AccountType + " " + dayTradingString);

                foreach (PropertyInfo p in a.GetType().GetProperties())
                {
                    string tagName = p.Name + " = ";
                    if (p.GetAttribute<DisplayNameAttribute>() is DisplayNameAttribute Result) tagName = Result.DisplayName + " = ";

                    if (p.PropertyType == typeof(double))
                    {
                        double v = (double)p.GetValue(a);
                        if (!double.IsNaN(v))
                        {
                            tagName += v.ToString();
                            tr.Nodes.Add(tagName);
                        }
                    }
                }

                TreeViewAccount.Nodes.Add(tr);

       
            }

            TreeViewAccount.ExpandAll();
        }*/


        /*
        public void PositionUpdatedHandler(int status, DateTime time, string msg)
        {
            Task UpdateUI = new Task(() => {
                Invoke((MethodInvoker)delegate {

                    lock (AccountManager.List)
                    {



                        PositionsGrid.Rows.Clear();
                        int i = 0;
                        foreach (InteractiveBrokersAccount ac in AccountManager.List)
                        {
                            Console.WriteLine(ac.AccountId + " ac.Positions.Count = " + ac.Positions.Count);
                            foreach (Contract c in ac.Positions.Keys) 
                            {
                                var pos = ac.Positions[c];

                                PositionsGrid.Rows.Add(1);
                                PositionsGrid[0, i].Value = c.Name + " @ " + c.Exchange;
                                PositionsGrid[1, i].Value = ac.AccountId;
                                PositionsGrid[2, i].Value = pos.Quantity;
                                PositionsGrid[3, i].Value = pos.AveragePrice;
                                PositionsGrid[4, i].Value = pos.Value;
                                i++;
                            }
                        }
                    }
                    //tabAccount.Invalidate(true);
                });
            });
            UpdateUI.Start();
        }
        */






        public void ToggleConnect()
        {
            if (!Root.NetConnected)
            {
                btnConnect.Text = "Connect";
                LbStatus.Text = "Not Connected";
                //MainTab.Enabled = false;
                foreach (var tab in MainTab.TabPages)
                {
                    //if (tab is TabPage page && page.Text != "Data") { page.Enabled = false; }
                }
            }
            else
            {
                btnConnect.Text = "Disconnect";
                LbStatus.Text = Status;
                //MainTab.Enabled = true;
                foreach (var tab in MainTab.TabPages)
                {
                    //if (tab is TabPage page) { page.Enabled = true; }
                }
            }
        }
    }
}
