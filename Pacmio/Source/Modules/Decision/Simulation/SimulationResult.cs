/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Pacmio
{
    public class SimulationResult
    {
        public void AddTrade(TradeInfo tld)
        {
            // if (!TradeLog.ContainsKey(tld.ExecuteTime)) 
            TradeLog.Add(tld.ExecuteTime, tld);

            if (tld.RealizedPnL != 0)
            {
                double m_PnL = tld.RealizedPnL;
                TradeCount++;
                Accumulation += m_PnL;

                if (m_PnL > 0)
                {
                    WinCount++;
                    WinAccumulation += m_PnL;
                    MaxSingleWin = (MaxSingleWin == 0) ? m_PnL : Math.Max(MaxSingleWin, m_PnL);
                    MinSingleWin = (MinSingleWin == 0) ? m_PnL : Math.Min(MinSingleWin, m_PnL);
                }
                else if (m_PnL < 0)
                {
                    LossCount++;
                    LossAccumulation += m_PnL;
                    MaxSingleLoss = (MaxSingleLoss == 0) ? m_PnL : Math.Min(MaxSingleLoss, m_PnL);
                    MinSingleLoss = (MinSingleLoss == 0) ? m_PnL : Math.Max(MinSingleLoss, m_PnL);
                }

                if (tld.Quantity > 0)
                {
                    ShortPnL += m_PnL;
                }
                else
                {
                    LongPnL += m_PnL;
                }
            }
        }

        public void Reset()
        {


        }

        private readonly SortedDictionary<DateTime, TradeInfo> TradeLog = new();

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
