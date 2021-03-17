/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public class CandleStickShadowStarAnalysis : BarAnalysis
    {
        public CandleStickShadowStarAnalysis()
        {
            Name = GetType().Name;
        }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double high = b.High;
                double low = b.Low;
                double hl_Range = Math.Abs(high - low);

                if (hl_Range > 0)
                {
                    double open = b.Open;
                    double close = b.Close;
                    double oc_Range = Math.Abs(open - close);

                    double body_shadow_ratio = oc_Range / hl_Range;

                    if (body_shadow_ratio < 0.33)
                    {
                        double trend_1 = (i > 0) ? bt[i - 1].TrendStrength : 0;

                        double top_shadow = Math.Abs(high - Math.Max(open, close));
                        double buttom_shadow = Math.Abs(Math.Min(open, close) - low);

                        if (buttom_shadow > 0)
                        {
                            double shadow_ratio = top_shadow / buttom_shadow;
                            if (shadow_ratio > 0.9 && shadow_ratio < 1.1)
                            {
                                b.CandleStickList.Add(CandleStickType.SpinningTop);
                            }
                            else if (shadow_ratio < 0.2 && oc_Range / buttom_shadow < 0.3)
                            {
                                b.CandleStickList.Add(CandleStickType.LongButtomShadows);

                                if (trend_1 > 2)
                                    b.CandleStickList.Add(CandleStickType.HangingMan);
                                else if (trend_1 < -2)
                                    b.CandleStickList.Add(CandleStickType.Hammer);
                            }
                        }

                        if (top_shadow > 0)
                        {
                            double shadow_ratio = buttom_shadow / top_shadow;
                            if (shadow_ratio < 0.2 && oc_Range / top_shadow < 0.3)
                            {
                                b.CandleStickList.Add(CandleStickType.LongTopShadows);
                                if (trend_1 > 2)
                                    b.CandleStickList.Add(CandleStickType.ShootingStar);
                                else if (trend_1 < -2)
                                    b.CandleStickList.Add(CandleStickType.InvertedHammer);
                            }
                        }
                    }
                }

            }
        }
    }
}
