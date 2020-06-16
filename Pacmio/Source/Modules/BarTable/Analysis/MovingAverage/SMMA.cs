/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Ref 1: https://www.forex.in.rs/smoothed-moving-average/
/// 
/// ***************************************************************************
using Xu;

namespace Pacmio
{
    public sealed class SMMA : SMA
    {
        public SMMA(int interval) : this(BarTable.Column_Close, interval) { }

        public SMMA(NumericColumn column, int interval) : base(column, interval)
        {
            Description = "Smoothed Moving Average " + LineSeries.Label;
        }

        #region Calculation

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            double sum = (bap.StartPt == 0) ? bt[0][Column] * Interval : bt[bap.StartPt - 1][Result_Column] * Interval;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (i != 0)
                    sum = sum - bt[i - 1][Result_Column] + bt[i][Column];

                bt[i][Result_Column] = sum / Interval;
            }
        }

        #endregion Calculation
    }
}
