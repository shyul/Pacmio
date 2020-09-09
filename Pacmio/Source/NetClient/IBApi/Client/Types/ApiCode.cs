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

        public static T GetEnum<T>(string code) where T : Enum
        {
            /*
            foreach (T item in Enum.GetValues(typeof(T)) as T[])
            {
                (bool IsValid, ApiCode Result) = item.GetAttribute<ApiCode>();
                if (IsValid && Result.Code == code) return (true, item);
            }*/

            var res = ReflectionTool.ToArray<T>().Where(n => n.GetAttribute<ApiCode>() is ApiCode res && res.Code == code);

            if (res.Count() > 0)
                return res.First();
            else
            {
                Console.WriteLine("???? Unknown IB API Code: " + code);
                return default(T);
            }

        }
    }
}
