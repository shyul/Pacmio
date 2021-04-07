/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu.Chart;

namespace Pacmio
{
    public interface IChartSeries : IChartAnalysis
    {
        /*
        bool HasXAxisBar { get; set; }

        float AreaRatio { get; set; }
        */

        Color Color { get; }

        Series MainSeries { get; }

        void ConfigChart(BarChart bc);
    }
}
