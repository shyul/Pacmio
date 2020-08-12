/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// https://school.stockcharts.com/doku.php?id=technical_indicators:average_directional_index_adx
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class ADX : ATR, IDualData, IOscillator
    {
        public ADX(int interval = 14)
        {
            Interval = interval;

            string label = "(" + Interval.ToString() + ")";
            AreaName = GroupName = Name = GetType().Name + label;
            Description = "Average Directional Index " + label;

            TR = new NumericColumn(Name + "_TR");
            PDM = new NumericColumn(Name + "_PDM");
            MDM = new NumericColumn(Name + "_MDM");
            PDM_MA = new NumericColumn(Name + "_PDM_MA");
            MDM_MA = new NumericColumn(Name + "_MDM_MA");
            PDI = new NumericColumn(Name + "_PDI") { Label = "+DI" };
            MDI = new NumericColumn(Name + "_MDI") { Label = "-DI" };
            DX_MA = new NumericColumn(Name + "_DX_MA");

            //Result_Column = new NumericColumn(Name);

            LineSeries = new LineSeries(DX_MA, Color.DarkSlateGray, LineType.Default, 1.5f)
            {
                Name = Name,
                Label = string.Empty,
                LegendName = GroupName,
                Importance = Importance.Major,
                IsAntialiasing = true,
                DrawLimitShade = false
            };

            CloudSeries = new CloudSeries(PDI, MDI)
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

        #region Calculation

        public double Reference { get; set; } = 25;

        public double UpperLimit { get; set; } = double.NaN;

        public double LowerLimit { get; set; } = double.NaN;

        public NumericColumn TR { get; }

        public NumericColumn PDM { get; }

        public NumericColumn MDM { get; }

        public NumericColumn PDM_MA { get; }

        public NumericColumn MDM_MA { get; }

        public NumericColumn PDI { get; }

        public NumericColumn MDI { get; }

        public NumericColumn DX_MA { get; }

        public override NumericColumn Column_Result => DX_MA;

        public NumericColumn Column_High => PDI;

        public NumericColumn Column_Low => MDI;

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                Bar b_1 = bt[i - 1];

                double tr = b.TrueRange;
                double tr_ma;

                if (i < Interval)
                {
                    double tr_ma_sum = 0;
                    for (int j = 0; j < Interval; j++)
                    {
                        int k = i - j;
                        if (k < 0) k = 0;
                        tr_ma_sum += bt[k].TrueRange;
                    }
                    tr_ma = b[TR] = tr_ma_sum;
                }
                else
                {
                    double prior_tr_ma = b_1[TR];
                    tr_ma = b[TR] = tr + prior_tr_ma - (prior_tr_ma / Interval);
                }

                double current_high = b.High; // C4
                double current_low = b.Low; // D4

                double prior_high = current_high; // C3
                double prior_low = current_low; // D3

                if (i > 0)
                {
                    prior_high = b_1.High; // C3
                    prior_low = b_1.Low; // D3
                }

                double high_diff = current_high - prior_high; // C4 - C3
                double low_diff = prior_low - current_low; // D3 - D4

                double pdm = (high_diff > low_diff && high_diff > 0) ? high_diff : 0;
                double mdm = (low_diff > high_diff && low_diff > 0) ? low_diff : 0;

                b[PDM] = pdm;
                b[MDM] = mdm;

                //Console.WriteLine("pdm = " + pdm.ToString("0.##") + "; mdm = " + mdm.ToString("0.##"));

                // Wilder's Smoothing Techniques
                // https://school.stockcharts.com/doku.php?id=technical_indicators:average_directional_index_adx#wilder_s_smoothing_techniques

                double pdm_ma = 0, mdm_ma = 0;

                if (i < Interval)
                {
                    for (int j = 0; j < Interval; j++)
                    {
                        int k = i - j;
                        if (k < 0) k = 0;

                        pdm_ma += bt[k][PDM];
                        mdm_ma += bt[k][MDM];
                    }

                    b[PDM_MA] = pdm_ma; // /= Interval;
                    b[MDM_MA] = mdm_ma; // /= Interval;
                }
                else
                {
                    double prior_pdm_ma = b_1[PDM_MA];
                    b[PDM_MA] = pdm_ma = pdm + prior_pdm_ma - (prior_pdm_ma / Interval);

                    double prior_mdm_ma = b_1[MDM_MA];
                    b[MDM_MA] = mdm_ma = mdm + prior_mdm_ma - (prior_mdm_ma / Interval);
                }

                //Console.WriteLine("pdm_ma = " + pdm_ma.ToString("0.##") + "; mdm_ma = " + mdm_ma.ToString("0.##"));
                //Console.WriteLine(pdm.ToString("0.##") + ",\t" + pdm_ma.ToString("0.##") + ",\t" + mdm.ToString("0.##") + ",\t" + mdm_ma.ToString("0.##"));
                //Console.WriteLine(Table[i][TR].ToString("0.##") + ",\t" + tr_ma.ToString("0.##"));

                double pdi = 0, mdi = 0, dx = 0; //, tr_ma = bt[i][TR];

                if (tr_ma != 0)
                {
                    pdi = pdm_ma * 100 / tr_ma;
                    b[PDI] = pdi;

                    mdi = mdm_ma * 100 / tr_ma;
                    b[MDI] = mdi;
                }
                else
                {
                    b[PDI] = b[MDI] = 0;
                }

                double sum_i = pdi + mdi;
                if (sum_i != 0) dx = Math.Abs((pdi - mdi) * 100 / sum_i);

                b[Column_Result] = dx;

                if (i <= 2 * Interval)
                {
                    sum_i = 0;
                    for (int j = 0; j < Interval; j++)
                    {
                        int k = i - j;
                        if (k < 0) k = 0;
                        sum_i += bt[k][Column_Result];
                    }
                    b[DX_MA] = sum_i / Interval;
                }
                else
                {
                    double prior = b_1[DX_MA];
                    b[DX_MA] = (prior * (Interval - 1) + dx) / Interval;
                }
            }
        }

        #endregion Calculation

        #region Series

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

        public CloudSeries CloudSeries { get; }

        public override bool ChartEnabled { get => Enabled && LineSeries.Enabled; set => CloudSeries.Enabled = LineSeries.Enabled = value; }

        public override void ConfigChart(BarChart bc)
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
                a.AddSeries(LineSeries);
            }
        }

        #endregion Series
    }
}
