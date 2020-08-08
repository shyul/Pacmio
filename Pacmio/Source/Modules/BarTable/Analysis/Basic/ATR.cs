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
    public class ATR : BarAnalysis, ISingleData, IChartSeries
    {
        public ATR(int interval = 14)
        {
            Interval = interval;

            string label = "(" + Interval.ToString() + ")";
            Name = GetType().Name + label;
            AreaName = GroupName = GetType().Name;
            Description = "Average True Range " + label;

            Column_Result = new NumericColumn(Name);
            LineSeries = new LineSeries(Column_Result, Color.DarkSlateGray, LineType.Default, 1.5f)
            {
                Name = Name,
                Label = label,
                LegendName = GroupName,
                Importance = Importance.Major,
                IsAntialiasing = true,
                DrawLimitShade = false,
            };
        }

        protected ATR() { }

        #region Parameters

        public virtual int Interval { get; protected set; }

        public override int GetHashCode() => GetType().GetHashCode() ^ Interval;

        #endregion Parameters

        #region Calculation

        public virtual NumericColumn Column_Result { get; protected set; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                double tr = bt[i].TrueRange;

                if (i < Interval)
                {
                    double tr_ma = 0;
                    for (int j = 0; j < Interval; j++)
                    {
                        int k = i - j;
                        if (k < 0) k = 0;
                        tr_ma += bt[k].TrueRange;
                    }
                    bt[i][Column_Result] = tr_ma / Interval;
                }
                else
                {
                    double prior_tr_ma = bt[i - 1][Column_Result];
                    bt[i][Column_Result] = (tr + (prior_tr_ma * (Interval - 1))) / Interval; //  tr + prior_tr_ma - (prior_tr_ma / Interval);
                }
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => LineSeries.Color; set => LineSeries.Color = value; }

        public float LineWidth { get => LineSeries.Width; set => LineSeries.Width = value; }

        public LineType LineType { get => LineSeries.LineType; set => LineSeries.LineType = value; }

        public LineSeries LineSeries { get; protected set; }

        public virtual bool ChartEnabled { get => Enabled && LineSeries.Enabled; set => LineSeries.Enabled = value; }

        public int SeriesOrder { get => LineSeries.Order; set => LineSeries.Order = value; }

        public virtual bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; protected set; }

        public float AreaRatio { get; set; } = 8;

        public int AreaOrder { get; set; } = 0;

        public virtual void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                Area a = bc.AddArea(new Area(bc, AreaName, AreaRatio)
                {
                    Order = AreaOrder,
                    HasXAxisBar = HasXAxisBar,
                });
                a.AddSeries(LineSeries);
            }
        }

        #endregion Series
    }
}
