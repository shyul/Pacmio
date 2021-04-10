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

namespace Pacmio.Analysis
{
    public class ExecutionDatum : IDatum
    {
        public TradeExecutionType Type
        {
            get => m_Type;

            set
            {
                TradeExecutionType type = value;

                if (type != TradeExecutionType.None)
                    if (type == m_Type)
                        DecisionCount++;
                    else
                        DecisionCount = 1;
                else
                    DecisionCount = 0;

                m_Type = type;
            }
        }

        private TradeExecutionType m_Type = TradeExecutionType.None;

        public double Size { get; } = 1;

        public double LimitPrice { get; } = double.NaN;

        public double AuxPrice { get; } = double.NaN;

        public int DecisionCount { get; private set; } = 0;
    }
}