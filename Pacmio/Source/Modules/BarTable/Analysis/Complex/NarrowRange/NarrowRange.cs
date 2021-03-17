/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// https://school.stockcharts.com/doku.php?id=trading_strategies:narrow_range_day_nr7
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public sealed class NarrowRange : BarAnalysis
    {
        public NarrowRange(int max_interval = 9)
        {
            MaximumInterval = max_interval;

            string label = "(" + MaximumInterval.ToString() + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = "Narrow Range " + label;

            Column_Result = new NumericColumn(Name, label);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ MaximumInterval;

        public int MaximumInterval { get; }

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double range = b.High - b.Low;

                int j = 1;
                for (; j <= MaximumInterval; j++)
                {
                    int k = i - j;

                    if (k < 0)
                    {
                        k--;
                        break;
                    }
                    else
                    {
                        Bar b_1 = bt[k];
                        double range_1 = b_1.High - b_1.Low;

                        if (range >= range_1)
                            break;
                    }
                }

                b[Column_Result] = j;
            }
        }
    }
}
