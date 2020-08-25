/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Aroon Indicator
/// 
/// Ref 1: https://school.stockcharts.com/doku.php?id=technical_indicators:aroon
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
    public class Aroon : BarAnalysis, IDualData, IOscillator
    {
        public Aroon(int interval = 14)
        {
            Interval = interval;

            string label = "(" + Interval.ToString() + ")";
            Name = GetType().Name + label;
            AreaName = GroupName = GetType().Name;
            Description = "Aroon " + label;

            Column_High = new NumericColumn(Name + "_H");
            Column_Low = new NumericColumn(Name + "_L");
            Column_Result = new NumericColumn(Name + "_OSC");

            LineSeries = new LineSeries(Column_Result, Color.DarkSlateGray, LineType.Default, 1.5f)
            {
                Name = Name,
                Label = string.Empty,
                LegendName = GroupName,
                Importance = Importance.Major,
                IsAntialiasing = true,
                DrawLimitShade = false
            };

            CloudSeries = new CloudSeries(Column_High, Column_Low)
            {
                Name = Name + "_DI",
                Label = label,
                LegendName = GroupName,
                Importance = Importance.Minor,
                //Color = Color.OliveDrab,
                //ShadeColor = Color.OliveDrab,
                //FillColor = Color.OliveDrab.Opaque(64),
                //LowColor = Color.OrangeRed,
                //LowShadeColor = Color.OrangeRed,
                //LowFillColor = Color.OrangeRed.Opaque(64),
                Width = 1f,
                IsAntialiasing = true
            };

      

            UpperColor = Color.OliveDrab;
            LowerColor = Color.OrangeRed;
        }

        #region Parameters

        public double Reference { get; set; } = 50;

        public double UpperLimit { get; set; } = 70;

        public double LowerLimit { get; set; } = 30;

        public int Interval { get; }

        #endregion Parameters

        #region Calculation

        public NumericColumn Column_Result { get; }

        public NumericColumn Column_High { get; }

        public NumericColumn Column_Low { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    Dictionary<double, int> high_index = new Dictionary<double, int>();
                    Dictionary<double, int> low_index = new Dictionary<double, int>();

                    for (int j = 0; j < Interval; j++)
                    {
                        int k = i - j;
                        if (k < 0) k = 0;
                        if (bt[k] is Bar b_peak)
                        {
                            double high = b_peak.High, low = b_peak.Low;

                            if (!high_index.ContainsKey(high))
                                high_index[high] = j;

                            if (!low_index.ContainsKey(low))
                                low_index[low] = j;
                        }
                    }

                    int days_high = high_index.OrderByDescending(n => n.Key).First().Value;
                    int days_low = low_index.OrderBy(n => n.Key).First().Value;

                    double aroon_up = b[Column_High] = 100 * (Interval - days_high) / Interval;
                    double aroon_down = b[Column_Low] = 100 * (Interval - days_low) / Interval;

                    b[Column_Result] = aroon_up - aroon_down;
                }
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => LineSeries.Color; set => LineSeries.Color = value; }

        public float LineWidth { get => LineSeries.Width; set => LineSeries.Width = CloudSeries.Width = value; }

        public LineType LineType { get => CloudSeries.LineType; set => LineSeries.LineType = CloudSeries.LineType = value; }

        public Series MainSeries => CloudSeries;

        public Color UpperColor // Color.OliveDrab;
        {
            get => CloudSeries.Color;
            set
            {
                CloudSeries.Color = CloudSeries.EdgeColor = value;
                CloudSeries.FillColor = value.Opaque(64);
            }
        }

        public Color LowerColor  // Color.OrangeRed;
        {
            get => CloudSeries.LowerColor;
            set
            {
                CloudSeries.LowerColor = CloudSeries.LowerEdgeColor = value;
                CloudSeries.LowFillColor = value.Opaque(64);
            }
        }

        public LineSeries LineSeries { get; }

        public CloudSeries CloudSeries { get; }

        public virtual bool ChartEnabled { get => Enabled && CloudSeries.Enabled; set => CloudSeries.Enabled = value; }

        public int SeriesOrder { get => CloudSeries.Order; set => CloudSeries.Order = value; }

        public virtual bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; protected set; }

        public float AreaRatio { get; set; } = 8;

        public int AreaOrder { get; set; } = 0;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartOscillatorArea a = bc[AreaName] is BarChartOscillatorArea oa ? oa :
                    bc.AddArea(new BarChartOscillatorArea(bc, AreaName, AreaRatio)
                    {
                        Order = AreaOrder,
                        Reference = Reference,
                        HasXAxisBar = HasXAxisBar,
                        //UpperColor = Color.YellowGreen,
                        //LowerColor = Color.PaleVioletRed,
                        //FixedTickStep_Right = 20,
                    });

                a.AddSeries(CloudSeries);
                //a.AddSeries(LineSeries);
            }
        }

        #endregion Series
    }
}