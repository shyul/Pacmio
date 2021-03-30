/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public class HigherTimeFrameDatum : IDatum
    {
        public HigherTimeFrameDatum(Bar b_ht) => BarHT = b_ht;

        public Bar BarHT { get; }
    }
}
