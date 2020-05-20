/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - shyu.lee@gmail.com
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xu;

namespace Pacmio
{
    public class Pivot : DataColumn
    {
        public Pivot(BarTable bt, int maxProminence = 100)
        {
            Table = bt;
            MaximumTestRange = maxProminence;
        }

        public Pivot(BarTable bt, string param)
        {
            Table = bt;
            SetParameterByString(param);
        }

        protected virtual void SetParameterByString(string param)
        {
            string[] paramList = param.GetTokens();

            if (paramList.Length == 1)
            {
                MaximumTestRange = paramList[0].ToInt32(0);
            }
            else
                throw new FormatException(GetType() + ": Error parameter format.");

            if (MaximumTestRange == 0) throw new FormatException(GetType() + ": Error parameter format.");
        }

        public override void Setup()
        {
            Table.Column_HIGH.Children.CheckAdd(this);
            Table.Column_LOW.Children.CheckAdd(this);
        }

        public override void RemoveSetup()
        {

        }

        public override int GetHashCode() => GetType().GetHashCode() ^ MaximumTestRange;

        public override string Name => GetType().Name + "(" + MaximumTestRange.ToString() + ")";

        public virtual int MaximumTestRange { get; protected set; } = 100;

        protected override void Calculate()
        {
            if (StartPt > StopPt - MaximumTestRange)
                StartPt = StopPt - MaximumTestRange;

            if (StartPt < 0)
                StartPt = 0;

            List<(int index, int d)> result = GetList(Table, StartPt, StopPt, MaximumTestRange);

            foreach (var (index, d) in result)
                Table[index][this] = d;
        }

        public static List<(int, int)> GetList(BarTable bt, int startPt, int stopPt, int maxProminence)
        {
            List<(int, int)> resultList = new List<(int, int)>();

            for (int i = startPt; i < stopPt; i++) //Parallel.For(StartPt, StopPt, i => {  });
            {
                int result = 0;
                int j = 1;

                double high_data = bt[i].High;
                double low_data = bt[i].Low;

                bool test_high = true, test_low = true;

                while (j < maxProminence)
                {
                    if ((!test_high) && (!test_low)) break;

                    if (i - j < 0) break;

                    int right_index = i + j;
                    if (right_index >= stopPt) right_index = stopPt - 1;  // break;

                    if (test_high)
                    {
                        double left_high = bt[i - j].High;
                        double right_high = bt[right_index].High;

                        if (high_data >= left_high && high_data >= right_high)
                            result = j;
                        else
                            test_high = false;
                    }

                    if (test_low)
                    {
                        double left_low = bt[i - j].Low;
                        double right_low = bt[right_index].Low;

                        if (low_data <= left_low && low_data <= right_low)
                            result = -j;
                        else
                            test_low = false;
                    }

                    j++;
                }

                resultList.Add((i, result));
                //Table[i][this] = result;
            }
            return resultList;
        }
    }
}
