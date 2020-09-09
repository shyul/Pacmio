/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Reflection;
using System.IO;
using Xu;

namespace Pacmio.IB
{
    [Serializable, DataContract]
    public class Parameters
    {
        [DataMember]
        public int MaximumSubscription { get; private set; } = 99;

        [DataMember]
        public Dictionary<(string ExhcangeCode, string TypeCode), (string ListingExch, string ServiceDataType, int AggGroup)> ExchangeDescription { get; private set; }
            = new Dictionary<(string ExhcangeCode, string TypeCode), (string ListingExch, string ServiceDataType, int AggGroup)>();

        [DataMember]
        public Dictionary<string, Dictionary<string, string>> SmartComponents { get; private set; } = new Dictionary<string, Dictionary<string, string>>();

        public Dictionary<string, string> GetSmartComponents(string bboExchange)
        {
            if (!SmartComponents.ContainsKey(bboExchange))
                SmartComponents.Add(bboExchange, new Dictionary<string, string>());

            return SmartComponents[bboExchange];
        }

        public static string FileName => Root.ResourcePath + "IBParameters.Json";
    }


    public static partial class Client
    {
        public static Parameters Parameters { get; private set; } 



        public static void Save()
        {
            lock (Parameters)
            {
                Parameters.SerializeJsonFile(Parameters.FileName);
            }
        }

        public static void Load()
        {
            if (Parameters is null)
            {
                if (File.Exists(Parameters.FileName))
                    Parameters = Serialization.DeserializeJsonFile<Parameters>(Parameters.FileName);
                else
                    Parameters = new Parameters();
            }
        }
    }
}
