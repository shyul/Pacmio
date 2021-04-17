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
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class CrossExecution : Indicator
    {
        public CrossExecution(BarFreq barFreq, PriceType type, int fast = 5, int slow = 20) : base(barFreq, type)
        {
            Fast_SingleData = new SMA(fast) { Color = Color.Orange };
            Slow_SingleData = new SMA(slow) { Color = Color.YellowGreen };

            DualDataSignal = new DualDataSignal(Fast_SingleData, Slow_SingleData);

            string label = "(" + Fast_SingleData.Name + "," + Slow_SingleData.Name + ")";
            GroupName = Name = GetType().Name + label;



            DualDataSignal.AddChild(this);


            SignalColumns = new SignalColumn[] { DualDataSignal.Column_Result };
            SignalSeries = new(this);
            BarAnalysisSet = new(this);
        }

        public ISingleData Fast_SingleData { get; }

        public ISingleData Slow_SingleData { get; }

        public DualDataSignal DualDataSignal { get; }

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}


