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
    public class StrategyTrainingDatum : IDatum
    {
        public int Trades { get; }

        public double MaximumPosition { get; }

        public int WinCount { get; }

        public int LossCount { get; }

        public double WinRate
        {
            get
            {
                if (WinCount > 0)
                    return WinCount / (WinCount + LossCount);
                else
                    return 0;
            }
        }

        public double RiskRewardRatio { get; }



        public double Accumulation { get; private set; } = 0;


    }
}
