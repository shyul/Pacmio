/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Settings Types for Pacmio
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace Pacmio
{
    [Serializable, DataContract]
    public sealed class PacSettings
    {
        [DataMember, Browsable(true), Category("1: Basic"), DisplayName("Workplace Path")]
        [Description("Workplace Path.")]
        public string WorkplacePath { get; set; } = "D:\\Pacmio\\";

        [DataMember, DefaultValue(typeof(string), "C:\\Temp\\Pacmio\\"), Browsable(true), Category("1: Basic"), DisplayName("Cache Path")]
        [Description("Cache Path.")]
        public string CachePath { get; set; } = Path.GetTempPath() + "Pacmio\\";

        [DataMember, Browsable(true), Category("1: Basic")]
        public string QuandlKey { get; set; } = "key";

        #region IBClient

        [DataMember, Browsable(true), Category("2: IB Client")]
        public string IBServerAddress { get; set; } = "127.0.0.1";

        [DataMember, Browsable(true), Category("2: IB Client")]
        public int IBServerPort { get; set; } = 15062;

        [DataMember, Browsable(true), Category("2: IB Client")]
        public int IBClientId { get; set; } = 180;

        [DataMember, Browsable(true), Category("2: IB Client")]
        public int IBTimeout { get; set; } = 1000;

        #endregion IBClient

        #region Trade-Ideas

        [DataMember, Browsable(true), Category("3: Trade-Ideas Client")]
        public string TIServerAddress { get; set; } = "server.trade-ideas.com";

        [DataMember, Browsable(true), Category("3: Trade-Ideas Client")]
        public int TIServerPort { get; set; } = 443;

        [DataMember, Browsable(true), Category("3: Trade-Ideas Client")]
        public string TIUsername { get; set; } = "phil";

        [DataMember, Browsable(true), Category("3: Trade-Ideas Client")]
        public string TIPassword { get; set; } = "password";

        #endregion Trade-Ideas
    }
}
