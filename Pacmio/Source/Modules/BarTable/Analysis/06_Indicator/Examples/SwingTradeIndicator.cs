/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// 3 Criteria for Finding Swing Stocks
/// https://youtu.be/GHG3Kf-FYvw
/// 
/// 1. Volatility: 
///     https://www.investopedia.com/terms/v/volatility.asp#:~:text=Volatility%20is%20a%20statistical%20measure,same%20security%20or%20market%20index.
///     https://www.wallstreetmojo.com/volatility-formula/
///     
/// 2. A stock is a good deal, RSI is below 30, buying low and selling high. No buying high and expect to sell higher...
///     Cross Below 30, within 5 Bars (4 hours)
/// 
/// 3. Signs of up trend -- using trendline
/// 
/// 
/// 4. Descent about of trading volume
///     100,000
/// 
/// Trailing 180 days.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public class SwingTradeIndicator : Indicator
    {

        public override IEnumerable<SignalColumn> SignalColumns { get; } = new List<SignalColumn>();


        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}
