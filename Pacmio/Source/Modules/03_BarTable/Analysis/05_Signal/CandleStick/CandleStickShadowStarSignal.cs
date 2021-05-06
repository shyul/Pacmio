/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public class CandleStickShadowStarSignal : SignalAnalysis
    {
        public CandleStickShadowStarSignal(TimePeriod tif, BarFreq barFreq, PriceType priceType = PriceType.Trades)
            : base(tif, barFreq, priceType)
        {

            Label = "(" + "," + "," + barFreq + "," + priceType + ")";
            Name = GetType().Name + Label;

            Column_Result = new(this, typeof(CandleStickSignalDatum));
        }

        public Dictionary<CandleStickType, double[]> TypeToTrailPoints { get; set; } = new()
        {
            { CandleStickType.SpinningTop, new double[] { 0 } },
            { CandleStickType.LongButtomShadows, new double[] { 0 } },
            { CandleStickType.HangingMan, new double[] { 0 } },
            { CandleStickType.Hammer, new double[] { 0 } },
            { CandleStickType.LongTopShadows, new double[] { 0 } },
            { CandleStickType.ShootingStar, new double[] { 0 } },
            { CandleStickType.InvertedHammer, new double[] { 0 } },
        };

        public void AddType(CandleStickSignalDatum d, CandleStickType type)
        {
            if (!d.List.Contains(type))
            {
                d.List.Add(type);

                if (TypeToTrailPoints.ContainsKey(type))
                    d.SetPoints(TypeToTrailPoints[type]);
            }
        }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                if (BarFreq >= BarFreq.Daily || TimeInForce.Contains(b.Time))
                {
                    double high = b.High;
                    double low = b.Low;
                    double hl_Range = Math.Abs(high - low);

                    if (hl_Range > 0)
                    {
                        CandleStickSignalDatum d = new(b, Column_Result);

                        double open = b.Open;
                        double close = b.Close;
                        double oc_Range = b.Body;

                        double body_shadow_ratio = b.BodyRatio;

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
                                    AddType(d, CandleStickType.SpinningTop);
                                }
                                else if (shadow_ratio < 0.2 && oc_Range / buttom_shadow < 0.3)
                                {
                                    AddType(d, CandleStickType.LongButtomShadows);

                                    if (trend_1 > 2)
                                        AddType(d, CandleStickType.HangingMan);
                                    else if (trend_1 < -2)
                                        AddType(d, CandleStickType.Hammer);
                                }
                            }

                            if (top_shadow > 0)
                            {
                                double shadow_ratio = buttom_shadow / top_shadow;
                                if (shadow_ratio < 0.2 && oc_Range / top_shadow < 0.3)
                                {
                                    AddType(d, CandleStickType.LongTopShadows);
                                    if (trend_1 > 2)
                                        AddType(d, CandleStickType.ShootingStar);
                                    else if (trend_1 < -2)
                                        AddType(d, CandleStickType.InvertedHammer);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
