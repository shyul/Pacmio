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
    public sealed class VolumeByPriceAnalysis : BarAnalysis, ILevelAnalysis
    {
        public VolumeByPriceAnalysis()
        {
            Name = GetType() + " Test Only";

            Column_Result = new DatumColumn(Name, typeof(VolumeByPriceDatum));
        }

        public int NumOfLevels { get; } = 10;

        public int MaximumInterval { get; } = 200;

        public string AreaName => MainBarChartArea.DefaultName;

        public DatumColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                VolumeByPriceDatum datum = new();
                b[Column_Result] = datum;

                var bars = bt[i, MaximumInterval];
                //Console.WriteLine("Bars i = " + i + " | LastIndex = " + bars.Last().Index + " | Bar Index = " + bt[i].Index + " | Count = " + bars.Count);

                datum.IntervalRange = new Range<double>(bars.Select(n => n.Typical));
                double max_price = datum.IntervalRange.Maximum;
                double min_price = datum.IntervalRange.Minimum;

                if (!datum.IntervalRange.IsEmpty)
                {
                    double delta_price = (max_price - min_price) / NumOfLevels;
                    for (int j = 0; j < NumOfLevels; j++)
                    {
                        double min = min_price + j * delta_price;
                        double max = min_price + (j + 1) * delta_price;
                        var key = new Range<double>(min, max);
                        var value = bars.Where(n => key.ContainsNoMax(n.Typical)).Select(n => n.Volume).Sum();
                        datum.PriceRangeToVolumeLUT[key] = value;
                        datum.Levels.Add(new Level((min + max) / 2, value));
                    }
                }
            }
        }
    }
}
