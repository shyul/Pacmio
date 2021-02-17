/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// 1. Pre-Market Gapper (10%, total pre-market volume, social media sentiment)
/// 2. Halted and Resumed
/// 3. Low Share out-standing / Small-cap / and high *** volume spike ***
/// 4. Reversal??
/// 5. Social Mention
/// 
/// ***************************************************************************

using Pacmio.IB;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    public abstract class Execution : BarAnalysis
    {

    }
}
