/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// 1. Gain
/// 2. Typical Price
/// 3. True Range
/// 4. Gap
/// 
/// ***************************************************************************

using System;
using System.Linq;

namespace Pacmio.Analysis
{
    public sealed class NativeGainAnalysis : BarAnalysis
    {
        public override int GetHashCode() => GetType().GetHashCode();

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            Bar b = bap.StartPt < 1 ? bt[0] : bt[bap.StartPt - 1];

            double high_1 = b.High;
            double low_1 = b.Low;
            double close_1 = b.Close;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                b = bt[i];
                double open = b.Open;
                double high = b.High;
                double low = b.Low;
                double close = b.Close;

                b.Gain = close - close_1;
                b.GainPercent = (close_1 == 0) ? 0 : (100 * b.Gain / close_1);

                double[] list = new double[] { high - low, Math.Abs(high - close_1), Math.Abs(low - close_1) };
                b.TrueRange = list.Max();
                b.Typical = (high + low + close) / 3.0;

                if (i > 0)
                {
                    if (open > high_1)
                    {
                        b.Gap = open - high_1;
                        b.GapPercent = 100 * b.Gap / high_1;
                    }
                    else if (open < low_1)
                    {
                        b.Gap = open - low_1;
                        b.GapPercent = 100 * b.Gap / low_1;
                    }
                    else 
                    {
                        b.Gap = b.GapPercent = 0;
                    }
                }
                else
                {
                    b.Gap = b.GapPercent = 0;
                }

                high_1 = high;
                low_1 = low;
                close_1 = close;
            }
        }
    }
}
