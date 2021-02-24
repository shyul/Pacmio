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
    public class WatchListGridView : GridWidget<StockData>, IEquatable<WatchListGridView>
    {
        public WatchListGridView(WatchList wt) : base("WatchList: " + wt.Name)
        {
            WatchList = wt;
            DataIsUpdated();
        }

        public WatchList WatchList { get; }

        public override void DataIsUpdated()
        {
            SourceRows = WatchList.Contracts.Where(n => n is Stock).Select(n => n as Stock).Select(n => n.StockData).ToList();

            if (Rows is not null)
                foreach (var s in Rows.Where(n => !SourceRows.Contains(n)))
                    s.RemoveDataConsumer(this);

            foreach (var s in SourceRows) s.AddDataConsumer(this);

            base.DataIsUpdated();
        }

        #region Equality

        public bool Equals(WatchListGridView other) => WatchList is WatchList wt && wt == other.WatchList;

        public static bool operator ==(WatchListGridView s1, WatchListGridView s2) => s1.Equals(s2);
        public static bool operator !=(WatchListGridView s1, WatchListGridView s2) => !s1.Equals(s2);

        public override bool Equals(object other) => other is WatchListGridView mdv ? Equals(mdv) : false;

        public override int GetHashCode() => WatchList.GetHashCode();

        #endregion Equality

        public Contract SelectedContract
        {
            get
            {
                lock (DataLockObject)
                    if (Rows.Count() > SelectedIndex)
                        return Rows.ElementAt(SelectedIndex).Contract;
                    else
                        return null;
            }
        }

        public override Rectangle GridBounds => new Rectangle(new Point(0, 0), Size);

    }
}
