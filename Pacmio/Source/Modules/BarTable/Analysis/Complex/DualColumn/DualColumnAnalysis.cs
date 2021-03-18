/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public sealed class DualColumnAnalysis : BarAnalysis
    {
        public DualColumnAnalysis(IDualData analysis)
        {
            Fast_Column = analysis.Column_High;
            Slow_Column = analysis.Column_Low;

            analysis.AddChild(this);

            string label = "(" + analysis.Name + ")";
            GroupName = Name = GetType().Name + label;
            Column_Result = new(Name, typeof(DualColumnDatum));
        }

        public DualColumnAnalysis(ISingleData fast_analysis, ISingleData slow_analysis)
        {
            Fast_Column = fast_analysis.Column_Result;
            Slow_Column = slow_analysis.Column_Result;

            fast_analysis.AddChild(this);
            slow_analysis.AddChild(this);

            string label = "(" + fast_analysis.Name + "," + slow_analysis.Name + ")";
            GroupName = Name = GetType().Name + label;
            Column_Result = new(Name, typeof(DualColumnDatum));
        }

        public DualColumnAnalysis(NumericColumn fast_column, NumericColumn slow_column)
        {
            Fast_Column = fast_column;
            Slow_Column = slow_column;

            string label = "(" + Fast_Column.Name + "," + Slow_Column.Name + ")";
            GroupName = Name = GetType().Name + label;
            Column_Result = new(Name, typeof(DualColumnDatum));
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Fast_Column.GetHashCode() ^ Slow_Column.GetHashCode();

        public NumericColumn Fast_Column { get; }

        public NumericColumn Slow_Column { get; }

        public DatumColumn Column_Result { get; }

        #endregion Parameters

        #region Calculation

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            if (bap.StartPt < 1)
                bap.StartPt = 1;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double value_fast = b[Fast_Column];
                double value_slow = b[Slow_Column];

                DualColumnDatum d = new();
                b[Column_Result] = d;
                List<DualColumnType> dualDataTypes = d.List;

                if (!double.IsNaN(value_fast) && !double.IsNaN(value_slow))
                {
                    double delta = d.Difference = value_fast - value_slow;

                    if (delta == value_slow) d.DifferenceRatio = 1;
                    else if (value_slow == 0) d.DifferenceRatio = 0;
                    else d.DifferenceRatio = delta / value_slow;

                    if (value_fast == value_slow) d.Ratio = 1;
                    else if (value_slow == 0) d.Ratio = 0;
                    else d.Ratio = value_fast / value_slow;

                    double delta_abs = Math.Abs(delta);

                    if (delta > 0)
                    {
                        dualDataTypes.Add(DualColumnType.Above);
                    }
                    else if (delta < 0)
                    {
                        dualDataTypes.Add(DualColumnType.Below);
                    }

                    Bar b_1 = bt[i - 1];

                    double last_value_fast = b_1[Fast_Column];
                    double last_value_slow = b_1[Slow_Column];

                    if (!double.IsNaN(last_value_fast) && !double.IsNaN(last_value_slow))
                    {
                        double last_delta = last_value_fast - last_value_slow;

                        if (value_fast > last_value_fast && value_slow > last_value_slow)
                        {
                            dualDataTypes.Add(DualColumnType.TrendUp);
                        }
                        else if (value_fast < last_value_fast && value_slow < last_value_slow)
                        {
                            dualDataTypes.Add(DualColumnType.TrendDown);
                        }

                        if (delta >= 0 && last_delta < 0)
                        {
                            dualDataTypes.Add(DualColumnType.CrossUp);
                        }
                        else if (delta <= 0 && last_delta > 0)
                        {
                            dualDataTypes.Add(DualColumnType.CrossDown);
                        }

                        double last_delta_abs = Math.Abs(last_delta);

                        if (delta_abs > last_delta_abs)
                        {
                            dualDataTypes.Add(DualColumnType.Expansion);
                        }
                        else
                        {
                            dualDataTypes.Add(DualColumnType.Contraction);
                        }
                    }
                }
            }
        }

        #endregion Calculation


    }
}
