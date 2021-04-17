/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;
using Pacmio.Analysis;

namespace Pacmio
{
    public abstract class Filter : Indicator
    {
        protected Filter(BarFreq barFreq, PriceType type) : base(barFreq, type) { }

        public virtual Range<double> PriceRange { get; }

        public virtual Range<double> VolumeRange { get; }

        public abstract SignalColumn SignalColumn { get; }
    }
}
