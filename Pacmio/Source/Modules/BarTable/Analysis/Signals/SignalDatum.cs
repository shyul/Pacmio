
using System.Collections.Generic;
/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************
namespace Pacmio
{
    public class SignalDatum
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
    }
}
