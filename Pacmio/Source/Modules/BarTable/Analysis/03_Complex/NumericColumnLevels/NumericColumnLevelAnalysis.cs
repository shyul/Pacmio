/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
///
/// VWAP Cross, SMA, EMA, 200, 50, 20, 5
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Xu;

namespace Pacmio.Analysis
{
    public class NumericColumnLevelAnalysis : BarAnalysis, ILevelAnalysis
    {
        public NumericColumnLevelAnalysis(string areaName = MainBarChartArea.DefaultName)
        {
            Name = GetType().Name;
            Column_Result = new(Name, typeof(NumericColumnLevelDatum));
            AreaName = areaName;
        }

        public void Add(ISingleData isd, double strength)
        {
            if (isd is IChartSeries ics && ics.AreaName != AreaName)
                return;

            StrengthLUT[isd.Column_Result] = strength;
            isd.AddChild(this);
        }

        public void Add(IDualData idd, double high_strength, double low_strength)
        {
            if (idd is IChartSeries ics && ics.AreaName != AreaName)
                return;

            StrengthLUT[idd.Column_High] = high_strength;
            StrengthLUT[idd.Column_Low] = low_strength;
            idd.AddChild(this);
        }

        private Dictionary<NumericColumn, double> StrengthLUT { get; } = new();

        public DatumColumn Column_Result { get; }

        public string AreaName { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                NumericColumnLevelDatum d = new NumericColumnLevelDatum();

                foreach (var item in StrengthLUT)
                {
                    d.Levels.Add(new Level(b[item.Key], item.Value));
                }

                b[Column_Result] = d;
            }
        }
    }
}