/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Xu;

namespace Pacmio
{
    public interface ISingleData : IDependable
    {
        NumericColumn Column_Result { get; }
    }
}
