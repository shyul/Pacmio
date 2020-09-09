/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using Pacmio.Analysis;
using System;
using System.Linq;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio.IB
{
    [AttributeUsage(AttributeTargets.Field), Serializable, DataContract]
    public sealed class ApiCode : Attribute
    {
        public ApiCode(string code) => Code = code;

        public string Code { get; private set; }

        public static string GetCode(Enum eu)
        {
            if (eu.GetAttribute<ApiCode>() is ApiCode ac)
                return ac.Code;
            else
                return null;
        }

        /*
        public static (bool Valid, string Code) GetIbCode(Enum eu)
        {
            (bool IsValid, ApiCode Result) = eu.GetAttribute<ApiCode>();

            if (IsValid)
                return (true, Result.Code);
            else
                return (false, string.Empty);
        }

        */




        public static (bool Valid, T Enum) GetEnum<T>(string code) where T : Enum//  struct, IConvertible
        {
            /*
            foreach (T item in Enum.GetValues(typeof(T)) as T[])
            {
                (bool IsValid, ApiCode Result) = item.GetAttribute<ApiCode>();
                if (IsValid && Result.Code == code) return (true, item);
            }*/

            var res = (Enum.GetValues(typeof(T)) as T[]).Where(n => n.GetAttribute<ApiCode>() is ApiCode res && res.Code == code);

            if (res.Count() > 0) 
                return (true, res.First());
            else
                return (false, default(T));
        }
    }
}
