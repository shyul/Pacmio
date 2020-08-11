/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Pacmio
{
    public class PivotLevel : IPivot
    {
        public PivotLevel(IChartPattern source, Pivot pt1)
        {
            Source = source;
            P1 = pt1;
            Level = P1.Level;
        }

        public Pivot P1 { get; }

        public int X1 => P1.Index;

        public double Y1 => P1.Level;

        public IChartPattern Source { get; }

        public double Weight { get; set; }

        public double Level { get; protected set; }
    }
}
