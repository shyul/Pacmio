/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    public class MovingAverageCrossIndicator : DualDataIndicator
    {
        public MovingAverageCrossIndicator(MovingAverageType type_fast, int interval_fast, MovingAverageType type_slow, int interval_slow)
        {
            (Fast_MA, Slow_MA) = Config(type_fast, interval_fast, type_slow, interval_slow);
            Fast_Column = Fast_MA.Result_Column;
            Slow_Column = Slow_MA.Result_Column;

            string label = "(" + Fast_MA.Name + "," + Slow_MA.Name + ")";
            GroupName = Name = GetType().Name + label;

            SignalColumn = new SignalColumn(Name, label);
            SignalColumns = new SignalColumn[] { SignalColumn };
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Fast_MA.GetHashCode() ^ Slow_MA.GetHashCode();

        public SMA Fast_MA { get; }

        public SMA Slow_MA { get; }

        public static (SMA Fast, SMA Slow) Config(MovingAverageType type_fast, int interval_fast, MovingAverageType type_slow, int interval_slow)
        {
            SMA MA_Fast = null, MA_Slow = null;

            switch (type_fast)
            {
                case MovingAverageType.Simple:
                    MA_Fast = new SMA(interval_fast);
                    break;
                case MovingAverageType.Smoothed:
                    MA_Fast = new SMMA(interval_fast);
                    break;
                case MovingAverageType.Exponential:
                    MA_Fast = new EMA(interval_fast);
                    break;
                case MovingAverageType.Weighted:
                    MA_Fast = new WMA(interval_fast);
                    break;
                case MovingAverageType.Hull:
                    MA_Fast = new HMA(interval_fast);
                    break;
            }

            switch (type_slow)
            {
                case MovingAverageType.Simple:
                    MA_Slow = new SMA(interval_slow);
                    break;
                case MovingAverageType.Smoothed:
                    MA_Slow = new SMMA(interval_slow);
                    break;
                case MovingAverageType.Exponential:
                    MA_Slow = new EMA(interval_slow);
                    break;
                case MovingAverageType.Weighted:
                    MA_Slow = new WMA(interval_slow);
                    break;
                case MovingAverageType.Hull:
                    MA_Slow = new HMA(interval_slow);
                    break;
            }

            return (MA_Fast, MA_Slow);
        }
    }
}
