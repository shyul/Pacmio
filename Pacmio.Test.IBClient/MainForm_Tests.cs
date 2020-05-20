using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using Xu;
using Pacmio;
using Pacmio.IB;

namespace TestClient
{
    public partial class MainForm : Form
    {
        public string Status => "Server Ver: " + Root.IBClient.ServerVersion + " | Next Valid ID: " + Root.IBClient.RequestId + " | Connect Time: " + Root.IBClient.ConnectTime.ToString();

        public static CancellationTokenSource Cts { get; set; }

        public static Progress<int> Progress;

        public bool ValidateSymbol()
        {
            string symbol = TbSymbolName.Text.ToUpper();
            var siList = ContractList.GetOrFetch(symbol, "US");

            if (siList.Count() > 0)
            {
                ContractTest.ActiveContract = siList.First();
                TbSymbolName.Text = ContractTest.ActiveContract.Name;
                TbSymbolName.ForeColor = Color.Green;
                SelectSecurityType.Text = ContractTest.ActiveContract.TypeName.ToString();
                SelectExchange.Text = ContractTest.ActiveContract.Exchange.ToString();
                return true;
            }
            else
            {
                TbSymbolName.ForeColor = Color.Red;
                SelectSecurityType.Text = string.Empty;
                SelectExchange.Text = string.Empty;
                return false;
            }
        }

        public void IBClientOnConnectedHandler(ConnectionStatus status, DateTime time, string msg)
        {
            Task UpdateUI = new Task(() => {
                Invoke((MethodInvoker)delegate {
                    ToggleConnect();
                });
            });
            UpdateUI.Start();
        }

        public void AccountUpdatedHandler(int status, DateTime time, string msg)
        {
            Task UpdateUI = new Task(() => {
                Invoke((MethodInvoker)delegate {
                    UpdateAccountList();
                    tabAccount.Invalidate(true);
                });
            });
            UpdateUI.Start();
        }

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
                            Console.WriteLine(ac.AccountCode + " ac.Positions.Count = " + ac.Positions.Count);
                            foreach (Contract c in ac.Positions.Keys) 
                            {
                                var pos = ac.Positions[c];

                                PositionsGrid.Rows.Add(1);
                                PositionsGrid[0, i].Value = c.Name + " @ " + c.Exchange;
                                PositionsGrid[1, i].Value = ac.AccountCode;
                                PositionsGrid[2, i].Value = pos.Quantity;
                                PositionsGrid[3, i].Value = pos.CostBasis;
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





        public void MarketQuoteHandler(int status, DateTime time, string msg)
        {
            //if((DateTime.Now - LastQuoteUpdate).TotalMilliseconds > 200) 
            //{
            Task UpdateUI = new Task(() => {
                Invoke((MethodInvoker)delegate {

                    //MarketQuoteGrid.Suspend();
                    lock (Root.IBClient.ActiveTicks)
                    //foreach (int tickerId in Root.IBClient.ActiveQuotes.Keys)
                    {
                        int tickerId = status;
                        Contract c = Root.IBClient.ActiveTicks[tickerId];
                        MarketDataTest.UpdateMarketQuote(tickerId, c);
                    }
                    //MarketQuoteGrid.Resume();
                    MarketDataTest.LastQuoteUpdate = DateTime.Now;
                });
            });
            UpdateUI.Start();
            //}
        }


        public void OrderTableHandler(int status, DateTime time, string msg)
        {
            Task UpdateUI = new Task(() => {
                Invoke((MethodInvoker)delegate {

                    if(status == IncomingMessage.OpenOrder || status == IncomingMessage.CompletedOrder) 
                    {
                        int permId = msg.ToInt32(-1);
                        OrderInfo od = OrderManager.Get(permId);

                        if (!(od is null))
                            OrderTest.UpdateTable(od);
                    }
                });
            });
            UpdateUI.Start();
        }


        public void TradeTableHandler(int status, DateTime time, string msg)
        {
            Task UpdateUI = new Task(() => {
                Invoke((MethodInvoker)delegate {

                    if (status == IncomingMessage.ExecutionData || status == IncomingMessage.CommissionsReport)
                    {
                        string execId = msg;
                        TradeInfo ti = TradeLogManager.Get(execId);

                        if (!(ti is null))
                            TradeTest.UpdateTable(ti);
                    }
                });
            });
            UpdateUI.Start();
        }

        public void UpdateAccountList()
        {
            ListBoxAccount.Items.Clear();
            TreeViewAccount.Nodes.Clear();
            foreach (var a in AccountManager.List)
            {
                ListBoxAccount.Items.Add(a.AccountCode);

                //if (a.AccountReady) 
                //
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

                TreeNode tr = new TreeNode(a.AccountCode + ": " + a.AccountType + " " + dayTradingString);

                foreach (PropertyInfo p in a.GetType().GetProperties())
                {
                    string tagName = p.Name + " = ";
                    (bool IsValid, DisplayNameAttribute Result) = p.GetAttribute<DisplayNameAttribute>();
                    if (IsValid) tagName = Result.DisplayName + " = ";

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

                //}
            }

            TreeViewAccount.ExpandAll();
        }

        public void ToggleConnect()
        {
      

            if (!Root.IBClient.Connected)
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
