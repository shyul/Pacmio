﻿/// ***************************************************************************
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

namespace Pacmio
{
    public class BoxRange
    {
        public void Insert(IPivot p)
        {
            if (WeightList.ContainsKey(p.Source))
                WeightList[p.Source] += p.Weight;
            else
                WeightList.Add(p.Source, p.Weight);

            Box.Insert(p.Level);
        }

        public double Offset(double level)
        {
            double offset = level - Box.Min;

            if (offset >= 0)
                return offset;

            offset = Box.Max - level;

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

        public Dictionary<IChartPattern, double> WeightList { get; } = new Dictionary<IChartPattern, double>();

        public Range<double> Box { get; } = new Range<double>(double.MaxValue, double.MinValue);

        public double Level => (Box.Max + Box.Min) / 2;
    }
}