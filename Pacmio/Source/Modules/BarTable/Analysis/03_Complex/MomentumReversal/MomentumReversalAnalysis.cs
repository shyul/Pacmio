/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class MomentumReversalAnalysis : BarAnalysis
    {
        public MomentumReversalAnalysis(ILevelAnalysis analysis, double tolerance = 0.15)
        {
            LevelAnalysis = analysis;
            ReversalRangeTolerancePercent = tolerance;
            Name = GetType().Name + "_" + analysis.Name + "_" + ReversalRangeTolerancePercent;

            Column_Momentum = new NumericColumn(Name + "_Momentum", "Momentum");
            Column_Reversal = new NumericColumn(Name + "_Reversal", "Reversal");

            analysis.AddChild(this);
        }

        /// <summary>
        /// Percent of High to Low range. eg: 5% means 1 + 0.05 + 0.05 = 1.1 of HL
        /// </summary>
        public double ReversalRangeTolerancePercent { get; }

        public ILevelAnalysis LevelAnalysis { get; }

        public NumericColumn Column_Momentum { get; }

        public NumericColumn Column_Reversal { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                if (b[LevelAnalysis] is ILevelDatum ild) 
                {
                    List<Level> levels = ild.Levels;
                    double open = b.Open;
                    double close = b.Close;
                    List<double> bar = (new double[] { open, b.High, b.Low, close }).OrderBy(n => n).ToList();

                    Range<double> body_range = new(bar[1], bar[2]);

                    double tolerance = (b.High - b.Low) * ReversalRangeTolerancePercent;

                    double high = b.High + tolerance;
                    double low = b.Low - tolerance;

                    Range<double> lower_wick_range = new(low, bar[1]);
                    Range<double> upper_wick_range = new(bar[2], high);

                    double momo = levels.Where(n => body_range.Contains(n.LevelValue)).Select(n => Math.Abs(n.Strength)).Sum();

                    if (close > open)
                        b[Column_Momentum] = momo;
                    else if (close < open)
                        b[Column_Momentum] = -momo;
                    else
                        b[Column_Momentum] = 0;

                    double pos_rev = levels.Where(n => lower_wick_range.Contains(n.LevelValue)).Select(n => Math.Abs(n.Strength)).Sum();
                    double neg_rev = levels.Where(n => upper_wick_range.Contains(n.LevelValue)).Select(n => Math.Abs(n.Strength)).Sum();

                    b[Column_Reversal] = pos_rev - neg_rev;
                }
            }
        }
    }

    /*
    public class MomentumReversalAnalysis : BarAnalysis
    {
        public MomentumReversalAnalysis(string name, IEnumerable<ILevelAnalysis> list)
        {
            Name = GetType().Name + "_" + name;



            list.Where(n => n.AreaName == MainBarChartArea.DefaultName).RunEach(n => { LevelAnalyses.CheckAdd(n); n.AddChild(this); });
            Columns = LevelAnalyses.Select(n => n.Column_Result).ToList();
        }

        public List<ILevelAnalysis> LevelAnalyses { get; } = new();

        public List<DatumColumn> Columns { get; }

        public NumericColumn Column_Momentum { get; }

        public NumericColumn Column_BullishReversal { get; }

        public NumericColumn Column_BearishReversal { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                List<Level> levels = b.GetLevel(this);

                List<double> bar = (new double[] { b.Open, b.High, b.Low, b.Close }).OrderBy(n => n).ToList();

                Range<double> body_range = new(bar[1], bar[2]);
                Range<double> lower_wick_range = new(bar[0], bar[1]);
                Range<double> upper_wick_range = new(bar[2], bar[3]);

                b[Column_Momentum] = levels.Where(n => body_range.Contains(n.LevelValue)).Select(n => n.Strength).Sum();
                b[Column_BullishReversal] = levels.Where(n => lower_wick_range.Contains(n.LevelValue)).Select(n => n.Strength).Sum();
                b[Column_BearishReversal] = levels.Where(n => upper_wick_range.Contains(n.LevelValue)).Select(n => n.Strength).Sum();
            }
        }
    }
    */
}
