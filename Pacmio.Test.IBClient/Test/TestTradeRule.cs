using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Xu;
using Xu.Chart;
using Pacmio;
using Pacmio.IB;
using System.Threading;
using IbXmlScannerParameter;

namespace TestClient
{
    public class TestTradeRule : TradeRule
    {
        public TestTradeRule(BarFreq freq) : base("TestTradeRule") 
        {
            Analyses.Add(freq, BarTableTest.TestBarAnalysisSet);
        }
    }
}
