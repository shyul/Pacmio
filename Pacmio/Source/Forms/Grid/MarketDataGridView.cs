/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    public class MarketDataGridView : GridWidget<StockData>
    {
        public MarketDataGridView(string name) : base(name)
        {

            //LoadConfiguration();
        }

        public void LoadConfiguration(string fileName) 
        {
        
        }

        public void SaveConfiguration()
        {

        }

        public void Add(StockData s)
        {
            Console.WriteLine("MarketDataGridView Adding: " + s.Contract.ToString());

            List<StockData> newRows = new List<StockData>();
            if (Rows is StockData[] rows)
            { 
                newRows.AddRange(rows);
            }
            newRows.CheckAdd(s);
            s.AddDataView(this);
            Update(newRows);
        }

        public void Remove(StockData s)
        {
            List<StockData> list = new List<StockData>();
            if (Rows is StockData[] rows) list.AddRange(rows);
            list.CheckRemove(s);
            s.RemoveDataView(this);
            Update(list);
        }

        public bool Contains(MarketData md) => Rows is StockData[] rows && rows.Where(n => n == md).Count() > 0;

        public Contract SelectedContract => Rows[SelectedIndex].Contract;

        public override Rectangle GridBounds => new Rectangle(new Point(0, 0), Size);

    }
}
