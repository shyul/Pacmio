﻿/// ***************************************************************************
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
        protected SignalAnalysis(BarFreq barFreq, PriceType priceType = PriceType.Trades)
        {
            BarFreq = barFreq;
            PriceType = priceType;
        }

        public BarFreq BarFreq { get; }

        public PriceType PriceType { get; }

        public abstract SignalColumn Column_Result { get; }

        public Color BullishColor
        {
            get
            {
                return BullishTheme.ForeColor;
            }
            protected set
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
            protected set
            {
                BearishTheme.ForeColor = value;
                BearishTheme.FillColor = value.Opaque(64);
                BearishTheme.EdgeColor = value.Opaque(255);
            }
        }

        public ColorTheme BullishTheme { get; set; } = new();

        public ColorTheme BearishTheme { get; set; } = new();

        #region Equality

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode() ^ BarFreq.GetHashCode() ^ PriceType.GetHashCode();
        public bool Equals(SignalAnalysis other) => GetType() == other.GetType() && Name == other.Name && BarFreq == other.BarFreq && PriceType == other.PriceType;
        public static bool operator !=(SignalAnalysis s1, SignalAnalysis s2) => !s1.Equals(s2);
        public static bool operator ==(SignalAnalysis s1, SignalAnalysis s2) => s1.Equals(s2);
        public override bool Equals(object other) => other is SignalAnalysis ba && Equals(ba);

        #endregion Equality
    }
}
