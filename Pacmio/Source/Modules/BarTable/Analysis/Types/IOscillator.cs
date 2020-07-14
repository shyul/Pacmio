/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;

namespace Pacmio
{
    public interface IOscillator : ISingleData
    {
        double Reference { get; }

        double LowerLimit { get; }

        double UpperLimit { get; }

        Color LowerColor { get; }

        Color UpperColor { get; }

        string AreaName { get; }

        float AreaRatio { get; set; }

        int AreaOrder { get; set; }
    }
}
