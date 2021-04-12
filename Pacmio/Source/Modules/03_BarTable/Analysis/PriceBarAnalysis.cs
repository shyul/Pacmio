/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
///  
/// ***************************************************************************

using System;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio
{
    public sealed class PriceBarAnalysis : BarAnalysis, ISingleData, IDualData
    {
        public PriceBarAnalysis()
        {
            GroupName = Name = GetType().Name;
            Description = "Price";
        }

        public override int GetHashCode() => GetType().GetHashCode();

        public NumericColumn Column_Result => Bar.Column_Close;

        public NumericColumn Column_High => Bar.Column_High;

        public NumericColumn Column_Low => Bar.Column_Low;

        public Color UpperColor => Color.Green;

        public Color LowerColor => Color.Red;

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            Bar b = bap.StartPt < 1 ? bt[0] : bt[bap.StartPt - 1];

            double high_1 = b.High;
            double low_1 = b.Low;
            double close_1 = b.Close;
            int trend_1 = b.TrendStrength;
            double range_1 = b.Range;
            int nr = 0;

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
                double body = b.Body = Math.Abs(close - open);
                b.BodyRatio = range == 0 ? 0 : (body / range);

                b.Type = BarType.None;

                if (gain > 0)
                {
                    if (close > open)
                    {
                        b.Type = BarType.White;
                    }
                    else if (open > close)
                    {
                        b.Type = BarType.Black;
                    }
                }
                else if (gain < 0)
                {
                    if (close > open)
                    {
                        b.Type = BarType.HollowRed;
                    }
                    else if (open > close)
                    {
                        b.Type = BarType.Red;
                    }
                }

                if (range < range_1)
                {
                    nr++;
                }
                else if (range > range_1)
                {
                    nr = 0;
                }

                b.NarrowRange = nr;

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
