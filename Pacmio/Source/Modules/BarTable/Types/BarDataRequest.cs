/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;
using Xu;
using Pacmio.IB;

namespace Pacmio
{
    public struct BarDataRequest
    {
        public BarDataRequest(BarTable bt, Period period)
        {
            Table = bt;
            Period = period;
        }

        public BarTable Table { get; private set; }
        public Period Period { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj is BarDataRequest bdr)
            {
                return this == bdr;
            }
            else if (obj is BarTable bt)
            {
                return Table == bt;
            }
            else
                return false;
        }

        public override int GetHashCode() => Table.GetHashCode() ^ Period.GetHashCode();
        public static bool operator ==(BarDataRequest left, BarDataRequest right) => left.Table == right.Table && left.Period == right.Period;
        public static bool operator !=(BarDataRequest left, BarDataRequest right) => !(left.GetHashCode() == right.GetHashCode());
    }
}
