/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public interface IOscillator : ISingleData, IChartSeries, IDependable
    {
        int Order { get; }

        double Reference { get; }

        double UpperLimit { get; }

        double LowerLimit { get; }

        Color UpperColor { get; }

        Color LowerColor { get; }

        int AreaOrder { get; set; }
    }
}
