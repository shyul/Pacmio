﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public enum SingleColumnType : int
    {
        None = 0,

        Above,

        Below,

        Within,

        //Expansion,

        //Contraction,

        CrossUp,

        CrossDown,

        ExitBelow,

        EnterFromBelow,

        ExitAbove,

        EnterFromAbove,
    }
}