/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using Xu;

namespace Pacmio
{
    public class SimulationResult
    {

        public double MaxTradeValue { get; set; } = 0;

        public double TradeCount { get; private set; } = 0;

        public double WinCount { get; private set; } = 0;

        public double LossCount { get; private set; } = 0;

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

        public double Accumulation { get; private set; } = 0;

        public double AverageGain => (TradeCount > 0) ? Accumulation / TradeCount : 0;

        public double WinAccumulation { get; private set; } = 0;

        public double AverageWin => (WinCount > 0) ? WinAccumulation / WinCount : 0;

        public double LossAccumulation { get; private set; } = 0;

        public double AverageLoss => (LossCount > 0) ? LossAccumulation / LossCount : 0;

        public double MaxSingleWin { get; private set; } = 0;

        public double MinSingleWin { get; private set; } = 0;

        public double MaxSingleLoss { get; private set; } = 0;

        public double MinSingleLoss { get; private set; } = 0;

        public double LongPnL { get; private set; } = 0;

        public double ShortPnL { get; private set; } = 0;

    }
}
