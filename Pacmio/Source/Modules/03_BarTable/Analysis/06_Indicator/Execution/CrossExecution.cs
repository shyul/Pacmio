/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class CrossExecution : Indicator
    {
        public CrossExecution(int fast = 5, int slow = 20)
        {
            Fast_SingleData = new SMA(fast) { Color = Color.Orange };
            Slow_SingleData = new SMA(slow) { Color = Color.YellowGreen };

            DualDataSignal = new DualDataSignal(Fast_SingleData, Slow_SingleData);

            string label = "(" + Fast_SingleData.Name + "," + Slow_SingleData.Name + ")";
            GroupName = Name = GetType().Name + label;



            DualDataSignal.AddChild(this);


            SignalColumns = new SignalColumn[] { DualDataSignal.Column_Result };
            SignalSeries = new(this);
            BarAnalysisSet = new(this);
        }

        public ISingleData Fast_SingleData { get; }

        public ISingleData Slow_SingleData { get; }

        public DualDataSignal DualDataSignal { get; }

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }

    /*
     * 
         public class MovingAverageCrossStrategy
    {


        public void Config(BarFreq freq, MovingAverageType type_fast, int interval_fast, MovingAverageType type_slow, int interval_slow)
        {
            //BarAnalysisSet bas = new BarAnalysisSet();

            switch (type_fast)
            {
                case MovingAverageType.Simple:
                    MA_Fast = new SMA(interval_fast);
                    break;
                case MovingAverageType.Smoothed:
                    MA_Fast = new SMMA(interval_fast);
                    break;
                case MovingAverageType.Exponential:
                    MA_Fast = new EMA(interval_fast);
                    break;
                case MovingAverageType.Weighted:
                    MA_Fast = new WMA(interval_fast);
                    break;
                case MovingAverageType.Hull:
                    MA_Fast = new HMA(interval_fast);
                    break;
            }

            switch (type_slow)
            {
                case MovingAverageType.Simple:
                    MA_Slow = new SMA(interval_slow);
                    break;
                case MovingAverageType.Smoothed:
                    MA_Slow = new SMMA(interval_slow);
                    break;
                case MovingAverageType.Exponential:
                    MA_Slow = new EMA(interval_slow);
                    break;
                case MovingAverageType.Weighted:
                    MA_Slow = new WMA(interval_slow);
                    break;
                case MovingAverageType.Hull:
                    MA_Slow = new HMA(interval_slow);
                    break;
            }

            //bas.List = new List<BarAnalysis>() { MA_Fast, MA_Slow};


        }

        public SMA MA_Fast { get; private set; }

        public SMA MA_Slow { get; private set; }

        public SignalColumn SignalColumn { get; }




        // Make this part to research manager...
        public void Tweak(IEnumerable<(MovingAverageType type_fast, int interval_fast, MovingAverageType type_slow, int interval_slow)> configs) // arg: Tweak Plan....
        {

        }


    }
     * 
    public class CrossIndicator : Indicator
    {
        public CrossIndicator(ISingleData fast_MA, ISingleData slow_MA)
        {
            FastMovingAverage = fast_MA;
            SlowMovingAverage = slow_MA;

            DualColumnAnalysis = new DualDataSignal(FastMovingAverage, SlowMovingAverage);
            DualColumnAnalysis.AddChild(this);

            string label = "(" + FastMovingAverage.Name + "," + SlowMovingAverage.Name + ")";
            GroupName = Name = GetType().Name + label;

            if (fast_MA is IChartSeries fast_ics && slow_MA is IChartSeries slow_ics)
            {
                CrossSignalColumn = new IndicatorColumn(Name, label)
                {
                    BullishColor = fast_ics.Color,
                    BearishColor = slow_ics.Color
                };
            }
            else
            {
                CrossSignalColumn = new IndicatorColumn(Name, label)
                {
                    BullishColor = Color.Green,
                    BearishColor = Color.Red
                };
            }

            SignalColumns = new SignalColumn[] { CrossSignalColumn };

            SignalSeries = new SignalSeries(this);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ FastMovingAverage.GetHashCode() ^ SlowMovingAverage.GetHashCode();

        public ISingleData FastMovingAverage { get; }

        public ISingleData SlowMovingAverage { get; }

        public DualDataSignal DualColumnAnalysis { get; }

        public virtual Dictionary<DualDataSignalType, double[]> TypeToScore { get; } = new Dictionary<DualDataSignalType, double[]>
        {
            { DualDataSignalType.Above, new double[] { 0.5 } },
            { DualDataSignalType.Below, new double[] { -0.5 } },
            { DualDataSignalType.Expansion, new double[] { 1 } },
            { DualDataSignalType.Contraction, new double[] { 1 } },
            { DualDataSignalType.CrossUp, new double[] { 4, 3.5, 3, 2.5 } },
            { DualDataSignalType.CrossDown, new double[] { -4, -3.5, -3, -2.5 } },
            { DualDataSignalType.TrendUp, new double[] { 0.5 } },
            { DualDataSignalType.TrendDown, new double[] { -0.5 } },
        };

        public SignalColumn CrossSignalColumn { get; protected set; }

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                SignalDatum sd = b[CrossSignalColumn] as SignalDatum;

                if (b[DualColumnAnalysis.Column_Result] is DualDataSignalDatum d)
                {
                    var list = d.List;
                    List<double> point_list = new();

                    foreach (var type in list)
                    {
                        SignalDatum.MergePoints(point_list, TypeToScore[type]);
                    }

                    if (i > 0)
                    {
                        SignalDatum sd_1 = bt[i - 1][CrossSignalColumn] as SignalDatum;
                        sd.Set(point_list.ToArray(), list.ToString(","), sd_1);
                    }
                    else
                        sd.Set(point_list.ToArray(), list.ToString(","));
                }
            }
        }

        
        public MovingAverageCrossIndicator(SMA fast_MA, SMA slow_MA)
        {
            Fast_MA = fast_MA;
            Slow_MA = slow_MA;

            Fast_Column = Fast_MA.Column_Result;
            Slow_Column = Slow_MA.Column_Result;

            Fast_MA.AddChild(this);
            Slow_MA.AddChild(this);

            string label = "(" + Fast_MA.Name + "," + Slow_MA.Name + ")";
            GroupName = Name = GetType().Name + label;

            SignalColumn = new SignalColumn(Name, label)
            {
                BullishColor = Fast_MA.Color,
                BearishColor = Slow_MA.Color
            };

            SignalColumns = new SignalColumn[] { SignalColumn };
        }

        public MovingAverageCrossIndicator(MovingAverageType type_fast, int interval_fast, MovingAverageType type_slow, int interval_slow)
        {
            (Fast_MA, Slow_MA) = Config(type_fast, interval_fast, type_slow, interval_slow);

            Fast_Column = Fast_MA.Column_Result;
            Slow_Column = Slow_MA.Column_Result;

            Fast_MA.AddChild(this);
            Slow_MA.AddChild(this);

            string label = "(" + Fast_MA.Name + "," + Slow_MA.Name + ")";
            GroupName = Name = GetType().Name + label;

            SignalColumn = new SignalColumn(Name, label);
            SignalColumns = new SignalColumn[] { SignalColumn };
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Fast_MA.GetHashCode() ^ Slow_MA.GetHashCode();

        public SMA Fast_MA { get; }

        public SMA Slow_MA { get; }

        public static (SMA Fast, SMA Slow) Config(MovingAverageType type_fast, int interval_fast, MovingAverageType type_slow, int interval_slow)
        {
            SMA MA_Fast = null, MA_Slow = null;

            switch (type_fast)
            {
                case MovingAverageType.Simple:
                    MA_Fast = new SMA(interval_fast);
                    break;
                case MovingAverageType.Smoothed:
                    MA_Fast = new SMMA(interval_fast);
                    break;
                case MovingAverageType.Exponential:
                    MA_Fast = new EMA(interval_fast);
                    break;
                case MovingAverageType.Weighted:
                    MA_Fast = new WMA(interval_fast);
                    break;
                case MovingAverageType.Hull:
                    MA_Fast = new HMA(interval_fast);
                    break;
            }

            switch (type_slow)
            {
                case MovingAverageType.Simple:
                    MA_Slow = new SMA(interval_slow);
                    break;
                case MovingAverageType.Smoothed:
                    MA_Slow = new SMMA(interval_slow);
                    break;
                case MovingAverageType.Exponential:
                    MA_Slow = new EMA(interval_slow);
                    break;
                case MovingAverageType.Weighted:
                    MA_Slow = new WMA(interval_slow);
                    break;
                case MovingAverageType.Hull:
                    MA_Slow = new HMA(interval_slow);
                    break;
            }

            return (MA_Fast, MA_Slow);
        }

        */
    /*
    for (int i = bap.StartPt; i < bap.StopPt; i++)
    {
        SignalDatum sd = bt[i][SignalColumn] as SignalDatum;

        var (points, description) = DualDataSignal(bt, i, Fast_Column, Slow_Column, TypeToScore);

        if (i > 0)
        {
            SignalDatum sd_1 = bt[i - 1][SignalColumn] as SignalDatum;
            sd.Set(points, description, sd_1);
        }
        else
            sd.Set(points, description);

        //Console.WriteLine("Score: " + sd.Score);
    }*/
    /*
    public static List<DualColumnType> DualDataSignal(BarTable bt, int i, NumericColumn fast_Column, NumericColumn slow_Column)
    {
        double value_fast = bt[i, fast_Column];
        double value_slow = bt[i, slow_Column];

        List<DualColumnType> dualDataTypes = new List<DualColumnType>();

        if (!double.IsNaN(value_fast) && !double.IsNaN(value_slow))
        {
            double delta = value_fast - value_slow;
            double delta_abs = Math.Abs(delta);

            if (delta > 0)
            {
                dualDataTypes.Add(DualColumnType.Above);
            }
            else if (delta < 0)
            {
                dualDataTypes.Add(DualColumnType.Below);
            }

            double last_value_fast = bt[i - 1, fast_Column];
            double last_value_slow = bt[i - 1, slow_Column];

            if (!double.IsNaN(last_value_fast) && !double.IsNaN(last_value_slow))
            {
                double last_delta = last_value_fast - last_value_slow;

                if (value_fast > last_value_fast && value_slow > last_value_slow)
                {
                    dualDataTypes.Add(DualColumnType.TrendUp);
                }
                else if (value_fast < last_value_fast && value_slow < last_value_slow)
                {
                    dualDataTypes.Add(DualColumnType.TrendDown);
                }

                if (delta >= 0 && last_delta < 0)
                {
                    dualDataTypes.Add(DualColumnType.CrossUp);
                }
                else if (delta <= 0 && last_delta > 0)
                {
                    dualDataTypes.Add(DualColumnType.CrossDown);
                }

                double last_delta_abs = Math.Abs(last_delta);

                if (delta_abs > last_delta_abs)
                {
                    dualDataTypes.Add(DualColumnType.Expansion);
                }
                else
                {
                    dualDataTypes.Add(DualColumnType.Contraction);
                }
            }
        }

        return dualDataTypes;
    }

    public static (double[] points, string description) DualDataSignal(BarTable bt, int i, NumericColumn fast_Column, NumericColumn slow_Column, Dictionary<DualColumnType, double[]> typeToScore)
    {
        List<DualColumnType> dualDataTypes = new List<DualColumnType>();
        List<double> point_list = new List<double>();

        double value_fast = bt[i, fast_Column];
        double value_slow = bt[i, slow_Column];

        if (!double.IsNaN(value_fast) && !double.IsNaN(value_slow))
        {
            double delta = value_fast - value_slow;
            double delta_abs = Math.Abs(delta);

            if (delta > 0)
            {
                dualDataTypes.Add(DualColumnType.Above);
                SignalDatum.MergePoints(point_list, typeToScore[DualColumnType.Above]);
            }
            else if (delta < 0)
            {
                dualDataTypes.Add(DualColumnType.Below);
                SignalDatum.MergePoints(point_list, typeToScore[DualColumnType.Below]);
            }

            double last_value_fast = bt[i - 1, fast_Column];
            double last_value_slow = bt[i - 1, slow_Column];

            if (!double.IsNaN(last_value_fast) && !double.IsNaN(last_value_slow))
            {
                double last_delta = last_value_fast - last_value_slow;

                if (value_fast > last_value_fast && value_slow > last_value_slow)
                {
                    dualDataTypes.Add(DualColumnType.TrendUp);
                    SignalDatum.MergePoints(point_list, typeToScore[DualColumnType.TrendUp]);
                }
                else if (value_fast < last_value_fast && value_slow < last_value_slow)
                {
                    dualDataTypes.Add(DualColumnType.TrendDown);
                    SignalDatum.MergePoints(point_list, typeToScore[DualColumnType.TrendDown]);
                }

                if (delta >= 0 && last_delta < 0)
                {
                    dualDataTypes.Add(DualColumnType.CrossUp);
                    SignalDatum.MergePoints(point_list, typeToScore[DualColumnType.CrossUp]);
                }
                else if (delta <= 0 && last_delta > 0)
                {
                    dualDataTypes.Add(DualColumnType.CrossDown);
                    SignalDatum.MergePoints(point_list, typeToScore[DualColumnType.CrossDown]);
                }

                double last_delta_abs = Math.Abs(last_delta);

                if (delta_abs > last_delta_abs)
                {
                    dualDataTypes.Add(DualColumnType.Expansion);
                    if (delta > 0)
                        SignalDatum.MergePoints(point_list, typeToScore[DualColumnType.Expansion]);
                    else if (delta < 0)
                        SignalDatum.MergePointsNegative(point_list, typeToScore[DualColumnType.Expansion]);
                }
                else
                {
                    dualDataTypes.Add(DualColumnType.Contraction);
                    if (delta > 0)
                        SignalDatum.MergePointsNegative(point_list, typeToScore[DualColumnType.Contraction]);
                    else if (delta < 0)
                        SignalDatum.MergePoints(point_list, typeToScore[DualColumnType.Contraction]);
                }
            }
        }

        return (point_list.ToArray(), dualDataTypes.ToString(","));
    }}

            #region Point Tools

        public static void MergePoints(List<double> list, double[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (i < list.Count)
                    list[i] += points[i];
                else
                    list.Add(points[i]);
            }
        }

        public static void MergePointsNegative(List<double> list, double[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (i < list.Count)
                    list[i] -= points[i];
                else
                    list.Add(-points[i]);
            }
        }

        public static void MergePoints(List<double> list, double point)
        {
            if (list.Count > 0)
                list[0] += point;
            else
                list.Add(point);
        }

        #endregion Point Tools

    */

}


