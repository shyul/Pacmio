/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Data;
using System.Drawing.Text;
using System.Windows.Forms;
using Xu;
using Pacmio;
using Pacmio.IB;
using System.Threading.Tasks;
using System.Threading;
using TestClient.UI;

namespace TestClient
{
    public static class MarketDataTest
    {
        public static readonly MarketDataGridView GridView = new MarketDataGridView("Market Data", new MarketDataTable());
        public static MarketDataTable MarketDataTable => GridView.MarketDataTable;
        public static DateTime LastQuoteUpdate { get; set; } = DateTime.MinValue;

        public static void InitializeTable()
        {
            MarketData.UpdatedHandler += MarketDataTest.MarketDataHandler;
            Root.Form.AddForm(DockStyle.Fill, 0, GridView);
        }

        public static void MarketDataHandler(int status, DateTime time, string msg)
        {
            /*
            Task UpdateUI = new Task(() => {
                Invoke((MethodInvoker)delegate {
                    lock (Root.IBClient.ActiveTicks)
                    {
                        int tickerId = status;
                        Contract c = Root.IBClient.ActiveTicks[tickerId];
                        MarketDataTest.UpdateMarketQuote(tickerId, c);
                    }
                    MarketDataTest.LastQuoteUpdate = DateTime.Now;
                });
            });
            UpdateUI.Start();
            */

            lock (Root.IBClient.ActiveTicks)
            {
                int tickerId = status;
                Contract c = Root.IBClient.ActiveTicks[tickerId];
                MarketDataTable.Add(c);
                GridView.Invalidate();
            }
            LastQuoteUpdate = DateTime.Now;
        }
    }
}
