/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class GainAnalysis : BarAnalysis, IChartSeries
    {
        public GainAnalysis(NumericColumn column, int maximumPeakProminence)
        {
            Column = column;

            string label = "(" + Column.Name + ")";
            Name = AreaName = GroupName = GetType().Name + label;

            Column_Gain = new NumericColumn(Name + "_Gain");
            Column_Percent = new NumericColumn(Name + "_Percent");

            Description = (Column == Bar.Column_Close) ? GetType().Name : GetType().Name + " (" + Column.Name + ")";

            ColumnSeries_Percent = new ColumnSeries(Column_Percent, Color.FromArgb(88, 168, 208), Color.FromArgb(32, 104, 136), 50)
            {
                Name = Name + "_GAIN",
                LegendName = GroupName + "_GAIN",
                Label = "GAIN",
                Importance = Importance.Minor,
                Order = 200
            };
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode();

        #region Calculation

        public NumericColumn Column { get; }

        public NumericColumn Column_Gain { get; }

        public NumericColumn Column_Percent { get; }

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

                // Get Gain
                double data = b[Column];
                double gain = b[Column_Gain] = data - data_1;

                b[Column_Percent] = (data_1 == 0) ? 0 : (100 * gain / data_1);

                data_1 = data;
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => ColumnSeries_Percent.Color; set => ColumnSeries_Percent.Color = ColumnSeries_Percent.ShadeColor = value; }

        public ColumnSeries ColumnSeries_Percent { get; }


        public bool ChartEnabled { get => Enabled && ColumnSeries_Percent.Enabled; set => ColumnSeries_Percent.Enabled = value; }

        public int SeriesOrder { get => ColumnSeries_Percent.Order; set => ColumnSeries_Percent.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 12;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                Area a_gain = bc.AddArea(new Area(bc, AreaName + "_gain", AreaRatio)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a_gain.AddSeries(ColumnSeries_Percent);
            }
        }

        #endregion Series
    }
}
