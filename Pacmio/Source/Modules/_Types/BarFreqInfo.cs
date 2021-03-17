/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class BarFreqInfo : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="length"></param>
        public BarFreqInfo(TimeUnit unit, int length)
        {
            Frequency = new Frequency(unit, length);

            TimeSpan proposedDuration = new TimeSpan(0, 0, 0, (Frequency.Span.TotalSeconds * 4500).ToInt32());

            if (unit == TimeUnit.Minutes && length == 1)
            {
                Duration = new Frequency(TimeUnit.Days, 2);
                DurationString = "2 D";
            }
            else if (unit == TimeUnit.Seconds && length == 30)
            {
                Duration = new Frequency(TimeUnit.Days, 3);
                DurationString = "3 D";
            }
            else if (proposedDuration.TotalDays >= 1 && proposedDuration.TotalDays <= 360)
            {
                int days = Math.Floor(proposedDuration.TotalDays).ToInt32();
                Duration = new Frequency(TimeUnit.Days, days);
                DurationString = days + " D";
            }
            else if (proposedDuration.TotalDays > 360)
            {
                int years = Math.Ceiling(proposedDuration.TotalDays / 360).ToInt32(1);
                Duration = new Frequency(TimeUnit.Years, years);
                DurationString = years + " Y";
            }
            else
            {
                int seconds = Math.Floor(proposedDuration.TotalSeconds).ToInt32();
                Duration = new Frequency(TimeUnit.Seconds, seconds);
                DurationString = seconds + " S";
            }
        }

        public string Name => Frequency.ToString();

        public Frequency Frequency { get; private set; }

        public Frequency Duration { get; private set; }

        public string DurationString { get; private set; }
    }
}
