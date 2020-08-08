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
        NumericColumn Column_High { get; }

        NumericColumn Column_Low { get; }
    }
}
