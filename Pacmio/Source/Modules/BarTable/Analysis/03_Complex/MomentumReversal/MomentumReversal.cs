﻿/// ***************************************************************************
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
    public class MomentumReversal : BarAnalysis
    {
        public MomentumReversal(string name, IEnumerable<ILevelAnalysis> list)
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
}
