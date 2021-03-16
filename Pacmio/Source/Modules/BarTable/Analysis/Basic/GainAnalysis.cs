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
    public sealed class GainAnalysis : BarAnalysis, ISingleData, IChartSeries
    {
        public GainAnalysis() : this(Bar.Column_Close) { }

        public GainAnalysis(NumericColumn column)
        {
            Column = column;

            string label = "(" + Column.Name + ")";
            Name = AreaName = GroupName = GetType().Name + label;

            Column_Gain = new NumericColumn(Name + "_Gain");
            Column_Percent = new NumericColumn(Name + "_Percent") { Label = "" };
            //Column_Gap = new NumericColumn(Name + "_Gap") { Label = "" };

            Description = (Column == Bar.Column_Close) ? GetType().Name : GetType().Name + " (" + Column.Name + ")";

            ColumnSeries = new AdColumnSeries(Column_Percent, Column_Percent, 50, 0, 0)
            {
                Name = Name,
                LegendName = "Gain " + label + " %",
                Label = "",
                LegendLabelFormat = "G5",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false,
                Order = 200
            };

            UpperColor = Color.Green;
            LowerColor = Color.Red;
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode();

        #region Calculation

        public NumericColumn Column { get; }

        public NumericColumn Column_Gain { get; }

        public NumericColumn Column_Percent { get; }

        public NumericColumn Column_Result => Column_Percent;

        //public NumericColumn Column_Gap { get; }

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

        public Color Color { get => UpperColor; set => UpperColor = value; }

        public Color UpperColor
        {
            get => ColumnSeries.Color;

            set
            {
                Color c = value;
                ColumnSeries.Color = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries.EdgeColor = ColumnSeries.TextTheme.ForeColor = c;
            }
        }

        public Color LowerColor
        {
            get => ColumnSeries.LowerColor;

            set
            {
                Color c = value;
                ColumnSeries.LowerColor = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries.LowerEdgeColor = ColumnSeries.LowerTextTheme.ForeColor = c;
            }
        }

        public Series MainSeries => ColumnSeries;

        public AdColumnSeries ColumnSeries { get; }

        public bool ChartEnabled { get => Enabled && ColumnSeries.Enabled; set => ColumnSeries.Enabled = value; }

        public int SeriesOrder { get => ColumnSeries.Order; set => ColumnSeries.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 10;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartOscillatorArea a = bc[AreaName] is BarChartOscillatorArea oa ? oa :
                    bc.AddArea(new BarChartOscillatorArea(bc, AreaName, AreaRatio)
                    {
                        Reference = 0,
                        HasXAxisBar = HasXAxisBar,
                        //FixedTickStep_Right = 0.02,
                    });
                a.AddSeries(ColumnSeries);
            }
        }

        #endregion Series
    }
}
