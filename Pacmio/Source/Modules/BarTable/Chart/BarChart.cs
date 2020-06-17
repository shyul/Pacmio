/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class BarChart : ChartWidget, IDependable
    {
        public BarChart(string name, OhlcType type) : base(name)
        {
            Margin = new Padding(5, 15, 5, 5);
            Theme.FillColor = BackColor = Color.FromArgb(255, 255, 253, 245);
            Theme.EdgeColor = Theme.ForeColor = Color.FromArgb(192, 192, 192);

            Style[Importance.Major].Font = Main.Theme.FontBold;
            Style[Importance.Major].HasLabel = true;
            Style[Importance.Major].HasLine = true;
            Style[Importance.Major].Theme.EdgePen.DashPattern = new float[] { 5, 3 };
            Style[Importance.Major].Theme.EdgePen.Color = Color.FromArgb(180, 180, 180);

            Style[Importance.Minor].Font = Main.Theme.Font;
            Style[Importance.Minor].HasLabel = true;
            Style[Importance.Minor].HasLine = true;
            Style[Importance.Minor].Theme.EdgePen.DashPattern = new float[] { 1, 2 };

            AddArea(PositionArea = new PositionArea(this));
            AddArea(SignalArea = new SignalArea(this));
            AddArea(MainArea = new MainArea(this, MainArea.DefaultName, 50, 0.3f) { HasXAxisBar = true, });

            OhlcType = type;

            ResumeLayout(false);
            PerformLayout();
        }

        protected override void Dispose(bool disposing)
        {
            Remove(true);
            GC.Collect();
        }

        public ICollection<IDependable> Children { get; } = new HashSet<IDependable>();

        public ICollection<IDependable> Parents { get; } = new HashSet<IDependable>();

        public void Remove(bool recursive)
        {
            if (recursive || Children.Count == 0)
            {
                foreach (IDependable child in Children)
                    child.Remove(true);

                foreach (IDependable parent in Parents)
                    parent.CheckRemove(this);

                if (Children.Count > 0) throw new Exception("Still have children in this BarChart");
            }
            else
            {
                if (Children.Count > 0)
                {
                    foreach (var child in Children)
                        child.Enabled = false;
                }
                Enabled = false;
            }
        }

        public string Title { get => MainArea.PriceSeries.Legend.Label; set => MainArea.PriceSeries.Legend.Label = value; }

        public override string Label { get => MainArea.PriceSeries.Label; set => MainArea.PriceSeries.Label = value; }

        public override string Description { get => MainArea.PriceSeries.Description; set => MainArea.PriceSeries.Description = value; }

        public readonly MainArea MainArea;
        public readonly SignalArea SignalArea;
        public readonly PositionArea PositionArea;

        public void Config(IEnumerable<IChartSeries> ics)
        {
            // Remove all areas and series.
            List<Area> areaToRemove = Areas.Where(n => n != MainArea && n != SignalArea && n != PositionArea).ToList();
            areaToRemove.ForEach(n => Areas.Remove(n));
            foreach (var ic in ics)
                ic.ConfigChart(this);
        }
        
        public BarTable BarTable
        {
            get
            {
                return m_barTable;
            }
            set
            {
                if (m_barTable is BarTable bt)
                {
                    bt.RemoveChild(this);
                }

                m_barTable = value;
                m_barTable.AddChild(this);
                StopPt = m_barTable.LastIndex;
                TabName = Name = m_barTable.Name;
            }
        }

        private BarTable m_barTable = null;

        public override ITable Table => m_barTable;

        public override int DataCount => m_barTable.Count;

        public override string this[int i]
        {
            get
            {
                if (BarFreq < BarFreq.Minute)
                    return m_barTable.IndexToTime(i + StartPt).ToString("MMM-dd HH:mm:ss");
                else if (BarFreq < BarFreq.Daily)
                    return m_barTable.IndexToTime(i + StartPt).ToString("MMM-dd HH:mm");
                else
                    return m_barTable.IndexToTime(i + StartPt).ToString("MMM-dd-yyyy");
            }
        }

        public int LastIndex // Of the bar
        {
            get
            {
                int stopPt = StopPt - 1;

                if (stopPt < 0)
                    stopPt = 0;
                else if (stopPt >= Table.Count)
                    stopPt = Table.Count - 1;

                return stopPt;
            }
        }

        public Bar LastBar => m_barTable[LastIndex];

        public double LastClose => (LastBar is null) ? 0 : LastBar.Close;

        public DateTime LastTime => (LastBar is null) ? DateTime.Now : LastBar.Time;

        public string LastTimeString => this[LastIndex - StartPt];

        public OhlcType OhlcType { get => MainArea.PriceSeries.Type; set => MainArea.PriceSeries.Type = value; }

        public Period Period => new Period(m_barTable.IndexToTime(StartPt), m_barTable.IndexToTime(StopPt));

        public BarFreq BarFreq => m_barTable.BarFreq;

        public Frequency Frequency => m_barTable.Frequency;

        public TimeUnit TimeUnit => Frequency.Unit;

        public Frequency MajorTick { get; private set; }

        public Frequency MinorTick { get; private set; }

        public Range<double> ChartRange => MainArea.AxisY(AlignType.Right).Range;

        protected override void CoordinateLayout()
        {
            ResumeLayout(true);
            if (IsActive && ReadyToShow && m_barTable is BarTable bt)
            {
                SignalArea.Visible = false; // BarTable.HasSignalAnalysis;
                PositionArea.Visible = false;
                //BarTable.HasSignalAnalysis;

                lock (bt.DataObjectLock)
                    lock (GraphicsObjectLock)
                    {


                        AxisX.TickList.Clear();

                        // Get Text Width Major: 1 ~ 10 

                        // Get Text With Minor: One Tick lower, and manual tick size

                        // Get IndexCount;

                        // Get Width

                        // Tick: At least 4 ticks smaller... 

                        // Minor: 1 sec / 5 sec / 15 sec / 30 sec / 1 min / 5 min / 15 min / 30 min / 1 hour / 2 hours / 3 hours / 4 hours / 8 hours

                        // Major: One unit bigger, want to contain at least 4 minor ticks

                        int tickMulti = 1;
                        int tickWidth = AxisX.TickWidth;

                        //int majorTextWidth = TextRenderer.MeasureText("0000", Style[Importance.Major].Font).Width;
                        int minorTextWidth = TextRenderer.MeasureText("000", Style[Importance.Minor].Font).Width;

                        while (tickWidth <= minorTextWidth) { tickWidth++; tickMulti++; }

                        int px = 0;
                        switch (TimeUnit)
                        {
                            case (TimeUnit.Years): // 02, 03, 04 || 2002, 2003, 2004
                                MinorTick = new Frequency(TimeUnit.Years, Frequency.Length * tickMulti);
                                MajorTick = MinorTick * 5;
                                for (int i = StartPt; i < StopPt; i++)
                                {
                                    DateTime time = bt.IndexToTime(i);
                                    if (time.Year % MajorTick.Length == 0) AxisX.TickList.CheckAdd(px, (Importance.Major, time.Year.ToString()));
                                    if (time.Year % MinorTick.Length == 0) AxisX.TickList.CheckAdd(px, (Importance.Minor, time.Year.ToString().GetLast(2)));
                                    px++;
                                }
                                break;

                            case (TimeUnit.Months): // 1, 2, 3, 4 || Jan, Feb, Mar, Apr
                                MinorTick = Frequency * tickMulti;
                                MajorTick = MinorTick * 6;
                                for (int i = StartPt; i < StopPt; i++)
                                {
                                    DateTime time = bt.IndexToTime(i);
                                    if ((time.Month - 1) % MajorTick.Length == 0) AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("MMM-YY")));
                                    if ((time.Month - 1) % MinorTick.Length == 0) AxisX.TickList.CheckAdd(px, (Importance.Minor, time.ToString("MM")));
                                    px++;
                                }
                                break;

                            case (TimeUnit.Weeks): // 1, 8, 15, 23, 30
                                MinorTick = Frequency * tickMulti;
                                MajorTick = new Frequency(TimeUnit.Months, 1);

                                break;

                            case (TimeUnit.Days): // 1, 2, 3, 4, 5
                                if (tickMulti < 3)
                                {
                                    MinorTick = Frequency * tickMulti;
                                    MajorTick = new Frequency(TimeUnit.Weeks, 1);
                                    for (int i = StartPt; i < StopPt; i++)
                                    {
                                        DateTime time = bt.IndexToTime(i);
                                        DateTime last_time = bt.IndexToTime(i - 1);
                                        if (time.DayOfWeek < last_time.DayOfWeek) AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("MMM-dd"))); ///.WeekOfYear().ToString())); ;
                                        if (time.Day % MinorTick.Length == 0) AxisX.TickList.CheckAdd(px, (Importance.Minor, time.Day.ToString()));
                                        px++;
                                    }
                                }
                                else
                                {
                                    MinorTick = new Frequency(TimeUnit.Weeks, 1);
                                    MajorTick = new Frequency(TimeUnit.Months, 1);
                                    for (int i = StartPt; i < StopPt; i++)
                                    {
                                        DateTime time = bt.IndexToTime(i);
                                        DateTime last_time = bt.IndexToTime(i - 1);
                                        if (time.Day < last_time.Day)
                                        {
                                            if (time.Month < last_time.Month)
                                                AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("yyyy")));
                                            else
                                                AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("MMM")));
                                        }

                                        if (time.DayOfWeek < last_time.DayOfWeek) AxisX.TickList.CheckAdd(px, (Importance.Minor, time.Day.ToString()));
                                        px++;
                                    }
                                }
                                break;

                            case (TimeUnit.Hours): // 09, 10, 11, ... 16
                                MinorTick = Frequency * tickMulti;
                                MajorTick = new Frequency(TimeUnit.Hours, 1);
                                for (int i = StartPt; i < StopPt; i++)
                                {
                                    DateTime time = bt.IndexToTime(i);
                                    DateTime last_time = bt.IndexToTime(i - 1);
                                    if (time.Hour < last_time.Hour)
                                    {
                                        if (time.Day < last_time.Day)
                                            AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("dd")));
                                        else
                                            AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("HH:mm")));
                                    }

                                    //if (time.DayOfWeek < last_time.DayOfWeek) AxisX.TickList.CheckAdd(px, (Importance.Minor, time.Day.ToString()));
                                    px++;
                                }
                                break;

                            case (TimeUnit.Minutes): // How many minutes 
                                MinorTick = Frequency * 5;
                                MajorTick = new Frequency(TimeUnit.Minutes, 30);
                                for (int i = StartPt; i < StopPt; i++)
                                {
                                    DateTime time = bt.IndexToTime(i);
                                    DateTime last_time = bt.IndexToTime(i - 1);

                                    if (time.Hour > last_time.Hour)
                                        AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("HH:mm")));
                                    else if (time.Day > last_time.Day)
                                        AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("MMM-dd")));

                                    if (time.Minute % 10 == 0) AxisX.TickList.CheckAdd(px, (Importance.Minor, time.ToString("HH:mm")));
                                    px++;
                                }
                                break;

                            case (TimeUnit.Seconds):
                                MinorTick = Frequency * 10;
                                MajorTick = new Frequency(TimeUnit.Minutes, 30);
                                for (int i = StartPt; i < StopPt; i++)
                                {
                                    DateTime time = bt.IndexToTime(i);
                                    DateTime last_time = bt.IndexToTime(i - 1);

                                    if (time.Day > last_time.Day)
                                        AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("MMM-dd")));
                                    else if (time.Hour > last_time.Hour)
                                        AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("HH:mm")));

                                    if (time.Second == 0)
                                        if (time.Minute == 30)
                                            AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("HH:mm")));
                                        else if (time.Minute % 5 == 0)
                                            AxisX.TickList.CheckAdd(px, (Importance.Minor, time.ToString("mm")));

                                    px++;
                                }


                                break;

                            case (TimeUnit.None):
                            default:
                                throw new Exception("Invalid TimeInterval Type!");
                        }

                        double ctr = (ChartRange.Minimum != 0) ? (100 * (ChartRange.Maximum - ChartRange.Minimum) / ChartRange.Minimum) : 100;
                        Title = bt.Name + " | " + IndexCount + " Units | CTR: " + ctr.ToString("0.##") + "% | " + LastTimeString;

                        ChartBounds = new Rectangle(
                          LeftYAxisLabelWidth + Margin.Left,
                          Margin.Top,
                          ClientRectangle.Width - LeftYAxisLabelWidth - Margin.Left - RightYAxisLabelWidth - Margin.Right,
                          ClientRectangle.Height - Margin.Top - Margin.Bottom
                          );

                        if (ChartBounds.Width > RightBlankAreaWidth)
                        {
                            AxisX.IndexCount = IndexCount;
                            AxisX.Coordinate(ChartBounds.Width - RightBlankAreaWidth);

                            int ptY = ChartBounds.Top, totalY = TotalAreaHeightRatio;

                            if (AutoScaleFit)
                            {
                                foreach (Area ca in Areas)
                                {
                                    if (ca.Visible && ca.Enabled)
                                    {
                                        if (ca.HasXAxisBar)
                                        {
                                            ca.Bounds = new Rectangle(ChartBounds.X, ptY, ChartBounds.Width, ChartBounds.Height * ca.HeightRatio / totalY - AxisXLabelHeight);
                                            ptY += ca.Bounds.Height + AxisXLabelHeight;
                                            ca.TimeLabelY = ca.Bounds.Bottom + AxisXLabelHeight / 2 + 1;
                                        }
                                        else
                                        {
                                            ca.Bounds = new Rectangle(ChartBounds.X, ptY, ChartBounds.Width, ChartBounds.Height * ca.HeightRatio / totalY);
                                            ptY += ca.Bounds.Height;
                                        }
                                        ca.Coordinate();
                                    }
                                }
                            }
                            else
                            {



                            }
                        }
                    }
            }
            else 
            {
                Title = "No Data";
            }
            PerformLayout();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics; 
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (DataCount < 1)
            {
                g.DrawString("No Data", Main.Theme.FontBold, Main.Theme.GrayTextBrush, new Point(Bounds.Width / 2, Bounds.Height / 2), AppTheme.TextAlignCenter);
            }
            else if (IsActive && !ReadyToShow)
            {
                g.DrawString("Preparing Data... Stand By.", Main.Theme.FontBold, Main.Theme.GrayTextBrush, new Point(Bounds.Width / 2, Bounds.Height / 2), AppTheme.TextAlignCenter);
            }
            else if (IsActive && ReadyToShow && ChartBounds.Width > 0 && m_barTable is BarTable bt)
            {
                lock (bt.DataObjectLock)
                    lock (GraphicsObjectLock)
                    {
                        for (int i = 0; i < Areas.Count; i++)
                        {
                            Area ca = Areas[i];
                            if (ca.Visible && ca.Enabled)
                            {
                                ca.Draw(g);
                                if (ca.HasXAxisBar)
                                {
                                    for (int j = 0; j < IndexCount; j++)
                                    {
                                        int x = IndexToPixel(j);
                                        int y = ca.Bottom;
                                        g.DrawLine(ca.Theme.EdgePen, x, y, x, y + 1);

                                        if (i < Areas.Count - 1)
                                        {
                                            y = Areas[i + 1].Top;
                                            g.DrawLine(ca.Theme.EdgePen, x, y, x, y - 1);
                                        }
                                    }
                                }
                            }
                        }

                        var overlayList = BarTable.IChartOverlays;
                        foreach (var (ic, bap) in overlayList)
                        {
                            ic.Draw(g, this, bap);
                        }
                    }
            }
        }
    }
}
