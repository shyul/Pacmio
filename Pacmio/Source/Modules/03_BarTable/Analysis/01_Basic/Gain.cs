/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public sealed class Gain : BarAnalysis, ISingleData
    {
        public Gain() : this(Bar.Column_Close) { }

        public Gain(NumericColumn column)
        {
            Column = column;

            string label = "(" + Column.Name + ")";
            Name = GroupName = GetType().Name + label;

            Column_Gain = new NumericColumn(Name + "_Gain");
            Column_Percent = new NumericColumn(Name + "_Percent") { Label = "" };

            Description = (Column == Bar.Column_Close) ? GetType().Name : GetType().Name + " (" + Column.Name + ")";
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode();

        #region Calculation

        public NumericColumn Column { get; }

        public NumericColumn Column_Gain { get; }

        public NumericColumn Column_Percent { get; }

        public NumericColumn Column_Result => Column_Percent;

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            double data_1;

            // Define the bondary condition
            if (bap.StartPt < 1)
            {
                if (bap.StartPt < 0) bap.StartPt = 0;
                data_1 = bt[0][Column];
            }
            else
            {
                data_1 = bt[bap.StartPt - 1][Column];
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                double data = b[Column];
                double gain = b[Column_Gain] = data - data_1;

                b[Column_Percent] = (data_1 == 0) ? 0 : (100 * gain / data_1);

                data_1 = data;
            }
        }

        #endregion Calculation
    }
}
