/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

namespace Pacmio
{
    public interface IMarketDepth
    {
        bool Request_MarketDepth();

        void Cancel_MarketDepth();

    }
}
