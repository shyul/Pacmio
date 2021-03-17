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
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio.Analysis
{
    public class MovingAverageCrossIndicator : Indicator
    {
        protected override void Calculate(BarAnalysisPointer bap)
        {

        }

        /*
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
        public SignalColumn SignalColumn { get; protected set; }

        public virtual Dictionary<DualColumnType, double[]> TypeToScore { get; } = new Dictionary<DualColumnType, double[]>
        {
        { DualColumnType.Above, new double[] { 0.5 } },
        { DualColumnType.Below, new double[] { -0.5 } },
        { DualColumnType.Expansion, new double[] { 1 } },
        { DualColumnType.Contraction, new double[] { 1 } },
        { DualColumnType.CrossUp, new double[] { 4, 3.5, 3, 2.5 } },
        { DualColumnType.CrossDown, new double[] { -4, -3.5, -3, -2.5 } },
        { DualColumnType.TrendUp, new double[] { 0.5 } },
        { DualColumnType.TrendDown, new double[] { -0.5 } },
        };*/
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
        }


    }
}


