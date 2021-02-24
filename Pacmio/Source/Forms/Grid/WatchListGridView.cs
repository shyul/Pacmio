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
        public WatchListGridView(WatchList wt) : base(wt.Name)
        {
            WatchList = wt;
            DataIsUpdated();
        }

        public WatchList WatchList { get; }

        public override void DataIsUpdated()
        {
            var rows = WatchList.Contracts.Where(n => n is Stock).Select(n => n as Stock).Select(n => n.StockData).ToList();

            if (Rows is not null)
                foreach (var s in Rows.Where(n => !rows.Contains(n)))
                    s.RemoveDataConsumer(this);

            foreach (var s in rows) s.AddDataConsumer(this);

            var pi = ColumnConfigurations.Where(n => n.Key.PropertyType is IComparable && n.Value.SortPriority < int.MaxValue).OrderBy(n => n.Value.SortPriority).Select(n => n.Key);

            if (pi.Count() > 0)
            {
                var orderedList = rows.OrderBy(n => pi.First().GetValue(n, null));

                if (pi.Count() > 1)
                {
                    foreach (PropertyInfo p in pi.Skip(1))
                    {
                        orderedList = orderedList.ThenBy(n => p.GetValue(n, null));
                    }
                }

                lock (DataLockObject)
                    lock (GraphicsLockObject)
                    {
                        Rows = orderedList.ToArray();
                    }
            }
            else
            {
                lock (DataLockObject)
                    lock (GraphicsLockObject)
                    {
                        Rows = rows.ToArray();
                    }
            }

            Console.WriteLine("WatchList is updated!");

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
