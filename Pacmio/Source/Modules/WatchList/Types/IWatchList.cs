/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{


    public interface IWatchList
    {
        void Start();

        void Stop();

        ICollection<Contract> Snapshot();

        ICollection<Contract> List { get; }

        // Add event / Interrupt source
    }
}
