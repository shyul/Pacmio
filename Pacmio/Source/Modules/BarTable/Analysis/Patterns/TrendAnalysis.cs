/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Pacmio
{
    public class TrendAnalysis : BarAnalysis, IPattern
    {


        public bool IsLogarithmic { get; }

        public virtual int Interval { get; }

        public virtual int RankLimit { get; }

        public double Tolerance { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}
