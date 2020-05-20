/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// Delta-Neutral Contract.
    /// </summary>
    public class DeltaNeutralContract
    {
        /// <summary>
        /// The unique contract identifier specifying the security. Used for Delta-Neutral Combo contracts.
        /// </summary>
        public int ConId { get; set; }

        /// <summary>
        /// The underlying stock or future delta. Used for Delta-Neutral Combo contracts.
        /// </summary>
        public double Delta { get; set; }

        /// <summary>
        /// The price of the underlying. Used for Delta-Neutral Combo contracts.
        /// </summary>
        public double Price { get; set; }
    }
}
