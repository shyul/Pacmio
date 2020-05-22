/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class Divergence : BarAnalysis, ISignalAnalysis, IChartOverlay
    {
        public Divergence(IOscillator indicatorAnlysis, int searchRange = 50, int minimumPeakProminence = 3) 
        {
            Indicator_Column = indicatorAnlysis.Result_Column;
            IndicatorAreaName = indicatorAnlysis.AreaName;

            Upper_Indicator_Limit = indicatorAnlysis.UpperLimit;
            Lower_Indicator_Limit = indicatorAnlysis.LowerLimit;

            SearchRange = searchRange;
            MinimumPeakProminence = minimumPeakProminence;

            string label = "[" + Indicator_Column.Name + "," + SearchRange + "," + MinimumPeakProminence + "]";

            Name = GetType().Name + label;
            Description = GetType().Name + " " + label;
            GroupName = GetType().Name + ": ";

            Signal_Column = new ObjectColumn(Name, typeof(DivergenceSignalDatum)) { Label = label };

            BullishTheme = new ColorTheme(indicatorAnlysis.UpperColor, indicatorAnlysis.UpperColor.Opaque(64));
            BearishTheme = new ColorTheme(indicatorAnlysis.LowerColor, indicatorAnlysis.LowerColor.Opaque(64));
        }

        #region Parameters

        public int SearchRange { get; set; } = 50; 

        public int MinimumPeakProminence { get; set; } = 5;

        public double Upper_Indicator_Limit { get; }

        public double Lower_Indicator_Limit { get; }

        public NumericColumn Indicator_Column { get; }

        public string IndicatorAreaName { get; private set; } = null;

        #endregion Parameters

        #region Calculation

        public ObjectColumn Signal_Column { get; private set; }

        public ColorTheme BullishTheme { get; }// = new ColorTheme(Color.CornflowerBlue, Color.CornflowerBlue.Opaque(64));

        public ColorTheme BearishTheme { get; }// = new ColorTheme(Color.PaleVioletRed, Color.PaleVioletRed.Opaque(64));

        public readonly Dictionary<DivergenceType, double> TypeToScore = new Dictionary<DivergenceType, double>
        {
            { DivergenceType.Positive, 3 },
            { DivergenceType.Negative, 3 },
            { DivergenceType.HiddenPositive, 3.5 },
            { DivergenceType.HiddenNegative, 3.5 },
        };

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            int min_peak_start = bap.StopPt - TableList.MaximumPeakProminence * 2 - 1;
            if (bap.StartPt > min_peak_start) bap.StartPt = min_peak_start;

            if (bap.StartPt < 0) return;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                bt[i][Signal_Column] = new DivergenceSignalDatum();// { Index = i };

                List<(int index, double peak, double value, double indicator_value)> upper_side_list = new List<(int index, double peak, double value, double indicator_value)>();
                List<(int index, double peak, double value, double indicator_value)> lower_side_list = new List<(int index, double peak, double value, double indicator_value)>();

                // Search Back
                for (int j = i - MinimumPeakProminence; j > i - SearchRange; j--) // i-5 ~ i-50
                {
                    if (j < 0) break;

                    Bar b = bt[j];
                    double peak_n_trough = b.Peak;
                    if (peak_n_trough >= MinimumPeakProminence)
                    {
                        //double indicator = b[Indicator_Column];
                        double indicator = bt[j + 2, 5].Select(n => n[Indicator_Column]).Max();
                        upper_side_list.Add((j, peak_n_trough, b.High, indicator));
                    }
                    else if (peak_n_trough <= -MinimumPeakProminence)
                    {
                        //double indicator = b[Indicator_Column];
                        double indicator = bt[j + 2, 5].Select(n => n[Indicator_Column]).Min();
                        lower_side_list.Add((j, peak_n_trough, b.Low, indicator));
                    }
                }

                bool has_higher_high = false;
                bool has_lower_high = false;
                (int index, double peak, double value, double indicator_value) upper_point1 = (0, 0, 0, 0);
                (int index, double peak, double value, double indicator_value) upper_point2 = (0, 0, 0, 0);

                if (upper_side_list.Count > 1)
                {
                    var upside = upper_side_list.OrderByDescending(n => n.value).Take(2).OrderBy(n => n.index); // The highest two highs
                    upper_point1 = upside.ElementAt(0);
                    upper_point2 = upside.ElementAt(1);
                    has_higher_high = upper_point1.value < upper_point2.value; // has higher high
                    has_lower_high = upper_point1.value > upper_point2.value;
                }

                bool has_lower_low = false;
                bool has_higher_low = false;
                (int index, double peak, double value, double indicator_value) lower_point1 = (0, 0, 0, 0);
                (int index, double peak, double value, double indicator_value) lower_point2 = (0, 0, 0, 0);

                if (lower_side_list.Count > 1)
                {
                    var downside = lower_side_list.OrderByDescending(n => n.value).Take(2).OrderBy(n => n.index); // The lowest two lows
                    lower_point1 = downside.ElementAt(0);
                    lower_point2 = downside.ElementAt(1);
                    has_lower_low = lower_point1.value > lower_point2.value; // has lower low
                    has_higher_low = lower_point1.value < lower_point2.value;
                }

                if (has_higher_high && !has_lower_low) // higher high and uptrend is confirmed
                {
                    //int distance = upper_point2.index - upper_point1.index;
                    if (upper_point1.indicator_value > upper_point2.indicator_value && upper_point1.indicator_value >= Upper_Indicator_Limit)  // lower highs of the indicator value
                    {
                        DivergenceSignalDatum dsd = (DivergenceSignalDatum)bt[upper_point2.index][Signal_Column];
                        dsd.Type = DivergenceType.Negative;
                        dsd.BearishScore = TypeToScore[DivergenceType.Negative];
                        //dsd.Distance = distance;
                        dsd.Point1 = upper_point1;
                        dsd.Point2 = upper_point2;
                        //Console.WriteLine("Found: DivergenceType.Negative");
                    }
                }
                else if (!has_higher_high && has_lower_low)
                {
                    //int distance = lower_point2.index - lower_point1.index;
                    if (lower_point1.indicator_value < lower_point2.indicator_value && lower_point1.indicator_value <= Lower_Indicator_Limit)  // higher low of the indicator value
                    {
                        DivergenceSignalDatum dsd = (DivergenceSignalDatum)bt[lower_point2.index][Signal_Column];
                        dsd.Type = DivergenceType.Positive;
                        dsd.BullishScore = TypeToScore[DivergenceType.Positive];
                        //dsd.Distance = distance;
                        dsd.Point1 = lower_point1;
                        dsd.Point2 = lower_point2;
                        //Console.WriteLine("Found: DivergenceType.Positive");
                    }
                }
                else if (has_higher_low && !has_lower_high) // Confirms the continuation
                {
                    //int distance = lower_point2.index - lower_point1.index;
                    if (lower_point1.indicator_value > lower_point2.indicator_value && lower_point2.indicator_value <= Lower_Indicator_Limit)  // lower low of the indicator value
                    {
                        DivergenceSignalDatum dsd = (DivergenceSignalDatum)bt[lower_point2.index][Signal_Column];
                        dsd.Type = DivergenceType.HiddenPositive;
                        dsd.BullishScore = TypeToScore[DivergenceType.HiddenPositive];
                        //dsd.Distance = distance;
                        dsd.Point1 = lower_point1;
                        dsd.Point2 = lower_point2;
                        //Console.WriteLine("Found: DivergenceType.HiddenPositive");
                    }
                }
                else if (has_lower_high && !has_higher_low) // Confirms the continuation
                {
                    //int distance = upper_point2.index - upper_point1.index;
                    if (upper_point1.indicator_value < upper_point2.indicator_value && upper_point2.indicator_value >= Upper_Indicator_Limit)  // higher highs of the indicator value
                    {
                        DivergenceSignalDatum dsd = (DivergenceSignalDatum)bt[upper_point2.index][Signal_Column];
                        dsd.Type = DivergenceType.HiddenNegative;
                        dsd.BearishScore = TypeToScore[DivergenceType.HiddenNegative];
                        dsd.Point1 = upper_point1;
                        dsd.Point2 = upper_point2;
                        //Console.WriteLine("Found: DivergenceType.HiddenNegative");
                    }
                }

            }
        }

        #endregion Calculation

        #region Chart Overlay

        public void Draw(Graphics g, BarChart bc, BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            
            Area mainArea = bc.MainArea;
            ContinuousAxis mainAxisY = mainArea.AxisY(AlignType.Right);

            //int pt = 0;
            for (int i = mainArea.StartPt; i < mainArea.StopPt; i++)
            {
                if (i >= bt.Count) break;
                 
                if (i >= 0)
                {
                    if(bt[i][Signal_Column] is DivergenceSignalDatum dsd && dsd.Distance > MinimumPeakProminence) 
                    {
                        var (index1, _, value1, indicator_value1) = dsd.Point1;
                        var (index2, _, value2, indicator_value2) = dsd.Point2;

                        int pt1 = index1 - mainArea.StartPt;
                        int pt2 = index2 - mainArea.StartPt;

                        if (pt1 > 0 && pt2 > 0)
                        {
                            int x1 = mainArea.IndexToPixel(pt1);
                            int x2 = mainArea.IndexToPixel(pt2);
                            int y1 = mainAxisY.ValueToPixel(value1);
                            int y2 = mainAxisY.ValueToPixel(value2);

                            g.DrawLine(new Pen(new SolidBrush(Color.Red), 2), x1, y1, x2, y2);

                            if (bc[IndicatorAreaName] is OscillatorArea oa)
                            {
                                ContinuousAxis indicatorAxisY = oa.AxisY(AlignType.Right);

                                int iy1 = indicatorAxisY.ValueToPixel(indicator_value1);
                                int iy2 = indicatorAxisY.ValueToPixel(indicator_value2);


                                g.DrawLine(new Pen(new SolidBrush(Color.Red), 2), x1, iy1, x2, iy2);

                            }
                        }


                    }
                }
                //pt++;
            }

            //for (int i = oa.StartPt; i < oa.StopPt; i++)

        }

        #endregion Chart Overlay
    }
}
