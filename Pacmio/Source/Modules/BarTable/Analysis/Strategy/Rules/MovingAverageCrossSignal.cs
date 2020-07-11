/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Collections.Generic;

namespace Pacmio
{
    public class MovingAverageCrossSignal : Strategy
    {
        public MovingAverageCrossSignal(string name) : base(name)
        {
            Name = name;
            SignalColumn = new SignalColumn(Name);

        }

        public void Config(MovingAverageType type_fast, int interval_fast, MovingAverageType type_slow, int interval_slow)
        {
            m_barAnalysisSet.Clear();

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
        }

        public SMA MA_Fast { get; private set; }

        public SMA MA_Slow { get; private set; }

        public SignalColumn SignalColumn { get; }

        public override BarAnalysisSet BarAnalysisSet(BarFreq _) => m_barAnalysisSet;

        private readonly BarAnalysisSet m_barAnalysisSet = new BarAnalysisSet();

        public void Tweak(IEnumerable<(MovingAverageType type_fast, int interval_fast, MovingAverageType type_slow, int interval_slow)> configs) // arg: Tweak Plan....
        {

        }

        public void Calculate(BarTable bt, int i)
        {


        }

        public void Evaluate(BarTable bt)
        {

        }
    }
}
