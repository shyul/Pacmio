/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Pacmio
{
    public interface IBusiness : IEquatable<BusinessInfo>
    {
        string Name { get; }

        Exchange Exchange { get; }

        string TypeName { get; }

        string FullName { get; }

        string Industry { get; set; }

        string Category { get; set; }

        string Subcategory { get; set; }

        string ISIN { get; set; }

        BusinessInfo BusinessInfo { get; }
    }
}
