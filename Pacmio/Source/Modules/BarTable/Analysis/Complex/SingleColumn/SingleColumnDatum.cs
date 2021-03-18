/// ***************************************************************************
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

namespace Pacmio.Analysis
{
    public class SingleColumnDatum : IDatum
    {
        public SingleColumnType Type { get; set; } = SingleColumnType.None;

        //public List<SingleColumnType> List { get; } = new List<SingleColumnType>();
    }
}
