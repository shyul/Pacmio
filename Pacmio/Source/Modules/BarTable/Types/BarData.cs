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
    public struct BarData
    {
        public BarData(DataSource source, DateTime time, TimeSpan span, double open, double high, double low, double close, double volume, bool isAdjusted)
        {
            Source = source;
            Time = time;
            Span = span;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            IsAdjusted = isAdjusted;
        }

        public DataSource Source { get; private set; }
        public DateTime Time { get; private set; }
        public TimeSpan Span { get; private set; }
        public double Open { get; private set; }
        public double High { get; private set; }
        public double Low { get; private set; }
        public double Close { get; private set; }
        public double Volume { get; private set; }
        public bool IsAdjusted { get; private set; }

        public override bool Equals(object obj) => (obj is BarData bd) ? this == bd : false;

        public override int GetHashCode() => Source.GetHashCode() ^ Span.GetHashCode() ^ Time.GetHashCode();
        public static bool operator ==(BarData left, BarData right) => left.GetHashCode() == right.GetHashCode();
        public static bool operator !=(BarData left, BarData right) => left.GetHashCode() != right.GetHashCode();
    }
}
