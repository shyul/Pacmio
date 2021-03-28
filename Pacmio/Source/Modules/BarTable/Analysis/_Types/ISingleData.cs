/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public interface ISingleData : IDependable
    {
        NumericColumn Column_Result { get; }
    }
}
