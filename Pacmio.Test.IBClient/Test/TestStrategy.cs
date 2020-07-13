/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Pacmio;

namespace TestClient
{
    public class TestStrategy : Strategy
    {
        public TestStrategy(BarFreq freq) : base("TestTradeRule")
        {
            Analyses.Add(freq, BarTableTest.TestBarAnalysisSet);
        }

        protected Dictionary<BarFreq, BarAnalysisSet> Analyses { get; } = new Dictionary<BarFreq, BarAnalysisSet>();

        public override void ClearBarAnalysisSet() => Analyses.Clear();

        public override BarAnalysisSet this[BarFreq freq]
        {
            get
            {
                if (Analyses.ContainsKey(freq))
                    return Analyses[freq];
                else
                    return null;
            }
            set
            {
                if (value is BarAnalysisSet bas)
                    Analyses[freq] = new BarAnalysisSet(bas);
                else if (Analyses.ContainsKey(freq))
                    Analyses.Remove(freq);
            }
        } 
    }
}
