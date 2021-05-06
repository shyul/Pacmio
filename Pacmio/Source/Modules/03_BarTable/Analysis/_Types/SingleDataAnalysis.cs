/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
    public abstract class SingleDataAnalysis : BarAnalysis, ISingleData, IChartSeries
    {
        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Label.GetHashCode();



        public virtual NumericColumn Column_Result { get; protected set; }

        #endregion Parameters

        #region Series

        public Color Color { get => LineSeries.Color; set => LineSeries.Color = LineSeries.EdgeColor = value; }

        public float LineWidth { get => LineSeries.Width; set => LineSeries.Width = value; }

        public LineType LineType { get => LineSeries.LineType; set => LineSeries.LineType = value; }

        public Series MainSeries => LineSeries;

        public LineSeries LineSeries { get; protected set; }

        public virtual bool ChartEnabled { get => Enabled && LineSeries.Enabled; set => LineSeries.Enabled = value; }

        public int DrawOrder { get => LineSeries.Order; set => LineSeries.Order = value; }

        public virtual bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; set; } = MainBarChartArea.DefaultName;

        public int AreaOrder { get; set; } = 0;

        public float AreaRatio { get; set; } = 12;

        public virtual void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartArea a = bc.AddArea(new BarChartArea(bc, AreaName, AreaRatio)
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
