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
        public string Status => "Server Ver: " + Client.ServerVersion + " | Next Valid ID: " + Client.RequestId + " | Connect Time: " + Client.ConnectTime.ToString();

        public static CancellationTokenSource Cts { get; set; }

        public static Progress<float> Progress;

        public bool ValidateSymbol()
        {
            string symbol = TextBoxSingleContractName.Text.ToUpper();
            var siList = ContractList.GetOrFetch(symbol, "US");

            if (siList.Count() > 0)
            {
                ContractTest.ActiveContract = siList.First();
                TextBoxSingleContractName.Text = ContractTest.ActiveContract.Name;
                TextBoxSingleContractName.ForeColor = Color.Green;
                SelectBoxSingleContractSecurityType.Text = ContractTest.ActiveContract.TypeName.ToString();
                SelectBoxSingleContractExchange.Text = ContractTest.ActiveContract.Exchange.ToString();
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
                            Console.WriteLine(ac.AccountCode + " ac.Positions.Count = " + ac.Positions.Count);
                            foreach (Contract c in ac.Positions.Keys) 
                            {
                                var pos = ac.Positions[c];

                                PositionsGrid.Rows.Add(1);
                                PositionsGrid[0, i].Value = c.Name + " @ " + c.Exchange;
                                PositionsGrid[1, i].Value = ac.AccountCode;
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


        public void TradeTableHandler(int status, DateTime time, string msg)
        {
            Task UpdateUI = new Task(() => {
                Invoke((MethodInvoker)delegate {

                    if (status == IncomingMessage.ExecutionData || status == IncomingMessage.CommissionsReport)
                    {
                        string execId = msg;
                        TradeInfo ti = TradeInfoManager.Get(execId);

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
      

            if (!Root.IBConnected)
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
