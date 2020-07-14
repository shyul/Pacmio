/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;
using Xu;

namespace Pacmio
{
    public class DualDataIndicator : Indicator
    {
        protected DualDataIndicator() { }

        public DualDataIndicator(NumericColumn fast_column, NumericColumn slow_column)
        {
            Fast_Column = fast_column;
            Slow_Column = slow_column;

            string label = "(" + Fast_Column.Name + "," + Slow_Column.Name + ")";
            GroupName = Name = GetType().Name + label;

            SignalColumn = new SignalColumn(Name, label);
            SignalColumns = new SignalColumn[] { SignalColumn };
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Fast_Column.GetHashCode() ^ Slow_Column.GetHashCode();

        public NumericColumn Fast_Column { get; protected set; }

        public NumericColumn Slow_Column { get; protected set; }

        #endregion Parameters

        #region Calculation

        public SignalColumn SignalColumn { get; protected set; }

        public Dictionary<DualDataType, double[]> TypeToScore { get; } = new Dictionary<DualDataType, double[]>
        {
            { DualDataType.Above, new double[] { 0.25 } },
            { DualDataType.Below, new double[] { -0.25 } },
            { DualDataType.Expansion, new double[] { 1 } },
            { DualDataType.Contraction, new double[] { -0.25 } },
            { DualDataType.CrossUp, new double[] { 3, 2, 1 } },
            { DualDataType.CrossDown, new double[] { -3, -2, -1 } },
            { DualDataType.TrendUp, new double[] { 1 } },
            { DualDataType.TrendDown, new double[] { -1 } },
        };

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            if (bap.StartPt < 1)
            {
                bap.StartPt = 1;
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                SignalDatum sd = bt[i][SignalColumn];

                var (points, description) = SignalTool.DualDataSignal(bt, i, Fast_Column, Slow_Column, TypeToScore);

                if (i > 0)
                {
                    SignalDatum sd_1 = bt[i - 1][SignalColumn];
                    sd.Set(points, description, sd_1);
                }
                else
                    sd.Set(points, description);
            }

        }

        #endregion Calculation
    }
}
