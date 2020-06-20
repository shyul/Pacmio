/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Pacmio.IB;

namespace Pacmio
{
    /// <summary>
    /// Possible Order States
    /// </summary>
    [Serializable, DataContract]
    public enum OrderStatus : int
    {
        [ApiCode("Inactive"), Description("Inactive")]
        Inactive = 0,

        /// <summary>
        /// Indicates order has not yet been sent to IB server, 
        /// for instance if there is a delay in receiving the security definition. 
        /// Uncommonly received.
        /// </summary>
        [ApiCode("ApiPending"), Description("Api Pending")]
        ApiPending = 1,

        /// <summary>
        /// Indicates the order was sent from TWS,
        /// but confirmation has not been received that it has been received by the destination.
        /// Most commonly because exchange is closed.
        /// </summary>
        [ApiCode("PendingSubmit"), Description("Pending Submit")]
        PendingSubmit = 2,

        [ApiCode("PreSubmitted"), Description("Pre-Submitted")]
        PreSubmitted = 3,

        [ApiCode("Submitted"), Description("Submitted")]
        Submitted = 4,

        [ApiCode("Filled"), Description("Filled")]
        Filled = 5,

        [ApiCode("PendingCancel"), Description("Pending Cancel")]
        PendingCancel = 100,

        [ApiCode("ApiCancelled"), Description("API Cancelled")]
        ApiCancelled = 101,

        [ApiCode("Cancelled"), Description("Cancelled")]
        Cancelled = 102,
    }
}
