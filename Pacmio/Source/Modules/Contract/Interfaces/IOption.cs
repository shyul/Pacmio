/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Pacmio
{
    public interface IOption
    {
        string Right { get; set; }

        double Strike { get; set; }

        string Multiplier { get; set; }

        string LastTradeDateOrContractMonth { get; set; }
    }
}
