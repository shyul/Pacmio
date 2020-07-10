using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;
using Pacmio;
using Pacmio.IB;

namespace TestClient
{
    public static class TradeTest
    {
        private static readonly DataTable Table = new DataTable("Trade List");

        public static DataTable InitializeTable(DataGridView gv)
        {
            DataTable table = Table;
            gv.DataSource = table;

            DataColumn keyColumn = new DataColumn("ExecId", typeof(string));
            table.Columns.Add(keyColumn);
            table.PrimaryKey = new DataColumn[] { keyColumn };
            gv.Columns[0].Width = 150;
            gv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Contract", typeof(string));
            gv.Columns[1].Width = 120;
            gv.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            table.Columns.Add("Total Quantity", typeof(double));
            gv.Columns[2].Width = 60;
            gv.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            table.Columns.Add("Average Price", typeof(string));
            gv.Columns[3].Width = 80;
            gv.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Quantity", typeof(double));
            gv.Columns[4].Width = 60;
            gv.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            table.Columns.Add("Price", typeof(string));
            gv.Columns[5].Width = 80;
            gv.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Cash Qty", typeof(string));
            gv.Columns[6].Width = 80;
            gv.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            table.Columns.Add("Commissions", typeof(double));
            gv.Columns[7].Width = 80;
            gv.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            table.Columns.Add("Realized PnL", typeof(double));
            gv.Columns[8].Width = 90;
            gv.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Status", typeof(string)); // Liquidity
            gv.Columns[9].Width = 80;
            gv.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Mode Code", typeof(string));
            gv.Columns[10].Width = 80;
            gv.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Account", typeof(string));
            gv.Columns[11].Width = 70;
            gv.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Perm Id", typeof(int));
            gv.Columns[12].Width = 80;
            gv.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Exec Time", typeof(DateTime));
            gv.Columns[13].Width = 110;
            gv.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gv.Columns[13].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            gv.Sort(gv.Columns[13], ListSortDirection.Descending);

            return table;
        }

        public static void UpdateTable(TradeInfo ti)
        {
            Console.WriteLine("\n\nUpdating ########## " + ti.ContractInfo.name);

            var rows = Table.AsEnumerable()
                .Where(r => r.Field<string>("ExecId") == ti.ExecId);

            string contractName = ti.ContractInfo.name + " @ " + ti.ContractInfo.exchange;
            /*
            if (rows.Count() > 0)
            {
                DataRow row = rows.First();
                row.BeginEdit();
                row["Contract"] = contractName;
                row["Total Quantity"] = ti.TotalQuantity;
                row["Average Price"] = ti.AveragePrice;
                row["Quantity"] = ti.Quantity;
                row["Price"] = ti.Price;
                row["Cash Qty"] = (ti.Quantity * ti.Price).ToString("0.000"); // double.NaN; // Hint: Quantity * Limit Price, and always load market price to limit price by default
                row["Commissions"] = ti.Commissions;
                row["Realized PnL"] = ti.RealizedPnL;
                row["Status"] = ti.LastLiquidity.ToString();
                row["Mode Code"] = ti.ModeCode;
                row["Account"] = ti.AccountCode;
                row["Perm Id"] = ti.PermId;
                row["Exec Time"] = ti.ExecuteTime;
                row.EndEdit();
            }
            else
            {
                Table?.Rows.Add(ti.ExecId, contractName, ti.TotalQuantity, ti.AveragePrice, ti.Quantity,
                    ti.Price, (ti.Quantity * ti.Price).ToString("0.000"), ti.Commissions, ti.RealizedPnL,
                    ti.LastLiquidity.ToString(), ti.ModeCode, ti.AccountCode, ti.PermId, ti.ExecuteTime);
            }*/
        }
    }
}
