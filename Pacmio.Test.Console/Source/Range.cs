using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    #region Numbers
    public static class Format
    {
        public const char Splitter = ',';
        public const char Equal = '=';
        public const char Separater = '&';

        /*
        public static string EnumDescription<T>(this T enumerationValue) where T : struct
        {
            Type type = typeof(T); // enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));
            }
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return enumerationValue.ToString();
        }
        */
    }

    [Serializable, DataContract]
    public struct RangeInt16 : IEquatable<RangeInt16> //, IEnumerable<RangeInt16>
    {
        public RangeInt16(short num)
        {
            _Max = num;
            _Min = num;
        }
        public RangeInt16(short min, short max)
        {
            if (min > max)
            {
                _Min = max;
                _Max = min;
            }
            else
            {
                _Min = min;
                _Max = max;
            }
        }
        public RangeInt16(short[] nums)
        {
            _Max = short.MinValue;
            _Min = short.MaxValue;
            Insert(nums);
        }
        public RangeInt16(string s)
        {
            string[] a = s.Split(Format.Splitter);
            if (s.Length == 2)
            {
                try
                {
                    short max = Int16.Parse(a[0]);
                    short min = Int16.Parse(a[1]);
                    if (min > max)
                    {
                        _Min = max;
                        _Max = min;
                    }
                    else
                    {
                        _Min = min;
                        _Max = max;
                    }
                }
                catch
                {
                    _Max = short.MinValue;
                    _Min = short.MaxValue;
                }
            }
            else
            {
                _Max = short.MinValue;
                _Min = short.MaxValue;
            }
        }
        public void Insert(short num)
        {
            if (num < _Min) _Min = num;
            else if (num > _Max) _Max = num;
        }
        public void Insert(short[] nums)
        {
            for (short i = 0; i < nums.Length; i++) Insert(nums[i]);
        }
        public void Reset()
        {
            _Max = short.MinValue;
            _Min = short.MaxValue;
        }

        public bool IsWithin(short num)
        {
            if (num <= _Max && num >= _Min) return true;
            else return false;
        }
        public float Ratio(short num)
        {
            return Convert.ToSingle((num - _Min) * 1.0f / Span);
        }
        public float RatioWithBoundary(short num)
        {
            float ratio = Ratio(num);
            if (ratio > 1) ratio = 1;
            else if (ratio < 0) ratio = 0;
            return ratio;
        }
        public short Portion(float ratio)
        {
            return (short)(_Min + Span * ratio).ToInt32();
        }
        public override string ToString()
        {
            return _Max.ToString() + Format.Splitter + _Min.ToString();
        }

        public short Span { get { return (short)(_Max - _Min); } }
        public short Max { get { return _Max; } }
        public short Min { get { return _Min; } }

        public bool Equals(RangeInt16 value)
        {
            if (this._Max == value._Max && this._Min == value._Min)
            {
                return true;
            }
            else
                return false;
        }

        [DataMember]
        private short _Max;

        [DataMember]
        private short _Min;
    }

    [Serializable, DataContract]
    public struct RangeUInt16 : IEquatable<RangeUInt16>
    {
        public RangeUInt16(ushort num)
        {
            _Max = num;
            _Min = num;
        }
        public RangeUInt16(ushort min, ushort max)
        {
            if (min > max)
            {
                _Min = max;
                _Max = min;
            }
            else
            {
                _Min = min;
                _Max = max;
            }
        }
        public RangeUInt16(ushort[] nums)
        {
            _Max = ushort.MinValue;
            _Min = ushort.MaxValue;
            Insert(nums);
        }
        public RangeUInt16(string s)
        {
            string[] a = s.Split(Format.Splitter);
            if (s.Length == 2)
            {
                try
                {
                    ushort max = UInt16.Parse(a[0]);
                    ushort min = UInt16.Parse(a[1]);
                    if (min > max)
                    {
                        _Min = max;
                        _Max = min;
                    }
                    else
                    {
                        _Min = min;
                        _Max = max;
                    }
                }
                catch
                {
                    _Max = ushort.MinValue;
                    _Min = ushort.MaxValue;
                }
            }
            else
            {
                _Max = ushort.MinValue;
                _Min = ushort.MaxValue;
            }
        }
        public void Insert(ushort num)
        {
            if (num < _Min) _Min = num;
            else if (num > _Max) _Max = num;
        }
        public void Insert(ushort[] nums)
        {
            for (int i = 0; i < nums.Length; i++) Insert(nums[i]);
        }
        public void Reset()
        {
            _Max = ushort.MinValue;
            _Min = ushort.MaxValue;
        }
        public bool IsWithin(ushort num)
        {
            if (num <= _Max && num >= _Min) return true;
            else return false;
        }
        public float Ratio(ushort num)
        {
            return Convert.ToSingle((num - _Min) * 1.0f / Span);
        }
        public float RatioWithBoundary(ushort num)
        {
            float ratio = Ratio(num);
            if (ratio > 1) ratio = 1;
            else if (ratio < 0) ratio = 0;
            return ratio;
        }
        public ushort Portion(float ratio)
        {
            int portion = ((float)(_Min + Span * ratio)).ToInt32();
            if (portion < 0) portion = 0;
            return Convert.ToUInt16(portion);
        }
        public override string ToString()
        {
            return _Max.ToString() + Format.Splitter + _Min.ToString();
        }

        public ushort Span { get { return (ushort)(_Max - _Min); } }
        public ushort Max { get { return _Max; } }
        public ushort Min { get { return _Min; } }

        public bool Equals(RangeUInt16 value)
        {
            if (this._Max == value._Max && this._Min == value._Min)
            {
                return true;
            }
            else
                return false;
        }

        [DataMember]
        private ushort _Max;

        [DataMember]
        private ushort _Min;
    }

    [Serializable, DataContract]
    public struct RangeInt32 : IEquatable<RangeInt32>
    {
        public RangeInt32(int num)
        {
            _Max = num;
            _Min = num;
        }
        public RangeInt32(int min, int max)
        {
            if (min > max)
            {
                _Min = max;
                _Max = min;
            }
            else
            {
                _Min = min;
                _Max = max;
            }
        }
        public RangeInt32(int[] nums)
        {
            _Max = int.MinValue;
            _Min = int.MaxValue;
            Insert(nums);
        }
        public RangeInt32(string s)
        {
            string[] a = s.Split(Format.Splitter);
            if (s.Length == 2)
            {
                try
                {
                    int max = Int32.Parse(a[0]);
                    int min = Int32.Parse(a[1]);
                    if (min > max)
                    {
                        _Min = max;
                        _Max = min;
                    }
                    else
                    {
                        _Min = min;
                        _Max = max;
                    }
                }
                catch
                {
                    _Max = int.MinValue;
                    _Min = int.MaxValue;
                }
            }
            else
            {
                _Max = int.MinValue;
                _Min = int.MaxValue;
            }
        }
        public void Insert(int num)
        {
            if (num < _Min) _Min = num;
            else if (num > _Max) _Max = num;
        }
        public void Insert(int[] nums)
        {
            for (int i = 0; i < nums.Length; i++) Insert(nums[i]);
        }
        public void Reset()
        {
            _Max = int.MinValue;
            _Min = int.MaxValue;
        }

        public bool IsWithin(int num)
        {
            if (num <= _Max && num >= _Min) return true;
            else return false;
        }
        public float Ratio(int num)
        {
            return Convert.ToSingle((num - _Min) * 1.0f / Span);
        }
        public float RatioWithBoundary(int num)
        {
            float ratio = Ratio(num);
            if (ratio > 1) ratio = 1;
            else if (ratio < 0) ratio = 0;
            return ratio;
        }
        public int Portion(float ratio)
        {
            return (_Min + Span * ratio).ToInt32();
        }
        public override string ToString()
        {
            return _Max.ToString() + Format.Splitter + _Min.ToString();
        }

        public int Span { get { return _Max - _Min; } }
        public int Max { get { return _Max; } }
        public int Min { get { return _Min; } }

        public bool Equals(RangeInt32 value)
        {
            if (this._Max == value._Max && this._Min == value._Min)
            {
                return true;
            }
            else
                return false;
        }

        [DataMember]
        private int _Max;

        [DataMember]
        private int _Min;
    }

    [Serializable, DataContract]
    public struct RangeUInt32 : IEquatable<RangeUInt32>
    {
        public RangeUInt32(uint num)
        {
            _Max = num;
            _Min = num;
        }
        public RangeUInt32(uint min, uint max)
        {
            if (min > max)
            {
                _Min = max;
                _Max = min;
            }
            else
            {
                _Min = min;
                _Max = max;
            }
        }
        public RangeUInt32(uint[] nums)
        {
            _Max = uint.MinValue;
            _Min = uint.MaxValue;
            Insert(nums);
        }
        public RangeUInt32(string s)
        {
            string[] a = s.Split(Format.Splitter);
            if (s.Length == 2)
            {
                try
                {
                    uint max = UInt32.Parse(a[0]);
                    uint min = UInt32.Parse(a[1]);
                    if (min > max)
                    {
                        _Min = max;
                        _Max = min;
                    }
                    else
                    {
                        _Min = min;
                        _Max = max;
                    }
                }
                catch
                {
                    _Max = uint.MinValue;
                    _Min = uint.MaxValue;
                }
            }
            else
            {
                _Max = uint.MinValue;
                _Min = uint.MaxValue;
            }
        }
        public void Insert(uint num)
        {
            if (num < _Min) _Min = num;
            else if (num > _Max) _Max = num;
        }
        public void Insert(uint[] nums)
        {
            for (int i = 0; i < nums.Length; i++) Insert(nums[i]);
        }
        public void Reset()
        {
            _Max = uint.MinValue;
            _Min = uint.MaxValue;
        }
        public bool IsWithin(uint num)
        {
            if (num <= _Max && num >= _Min) return true;
            else return false;
        }
        public float Ratio(uint num)
        {
            return Convert.ToSingle((num - _Min) * 1.0f / Span);
        }
        public float RatioWithBoundary(uint num)
        {
            float ratio = Ratio(num);
            if (ratio > 1) ratio = 1;
            else if (ratio < 0) ratio = 0;
            return ratio;
        }
        public uint Portion(float ratio)
        {
            int portion = ((float)(_Min + Span * ratio)).ToInt32();
            if (portion < 0) portion = 0;
            return Convert.ToUInt32(portion);
        }
        public override string ToString()
        {
            return _Max.ToString() + Format.Splitter + _Min.ToString();
        }

        public uint Span { get { return _Max - _Min; } }
        public uint Max { get { return _Max; } }
        public uint Min { get { return _Min; } }

        public bool Equals(RangeUInt32 value)
        {
            if (this._Max == value._Max && this._Min == value._Min)
            {
                return true;
            }
            else
                return false;
        }

        [DataMember]
        private uint _Max;

        [DataMember]
        private uint _Min;
    }

    [Serializable, DataContract]
    public struct RangeInt64 : IEquatable<RangeInt64>
    {
        public RangeInt64(long num)
        {
            _Max = num;
            _Min = num;
        }
        public RangeInt64(long min, long max)
        {
            if (min > max)
            {
                _Min = max;
                _Max = min;
            }
            else
            {
                _Min = min;
                _Max = max;
            }
        }
        public RangeInt64(long[] nums)
        {
            _Max = long.MinValue;
            _Min = long.MaxValue;
            Insert(nums);
        }
        public RangeInt64(string s)
        {
            string[] a = s.Split(Format.Splitter);
            if (s.Length == 2)
            {
                try
                {
                    long max = Int64.Parse(a[0]);
                    long min = Int64.Parse(a[1]);
                    if (min > max)
                    {
                        _Min = max;
                        _Max = min;
                    }
                    else
                    {
                        _Min = min;
                        _Max = max;
                    }
                }
                catch
                {
                    _Max = long.MinValue;
                    _Min = long.MaxValue;
                }
            }
            else
            {
                _Max = long.MinValue;
                _Min = long.MaxValue;
            }
        }
        public void Insert(long num)
        {
            if (num < _Min) _Min = num;
            else if (num > _Max) _Max = num;
        }
        public void Insert(long[] nums)
        {
            for (int i = 0; i < nums.Length; i++) Insert(nums[i]);
        }
        public void Reset()
        {
            _Max = long.MinValue;
            _Min = long.MaxValue;
        }

        public bool IsWithin(long num)
        {
            if (num <= _Max && num >= _Min) return true;
            else return false;
        }
        public float Ratio(long num)
        {
            return Convert.ToSingle((num - _Min) * 1.0f / Span);
        }
        public float RatioWithBoundary(long num)
        {
            float ratio = Ratio(num);
            if (ratio > 1) ratio = 1;
            else if (ratio < 0) ratio = 0;
            return ratio;
        }
        public long Portion(float ratio)
        {
            return ((double)(_Min + Span * ratio)).ToInt64();
        }
        public override string ToString()
        {
            return _Max.ToString() + Format.Splitter + _Min.ToString();
        }

        public long Span { get { return _Max - _Min; } }
        public long Max { get { return _Max; } }
        public long Min { get { return _Min; } }

        public bool Equals(RangeInt64 value)
        {
            if (this._Max == value._Max && this._Min == value._Min)
            {
                return true;
            }
            else
                return false;
        }

        [DataMember]
        private long _Max;

        [DataMember]
        private long _Min;
    }

    [Serializable, DataContract]
    public struct RangeUInt64 : IEquatable<RangeUInt64>
    {
        public RangeUInt64(ulong num)
        {
            _Max = num;
            _Min = num;
        }
        public RangeUInt64(ulong min, ulong max)
        {
            if (min > max)
            {
                _Min = max;
                _Max = min;
            }
            else
            {
                _Min = min;
                _Max = max;
            }
        }
        public RangeUInt64(ulong[] nums)
        {
            _Max = ulong.MinValue;
            _Min = ulong.MaxValue;
            Insert(nums);
        }
        public RangeUInt64(string s)
        {
            string[] a = s.Split(Format.Splitter);
            if (s.Length == 2)
            {
                try
                {
                    ulong max = UInt64.Parse(a[0]);
                    ulong min = UInt64.Parse(a[1]);
                    if (min > max)
                    {
                        _Min = max;
                        _Max = min;
                    }
                    else
                    {
                        _Min = min;
                        _Max = max;
                    }
                }
                catch
                {
                    _Max = ulong.MinValue;
                    _Min = ulong.MaxValue;
                }
            }
            else
            {
                _Max = ulong.MinValue;
                _Min = ulong.MaxValue;
            }
        }
        public void Insert(ulong num)
        {
            if (num < _Min) _Min = num;
            else if (num > _Max) _Max = num;
        }
        public void Insert(ulong[] nums)
        {
            for (int i = 0; i < nums.Length; i++) Insert(nums[i]);
        }
        public void Reset()
        {
            _Max = ulong.MinValue;
            _Min = ulong.MaxValue;
        }
        public bool IsWithin(ulong num)
        {
            if (num <= _Max && num >= _Min) return true;
            else return false;
        }
        public float Ratio(ulong num)
        {
            return Convert.ToSingle((num - _Min) * 1.0f / Span);
        }
        public float RatioWithBoundary(ulong num)
        {
            float ratio = Ratio(num);
            if (ratio > 1) ratio = 1;
            else if (ratio < 0) ratio = 0;
            return ratio;
        }
        public ulong Portion(float ratio)
        {
            long portion = ((double)(_Min + Span * ratio)).ToInt64();
            if (portion < 0) portion = 0;
            return Convert.ToUInt64(portion);
        }
        public override string ToString()
        {
            return _Max.ToString() + Format.Splitter + _Min.ToString();
        }

        public ulong Span { get { return _Max - _Min; } }
        public ulong Max { get { return _Max; } }
        public ulong Min { get { return _Min; } }

        public bool Equals(RangeUInt64 value)
        {
            if (this._Max == value._Max && this._Min == value._Min)
            {
                return true;
            }
            else
                return false;
        }

        [DataMember]
        private ulong _Max;

        [DataMember]
        private ulong _Min;
    }

    [Serializable, DataContract]
    public struct RangeSingle : IEquatable<RangeSingle>
    {
        public RangeSingle(float num)
        {
            _Max = num;
            _Min = num;
        }
        public RangeSingle(float min, float max)
        {
            if (min > max)
            {
                _Min = max;
                _Max = min;
            }
            else
            {
                _Min = min;
                _Max = max;
            }
        }
        public RangeSingle(float[] nums)
        {
            _Max = float.MinValue;
            _Min = float.MaxValue;
            Insert(nums);
        }
        public RangeSingle(string s)
        {
            string[] a = s.Split(Format.Splitter);
            if (s.Length == 2)
            {
                try
                {
                    float max = Single.Parse(a[0]);
                    float min = Single.Parse(a[1]);
                    if (min > max)
                    {
                        _Min = max;
                        _Max = min;
                    }
                    else
                    {
                        _Min = min;
                        _Max = max;
                    }
                }
                catch
                {
                    _Max = float.MinValue;
                    _Min = float.MaxValue;
                }
            }
            else
            {
                _Max = float.MinValue;
                _Min = float.MaxValue;
            }


        }
        public void Insert(float num)
        {
            if (num < _Min) _Min = num;
            else if (num > _Max) _Max = num;
        }
        public void Insert(float[] nums)
        {
            for (int i = 0; i < nums.Length; i++) Insert(nums[i]);
        }
        public void Reset()
        {
            _Max = float.MinValue;
            _Min = float.MaxValue;
        }
        public bool IsWithin(float num)
        {
            if (num <= _Max && num >= _Min) return true;
            else return false;
        }
        public float Ratio(float num)
        {
            return Convert.ToSingle((num - _Min) / Span);
        }
        public float RatioWithBoundary(float num)
        {
            float ratio = Ratio(num);
            if (ratio > 1) ratio = 1;
            else if (ratio < 0) ratio = 0;
            return ratio;
        }
        public float Portion(float ratio)
        {
            return _Min + Span * ratio;
        }
        public override string ToString()
        {
            return _Max.ToString() + Format.Splitter + _Min.ToString();
        }

        public float Span { get { return _Max - _Min; } }
        public float Max { get { return _Max; } }
        public float Min { get { return _Min; } }

        public bool Equals(RangeSingle value)
        {
            if (this._Max == value._Max && this._Min == value._Min)
            {
                return true;
            }
            else
                return false;
        }

        [DataMember]
        private float _Max;

        [DataMember]
        private float _Min;
    }

    [Serializable, DataContract]
    public struct RangeDouble : IEquatable<RangeDouble>
    {
        public RangeDouble(double num)
        {
            _Max = num;
            _Min = num;
        }
        public RangeDouble(double min, double max)
        {
            if (min > max)
            {
                _Min = max;
                _Max = min;
            }
            else
            {
                _Min = min;
                _Max = max;
            }
        }
        public RangeDouble(double[] nums)
        {
            _Max = double.MinValue;
            _Min = double.MaxValue;
            Insert(nums);
        }
        public RangeDouble(string s)
        {
            string[] a = s.Split(Format.Splitter);
            if (s.Length == 2)
            {
                try
                {
                    double max = Double.Parse(a[0]);
                    double min = Double.Parse(a[1]);
                    if (min > max)
                    {
                        _Min = max;
                        _Max = min;
                    }
                    else
                    {
                        _Min = min;
                        _Max = max;
                    }
                }
                catch
                {
                    _Max = double.MinValue;
                    _Min = double.MaxValue;
                }
            }
            else
            {
                _Max = double.MinValue;
                _Min = double.MaxValue;
            }
        }
        public void Insert(double num)
        {
            if (num < _Min) _Min = num;
            else if (num > _Max) _Max = num;
        }
        public void Insert(double[] nums)
        {
            for (int i = 0; i < nums.Length; i++) Insert(nums[i]);
            /*
            Parallel.For(0, nums.Length, delegate (int i) {
                Insert(nums[i]);
            });*/
        }
        public void Insert(RangeDouble num)
        {
            if (num.Min < _Min) _Min = num.Min;
            if (num.Max > _Max) _Max = num.Max;
            //Insert(num.Max);
            //Insert(num.Min);
        }
        public void Reset()
        {
            _Max = double.MinValue;
            _Min = double.MaxValue;
        }
        public bool IsWithin(double num)
        {
            if (num <= _Max && num >= _Min) return true;
            else return false;
        }
        public float Ratio(double num)
        {
            return Convert.ToSingle((num - _Min) / Span);
        }
        public float RatioWithBoundary(double num)
        {
            float ratio = Ratio(num);
            if (ratio > 1) ratio = 1;
            else if (ratio < 0) ratio = 0;
            return ratio;
        }
        public double Portion(float ratio)
        {
            return _Min + Span * ratio;
        }
        public override string ToString()
        {
            return _Max.ToString() + Format.Splitter + _Min.ToString();
        }
        public RangeDouble Parse(string s)
        {
            return new RangeDouble(s);
        }

        public double Span { get { return _Max - _Min; } }
        public double Max { get { return _Max; } }
        public double Min { get { return _Min; } }

        public bool Equals(RangeDouble value)
        {
            if (this._Max == value._Max && this._Min == value._Min)
            {
                return true;
            }
            else
                return false;
        }
        /*
        public IEnumerable<int> Fibonacci()
        {
            int a = 0;
            int b = 1;

            while (true)
            {
                yield return a;
                yield return b;
                a += b;
                b += a;
            }
        }
        */
        [DataMember]
        private double _Max;

        [DataMember]
        private double _Min;
    }
    #endregion
}
