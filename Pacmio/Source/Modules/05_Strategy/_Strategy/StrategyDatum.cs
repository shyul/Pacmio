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
        public StrategyDatum(Bar b, Strategy s, bool isLive)
        {
            Bar = b;
            Strategy = s;

            if (isLive && s.AccountInfo is AccountInfo ac)
            {
                PositionInfo pi = ac[Contract];
                Quantity = pi.Quantity;
                AveragePrice = pi.AverageEntryPrice;

                if (Bar.Bar_1 is Bar b_1 && b_1[Strategy] is StrategyDatum sd_1) 
                {
                    sd_1.Quantity = Quantity;
                    sd_1.AveragePrice = AveragePrice;
                }
            }
            else if (Bar.Bar_1 is Bar b_1 && b_1[Strategy] is StrategyDatum sd_1)
            {
                Quantity = sd_1.Quantity;
                AveragePrice = sd_1.AveragePrice;
            }
        }

        public Strategy Strategy { get; }

        public Bar Bar { get; }

        public Contract Contract => Bar.Table.Contract;

        public MarketData MarketData => Contract.MarketData;

        #region Execution

        public IDecision Decision
        {
            get => m_Decision;

            set
            {
                if (value is IDecision exec)
                {
                    if (m_Decision is null || m_Decision.GetType() != exec.GetType())
                    {
                        DecisionCount++;
                    }
                    else
                    {
                        DecisionCount = 1;
                    }

                    m_Decision = exec;
                }
                else
                {
                    m_Decision = null;
                    DecisionCount = 0;
                }
            }
        }

        private IDecision m_Decision = null;

        public int DecisionCount { get; private set; } = 0;




        //public 

        #endregion Execution

        #region Position Track

        // A. Simulation Mode.
        // B. Actual Trade Mode. -- Keep update the last Bar's Datum Info
        // -- Run Snapshot first during the simulation

        public void AnalyzePosition(bool isLive)
        {
            // Entry Exec is proactive
            // Exit Exec is passive.

        }


        //
        public List<IExecution> Executions { get; } = new();




        public double Quantity { get; private set; } = 0;

        public double AveragePrice { get; private set; } = double.NaN;

        public double Cost => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        public double PnL => Cost == 0 ? 0 : Quantity * (Bar.Close - AveragePrice);

        public double PnL_Percent => Cost == 0 ? 0 : 100 * (Bar.Close - AveragePrice) / AveragePrice;

        #endregion Position Track
    }



}