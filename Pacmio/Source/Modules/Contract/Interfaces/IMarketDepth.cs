/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Pacmio
{
    public interface IMarketDepth : IBidAsk
    {
        bool Request_MarketDepth();

        void Cancel_MarketDepth();

        //DateTime LastTradeTime { get; }

        Dictionary<int, (DateTime Time, double Price, double Size, Exchange MarketMaker)> MarketDepth { get; }
    }
}
