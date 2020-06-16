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
    /// representing a leg within combo orders.
    /// </summary>
    public class ComboLeg //: IContract
    {
        public ComboLeg(int conId, int ratio, string action, string exchange, int openClose, int shortSaleSlot, string designatedLocation, int exemptCode)
        {
            ConId = conId;
            Ratio = ratio;
            Action = action;
            Exchange = exchange;
            OpenClose = openClose;
            ShortSaleSlot = shortSaleSlot;
            DesignatedLocation = designatedLocation;
            ExemptCode = exemptCode;
        }

        /// <summary>
        /// The Contract's IB's unique id
        /// </summary>
        public int ConId { get; set; }

        /// <summary>
        /// Select the relative number of contracts for the leg you are constructing.
        /// To help determine the ratio for a specific combination order, refer to the Interactive Analytics section of the User's Guide.
        /// </summary>
        public int Ratio { get; set; }

        /// <summary>
        /// The side (buy or sell) of the leg:\n
        /// - For individual accounts, only BUY and SELL are available. SSHORT is for institutions.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// The destination exchange to which the order will be routed.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// Specifies whether an order is an open or closing order.
        /// For instituational customers to determine if this order is to open or close a position.
        /// 0 - Same as the parent security. This is the only option for retail customers.\n
        /// 1 - Open. This value is only valid for institutional customers.\n
        /// 2 - Close. This value is only valid for institutional customers.\n
        /// 3 - Unknown
        /// </summary>
        public int OpenClose { get; set; }

        public const int SAME = 0;
        public const int OPEN = 1;
        public const int CLOSE = 2;
        public const int UNKNOWN = 3;

        /// <summary>
        /// For stock legs when doing short selling.
        /// Set to 1 = clearing broker, 2 = third party
        /// </summary>
        public int ShortSaleSlot { get; set; }

        /// <summary>
        /// When ShortSaleSlot is 2, this field shall contain the designated location.
        /// </summary>
        public string DesignatedLocation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ExemptCode { get; set; }

        public override int GetHashCode() => ConId;

        public bool Equals(Contract other)
        {
            if (this is null) return (other is null);
            else if (other is null) return (this is null);
            else
                return (ConId == other.ConId);
        }

        public override bool Equals(object obj)
        {
            /*
            if (object.ReferenceEquals(person1, null))
                return object.ReferenceEquals(person2, null);
                https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
             */
            if (obj is null)
                return this is null;
            else if (obj.GetType().IsSubclassOf(typeof(Contract)))
                return Equals((Contract)obj);
            else
                return false;
        }

        public static bool operator ==(ComboLeg s1, Contract s2) => s1.Equals(s2);
        public static bool operator !=(ComboLeg s1, Contract s2) => !s1.Equals(s2);
    }
}
