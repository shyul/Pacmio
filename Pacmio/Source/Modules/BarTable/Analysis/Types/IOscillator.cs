/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;

namespace Pacmio
{
    public interface IOscillator : ISingleData, IDependable
    {
        int Order { get; }

        double Reference { get; }

        double UpperLimit { get; }

        double LowerLimit { get; }

        Color UpperColor { get; }

        Color LowerColor { get; }

        string AreaName { get; }

        float AreaRatio { get; set; }

        int AreaOrder { get; set; }
    }
}
