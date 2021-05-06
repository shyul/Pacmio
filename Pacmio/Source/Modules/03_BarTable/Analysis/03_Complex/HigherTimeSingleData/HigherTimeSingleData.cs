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
    public class HigherTimeSingleData : SingleDataAnalysis
    {
        public HigherTimeSingleData(ISingleData isd, BarFreq freq, PriceType type = PriceType.Trades)
        {
            BarFreq = freq;
            PriceType = type;
            SingleData = isd;

            Label = "(" + SingleData.Name + "," + BarFreq + "," + PriceType + ")";
            Name = GetType().Name + Label;
            Description = "Higher Time Single Data " + Label;

            Column_Result = new(Name, Label);
            LineSeries = new LineSeries(Column_Result)
            {
                Name = Name,
                LegendName = GroupName,
                Label = Label,
                IsAntialiasing = true,
                DrawLimitShade = false
            };

            if (isd is SingleDataAnalysis sda) Color = sda.Color;
        }

        public ISingleData SingleData { get; }

        public BarFreq BarFreq { get; }

        public PriceType PriceType { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            BarTable bt_ht = bt.BarTableSet[BarFreq, PriceType];

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                Bar b_ht = bt_ht[b.Time];

                b[Column_Result] = b_ht[SingleData];
            }
        }
    }
}
