﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Xu;
using System.Linq;

namespace Pacmio
{
    public sealed class MarketDataTable : IContractTable, IStringTable
    {
        public int Count => Rows.Count;

        private readonly List<MarketData> Rows = new List<MarketData>();

        public void Add(MarketData md) => Rows.CheckAdd(md);

        public bool Contains(MarketData md) => Rows.Contains(md);

        public double this[int i, NumericColumn column]
        {
            get
            {
                if (i >= Count || i < 0)
                    return double.NaN;
                else
                    return (double)Rows[i][column];
            }
        }

        public string this[int i, StringColumn column]
        {
            get
            {
                if (i >= Count || i < 0)
                    return null;
                else
                    return (string)Rows[i][column];
            }
        }

        public Contract this[int i, ContractColumn column]
        {
            get
            {
                if (i >= Count || i < 0)
                    return null;
                else
                    return Rows[i].Contract;
            }
        }

        public object DataObjectLock { get; } = new object();

        public string Name { get; set; }

        public bool Enabled { get; set; } = true;

        #region IDependable

        public ICollection<IDependable> Children { get; } = new HashSet<IDependable>();

        public ICollection<IDependable> Parents { get; } = new HashSet<IDependable>();

        public void Remove(bool recursive)
        {
            if (recursive || Children.Count == 0)
            {
                foreach (IDependable child in Children)
                    child.Remove(true);

                foreach (IDependable parent in Parents)
                    parent.CheckRemove(this);

                if (Children.Count > 0) throw new Exception("Still have children in this BarTable");

                //Table.RemoveAnalysis(this);
            }
            else
            {
                if (Children.Count > 0)
                {
                    foreach (var child in Children)
                        child.Enabled = false;
                }
                Enabled = false;
            }
        }

        #endregion IDependable



    }
}