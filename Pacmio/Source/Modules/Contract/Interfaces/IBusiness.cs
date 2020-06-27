/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Xu;

namespace Pacmio
{
    public interface IBusiness : IEquatable<BusinessInfo>
    {
        string Name { get; }

        Exchange Exchange { get; }

        string TypeName { get; }

        string FullName { get; }

        string ISIN { get; set; }

        BusinessInfo BusinessInfo { get; }
    }
}
