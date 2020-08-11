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
        public PivotLevel(IPattern source, Pivot point, double weight)
        {
            Source = source;
            P1 = point;
            Weight = weight;
        }

        public Pivot P1 { get; }

        public int X1 => P1.Index;

        public double Y1 => P1.Level;

        public IPattern Source { get; }

        public double Weight { get; }
    }
}
