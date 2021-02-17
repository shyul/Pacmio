/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Pacmio;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace TestClient
{
    public static class ContractTest
    {
        public static Contract ActiveContract { get; set; }

        public static DataTable SymbolInfoTable { get; } = new DataTable();

        public static void InitializeTable(DataGridView gv)
        {
            SymbolInfoTable.TableName = "Symbol Info Table";
            gv.DataSource = SymbolInfoTable;

            System.Data.DataColumn keyColumn = new System.Data.DataColumn("Id", typeof(int));
            SymbolInfoTable.Columns.Add(keyColumn);
            //SymbolInfoTable.PrimaryKey = new System.Data.DataColumn[] { keyColumn };
            gv.Columns[0].Width = 35;

            SymbolInfoTable.Columns.Add("ConId", typeof(int));
            gv.Columns[1].Width = 80;

            SymbolInfoTable.Columns.Add("Status", typeof(string));
            gv.Columns[2].Width = 70;

            SymbolInfoTable.Columns.Add("Type", typeof(string));
            gv.Columns[3].Width = 50;

            SymbolInfoTable.Columns.Add("Symbol", typeof(string));
            gv.Columns[4].Width = 60;

            SymbolInfoTable.Columns.Add("Exchange", typeof(string));
            gv.Columns[5].Width = 100;

            SymbolInfoTable.Columns.Add("Full Name", typeof(string));
            gv.Columns[6].Width = 350;

            SymbolInfoTable.Columns.Add("ISIN", typeof(string));
            gv.Columns[7].Width = 120;

            SymbolInfoTable.Columns.Add("CUSIP", typeof(string));
            gv.Columns[8].Width = 120;

            SymbolInfoTable.AcceptChanges();
        }

        static int RowIndex { get; set; } = -1;
        public static object[] selector(Contract c)
        {
            RowIndex++;
            if (c is IBusiness it && it.BusinessInfo is BusinessInfo info)
            {
                //(bool biValid, BusinessInfo info) = it.GetBusinessInfo();
                return new object[] { RowIndex, c.ConId, c.Status.ToString(), c.TypeName, c.Name, c.ExchangeName, c.FullName, it.ISIN, info.CUSIP };
            }
            else
                return new object[] { RowIndex, c.ConId, c.Status.ToString(), c.TypeName, c.Name, c.ExchangeName, c.FullName, string.Empty, string.Empty };
        }

        public static void UpdateSymbolInfoTable(string name)
        {
            SymbolInfoTable.Rows.Clear();
            RowIndex = -1;
            Func<Contract, bool> all = si => si.Country == "US";
            Func<Contract, bool> searchName = si => si.Country == "US" && si.Name.Contains(name);

            name = name.ToUpper();
            var result = string.IsNullOrWhiteSpace(name) ?
                ContractList.Values.Where(all) :
                ContractList.Values.Where(searchName);

            var rows = result.Select(selector).ToArray();

            foreach (var rowLine in rows)
            {
                SymbolInfoTable.Rows.Add(rowLine);
            }
        }
    }
}
