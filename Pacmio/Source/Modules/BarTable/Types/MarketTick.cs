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
    public struct MarketTick
    {
        public MarketTick(Contract contract, DateTime time, double price, double size)
        {
            Contract = contract;
            Time = time;
            Price = price;
            Size = size;
        }

        public Contract Contract { get; private set; }
        public DateTime Time { get; private set; }
        public double Price { get; private set; }
        public double Size { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj is MarketTick mt)
            {
                return this == mt;
            }
            else
                return false;
        }

        public override int GetHashCode() => Contract.GetHashCode() ^ Time.GetHashCode() ^ Price.GetHashCode() ^ Size.GetHashCode();
        public static bool operator ==(MarketTick left, MarketTick right) => left.GetHashCode() == right.GetHashCode();
        public static bool operator !=(MarketTick left, MarketTick right) => left.GetHashCode() != right.GetHashCode();
    }
}
