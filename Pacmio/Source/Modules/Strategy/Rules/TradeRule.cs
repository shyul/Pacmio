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

            //Column_PnL = new NumericColumn(Name + "_PnL", "PnL");
        }

        public string Name => Indicator.Name;

        public BarFreq BarFreq { get; private set; }



        public int TrainingDays { get; set; } = 2;

        public int TradingDays { get; set; } = 1;


        //public NumericColumn Column_PnL { get; }

        public Range<Time> TimeRange { get; set; } = new Range<Time>(new Time(9, 30), new Time(16, 0));

        // Time Period

        public Indicator Indicator { get; set; } = new Indicator();


        public IndicatorParameter Parameter { get; } = new IndicatorParameter();




        public int Order { get => IsManuallyAdded ? m_order : int.MinValue; set => m_order = IsManuallyAdded ? value : int.MinValue; }
        private int m_order = int.MinValue;

        public bool IsManuallyAdded { get; set; } = false;

        public bool Equals(TradeRule other) => Name == other.Name;



        //public SimulationResult SimulationResult 
    }
}
