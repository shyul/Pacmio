﻿/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu.Chart;

namespace Pacmio
{
    public interface IChartSeries
    {
        /*
        bool ChartEnabled { get; set; }

        bool HasXAxisBar { get; set; }

        string AreaName { get; }

        float AreaRatio { get; set; }
        */

        Color Color { get; }

        int SeriesOrder { get; set; }

        Series MainSeries { get; }

        void ConfigChart(BarChart bc);
    }
}
