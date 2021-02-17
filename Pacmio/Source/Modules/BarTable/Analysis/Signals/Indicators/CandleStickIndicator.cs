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
    public class CandleStickIndicator : Indicator
    {
        protected override void Calculate(BarAnalysisPointer bap)
        {

        }

        #region CandleStick

        public static void CandleStickDojiMarubozuSignal(BarTable bt, int i, double doji_ratio = 0.12, double marubozu_ratio = 0.92)
        {
            Bar b = bt[i];
            double high = b.High;
            double low = b.Low;
            double hl_Length = Math.Abs(high - low);

            if (hl_Length > 0)
            {
                double open = b.Open;
                double close = b.Close;
                double oc_Length = Math.Abs(open - close);
                double body_shadow_ratio = oc_Length / hl_Length;

                if (body_shadow_ratio > marubozu_ratio) // Marubozu
                {
                    b.CandleStickTypes.Add(CandleStickType.Marubozu);
                }
                else if (body_shadow_ratio < doji_ratio)
                {
                    b.CandleStickTypes.Add(CandleStickType.Doji);

                    double avg_oc = (open + close) / 2;
                    double oc_position_ratio = (high - avg_oc) / hl_Length;

                    if (oc_position_ratio > 0.88)
                        b.CandleStickTypes.Add(CandleStickType.GravestoneDoji);
                    else if (oc_position_ratio < 0.12)
                        b.CandleStickTypes.Add(CandleStickType.DragonflyDoji);
                    else if (oc_position_ratio > 0.45 && oc_position_ratio < 0.55)
                        b.CandleStickTypes.Add(CandleStickType.LongLeggedDoji);
                }
            }
            else
            {
                b.CandleStickTypes.Add(CandleStickType.Doji);
            }
        }

        public static void CandleStickShadowStarSignal(BarTable bt, int i)
        {
            Bar b = bt[i];
            double high = b.High;
            double low = b.Low;
            double hl_Length = Math.Abs(high - low);

            NumericColumn column_trend = BarTable.TrueRangeAnalysis.Column_Typical;

            if (hl_Length > 0)
            {
                double open = b.Open;
                double close = b.Close;
                double oc_Length = Math.Abs(open - close);

                double body_shadow_ratio = oc_Length / hl_Length;

                if (body_shadow_ratio < 0.33)
                {
                    double trend_1 = (i > 0) ? bt[i - 1][column_trend] : 0;

                    double top_shadow = Math.Abs(high - Math.Max(open, close));
                    double buttom_shadow = Math.Abs(Math.Min(open, close) - low);

                    if (buttom_shadow > 0)
                    {
                        double shadow_ratio = top_shadow / buttom_shadow;
                        if (shadow_ratio > 0.9 && shadow_ratio < 1.1)
                        {
                            b.CandleStickTypes.Add(CandleStickType.SpinningTop);
                        }
                        else if (shadow_ratio < 0.2 && oc_Length / buttom_shadow < 0.3)
                        {
                            b.CandleStickTypes.Add(CandleStickType.LongButtomShadows);

                            if (trend_1 > 2)
                                b.CandleStickTypes.Add(CandleStickType.HangingMan);
                            else if (trend_1 < -2)
                                b.CandleStickTypes.Add(CandleStickType.Hammer);
                        }
                    }

                    if (top_shadow > 0)
                    {
                        double shadow_ratio = buttom_shadow / top_shadow;
                        if (shadow_ratio < 0.2 && oc_Length / top_shadow < 0.3)
                        {
                            b.CandleStickTypes.Add(CandleStickType.LongTopShadows);
                            if (trend_1 > 2)
                                b.CandleStickTypes.Add(CandleStickType.ShootingStar);
                            else if (trend_1 < -2)
                                b.CandleStickTypes.Add(CandleStickType.InvertedHammer);
                        }
                    }
                }
            }
        }

        #endregion CandleStick
    }
}
