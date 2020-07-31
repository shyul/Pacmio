/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Pacmio
{
    public class TrendAnalysis : BarAnalysis, IPattern
    {
        public TrendAnalysis(BarAnalysis ba, int interval, int minimumPeakProminence, int minimumTrendStrength = 5, double tolerance = 0.01, bool isLogarithmic = false)
        {
            IsLogarithmic = isLogarithmic;
            Tolerance = tolerance;
            Interval = interval;
            MinimumPeakProminence = minimumPeakProminence;
            MinimumTrendStrength = minimumTrendStrength;

            //string label = ;
            //Name = ;

            if (ba is ISingleData isd)
            {
                SourceData = isd;
                GainAnalysis = new Gain(SourceData.Result_Column, interval);
            }
            else
                throw new ArgumentException("BarAnalysis has to be ISingleData");

            if (ba is IChartSeries ics)
                ChartSeries = ics;
            else
                ChartSeries = null;
        }

        public TrendAnalysis(int interval, int minimumPeakProminence, int minimumTrendStrength = 5, double tolerance = 0.01, bool isLogarithmic = false)
        {
            IsLogarithmic = isLogarithmic;
            Tolerance = tolerance;
            Interval = interval;
            MinimumPeakProminence = minimumPeakProminence;
            MinimumTrendStrength = minimumTrendStrength;

            //string label = ;
            //Name = ;

            SourceData = null;
            ChartSeries = null;
            GainAnalysis = null;

        }

        #region Parameters

        public bool IsLogarithmic { get; }

        public virtual int Interval { get; }

        public virtual int RankLimit { get; }

        public double Tolerance { get; }

        public virtual int MinimumPeakProminence { get; }

        public virtual int MinimumTrendStrength { get; }

        #endregion Parameters

        #region Calculation

        public ISingleData SourceData { get; }

        public IChartSeries ChartSeries { get; }

        public Gain GainAnalysis { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            int min_start = bap.StopPt - Interval - 1;
            if (bap.StartPt > min_start) bap.StartPt = min_start;
            if (bap.StartPt < 0) bap.StartPt = 0;

            Dictionary<int, (DateTime time, double value, double prominence, double trendStrength)> positive_list = new Dictionary<int, (DateTime time, double value, double prominence, double trendStrength)>();
            Dictionary<int, (DateTime time, double value, double prominence, double trendStrength)> negative_list = new Dictionary<int, (DateTime time, double value, double prominence, double trendStrength)>();

            if (SourceData is null)
            {
                for (int i = bap.StartPt; i < bap.StopPt; i++)
                {
                    Bar b = bt[i];
                    double prominence = b[Bar.Column_Peak];
                    double trendStrength = b[Bar.Column_TrendStrength];

                    if (prominence > MinimumPeakProminence && trendStrength > MinimumTrendStrength)
                        positive_list[i] = (b.Time, b[Bar.Column_High], prominence, trendStrength);
                    else if (prominence < -MinimumPeakProminence && trendStrength < -MinimumTrendStrength)
                        negative_list[i] = (b.Time, b[Bar.Column_Low], prominence, trendStrength);
                }
            }
            else
            {
                for (int i = bap.StartPt; i < bap.StopPt; i++)
                {
                    Bar b = bt[i];
                    double value = b[SourceData.Result_Column];
                    double prominence = b[GainAnalysis.Column_Peak];
                    double trendStrength = b[GainAnalysis.Column_TrendStrength];

                    if (prominence > MinimumPeakProminence && trendStrength > MinimumTrendStrength)
                        positive_list[i] = (b.Time, value, prominence, trendStrength);
                    else if (prominence < -MinimumPeakProminence && trendStrength < -MinimumTrendStrength)
                        negative_list[i] = (b.Time, value, prominence, trendStrength);
                }
            }







        }

        #endregion Calculation
    }


}
