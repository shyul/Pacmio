/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// 1. Gain
/// 2. Typical Price
/// 3. True Range
/// 4. Gap
/// 5. Narrow Range: https://school.stockcharts.com/doku.php?id=trading_strategies:narrow_range_day_nr7
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
            int trend_1 = b.TrendStrength;
            double range_1 = b.Range;
            int nr_1 = 0;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                b = bt[i];
                double open = b.Open;
                double high = b.High;
                double low = b.Low;
                double close = b.Close;

                double gain = b.Gain = close - close_1;
                b.GainPercent = (close_1 == 0) ? 0 : (100 * gain / close_1);
                double range = b.Range = high - low;

                b.BarType = BarType.None;

                if (gain > 0)
                {
                    if (close > open)
                    {
                        b.BarType = BarType.White;
                    }
                    else if (open < close)
                    {
                        b.BarType = BarType.Black;
                    }
                }
                else if (gain < 0)
                {
                    if (close > open)
                    {
                        b.BarType = BarType.HollowRed;
                    }
                    else if (open < close)
                    {
                        b.BarType = BarType.Red;
                    }
                }

                if (range < range_1)
                {
                    nr_1++;
                }
                else if (range > range_1)
                {
                    nr_1 = 0;
                }

                b.NarrowRange = nr_1;

                double[] list = new double[] { range, Math.Abs(high - close_1), Math.Abs(low - close_1) };
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

                int trend = 0;
                if (close > close_1 || (high > high_1 && low > low_1))
                {
                    trend = (trend_1 > 0) ? trend_1 + 1 : 1;
                }
                else if (close < close_1 || (high < high_1 && low < low_1))
                {
                    trend = (trend_1 < 0) ? trend_1 - 1 : -1;
                }
                b.TrendStrength = trend;

                high_1 = high;
                low_1 = low;
                close_1 = close;
                trend_1 = trend;
                range_1 = range;
            }
        }
    }
}
