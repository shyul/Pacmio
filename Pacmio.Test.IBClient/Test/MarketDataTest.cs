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
        public static GridForm Form { get; } = new GridForm() { Text = "Market Data", Size = new Size(1600, 1200) };

        public static DataTable MarketDataTable { get; } = new DataTable();

        public static void InitializeTable()
        {
            InitializeTable(Form.GridView);
        }

        public static void InitializeTable(DataGridView gv)
        {
            MarketDataTable.TableName = "Market Quote Table";
            gv.DataSource = MarketDataTable;

            System.Data.DataColumn keyColumn = new System.Data.DataColumn("Id", typeof(int));
            MarketDataTable.Columns.Add(keyColumn);
            MarketDataTable.PrimaryKey = new System.Data.DataColumn[] { keyColumn };
            gv.Columns[0].Width = 80;
            //gv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Status", typeof(string));
            gv.Columns[1].Width = 70;
            gv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Symbol", typeof(string));
            gv.Columns[2].Width = 120;
            gv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Trade Time", typeof(string));
            gv.Columns[3].Width = 100;
            gv.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Bid Exchange", typeof(string));
            gv.Columns[4].Width = 90;
            gv.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Bid Size", typeof(double));
            gv.Columns[5].Width = 70;
            gv.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gv.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Bid", typeof(double));
            gv.Columns[6].Width = 60;
            gv.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //gv.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Ask", typeof(double));
            gv.Columns[7].Width = 60;
            gv.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //gv.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Ask Size", typeof(double));
            gv.Columns[8].Width = 70;
            gv.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Ask Exchange", typeof(string));
            gv.Columns[9].Width = 90;
            gv.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Last", typeof(double));
            gv.Columns[10].Width = 60;
            //gv.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Last Size", typeof(double));
            gv.Columns[11].Width = 80;
            gv.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("L.Exch", typeof(string));
            gv.Columns[12].Width = 50;
            gv.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Volume", typeof(double));
            gv.Columns[13].Width = 70;
            gv.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Last Close", typeof(double));
            gv.Columns[14].Width = 70;
            //gv.Columns[14].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Open", typeof(double));
            gv.Columns[15].Width = 70;
            //gv.Columns[15].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("High", typeof(double));
            gv.Columns[16].Width = 70;
            //gv.Columns[16].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Low", typeof(double));
            gv.Columns[17].Width = 70;
            //gv.Columns[17].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Short", typeof(double));
            gv.Columns[18].Width = 60;
            //gv.Columns[18].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("S Shares", typeof(double));
            gv.Columns[19].Width = 100;
            gv.Columns[19].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.Columns.Add("Position", typeof(double));
            gv.Columns[20].Width = 70;

            MarketDataTable.Columns.Add("Avg. Cost", typeof(double));
            gv.Columns[21].Width = 80;

            MarketDataTable.Columns.Add("Total Cost", typeof(double));
            gv.Columns[22].Width = 80;

            MarketDataTable.Columns.Add("Total Value", typeof(double));
            gv.Columns[23].Width = 80;

            MarketDataTable.Columns.Add("PnL", typeof(double));
            gv.Columns[24].Width = 70;

            MarketDataTable.Columns.Add("Account", typeof(string));
            gv.Columns[21].Width = 200;
            gv.Columns[21].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            MarketDataTable.AcceptChanges();
        }

        public static void UpdateMarketQuote(int tickerId, Contract c)
        {
            var rows = MarketDataTable.AsEnumerable()
               .Where(r => r.Field<int>("Id") == tickerId);

            MarketData q = c.MarketData;

            if (rows.Count() > 0)
            {
                DataRow row = rows.First();

                row.BeginEdit();
                row["Status"] = q.Status;
                row["Symbol"] = c.Name + " @ " + c.Exchange;
                row["Trade Time"] = q.LastTradeTime.ToString("MM-dd-yy HH:mm:ss");

                row["Bid Exchange"] = q.BidExchange;
                row["Bid Size"] = q.BidSize;
                row["Bid"] = q.Bid;

                row["Ask"] = q.Ask;
                row["Ask Size"] = q.AskSize;
                row["Ask Exchange"] = q.AskExchange;

                row["Last"] = q.Last;
                row["Last Size"] = q.LastSize;
                row["L.Exch"] = q.LastExchange;

                row["Volume"] = q.Volume;
                row["Last Close"] = q.LastClose;
                row["Open"] = q.Open;
                row["High"] = q.High;
                row["Low"] = q.Low;

                row["Short"] = q.Shortable;
                row["S Shares"] = q.ShortableShares;
                row.EndEdit();
            }
            else
            {
                MarketDataTable.Rows.Add(tickerId, q.Status, c.Name + " @ " + c.Exchange, q.LastTradeTime.ToString("MM-dd-yy HH:mm:ss"),
                     q.BidExchange, q.BidSize, q.Bid, q.Ask, q.AskSize, q.AskExchange, q.Last, q.LastSize, q.LastExchange,
                     q.Volume, q.LastClose, q.Open, q.High, q.Low, q.Shortable, q.ShortableShares);
            }
        }

        public static DateTime LastQuoteUpdate { get; set; } = DateTime.MinValue;
    }
}
