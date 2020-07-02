/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using Xu;

namespace Pacmio
{
    public class TradeRule : IEquatable<TradeRule>, ITradeSetting
    {
        public TradeRule(string name)
        {
            Name = name;
        }

        public string Name { get; set; } = "Default TradeRule";

        public int Order { get; set; } = 0;

        public Dictionary<BarFreq, BarAnalysisSet> TimeFrameList { get; set; } = new Dictionary<BarFreq, BarAnalysisSet>();

        #region Trading Timing

        public int TrainingDays { get; set; } = 2;

        public int TradingDays { get; set; } = 1;

        /// <summary>
        /// Use MultiTime here.
        /// </summary>
        public Range<Time> TradingTimeRange { get; set; } = new Range<Time>(new Time(9, 30), new Time(16, 0));

        #endregion Trading Timing

        public bool Equals(TradeRule other) => other is TradeRule tr && Name == tr.Name;
    }
}
