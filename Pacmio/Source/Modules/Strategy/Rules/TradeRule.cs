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
    public class TradeRule
    {
        public Contract Contract { get; set; }

        // Time Period

        public Indicator Indicator { get; set; } = new Indicator();


        public IndicatorParameter Parameter { get; } = new IndicatorParameter();


        public string Name => Indicator.Name;

        public int Order { get => IsManuallyAdded ? m_order : int.MinValue; set => m_order = IsManuallyAdded ? value : int.MinValue; }
        private int m_order = int.MinValue;

        public bool IsManuallyAdded { get; set; } = false;
    }
}
