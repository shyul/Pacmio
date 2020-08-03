/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Pacmio
{
    public class GainPointAnalysis : BarAnalysis
    {
        public GainPointAnalysis(BarAnalysis ba, int interval, int minimumPeakProminence, int minimumTrendStrength = 5, double tolerance = 0.01)
        {
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

        public GainPointAnalysis(int interval, int minimumPeakProminence, int minimumTrendStrength = 5, double tolerance = 0.01)
        {
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

        public GainPointColumn Result_Column { get; private set; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            int min_start = bap.StopPt - Interval - 1;
            if (bap.StartPt > min_start) bap.StartPt = min_start;
            if (bap.StartPt < 0) bap.StartPt = 0;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                GainPointDatum gpd = new GainPointDatum();
                b[Result_Column] = gpd;

                if (SourceData is null)
                {
                    double prominence = b[Bar.Column_Peak];
                    double trendStrength = b[Bar.Column_TrendStrength];

                    if (prominence > MinimumPeakProminence && trendStrength > MinimumTrendStrength)
                        gpd.PositiveList[i] = (b.Time, b[Bar.Column_High], prominence, trendStrength);
                    else if (prominence < -MinimumPeakProminence && trendStrength < -MinimumTrendStrength)
                        gpd.NegativeList[i] = (b.Time, b[Bar.Column_Low], prominence, trendStrength);
                }
                else
                {
                    double value = b[SourceData.Result_Column];
                    double prominence = b[GainAnalysis.Column_Peak];
                    double trendStrength = b[GainAnalysis.Column_TrendStrength];

                    if (prominence > MinimumPeakProminence && trendStrength > MinimumTrendStrength)
                        gpd.PositiveList[i] = (b.Time, value, prominence, trendStrength);
                    else if (prominence < -MinimumPeakProminence && trendStrength < -MinimumTrendStrength)
                        gpd.NegativeList[i] = (b.Time, value, prominence, trendStrength);
                }
            }

            #endregion Calculation
        }
    }
}
