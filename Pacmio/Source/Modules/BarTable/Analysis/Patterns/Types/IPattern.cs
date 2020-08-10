/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Pacmio
{
    public interface IPattern
    {
        string Name { get; }

        int TestInterval { get; }

        int MaximumResultCount { get; }

        double Tolerance { get; }

        bool ChartEnabled { get; }
    }
}
