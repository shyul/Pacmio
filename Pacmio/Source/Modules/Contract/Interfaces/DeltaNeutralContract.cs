/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


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
