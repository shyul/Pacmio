/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - shyu.lee@gmail.com
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// Allows to specify a price on an order's leg
    /// </summary>
    public class OrderComboLeg
    {
        public OrderComboLeg() { }

        public OrderComboLeg(double p_price)
        {
            Price = p_price;
        }

        /// <summary>
        /// The order's leg's price
        /// </summary>
        public double Price { get; set; } = double.NaN;

        public override bool Equals(object other)
        {
            if (this is null || other is null) // https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
                return false;
            if (other is OrderComboLeg ocl)
                return this == ocl;
            else
                return false;
        }

        public override int GetHashCode() => Price.GetHashCode();
    }
}
