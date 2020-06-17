/// ***************************************************************************
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
    public sealed class MarketDataTable : IContractTable
    {
        public int Count => Rows.Count; // Root.IBClient.ActiveTicks.Count;

        private readonly List<Contract> Rows = new List<Contract>();

        public Contract this[int i, ContractColumn column]
        {
            get
            {
                if (i >= Count || i < 0)
                    return null;
                else
                    return Rows[i];
            }
        }

        public double this[int i, NumericColumn column] => throw new NotImplementedException();





        public object DataObjectLock => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public bool Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #region IDependable

        public ICollection<IDependable> Children => throw new NotImplementedException();

        public ICollection<IDependable> Parents => throw new NotImplementedException();

        public void Remove(bool recursive)
        {
            throw new NotImplementedException();
        }

        #endregion IDependable


        public static readonly NumericColumn Column_Open = new NumericColumn("OPEN") { Label = "O" };
        public static readonly NumericColumn Column_High = new NumericColumn("HIGH") { Label = "H" };
        public static readonly NumericColumn Column_Low = new NumericColumn("LOW") { Label = "L" };
        public static readonly NumericColumn Column_Close = new NumericColumn("CLOSE") { Label = "C" };
        public static readonly NumericColumn Column_Volume = new NumericColumn("VOLUME");
    }
}
