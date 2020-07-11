/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;

namespace Pacmio
{
    public interface IChartSeries
    {
        Color Color { get; }

        bool ChartEnabled { get; set; }

        int SeriesOrder { get; set; }

        bool HasXAxisBar { get; set; }

        string AreaName { get; }

        float AreaRatio { get; set; }

        void ConfigChart(BarChart bc);
    }
}
