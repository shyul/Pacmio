﻿/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Xu;

namespace Pacmio.Analysis
{
    public interface ISingleComplex : IDependable
    {
        DatumColumn Column_Result { get; }
    }
}
