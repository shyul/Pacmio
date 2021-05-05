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
    public class PriceVolumeFilter : FilterAnalysis
    {
        public PriceVolumeFilter(
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

            Label = "(" + minPrice + "," + maxPrice + "," + minVolume + "," + maxVolume + "," + barFreq + "," + priceType + ")";
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

        protected PriceVolumeFilter(
            BarFreq barFreq = BarFreq.Daily,
            PriceType priceType = PriceType.Trades)
            : base(barFreq, priceType)
        { }

        public Range<double> VolumeRange { get; protected set; }

        public Range<double> PriceRange { get; protected set; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                    if (b.Bar_1 is Bar b_1)
                    {
                        if (PriceRange.Contains(b_1.Typical) && VolumeRange.Contains(b_1.Volume))
                            b[Column_Result] = b_1.Typical * b_1.Volume;
                        else
                            b[Column_Result] = 0;
                    }
                    else
                        b[Column_Result] = 0;
            }
        }
    }
}
