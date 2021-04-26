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
    public class CandleStickDojiMarubozuSignal : SignalAnalysis
    {
        public CandleStickDojiMarubozuSignal(BarFreq barFreq, double doji_ratio = 0.12, double marubozu_ratio = 0.92, PriceType priceType = PriceType.Trades) : base(barFreq, priceType)
        {
            Doji_Ratio = doji_ratio;
            Marubozu_Ratio = marubozu_ratio;
            Name = GetType().Name + Doji_Ratio + Marubozu_Ratio;

            Column_Result = new(this, typeof(CandleStickSignalDatum));
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Doji_Ratio.GetHashCode() ^ Marubozu_Ratio.GetHashCode();

        double Doji_Ratio { get; } = 0.12;

        double Marubozu_Ratio { get; } = 0.92;

        public Dictionary<CandleStickType, double[]> TypeToTrailPoints { get; set; } = new()
        {
            { CandleStickType.Marubozu, new double[] { 0 } },
            { CandleStickType.Doji, new double[] { 0 } },
            { CandleStickType.GravestoneDoji, new double[] { 0 } },
            { CandleStickType.DragonflyDoji, new double[] { 0 } },
            { CandleStickType.LongLeggedDoji, new double[] { 0 } },
        };

        public void AddType(CandleStickSignalDatum d, CandleStickType type)
        {
            if (!d.List.Contains(type))
            {
                d.List.Add(type);
                d.SetPoints(TypeToTrailPoints[type]);
            }
        }

        public override SignalColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                CandleStickSignalDatum d = new(b, Column_Result);

                double high = b.High;
                double hl_Range = b.Range;

                if (hl_Range > 0)
                {
                    double open = b.Open;
                    double close = b.Close;
                    double body_shadow_ratio = b.BodyRatio;

                    if (body_shadow_ratio > Marubozu_Ratio) // Marubozu
                    {
                        AddType(d, CandleStickType.Marubozu);
                    }
                    else if (body_shadow_ratio < Doji_Ratio)
                    {
                        AddType(d, CandleStickType.Doji);

                        double avg_oc = (open + close) / 2;
                        double oc_position_ratio = (high - avg_oc) / hl_Range;

                        if (oc_position_ratio > 0.88)
                            AddType(d, CandleStickType.GravestoneDoji);
                        else if (oc_position_ratio < 0.12)
                            AddType(d, CandleStickType.DragonflyDoji);
                        else if (oc_position_ratio > 0.45 && oc_position_ratio < 0.55)
                            AddType(d, CandleStickType.LongLeggedDoji);
                    }
                }
                else
                {
                    AddType(d, CandleStickType.Doji);
                }

            }
        }
    }
}
