/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    public class StrategyDatum : IDatum
    {
        public StrategyDatum(Bar b, Strategy s)
        {
            Bar = b;
            Strategy = s;
        }

        public Strategy Strategy { get; }

        public Bar Bar { get; }

        public Contract Contract => Bar.Table.Contract;

        public MarketData MarketData => Contract.MarketData;

        #region Execution

        public IExecution Execution
        {
            get => m_Execution;

            set
            {
                if (value is IExecution exec)
                {
                    if (m_Execution is null || m_Execution.GetType() != exec.GetType())
                    {
                        DecisionCount++;
                    }
                    else
                    {
                        DecisionCount = 1;
                    }

                    m_Execution = exec;
                }
                else
                {
                    m_Execution = null;
                    DecisionCount = 0;
                }
            }
        }

        private IExecution m_Execution = null;

        public int DecisionCount { get; private set; } = 0;

        #endregion Execution

        #region Position

        // A. Simulation Mode.
        // B. Actual Trade Mode. -- Keep update the last Bar's Datum Info
        // -- Run Snapshot first during the simulation
        public void SnapshotPosition(bool isLive = false)
        {
            if (isLive) // check if the bar is Live?
            {
                if (PositionDatum is null) PositionDatum = new PositionDatum();


                // Get Account Numner from Strategy
                // Find PositionInfo
                // Copy it from there...
                PositionDatum.Quantity = 0;
            }
            else if (PositionDatum is null)
            {
                if (Bar.Bar_1 is Bar b_1 && b_1[Strategy] is StrategyDatum sd_1 && sd_1.PositionDatum is PositionDatum psd)
                    PositionDatum = new PositionDatum(psd);
                else
                    PositionDatum = new PositionDatum();
            }
        }


        public PositionDatum PositionDatum { get; private set; }

        public double PnL => PositionDatum.Cost == 0 ? 0 : PositionDatum.Quantity * (Bar.Close - PositionDatum.AveragePrice);

        public double PnL_Percent => PositionDatum.Cost == 0 ? 0 : 100 * (Bar.Close - PositionDatum.AveragePrice) / PositionDatum.AveragePrice;

        #endregion Position
    }


    public class PositionDatum 
    {
        public PositionDatum() 
        {
            Quantity = 0;
            AveragePrice = double.NaN;
        }

        public PositionDatum(PositionDatum psd) 
        {
            Quantity = psd.Quantity;
            AveragePrice = psd.AveragePrice;
        }

        public double Quantity { get; set; }

        public double AveragePrice { get; set; }

        public double Cost => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);
    }
}