/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Xu;

namespace Pacmio
{
    public sealed class MarketDataTable : IContractTable, IStringTable
    {
        private readonly List<Contract> Rows = new List<Contract>();

        public int Count => Rows.Count;



        public void Add(Contract c) => Rows.CheckAdd(c);

        public bool Contains(Contract c) => Rows.Contains(c);

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
                    return Rows[i];
            }
        }

        public object DataLockObject { get; } = new object();

        public string Name { get; set; }

        public bool Enabled { get; set; } = true;
        public TableStatus Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
