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
            Description = "Gap Filter " + Label;

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

/*
[BOIL] STOCK USD @ ARCA | Pacmio.GapFilter | 9%
[METX] STOCK USD @ NASDAQ | Pacmio.GapFilter | 9%
[XELA] STOCK USD @ NASDAQ | Pacmio.GapFilter | 10%
[NGA] STOCK USD @ NYSE | Pacmio.GapFilter | 10%
[JAGX] STOCK USD @ NASDAQ | Pacmio.GapFilter | 10%
[AMTX] STOCK USD @ NASDAQ | Pacmio.GapFilter | 10%
[BLDP] STOCK USD @ NASDAQ | Pacmio.GapFilter | 10%
[TRCH] STOCK USD @ NASDAQ | Pacmio.GapFilter | 10%
[CLNE] STOCK USD @ NASDAQ | Pacmio.GapFilter | 10%
[AZRX] STOCK USD @ NASDAQ | Pacmio.GapFilter | 10%
[SNDL] STOCK USD @ NASDAQ | Pacmio.GapFilter | 10%
[PHUN] STOCK USD @ NASDAQ | Pacmio.GapFilter | 10%
[SUNW] STOCK USD @ NASDAQ | Pacmio.GapFilter | 10%
[TIGR] STOCK USD @ NASDAQ | Pacmio.GapFilter | 11%
[TLRY] STOCK USD @ NASDAQ | Pacmio.GapFilter | 11%
[SENS] STOCK USD @ AMEX | Pacmio.GapFilter | 11%
[GEVO] STOCK USD @ NASDAQ | Pacmio.GapFilter | 11%
[FRSX] STOCK USD @ NASDAQ | Pacmio.GapFilter | 11%
[STPK] STOCK USD @ NYSE | Pacmio.GapFilter | 11%
[UAVS] STOCK USD @ AMEX | Pacmio.GapFilter | 11%
[AMC] STOCK USD @ NYSE | Pacmio.GapFilter | 11%
[BTBT] STOCK USD @ NASDAQ | Pacmio.GapFilter | 11%
[GME] STOCK USD @ NYSE | Pacmio.GapFilter | 12%
[SNCA] STOCK USD @ NASDAQ | Pacmio.GapFilter | 12%
[YANG] STOCK USD @ ARCA | Pacmio.GapFilter | 12%
[EEIQ] STOCK USD @ NASDAQ | Pacmio.GapFilter | 13%
[BNGO] STOCK USD @ NASDAQ | Pacmio.GapFilter | 14%
[PLUG] STOCK USD @ NASDAQ | Pacmio.GapFilter | 14%
[OCGN] STOCK USD @ NASDAQ | Pacmio.GapFilter | 15%
[FCEL] STOCK USD @ NASDAQ | Pacmio.GapFilter | 15%
[EBON] STOCK USD @ NASDAQ | Pacmio.GapFilter | 15%
[ZOM] STOCK USD @ AMEX | Pacmio.GapFilter | 15%
[ASXC] STOCK USD @ AMEX | Pacmio.GapFilter | 16%
[FTFT] STOCK USD @ NASDAQ | Pacmio.GapFilter | 17%
[NSPR] STOCK USD @ AMEX | Pacmio.GapFilter | 17%
[SOS] STOCK USD @ NYSE | Pacmio.GapFilter | 17%
[YINN] STOCK USD @ ARCA | Pacmio.GapFilter | 17%
[RIOT] STOCK USD @ NASDAQ | Pacmio.GapFilter | 20%
[MARA] STOCK USD @ NASDAQ | Pacmio.GapFilter | 21%


[SOS] STOCK USD @ NYSE | Pacmio.GapFilter | 17%
*/
