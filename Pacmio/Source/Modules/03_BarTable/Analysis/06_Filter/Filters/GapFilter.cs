/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class GapFilter : PriceVolumeFilter
    {
        public GapFilter(
            double gapPercent = 4,
            double minPrice = 1,
            double maxPrice = 300,
            double minVolume = 5e5,
            double maxVolume = double.MaxValue,

            BarFreq barFreq = BarFreq.Daily,
            PriceType priceType = PriceType.Trades)
            : base(barFreq, priceType)
        {
            VolumeRange = new Range<double>(minVolume, maxVolume);
            PriceRange = new Range<double>(minPrice, maxPrice);
            GapPercent = gapPercent;

            Label = "(" + gapPercent + "," + minPrice + "," + maxPrice + "," + minVolume + "," + maxVolume + "," + barFreq + "," + priceType + ")";
            Name = GetType().Name + Label;
            AreaName = GroupName = GetType().Name;
            Description = "Price Volume Filter " + Label;

            Column_Result = new NumericColumn(Name, Label);
            LineSeries = new LineSeries(Column_Result)
            {
                Name = Name,
                Label = Label,
                LegendName = GroupName,
                Importance = Importance.Major,
                IsAntialiasing = true,
                DrawLimitShade = true,
            };

            BarAnalysisList = new BarAnalysisList(new BarAnalysis[] { this });
        }

        public double GapPercent { get; }

        public override string Label { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                if (PriceRange.Contains(b.Typical) && VolumeRange.Contains(b.Volume) && (b.GapPercent > GapPercent || b.GapPercent < -GapPercent))
                    b[Column_Result] = b.GapPercent;
                else
                    b[Column_Result] = 0;
            }
        }
    }
}
