/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using Xu;

namespace Pacmio
{
    public abstract class SignalAnalysis : BarAnalysis, IEquatable<SignalAnalysis>
    {
        protected SignalAnalysis(TimePeriod tif, BarFreq barFreq, PriceType priceType)
        {
            TimeInForce = tif;
            BarFreq = barFreq;
            PriceType = priceType;
        }

        /// <summary>
        /// Example: Only trade 9:30 AM to 10 AM
        /// </summary>
        public TimePeriod TimeInForce { get; }

        public BarFreq BarFreq { get; }

        public PriceType PriceType { get; }

        public SignalColumn Column_Result { get; protected set; }

        public Color BullishColor
        {
            get
            {
                return BullishTheme.ForeColor;
            }
            set
            {
                BullishTheme.ForeColor = value;
                BullishTheme.FillColor = value.Opaque(64);
                BullishTheme.EdgeColor = value.Opaque(255);
            }
        }

        public Color BearishColor
        {
            get
            {
                return BearishTheme.ForeColor;
            }
            set
            {
                BearishTheme.ForeColor = value;
                BearishTheme.FillColor = value.Opaque(64);
                BearishTheme.EdgeColor = value.Opaque(255);
            }
        }

        public ColorTheme BullishTheme { get; set; } = new();

        public ColorTheme BearishTheme { get; set; } = new();

        #region Equality

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode() ^ TimeInForce.GetHashCode() ^ BarFreq.GetHashCode() ^ PriceType.GetHashCode();
        public bool Equals(SignalAnalysis other) => GetType() == other.GetType() && Name == other.Name && TimeInForce == other.TimeInForce && BarFreq == other.BarFreq && PriceType == other.PriceType;
        public static bool operator !=(SignalAnalysis s1, SignalAnalysis s2) => !s1.Equals(s2);
        public static bool operator ==(SignalAnalysis s1, SignalAnalysis s2) => s1.Equals(s2);
        public override bool Equals(object other) => other is SignalAnalysis ba && Equals(ba);

        #endregion Equality
    }
}
