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
    public class TradeCapFilter : FilterAnalysis
    {
        public TradeCapFilter(
            double minPrice = 1,
            double maxPrice = 300,
            double minVolume = 5e5,
            double maxVolume = double.MaxValue,
            BarFreq barFreq = BarFreq.Daily,
            PriceType priceType = PriceType.Trades)
            : base(barFreq, priceType)
        {
            CapRange = new Range<double>(minVolume * minPrice, maxVolume * maxPrice);

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

        public Range<double> CapRange { get; protected set; }

        public override string Label { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                    if (b.Bar_1 is Bar b_1)
                    {
                        var cap = b_1.Typical * b_1.Volume;
                        if (CapRange.Contains(cap))
                            b[Column_Result] = cap;
                        else
                            b[Column_Result] = 0;
                    }
                    else
                        b[Column_Result] = 0;
            }
        }
    }
}
