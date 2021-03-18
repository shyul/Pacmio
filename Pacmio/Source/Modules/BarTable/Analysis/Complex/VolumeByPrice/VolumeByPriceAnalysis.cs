/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// https://school.stockcharts.com/doku.php?id=technical_indicators:volume_by_price
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public sealed class VolumeByPriceAnalysis : BarAnalysis
    {
        public int NumOfLevels { get; } = 10;

        public int MaximumInterval { get; } = 200;

        public DatumColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                VolumeByPriceDatum datum =  new VolumeByPriceDatum();
                b[Column_Result] = datum;

                var bars = bt[i - MaximumInterval, i];
                Console.WriteLine("Number of bars selected" + bars.Count);

                var price_list = bars.Select(n => n.Typical);

                double max_price = price_list.Max();
                double min_price = price_list.Min();
                double delta_price = (max_price - min_price) / MaximumInterval;

                datum.IntervalRange = new Range<double>(min_price, max_price);

                for (int j = 0; j < MaximumInterval; j++)
                {
                    double min = min_price + j * delta_price;
                    double max = min_price + (j + 1) * delta_price;
                    datum.PriceRangeToVolumeLUT.Add(new Range<double>(min, max), 0);
                }

                var price_to_volume_list = bars.Select(n => (n.Typical, n.Volume));

                foreach (var range in datum.PriceRangeToVolumeLUT.Keys)
                {
                    datum.PriceRangeToVolumeLUT[range] = price_to_volume_list.Where(n => range.ContainsNoMax(n.Typical)).Select(n => n.Volume).Sum();
                }
            }
        }
    }
}
