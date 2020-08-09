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

            Description = (Column == Bar.Column_Close) ? GetType().Name : GetType().Name + " (" + Column.Name + ")";

            ColumnSeries_Percent = new AdColumnSeries(Column_Percent, Column_Percent, 50, 0, 0)
            {
                Name = Name,
                LegendName = "Gain %",
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
            get
            {
                return ColumnSeries_Percent.Theme.ForeColor;
            }
            set
            {
                ColumnSeries_Percent.Theme.ForeColor = value;

                ColumnSeries_Percent.TextTheme.EdgeColor = value.Opaque(255);
                ColumnSeries_Percent.TextTheme.FillColor = ColumnSeries_Percent.TextTheme.EdgeColor.GetBrightness() < 0.6 ? ColumnSeries_Percent.TextTheme.EdgeColor.Brightness(0.85f) : ColumnSeries_Percent.TextTheme.EdgeColor.Brightness(-0.85f);
                ColumnSeries_Percent.TextTheme.ForeColor = ColumnSeries_Percent.TextTheme.EdgeColor;
            }
        }

        public Color LowerColor
        {
            get
            {
                return ColumnSeries_Percent.DownTheme.ForeColor;
            }
            set
            {
                ColumnSeries_Percent.DownTheme.ForeColor = value;

                ColumnSeries_Percent.DownTextTheme.EdgeColor = value.Opaque(255);
                ColumnSeries_Percent.DownTextTheme.FillColor = ColumnSeries_Percent.DownTextTheme.EdgeColor.GetBrightness() < 0.6 ? ColumnSeries_Percent.DownTextTheme.EdgeColor.Brightness(0.85f) : ColumnSeries_Percent.DownTextTheme.EdgeColor.Brightness(-0.85f);
                ColumnSeries_Percent.DownTextTheme.ForeColor = ColumnSeries_Percent.DownTextTheme.EdgeColor;
            }
        }

        public AdColumnSeries ColumnSeries_Percent { get; }

        public bool ChartEnabled { get => Enabled && ColumnSeries_Percent.Enabled; set => ColumnSeries_Percent.Enabled = value; }

        public int SeriesOrder { get => ColumnSeries_Percent.Order; set => ColumnSeries_Percent.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 12;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                OscillatorArea a = bc[AreaName] is OscillatorArea oa ? oa :
                    bc.AddArea(new OscillatorArea(bc, AreaName, AreaRatio)
                    {
                        Reference = 0,
                        HasXAxisBar = HasXAxisBar,
                        FixedTickStep_Right = 0.02,
                    });
                a.AddSeries(ColumnSeries_Percent);
            }
        }

        #endregion Series
    }
}
