/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public class RangeBound : Range<double>
    {
        public RangeBound() : base(double.MaxValue, double.MinValue) 
        {
        
        }

        public void Insert(PatternDatum pd)
        {
            /*
            if (WeightList.ContainsKey(p.Source))
                WeightList[p.Source] += p.Strength;
            else
                WeightList.Add(p.Source, p.Strength);

            Insert(pd.Level);*/
        }

        public double Distance(double level)
        {
            double offset = level - Minimum;

            if (offset >= 0)
                return offset;

            offset = Maximum - level;

            if (offset >= 0)
                return offset;
            else
                return double.MaxValue;
        }

        public double Weight
        {
            get
            {
                if (WeightList.Count > 0) { return WeightList.Values.Sum(); }
                else return 0;
            }
        }

        public Dictionary<PatternAnalysis, double> WeightList { get; } = new();

        public double Level => (Maximum + Minimum) / 2;
    }
}
