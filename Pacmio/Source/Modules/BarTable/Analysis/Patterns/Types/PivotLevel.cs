/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Security.Permissions;

namespace Pacmio
{
    public class PivotLevel : IPivot
    {
        public PivotLevel(Pivot point)
        {
            P1 = point;
        }

        public Pivot P1 { get; protected set; }

        public int X1 => P1.Index;

        public double Y1 => P1.Level;
    }
}
