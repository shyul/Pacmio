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
        public MarketDataGridView(WatchList wt) : base(wt.Name)
        {
            WatchList = wt;

            if (WatchList is DynamicWatchList dwt)
            {
                dwt.OnUpdateHandler += DynamicUpdateHandler;
            }
            else
            {
                Update(WatchList.Contracts);
            }
        }

        public WatchList WatchList { get; }

        public void DynamicUpdateHandler(int status, DateTime time, string msg)
        {
            Update(WatchList.Contracts);
        }

        public void Update(IEnumerable<Contract> list)
        {
            var rows = list.Where(n => n is Stock).Select(n => n as Stock).Select(n => n.StockData).ToArray();
            foreach (var s in rows) s.AddDataView(this);
            Update(rows);
        }

        public Contract SelectedContract
        {
            get
            {
                lock (DataLockObject)
                    if (Rows.Length > SelectedIndex)
                        return Rows[SelectedIndex].Contract;
                    else
                        return null;
            }
        }

        public override Rectangle GridBounds => new Rectangle(new Point(0, 0), Size);

    }
}
