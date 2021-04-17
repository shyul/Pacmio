/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Xu;
using Xu.Chart;
using Pacmio.Analysis;

namespace Pacmio
{
    public sealed class CandleStickSeries : OhlcSeries
    {
        public CandleStickSeries() : base(Bar.Column_Open,
                Bar.Column_High,
                Bar.Column_Low,
                Bar.Column_Close,
                Bar.Column_GainPercent)
        {
            Order = int.MaxValue;
            Importance = Importance.Huge;
            LegendName = "PriceSeries";
        }

        public override List<(string text, Font font, Brush brush)> ValueLabels(ITable table, int pt)
        {
            List<(string text, Font font, Brush brush)> labels = new();

            string text = string.Empty;
            double open = table[pt, Open_Column];
            text += !double.IsNaN(open) ? Open_Column.Label + ": " + open.ToString(LegendLabelFormat) + "  " : string.Empty;

            double high = table[pt, High_Column];
            text += !double.IsNaN(high) ? High_Column.Label + ": " + high.ToString(LegendLabelFormat) + "  " : string.Empty;

            double low = table[pt, Low_Column];
            text += !double.IsNaN(low) ? Low_Column.Label + ": " + low.ToString(LegendLabelFormat) + "    " : string.Empty;

            if (text.Length > 0) labels.Add((text, Main.Theme.Font, Legend.LabelBrush(Theme)));

            double close = table[pt, Close_Column];
            double percent = table[pt, Percent_Column];

            text = (!double.IsNaN(close) && !double.IsNaN(percent)) ? Close_Column.Label + ": " + close.ToString(LegendLabelFormat) + " ( " + percent.ToString("0.##") + "% )" : string.Empty;

            if (text.Length > 0) labels.Add((text, Main.Theme.Font, (percent < 0) ? Legend.LabelBrush(LowerTheme) : Legend.LabelBrush(Theme)));

            if (table is BarTable bt)
            {
                Bar b = bt[pt];
                //List<CandleStickType> list = b.CandleStickList;
                text = b.CandleStickList.ToString(", ").Trim().TrimEnd(',');
                if (text.Length > 0) labels.Add(("  " + text, Main.Theme.Font, (percent < 0) ? Legend.LabelBrush(LowerTheme) : Legend.LabelBrush(Theme)));
            }

            return labels;
        }
    }
}
