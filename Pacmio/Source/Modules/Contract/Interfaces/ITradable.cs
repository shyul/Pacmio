/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Pacmio
{
    public interface ITradable : IEquatable<BusinessInfo>
    {
        string Name { get; }

        Exchange Exchange { get; }

        string TypeName { get; }

        string ISIN { get; set; }

        bool AutoExchangeRoute { get; set; }

        (bool valid, BusinessInfo bi) GetBusinessInfo();

        bool Request_MarketTicks(string param);

        void Cancel_MarketTicks();

        double Price { get; }

        DateTime LastTradeTime { get; }
    }
}
