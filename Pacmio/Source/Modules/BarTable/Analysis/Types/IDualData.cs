/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************
/// 
using System.Drawing;
using Xu;

namespace Pacmio
{
    public interface IDualData : IDependable
    {
        NumericColumn Column_High { get; }

        NumericColumn Column_Low { get; }

        Color UpperColor { get; }

        Color LowerColor { get; }
    }
}
