/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public class MovingAverageCrossStrategy
    {


        public void Config(BarFreq freq, MovingAverageType type_fast, int interval_fast, MovingAverageType type_slow, int interval_slow)
        {
            //BarAnalysisSet bas = new BarAnalysisSet();

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

            //bas.List = new List<BarAnalysis>() { MA_Fast, MA_Slow};


        }

        public SMA MA_Fast { get; private set; }

        public SMA MA_Slow { get; private set; }

        public SignalColumn SignalColumn { get; }




        // Make this part to research manager...
        public void Tweak(IEnumerable<(MovingAverageType type_fast, int interval_fast, MovingAverageType type_slow, int interval_slow)> configs) // arg: Tweak Plan....
        {

        }


    }

  
}
