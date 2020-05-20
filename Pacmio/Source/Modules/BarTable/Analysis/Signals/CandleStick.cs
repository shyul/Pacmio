/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class CandleStick : BarAnalysis, ISignalAnalysis
    {
        public CandleStick()
        {
            GroupName = Name = GetType().Name;
            Description = "CandleStick Types";
            Signal_Column = new ObjectColumn(Name, typeof(CandleStickSignalDatum));
        }

        public override int GetHashCode() => GetType().GetHashCode();

        public ColorTheme BullishTheme { get; set; } = new ColorTheme(Color.DarkGreen, Color.Green);

        public ColorTheme BearishTheme { get; set; } = new ColorTheme(Color.DarkRed, Color.Red);

        public ObjectColumn Signal_Column { get; }

        public readonly Dictionary<CandleStickType, double> TypeToScore = new Dictionary<CandleStickType, double> 
        {
            { CandleStickType.Doji, 0 },
            { CandleStickType.GravestoneDoji, 0.5 },
            { CandleStickType.DragonflyDoji, 0.5 },
            { CandleStickType.LongLeggedDoji, 0 },
            { CandleStickType.Marubozu, 0.5 },
            { CandleStickType.HangingMan, 1 },
            { CandleStickType.Hammer, 1 },
            { CandleStickType.ShootingStar, 1 },
            { CandleStickType.InvertedHammer, 1 },
            { CandleStickType.LongBody, 0.25 },
            { CandleStickType.ThreeWhiteSoldiers, 0.5 },
            { CandleStickType.ThreeBlackCrows, 0.5 },
            { CandleStickType.StickSandwich, 0.5 },
            { CandleStickType.Harami, 0.25 },
            { CandleStickType.BearishEngulfing, 0.8 },
            { CandleStickType.DarkCloudCover, 0.7 },
            { CandleStickType.EveningAbandonedDoji, 1.25 },
            { CandleStickType.EveningDojiStar, 0.75 },
            { CandleStickType.EveningStar, 0.75 },
            { CandleStickType.UpsideGapTwoCrows, 0.5 },
            { CandleStickType.UpsideTasukiGap, 0.5 },
            { CandleStickType.BullishEngulfing, 0.8 },
            { CandleStickType.PiercingLine, 0.7 },
            { CandleStickType.MorningAbandonedDoji, 1.25 },
            { CandleStickType.MorningDojiStar, 0.75 },
            { CandleStickType.MorningStar, 0.75 },
            { CandleStickType.DownsideTasukiGap, 0.5 },
            { CandleStickType.FallingThreeMethods, 0.5 },
            { CandleStickType.RisingThreeMethods, 0.5 },
        };

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            double open_1, high_1, low_1, close_1, gain_1, trend_1;

            // Define the bondary condition
            if (bap.StartPt < 1)
            {
                if (bap.StartPt < 0) bap.StartPt = 0;
                open_1 = bt[0].Open;
                high_1 = bt[0].High;
                low_1 = bt[0].Low;
                close_1 = bt[0].Close;
                gain_1 = 0;
                trend_1 = 0;
            }
            else
            {
                Bar b_1 = bt[bap.StartPt - 1];

                open_1 = b_1.Open;
                high_1 = b_1.High;
                low_1 = b_1.Low;
                close_1 = b_1.Close;
                gain_1 = b_1.Gain;
                trend_1 = b_1.TrendStrength;
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                b.CandleStickTypes.Clear();

                CandleStickSignalDatum dsd = new CandleStickSignalDatum();
                b[Signal_Column] = dsd;

                double open = b.Open;
                double high = b.High;
                double low = b.Low;
                double close = b.Close;
                double gain = b.Gain;
                double trend = b.TrendStrength;

                double oc_Length = dsd.CandleLength = Math.Abs(open - close);
                double hl_Length = dsd.CandleShadowLength = Math.Abs(high - low);


                if (hl_Length > 0)
                {
                    double top_shadow = Math.Abs(high - Math.Max(open, close));
                    double buttom_shadow = Math.Abs(Math.Min(open, close) - low);

                    if (oc_Length / hl_Length < 0.12) // Doji Condition
                    {
                        b.CandleStickTypes.Add(CandleStickType.Doji);

                        double avg_oc = (open + close) / 2;

                        double oc_position_ratio = (high - avg_oc) / hl_Length;
                        //double oc_position_ratio = high - close / high -low;
                        //Console.WriteLine("high = " + high.ToString("0.##") + "; low = " + low.ToString("0.##") + "; close = " + close.ToString("0.##") + "; ratio = " + oc_position_ratio.ToString("0.##"));

                        if (oc_position_ratio > 0.88)
                        {
                            b.CandleStickTypes.Add(CandleStickType.GravestoneDoji);
                            dsd.BearishScore += TypeToScore[CandleStickType.GravestoneDoji];
                        }
                        else if (oc_position_ratio < 0.12)
                        {
                            b.CandleStickTypes.Add(CandleStickType.DragonflyDoji);
                            dsd.BullishScore += TypeToScore[CandleStickType.DragonflyDoji];
                        }
                        else if (oc_position_ratio > 0.45 && oc_position_ratio < 0.55)
                            b.CandleStickTypes.Add(CandleStickType.LongLeggedDoji);
                    }
                    else
                    {
                        double body_shadow_ratio = oc_Length / hl_Length;
                        if (body_shadow_ratio > 0.92) // Marubozu
                        {
                            b.CandleStickTypes.Add(CandleStickType.Marubozu);
                            if (close > open)
                                dsd.BullishScore += TypeToScore[CandleStickType.Marubozu];
                            else
                                dsd.BearishScore += TypeToScore[CandleStickType.Marubozu];
                        }
                        else if (body_shadow_ratio < 0.33) // Small candle body
                        {
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
                                    {
                                        b.CandleStickTypes.Add(CandleStickType.HangingMan);
                                        dsd.BearishScore += TypeToScore[CandleStickType.HangingMan];
                                        //Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStick.ToString(", "));
                                    }
                                    else if (trend_1 < -2)
                                    {
                                        b.CandleStickTypes.Add(CandleStickType.Hammer);
                                        dsd.BullishScore += TypeToScore[CandleStickType.Hammer];
                                        //Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStick.ToString(", "));
                                    }

                                }
                            }

                            if (top_shadow > 0)
                            {
                                double shadow_ratio = buttom_shadow / top_shadow;
                                if (shadow_ratio < 0.2 && oc_Length / top_shadow < 0.3)
                                {
                                    b.CandleStickTypes.Add(CandleStickType.LongTopShadows);
                                    if (trend_1 > 2)
                                    {
                                        b.CandleStickTypes.Add(CandleStickType.ShootingStar);
                                        dsd.BearishScore += TypeToScore[CandleStickType.ShootingStar];
                                        //Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStick.ToString(", "));
                                    }
                                    else if (trend_1 < -2)
                                    {
                                        b.CandleStickTypes.Add(CandleStickType.InvertedHammer);
                                        dsd.BullishScore += TypeToScore[CandleStickType.InvertedHammer];
                                        //Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStick.ToString(", "));
                                    }
                                }
                            }
                        }
                    }

                    //int avgLength = (i > 8) ? 8 : i; // i is already assured to > 0

                    if (i > 6) // At least three bars, max 8 bars
                    {
                        var bars = bt[i - 1, 8];// Rows.Skip(i - avgLength).Take(avgLength); // bar_0, bar_1, bar_2, bar_3, bar_4, bar_5 .. bar_8
                        double avg_candle_length = bars.Take(bars.Count() - 1).Select(n => ((CandleStickSignalDatum)n[Signal_Column]).CandleLength).Sum() / (bars.Count() - 1);// avgLength;

                        if (oc_Length > avg_candle_length * 1.5)
                        {
                            b.CandleStickTypes.Add(CandleStickType.LongBody);
                            if (close > open)
                                dsd.BullishScore += TypeToScore[CandleStickType.LongBody];
                            else
                                dsd.BearishScore += TypeToScore[CandleStickType.LongBody];
                        }
                        else if (oc_Length < avg_candle_length / 1.5)
                        {
                            b.CandleStickTypes.Add(CandleStickType.ShortBody);

                            if (high < low_1 || low > high_1)
                            {
                                b.CandleStickTypes.Add(CandleStickType.Star);
                                //Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStick.ToString(", "));
                            }
                        }

                        Bar b_1 = bt[i - 1];
                        CandleStickSignalDatum dsd_1 = (CandleStickSignalDatum)b_1[Signal_Column];

                        Bar b_2 = bt[i - 2];
                        CandleStickSignalDatum dsd_2 = (CandleStickSignalDatum)b_2[Signal_Column];
                        double open_2 = b_2.Open;
                        double high_2 = b_2.High;
                        double low_2 = b_2.Low;
                        double close_2 = b_2.Close;
                        double gain_2 = b_2.Gain;
                        double trend_2 = b_2.TrendStrength;

                        if (b.CandleStickTypes.Contains(CandleStickType.LongBody) &&
                            b_2.CandleStickTypes.Contains(CandleStickType.LongBody) &&
                            !b_2.CandleStickTypes.Contains(CandleStickType.ShortBody))
                        {
                            if (b.Gain > 0 && b_1.Gain > 0 && b_2.Gain > 0 && close > open && b_1.Close > b_1.Open && b_2.Close > b_2.Open)
                            {
                                b.CandleStickTypes.Add(CandleStickType.ThreeWhiteSoldiers);
                                dsd.BullishScore += TypeToScore[CandleStickType.ThreeWhiteSoldiers];
                                //Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStick.ToString(", "));
                            }
                            else if (b.Gain < 0 && b_1.Gain < 0 && b_2.Gain < 0 && close < open && b_1.Close < b_1.Open && b_2.Close < b_2.Open)
                            {
                                b.CandleStickTypes.Add(CandleStickType.ThreeBlackCrows);
                                dsd.BearishScore += TypeToScore[CandleStickType.ThreeBlackCrows];
                                //Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStick.ToString(", "));
                            }

                        }

                        if (gain < 0 && close < open) // The Black Body
                        {
                            if (gain_1 > 0 && close_1 > open_1) // The Previous Candle is White Body (Opposite)
                            {
                                if (gain_2 < 0 && close_2 < open_2 && low_1 > low && low_1 > low_2 && trend_2 < -2)
                                {
                                    double h3_range = Math.Max(Math.Max(high, high_1), high_2);
                                    double l3_range = Math.Min(Math.Min(low, low_1), low_2);
                                    double close_ratio = Math.Abs((close - close_2) / (h3_range - l3_range));
                                    //double close_ratio = Math.Abs((low - low_2) / (h3_range - l3_range));

                                    //double buttom_shadow_ratio = (buttom_shadow / hl_Length);
                                    //double buttom_shadow_2 = Math.Abs(Math.Min(open_2, close_2) - low_2);
                                    //double buttom_shadow_ratio_2 = (buttom_shadow_2 / Math.Abs(high_2 - low_2));

                                    if (close_ratio < 0.05)// && buttom_shadow_ratio < 0.15 && buttom_shadow_ratio_2 < 0.15)
                                    {
                                        b.CandleStickTypes.Add(CandleStickType.StickSandwich);
                                        dsd.BullishScore += TypeToScore[CandleStickType.StickSandwich];
                                        //Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStick.ToString(", "));
                                    }
                                }

                                if (high < high_1 && low > low_1) // Harami
                                {
                                    b.CandleStickTypes.Add(CandleStickType.Harami); // For Harami Cross: check with Doji
                                    if (close > open)
                                        dsd.BullishScore += TypeToScore[CandleStickType.Harami];
                                    else
                                        dsd.BearishScore += TypeToScore[CandleStickType.Harami];
                                }

                                if (trend_2 > 2) // Up Trend
                                {
                                    if (high > high_1) // New High
                                    {
                                        if (low < low_1) // Engulfing
                                        {
                                            b.CandleStickTypes.Add(CandleStickType.BearishEngulfing);
                                            dsd.BearishScore += TypeToScore[CandleStickType.BearishEngulfing];
                                        }

                                        if (b_1.CandleStickTypes.Contains(CandleStickType.LongBody)) // Long 
                                        {
                                            if (close < ((open_1 + close_1) / 2)) // Cross below the middle point
                                            {
                                                b.CandleStickTypes.Add(CandleStickType.DarkCloudCover);
                                                dsd.BearishScore += TypeToScore[CandleStickType.DarkCloudCover];
                                                //Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStick.ToString(", "));
                                            }
                                        }
                                    }
                                }
                            }

                            if (trend_2 > 2 && low_1 > high_2)
                            {
                                if (close < ((close_2 + open_2) / 2) && !b.CandleStickTypes.Contains(CandleStickType.ShortBody))
                                {
                                    if (b_1.CandleStickTypes.Contains(CandleStickType.Doji))
                                    {
                                        if (low_1 > high)
                                        {
                                            b.CandleStickTypes.Add(CandleStickType.EveningAbandonedDoji);
                                            dsd.BearishScore += TypeToScore[CandleStickType.EveningAbandonedDoji];
                                        }
                                        else
                                        {
                                            b.CandleStickTypes.Add(CandleStickType.EveningDojiStar);
                                            dsd.BearishScore += TypeToScore[CandleStickType.EveningDojiStar];
                                        }
                                    }
                                    else if (!b_1.CandleStickTypes.Contains(CandleStickType.LongBody))
                                    {
                                        b.CandleStickTypes.Add(CandleStickType.EveningStar);
                                        dsd.BearishScore += TypeToScore[CandleStickType.EveningStar];
                                    }
                                    //Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStick.ToString(", "));
                                }

                                if (gain_2 > 0 && close_2 > open_2) // The first candle has white body
                                {
                                    if (!b_2.CandleStickTypes.Contains(CandleStickType.ShortBody) &&
                                        gain_1 < 0 && close_1 < open_1 && // Followed by a black candle
                                        close < close_1 && open > open_1 && // Engulfing
                                        close > high_2) // The close of the last day is still above the first long white day.
                                    {
                                        b.CandleStickTypes.Add(CandleStickType.UpsideGapTwoCrows); // Never Verified
                                        dsd.BearishScore += TypeToScore[CandleStickType.UpsideGapTwoCrows];
                                        Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStickTypes.ToString(", ")); // Never Verified
                                    }

                                    if (gain_1 > 0 && close_1 > open_1 && // First Two White Body, and gap is defined by upper stream condition
                                        open > open_1 && open < close_1 && // opens within the body of the second day
                                        close > high_2 && close < low_1)
                                    {
                                        b.CandleStickTypes.Add(CandleStickType.UpsideTasukiGap);
                                        dsd.BullishScore += TypeToScore[CandleStickType.UpsideTasukiGap];
                                        //Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStick.ToString(", "));
                                    }
                                }
                            }
                        }
                        else if (gain > 0 && close > open) // The White Body
                        {
                            if (gain_1 < 0 && close_1 < open_1) // The Previous Candle is Black Body (Opposite)
                            {
                                if (high < high_1 && low > low_1) // Harami
                                {
                                    b.CandleStickTypes.Add(CandleStickType.Harami); // For Harami Cross: check with Doji
                                }

                                if (trend_2 < -2) // Down trend is forming
                                {
                                    if (low < low_1) // New Low
                                    {
                                        if (high > high_1) // Engulfing
                                        {
                                            b.CandleStickTypes.Add(CandleStickType.BullishEngulfing);
                                            dsd.BullishScore += TypeToScore[CandleStickType.BullishEngulfing];
                                        }

                                        if (b_1.CandleStickTypes.Contains(CandleStickType.LongBody)) // Long 
                                        {
                                            if (close > ((open_1 + close_1) / 2)) // Cross up the middle point
                                            {
                                                b.CandleStickTypes.Add(CandleStickType.PiercingLine);
                                                dsd.BullishScore += TypeToScore[CandleStickType.PiercingLine];
                                                //Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStick.ToString(", "));
                                            }
                                        }
                                    }
                                }
                            }

                            if (trend_2 < -2 && high_1 < low_2)
                            {
                                if (close > ((close_2 + open_2) / 2) && !b.CandleStickTypes.Contains(CandleStickType.ShortBody)) // high_1 < low && 
                                {
                                    if (b_1.CandleStickTypes.Contains(CandleStickType.Doji))
                                    {
                                        if (high_1 < low)
                                        {
                                            b.CandleStickTypes.Add(CandleStickType.MorningAbandonedDoji);
                                            dsd.BullishScore += TypeToScore[CandleStickType.MorningAbandonedDoji];
                                        }
                                        else
                                        {
                                            b.CandleStickTypes.Add(CandleStickType.MorningDojiStar);
                                            dsd.BullishScore += TypeToScore[CandleStickType.MorningDojiStar];
                                        }
                                    }
                                    else if (!b_1.CandleStickTypes.Contains(CandleStickType.LongBody))
                                    {
                                        b.CandleStickTypes.Add(CandleStickType.MorningStar);
                                        dsd.BullishScore += TypeToScore[CandleStickType.MorningStar];
                                    }
                                    //Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStick.ToString(", "));
                                }

                                if (gain_1 < 0 && close_1 < open_1 &&
                                    gain_2 < 0 && close_2 < open_2 && // First Two Black Body, and gap is defined by upper stream condition
                                    open < open_1 && open > close_1 && // opens within the body of the second day
                                    close < low_2 && close > high_1)
                                {
                                    b.CandleStickTypes.Add(CandleStickType.DownsideTasukiGap);
                                    dsd.BearishScore += TypeToScore[CandleStickType.DownsideTasukiGap];
                                    Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStickTypes.ToString(", "));
                                }
                            }
                        }

                        Bar b_3 = bt[i - 3];
                        CandleStickSignalDatum dsd_3 = (CandleStickSignalDatum)b_3[Signal_Column];
                        double open_3 = b_3.Open;
                        double high_3 = b_3.High;
                        double low_3 = b_3.Low;
                        double close_3 = b_3.Close;
                        double gain_3 = b_3.Gain;
                        double trend_3 = b_3.TrendStrength;

                        Bar b_4 = bt[i - 4];
                        CandleStickSignalDatum dsd_4 = (CandleStickSignalDatum)b_4[Signal_Column];
                        double open_4 = b_4.Open;
                        double high_4 = b_4.High;
                        double low_4 = b_4.Low;
                        double close_4 = b_4.Close;
                        double gain_4 = b_4.Gain;
                        double trend_4 = b_4.TrendStrength;

                        if (!b.CandleStickTypes.Contains(CandleStickType.ShortBody) &&
                            b_3.CandleStickTypes.Contains(CandleStickType.ShortBody) &&
                            b_2.CandleStickTypes.Contains(CandleStickType.ShortBody) &&
                            b_1.CandleStickTypes.Contains(CandleStickType.ShortBody) &&
                            !b_4.CandleStickTypes.Contains(CandleStickType.ShortBody))
                        {
                            List<double> num = new List<double>() { open_1, close_1, open_2, close_2, open_3, close_3 };

                            double h_3 = num.Max(); // Math.Max(high_1, Math.Max(high_2, high_3));
                            double l_3 = num.Min(); // Math.Min(low_1, Math.Min(low_2, low_3));

                            if (gain < 0 && close < open &&  // Not short Black body as first
                                gain_4 < 0 && close_4 < open_4 && // Not short Black body number 4
                                close_4 < l_3 && open_4 > h_3 &&
                                close < low_4) // New Low
                            {
                                b.CandleStickTypes.Add(CandleStickType.FallingThreeMethods);
                                dsd.BearishScore += TypeToScore[CandleStickType.FallingThreeMethods];
                                Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStickTypes.ToString(", "));
                            }
                            else if (gain > 0 && close > open &&
                                     gain_4 > 0 && close_4 > open_4 &&
                                     close_4 > h_3 && open_4 < l_3 &&
                                     close > high_4)
                            {
                                b.CandleStickTypes.Add(CandleStickType.RisingThreeMethods);
                                dsd.BullishScore += TypeToScore[CandleStickType.RisingThreeMethods];
                                Console.WriteLine("## Time = " + b.Time + " Trend = " + trend + "; Candlestick: " + b.CandleStickTypes.ToString(", "));
                            }
                        }
                    }
                }
                else
                {
                    b.CandleStickTypes.Add(CandleStickType.Doji);
                }

                open_1 = open;
                high_1 = high;
                low_1 = low;
                close_1 = close;
                gain_1 = gain;
                trend_1 = trend;
            }
        }

    }
}