/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    public class News
    {
        public int ConId { get; }

        public DateTime DateTime { get; }

        public string Title { get; }

        public int Rank { get; set; }

        public string URL { get; set; }

        public string Body { get; set; }
    }
}
