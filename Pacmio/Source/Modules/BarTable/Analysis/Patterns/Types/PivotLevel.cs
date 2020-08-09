/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Security.Permissions;

namespace Pacmio
{
    public class PivotLevel
    {
        public PivotLevel(IPattern source, Pivot point)
        {
            Source = source;
            P1 = point;
        }

        public IPattern Source { get; protected set; }

        public Pivot P1 { get; protected set; }

        public int X1 => P1.Index;

        public double Y1 => P1.Level;
    }
}
