/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public class SignalDatum : IDatum
    {
        public SignalDatum(Bar b, SignalColumn column)
        {
            Bar = b;
            SignalColumn = column;
            b[column] = this;
        }

        public SignalDatum(Bar b, SignalColumn column, double[] points) : this(b, column)
        {
            SetPoints(points);
        }

        public Bar Bar { get; }

        public SignalColumn SignalColumn { get; }

        public void SetPoints(double[] points)
        {
            if (points is null) return;

            List<double> point_list = TrailPoints.Merge(points).ToList();

            if (Bar.Bar_1 is Bar b_1 && b_1[SignalColumn] is SignalDatum sd_1 && sd_1.TrailPoints is List<double> pts_1 && pts_1.Count > 1)
            {
                for (int i = 1; i < pts_1.Count; i++)
                {
                    if (i - 1 < point_list.Count)
                        point_list[i - 1] += pts_1[i];
                    else
                        point_list.Add(pts_1[i]);
                }
            }

            TrailPoints = point_list;
        }

        public void ResetPoints() 
        {
            TrailPoints.Clear();
        }

        public virtual string Description { get; } = string.Empty;

        public List<double> TrailPoints { get; private set; } = new() { 0 };

        public void Add(double f)
        {
            for (int i = 0; i < TrailPoints.Count; i++)
            {
                TrailPoints[i] = TrailPoints[i] + f;
            }
        }

        public void Multiply(double f)
        {
            for (int i = 0; i < TrailPoints.Count; i++)
            {
                TrailPoints[i] = TrailPoints[i] * f;
            }
        }

        public double Points => TrailPoints.Count > 0 ? TrailPoints[0] : 0;
    }
}
