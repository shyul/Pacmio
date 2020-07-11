/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using Pacmio;

namespace TestClient
{
    public class TestStrategy : Strategy
    {
        public TestStrategy(BarFreq freq) : base("TestTradeRule")
        {
            Analyses.Add(freq, BarTableTest.TestBarAnalysisSet);
        }
    }
}
