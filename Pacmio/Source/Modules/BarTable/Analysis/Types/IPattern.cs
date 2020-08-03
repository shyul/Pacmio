/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Pacmio
{
    public interface IPattern
    {
        int Interval { get; }

        int MinimumRank { get; }

        double Tolerance { get; }
    }
}
