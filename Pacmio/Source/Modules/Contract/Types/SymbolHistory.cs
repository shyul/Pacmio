/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    public enum SymbolStartType : uint
    {
        [EnumMember]
        Unknown = 0,

        [EnumMember]
        IPO = 1,

        [EnumMember]
        TransferIn = 5,
    }

    [Serializable, DataContract]
    public enum SymbolFateType : uint
    {
        [EnumMember]
        Unknown = 0,

        [EnumMember]
        Current = 1,

        [EnumMember]
        TransferOut = 5,

        [EnumMember]
        Defunct = 100,
    }

    [Serializable, DataContract]
    public struct SymbolHistory : IEquatable<SymbolHistory>
    {
        [DataMember]
        public Period Period { get; set; }

        [DataMember, Browsable(true), Category("IDs"), DisplayName("ISIN")]
        public string ISIN { get; set; }

        [DataMember, Browsable(true), Category("IDs"), DisplayName("Full Name")]
        public string FullName { get; set; }

        [IgnoreDataMember]
        public bool IsCurrent => Fate == SymbolFateType.Current;

        [DataMember]
        public SymbolStartType StartType { get; set; }

        [DataMember]
        public SymbolFateType Fate { get; set; }


        public bool Equals(SymbolHistory other)
        {
            return false; // throw new NotImplementedException();
        }
        /*
        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(SymbolHistory left, SymbolHistory right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SymbolHistory left, SymbolHistory right)
        {
            return !(left == right);
        }*/
    }
}
