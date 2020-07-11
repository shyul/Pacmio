using Pacmio;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace TestClient
{
    public static class OrderTest
    {
        public static Account LiveAccount { get; set; }

        private static readonly DataTable Table = new DataTable("Order List");

        public static DataTable InitializeTable(DataGridView gv)
        {
            DataTable table = Table;
            gv.DataSource = table;

            DataColumn keyColumn = new DataColumn("PermId", typeof(int));
            table.Columns.Add(keyColumn);
            table.PrimaryKey = new DataColumn[] { keyColumn };
            gv.Columns[0].Width = 80;
            gv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Contract", typeof(string));
            gv.Columns[1].Width = 130;
            gv.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            table.Columns.Add("Quantity", typeof(double));
            gv.Columns[2].Width = 65;
            gv.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            table.Columns.Add("Order Type", typeof(string));
            gv.Columns[3].Width = 130;
            gv.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            table.Columns.Add("Cash Qty", typeof(string));
            gv.Columns[4].Width = 100;
            gv.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            table.Columns.Add("Time In Force", typeof(string));
            gv.Columns[5].Width = 200;
            gv.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gv.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            table.Columns.Add("Status", typeof(string));
            gv.Columns[6].Width = 90;
            gv.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Mode Code", typeof(string));
            gv.Columns[7].Width = 80;
            gv.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Account", typeof(string));
            gv.Columns[8].Width = 70;
            gv.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Order Id", typeof(int));
            gv.Columns[9].Width = 70;
            gv.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Client Id", typeof(int));
            gv.Columns[10].Width = 70;
            gv.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            table.Columns.Add("Exec Time", typeof(DateTime));
            gv.Columns[11].Width = 110;
            gv.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gv.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            gv.Sort(gv.Columns[6], ListSortDirection.Descending);

            return table;
        }

        public static void UpdateTable(OrderInfo od)
        {
            //Console.WriteLine("\n\nUpdating ########## " + od.ContractInfo.name);

            string contractName = od.Contract.ToString();

            string orderType = od.Type.ToString();

            if (od.Type == OrderType.Limit || od.Type == OrderType.MidPrice)
                orderType += " " + od.LimitPrice;
            else if (od.Type == OrderType.Stop)
                orderType += " " + od.AuxPrice;

            string tif = od.TimeInForce.ToString();

            if (od.TimeInForce == OrderTimeInForce.GoodAfterDate || od.TimeInForce == OrderTimeInForce.GoodUntilDate)
                tif += " " + od.EffectiveDateTime.ToString("yyyyMMdd HH:mm:ss");


            var rows = Table.AsEnumerable()
                .Where(r => r.Field<int>("PermId") == od.PermId);

            if (rows.Count() > 0)
            {
                DataRow row = rows.First();
                row.BeginEdit();
                row["Contract"] = contractName;
                row["Quantity"] = od.Quantity;
                row["Order Type"] = orderType;
                row["Cash Qty"] = (od.Quantity * od.LimitPrice).ToString("0.000"); // double.NaN; // Hint: Quantity * Limit Price, and always load market price to limit price by default
                row["Time In Force"] = tif;
                row["Status"] = od.Status.ToString();
                row["Mode Code"] = od.ModeCode;
                row["Account"] = od.AccountCode;
                row["Order Id"] = od.OrderId;
                row["Client Id"] = od.ClientId;
                row["Exec Time"] = od.OrderTime;
                row.EndEdit();
            }
            else
            {
                Table.Rows.Add(od.PermId, contractName,
                    od.Quantity, orderType, od.Quantity * od.LimitPrice,
                   tif, od.Status.ToString(), od.ModeCode, od.AccountCode, od.OrderId, od.ClientId, od.OrderTime);
            }
        }
    }
}
