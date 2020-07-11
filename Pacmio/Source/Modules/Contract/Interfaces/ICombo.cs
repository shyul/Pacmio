/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Collections.Generic;

namespace Pacmio
{
    public interface ICombo
    {
        /// <summary>
        /// The legs of a combined contract definition
        /// </summary>
        ICollection<ComboLeg> ComboLegs { get; set; }
    }
}
