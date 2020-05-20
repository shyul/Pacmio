/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class PriceChannel : BarAnalysis, IDualData, IChartSeries
    {
        public PriceChannel(int interval)
        {
            Interval = interval;

            string label = "(" + Interval.ToString() + ")";
            Description = "Box Range " + label;
            GroupName = Name = GetType().Name + label;

            High_Column = new NumericColumn(Name + "_HHI") { Label = "H" };
            Low_Column = new NumericColumn(Name + "_LLO") { Label = "L" };

            BandSeries = new BandSeries(High_Column, Low_Column, Color.LimeGreen)
            {
                Name = Name,
                LegendName = GroupName,
                IsAntialiasing = true,
                Importance = Importance.Minor,
            };

            Color = Color.LimeGreen;
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Interval;

        #region Parameters

        public int Interval { get; }

        #endregion Parameters

        #region Calculation

        public NumericColumn High_Column { get; }

        public NumericColumn Low_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double high = b.High;
                double low = b.Low;
                for (int j = 1; j < Interval; j++)
                {
                    int k = i - j;
                    if (k < 0) k = 0;

                    double compare_high = bt[k].High;
                    if (high < compare_high) high = compare_high;

                    double compare_low = bt[k].Low;
                    if (low > compare_low) low = compare_low;
                }

                b[High_Column] = high;
                b[Low_Column] = low;
            }
        }

        #endregion Calculation

        #region Series

        public Color Color
        {
            get
            {
                return BandSeries.Color;
            }
            set
            {
                BandSeries.Color =
                BandSeries.ShadeColor =
                BandSeries.LowColor =
                BandSeries.LowShadeColor =
                BandSeries.FillColor = value.Opaque(64);
            }
        }

        public float LineWidth { get => BandSeries.Width; set => BandSeries.Width = value; }

        public LineType LineType { get => BandSeries.LineType; set => BandSeries.LineType = value; }

        public BandSeries BandSeries { get; }

        public bool ChartEnabled { get => Enabled && BandSeries.Enabled; set => BandSeries.Enabled = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; private set; } = MainArea.DefaultName;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                Area a = bc.AddArea(new Area(bc, AreaName, 10)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a.AddSeries(BandSeries);
            }
        }

        #endregion Series
    }
}
