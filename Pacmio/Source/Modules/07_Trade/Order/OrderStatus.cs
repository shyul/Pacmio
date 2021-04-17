/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Pacmio.IB;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Pacmio
{
    /// <summary>
    /// Possible Order States
    /// </summary>
    [Serializable, DataContract]
    public enum OrderStatus : int
    {
        [EnumMember, Description("Unknown")]
        UNKNOWN = 0,

        [EnumMember, ApiCode("Inactive"), Description("Inactive")]
        Inactive = 5,

        [EnumMember, Description("Started")]
        Started = 10,

        /// <summary>
        /// Indicates order has not yet been sent to IB server, 
        /// for instance if there is a delay in receiving the security definition. 
        /// Uncommonly received.
        /// </summary>
        [EnumMember, ApiCode("ApiPending"), Description("Api Pending")]
        ApiPending = 11,

        /// <summary>
        /// Indicates the order was sent from TWS,
        /// but confirmation has not been received that it has been received by the destination.
        /// Most commonly because exchange is closed.
        /// </summary>
        [EnumMember, ApiCode("PendingSubmit"), Description("Pending Submit")]
        PendingSubmit = 12,

        [EnumMember, ApiCode("PreSubmitted"), Description("Pre-Submitted")]
        PreSubmitted = 13,

        [EnumMember, ApiCode("Submitted"), Description("Submitted")]
        Submitted = 14,

        [EnumMember, ApiCode("Filled"), Description("Filled")]
        Filled = 15,

        [EnumMember, ApiCode("PendingCancel"), Description("Pending Cancel")]
        PendingCancel = 100,

        [EnumMember, ApiCode("ApiCancelled"), Description("API Cancelled")]
        ApiCancelled = 101,

        [EnumMember, ApiCode("Cancelled"), Description("Cancelled")]
        Cancelled = 102,
    }


}
