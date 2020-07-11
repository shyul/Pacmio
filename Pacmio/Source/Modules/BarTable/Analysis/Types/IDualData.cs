/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Xu;

namespace Pacmio
{
    public interface IDualData : IDependable
    {
        NumericColumn High_Column { get; }

        NumericColumn Low_Column { get; }
    }
}
