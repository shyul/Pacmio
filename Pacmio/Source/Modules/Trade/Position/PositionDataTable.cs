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
    public sealed class PositionDataTable : IContractTable, IStringTable
    {
        private readonly List<PositionStatus> Rows = new List<PositionStatus>();

        public int Count => Rows.Count;

        public object DataLockObject => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public bool Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TableStatus Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string this[int i, StringColumn column] => throw new NotImplementedException();

        public double this[int i, NumericColumn column] => throw new NotImplementedException();

        public Contract this[int i, ContractColumn column] => throw new NotImplementedException();
    }
}
