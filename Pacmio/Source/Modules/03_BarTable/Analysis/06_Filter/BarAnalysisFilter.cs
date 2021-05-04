/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Filter 1) Signal Set; 2) Signal Threshold Range<double>(), within? / outside?; 3) Ranking Column?
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    /// <summary>
    /// Flawed, direction -> no score, but ranking!!!
    /// </summary>
    public sealed class BarAnalysisFilter : BarAnalysisSet
    {
        public BarAnalysisFilter(IEnumerable<SignalAnalysis> SignalList, double bullishLimit, double bearishLimit, NumericColumn rankColumn) : base(SignalList)
        {
            BullishLimit = bullishLimit;
            BearishLimit = bearishLimit;
            Column_Rank = rankColumn;
        }

        public NumericColumn Column_Rank { get; }

        public double BullishLimit { get; }

        public double BearishLimit { get; }

        public (bool bullish, bool bearish, double rank) Calculate(Bar b)
        {
            bool bullish = b.GetSignalScore(SignalList).Bullish >= BullishLimit;
            bool bearish = b.GetSignalScore(SignalList).Bearish <= BearishLimit;
            return (bullish, bearish, b[Column_Rank]);
        }

        public FilterScanResult RunScan(BarTableSet bts, Period pd)
        {
            bts.CalculateRefresh(this);
            BarTable bt = bts[TimeFrameList.Last()];
            var allbars = bt.Bars.Where(b => pd.Contains(b.Time));

            List<Bar> bullishBars = new();
            List<Bar> bearishBars = new();

            foreach (Bar b in allbars)
            {
                var (bullish, bearish, _) = Calculate(b);
                if (bullish) bullishBars.Add(b);
                if (bearish) bearishBars.Add(b);
            }

            return new(bts.Contract, allbars, bullishBars, bearishBars);
        }
    }

    public abstract class FilterAnalysis : SingleDataAnalysis
    {
        protected FilterAnalysis(BarFreq barFreq, PriceType priceType)
        {
            BarFreq = barFreq;
            PriceType = priceType;
        }

        public BarFreq BarFreq { get; }

        public PriceType PriceType { get; }


        public BarAnalysisList BarAnalysisList { get; }

        public FilterScanResult RunScan(BarTableSet bts, Period pd)
        {
            BarTable bt = bts[BarFreq, PriceType];
            bt.CalculateRefresh(BarAnalysisList);
            var allbars = bt.Bars.Where(b => pd.Contains(b.Time));

            List<Bar> bullishBars = new();
            List<Bar> bearishBars = new();

            foreach (Bar b in allbars)
            {
                if (b[Column_Result] > 0) bullishBars.Add(b);
                else if (b[Column_Result] < 0) bearishBars.Add(b);
            }

            return new(bts.Contract, allbars, bullishBars, bearishBars);
        }
    }



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

            //ChartEnabled = false;
        }

        protected PriceVolumeFilter(
            BarFreq barFreq = BarFreq.Daily,
            PriceType priceType = PriceType.Trades)
            : base(barFreq, priceType)
        {
        
        }

        public Range<double> VolumeRange { get; protected set; }

        public Range<double> PriceRange { get; protected set; }

        public override string Label { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                if (PriceRange.Contains(b.Typical) && VolumeRange.Contains(b.Volume))
                    b[Column_Result] = b.Typical * b.Volume;
                else
                    b[Column_Result] = 0;
            }
        }
    }

    public class GapFilter : PriceVolumeFilter
    {
        public GapFilter(
            double gapPercent = 4,
            double minVolume = 5e5,
            double maxVolume = double.MaxValue,
            double minPrice = 1,
            double maxPrice = 300,
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
