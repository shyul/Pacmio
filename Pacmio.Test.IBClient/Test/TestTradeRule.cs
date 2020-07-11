using Pacmio;

namespace TestClient
{
    public class TestTradeRule : Strategy
    {
        public TestTradeRule(BarFreq freq) : base("TestTradeRule")
        {
            Analyses.Add(freq, BarTableTest.TestBarAnalysisSet);
        }
    }
}
