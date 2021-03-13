/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Collections.Generic;
using Xu;

namespace Pacmio
{
    public class SignalDatum : IDatum
    {
        public void Set(double[] points, string description = "", SignalDatum sd_1 = null)
        {
            List<double> point_list = new List<double>();

            if (points is double[] pts && pts.Length > 0)
                point_list.AddRange(points);
            else
                point_list.Add(0);

            if (sd_1 is SignalDatum && sd_1.Points is double[] pts_1 && pts_1.Length > 1)
            {
                for (int i = 1; i < pts_1.Length; i++)
                {
                    if (i - 1 < point_list.Count)
                        point_list[i - 1] += pts_1[i];
                    else
                        point_list.Add(pts_1[i]);
                }

                if (description == "") description = sd_1.Description;
            }

            Description = description;
            Points = point_list.ToArray();
            Score = point_list[0];
        }

        public string Description { get; set; } = string.Empty;

        public double[] Points { get; private set; }

        public double Score { get; private set; }

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
    }
}
