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
    public class CandleStickDojiMarubozuSignal : BarAnalysis
    {
        public CandleStickDojiMarubozuSignal(double doji_ratio = 0.12, double marubozu_ratio = 0.92)
        {
            Doji_Ratio = doji_ratio;
            Marubozu_Ratio = marubozu_ratio;
            Name = GetType().Name + Doji_Ratio + Marubozu_Ratio;
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Doji_Ratio.GetHashCode() ^ Marubozu_Ratio.GetHashCode();

        double Doji_Ratio { get; } = 0.12;

        double Marubozu_Ratio { get; } = 0.92;

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double high = b.High;
                double hl_Range = b.Range;

                if (hl_Range > 0)
                {
                    double open = b.Open;
                    double close = b.Close;
                    double oc_Range = Math.Abs(open - close);
                    double body_shadow_ratio = oc_Range / hl_Range;

                    if (body_shadow_ratio > Marubozu_Ratio) // Marubozu
                    {
                        b.CandleStickList.Add(CandleStickType.Marubozu);
                    }
                    else if (body_shadow_ratio < Doji_Ratio)
                    {
                        b.CandleStickList.Add(CandleStickType.Doji);

                        double avg_oc = (open + close) / 2;
                        double oc_position_ratio = (high - avg_oc) / hl_Range;

                        if (oc_position_ratio > 0.88)
                            b.CandleStickList.Add(CandleStickType.GravestoneDoji);
                        else if (oc_position_ratio < 0.12)
                            b.CandleStickList.Add(CandleStickType.DragonflyDoji);
                        else if (oc_position_ratio > 0.45 && oc_position_ratio < 0.55)
                            b.CandleStickList.Add(CandleStickType.LongLeggedDoji);
                    }
                }
                else
                {
                    b.CandleStickList.Add(CandleStickType.Doji);
                }

            }
        }
    }
}
