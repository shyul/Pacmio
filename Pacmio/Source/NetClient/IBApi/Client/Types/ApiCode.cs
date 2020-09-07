/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio.IB
{
    [AttributeUsage(AttributeTargets.Field), Serializable, DataContract]
    public sealed class ApiCode : Attribute
    {
        public ApiCode(string code) => Code = code;

        public string Code { get; private set; }

        public static (bool Valid, string Code) GetIbCode(Enum eu)
        {
            (bool IsValid, ApiCode Result) = eu.GetAttribute<ApiCode>();

            if (IsValid)
                return (true, Result.Code);
            else
                return (false, string.Empty);
        }

        public static (bool Valid, T Enum) GetEnum<T>(string code) where T : Enum//  struct, IConvertible
        {
            foreach (T item in (T[])Enum.GetValues(typeof(T)))
            {
                (bool IsValid, ApiCode Result) = item.GetAttribute<ApiCode>();
                if (IsValid && Result.Code == code) return (true, item);
            }

            return (false, default(T));
        }
    }
}
