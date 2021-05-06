/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// 1. The Pattern will clapse and vanish if new ticks were added to Daily and above BarTables
///     p1: In the chart Last Bar public void DrawBackground(Graphics g, BarChart bc), 
///     p2:         private void SetCalculationPointer(int pt)
///     p3: BAS LastCalculateIndex is bound to use with BarChart
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class BarChart : ChartWidget
    {
        public BarChart(string name, OhlcType type) : base(name)
        {
            Icon = Properties.Resources.Icon_Chart;
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

            //AddArea(PositionArea = new PositionArea(this));
            //AddArea(SignalArea = new SignalArea(this));
            AddArea(MainArea = new MainBarChartArea(this, 50, 0.3f) { HasXAxisBar = true, });

            OhlcType = type;
            ResumeLayout(false);
            PerformLayout();

            BarChartManager.Add(this);
        }

        protected override void Dispose(bool disposing)
        {
            Close();
            GC.Collect();
        }

        public override void Close()
        {
            BarChartManager.Remove(this);

            if (m_BarTable is BarTable bt)
            {
                bt.RemoveDataConsumer(this);
            }

            Console.WriteLine(TabName + ": The BarChart is closing");
            AsyncUpdateUITask_Cts.Cancel();
            HostContainer?.Remove(this);
        }

        public string Title { get => MainArea.PriceSeries.Legend.Label; set => MainArea.PriceSeries.Legend.Label = value; }

        public override string Label { get => MainArea.PriceSeries.Label; set => MainArea.PriceSeries.Label = value; }

        public override string Description { get => MainArea.PriceSeries.Description; set => MainArea.PriceSeries.Description = value; }

        public MainBarChartArea MainArea { get; }

        public void Config(BarTable bt, BarAnalysisList bat)
        {
            lock (GraphicsLockObject)
            {
                ReadyToShow = false;
                BarTable = bt;
                BarAnalysisList = bat;

                if (m_BarTable is BarTable)
                {
                    while (bt.Status == TableStatus.Calculating || bt.Status == TableStatus.Ticking) { Thread.Sleep(1); }
                    // Add this line, or nothing will show
                    //StopPt = m_BarTable[BarAnalysisList].LastCalculateIndex;
                    LastIndexMax = bt[bat].LastCalculateIndex;
                    StopPt = LastIndexMax + 1;
                    // Add this line or the Pattern won't show.
                    //PointerSnapToEnd();
                    ReadyToShow = true;
                    m_AsyncUpdateUI = true;
                }

                //Console.WriteLine("IsActive = " + IsActive + " | m_ReadyToShow = " + m_ReadyToShow + " | IsBarTable = " + (m_BarTable is BarTable) + " | bt.ReadyToShow = " + m_BarTable.ReadyToShow);
                //Console.WriteLine("BarChart: StopPt = " + StopPt);
            }
            m_AsyncUpdateUI = true;
        }

        public override void RemoveDataSource()
        {
            lock (GraphicsLockObject)
            {
                ReadyToShow = false;
                m_BarTable = null;
            }
        }

        private void RemoveAllChartSeries()
        {
            lock (GraphicsLockObject)
            {
                List<Area> areaToRemove = Areas.Where(n => n != MainArea).ToList();
                areaToRemove.ForEach(n => Areas.Remove(n));
                MainArea.RemoveSeries();
            }
        }

        public BarAnalysisList BarAnalysisList
        {
            get => m_BarAnalysisList;

            private set
            {
                RemoveAllChartSeries();
                m_BarAnalysisList = value;

                if (m_BarAnalysisList is BarAnalysisList bat)
                {
                    foreach (var ic in bat.ChartSeries)
                    {
                        ic.ConfigChart(this);
                    }

                    foreach (var tg in bat.TagSeries)
                    {
                        tg.ConfigChart(this);
                    }
                    
                    if (bat.SignalList.Count() > 0)
                    {
                        AddArea(new SignalArea(this, BarAnalysisList)
                        {
                            Order = int.MinValue,
                            HasXAxisBar = false,
                        });
                    }
                }
            }
        }

        private BarAnalysisList m_BarAnalysisList = null;

        public BarTable BarTable
        {
            get => m_BarTable;

            private set
            {
                if (m_BarTable is BarTable bt0)
                {
                    bt0.RemoveDataConsumer(this);
                }

                m_BarTable = value;

                if (m_BarTable is BarTable bt)
                {
                    bt.AddDataConsumer(this);
                    TabName = Name = m_BarTable.Name;
                }
                else
                {
                    StopPt = 0;
                    TabName = "No BarTable";
                }
                /*
                if (m_BarAnalysisList is BarAnalysisList bat)
                {
                    m_BarTable.CalculateRefresh(bat);
                }*/
            }
        }

        private BarTable m_BarTable = null;

        public override ITable Table
        {
            get => m_BarTable;

            set
            {
                if (value is BarTable bt)
                    BarTable = bt;
                else
                    m_BarTable = null;
            }
        }

        public override int DataCount => m_BarTable is BarTable bt ? bt.Count : 0;

        public override string this[int i]
        {
            get
            {
                if (BarFreq < BarFreq.Minute)
                    return m_BarTable.IndexToTime(i + StartPt).ToString("MMM-dd HH:mm:ss");
                else if (BarFreq < BarFreq.Daily)
                    return m_BarTable.IndexToTime(i + StartPt).ToString("MMM-dd HH:mm");
                else
                    return m_BarTable.IndexToTime(i + StartPt).ToString("MMM-dd-yyyy");
            }
        }

        public override void PointerSnapToEnd()
        {
            lock (GraphicsLockObject)
            {
                if (m_BarTable is BarTable bt && m_BarAnalysisList is BarAnalysisList bat)
                {
                    LastIndexMax = bt[bat].LastCalculateIndex;
                    StopPt = LastIndexMax + 1;
                }
                else
                {
                    LastIndexMax = -1;
                    StopPt = 0;
                }
            }

            m_AsyncUpdateUI = true; // async update
        }

        public override void PointerSnapToNextTick()
        {
            lock (GraphicsLockObject)
            {
                if (m_BarTable is BarTable bt &&
                ((StopPt > bt.LastCalculateIndex - 2 && StopPt < bt.LastCalculateIndex + 2) || StopPt == 0))
                {
                    LastIndexMax = bt.LastCalculateIndex;
                    StopPt = LastIndexMax + 1;
                }
            }
            m_AsyncUpdateUI = true;
        }

        public Bar LastBar => LastIndex < 0 ? null : m_BarTable[LastIndex];

        public Bar LastBar_1 => LastIndex < 1 ? null : m_BarTable[LastIndex - 1];

        public double LastClose => LastBar is null ? double.NaN : LastBar.Close;

        public DateTime LastTime => LastBar is null ? DateTime.Now : LastBar.Time;

        public string LastTimeString => this[LastIndex - StartPt];

        public OhlcType OhlcType { get => MainArea.PriceSeries.Type; set => MainArea.PriceSeries.Type = value; }

        public Period Period => new(m_BarTable.IndexToTime(StartPt), m_BarTable.IndexToTime(StopPt));

        public BarFreq BarFreq => m_BarTable.BarFreq;

        public Frequency Frequency => m_BarTable.Frequency;

        public TimeUnit TimeUnit => Frequency.Unit;

        public Frequency MajorTick { get; private set; }

        public Frequency MinorTick { get; private set; }

        public Range<double> ChartRange => MainArea.AxisY(AlignType.Right).Range;

        public double ChartRangePercent => ChartRange.Minimum != 0 ? (100 * (ChartRange.Maximum - ChartRange.Minimum) / ChartRange.Minimum) : 100;

        public override bool ReadyToShow { get => IsActive && m_ReadyToShow && m_BarTable is BarTable bt && bt.ReadyToShow; set => m_ReadyToShow = value; }

        public override void CoordinateOverlay() { }

        protected override void CoordinateLayout()
        {
            ResumeLayout(true);
            ChartBounds = new Rectangle(
                LeftYAxisLabelWidth + Margin.Left,
                Margin.Top,
                ClientRectangle.Width - LeftYAxisLabelWidth - Margin.Left - RightYAxisLabelWidth - Margin.Right,
                ClientRectangle.Height - Margin.Top - Margin.Bottom);

            if (ReadyToShow)
            {
                lock (m_BarTable.DataLockObject)
                    lock (GraphicsLockObject)
                    {
                        //SignalArea.Visible = HasSignalColumn; //true;
                        //PositionArea.Visible = HasPositionColumn;

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
                            case TimeUnit.Years: // 02, 03, 04 || 2002, 2003, 2004
                                MinorTick = new Frequency(TimeUnit.Years, Frequency.Length * tickMulti);
                                MajorTick = MinorTick * 5;
                                for (int i = StartPt; i < StopPt; i++)
                                {
                                    DateTime time = m_BarTable.IndexToTime(i);
                                    if (time.Year % MajorTick.Length == 0) AxisX.TickList.CheckAdd(px, (Importance.Major, time.Year.ToString()));
                                    if (time.Year % MinorTick.Length == 0) AxisX.TickList.CheckAdd(px, (Importance.Minor, time.Year.ToString().GetLast(2)));
                                    px++;
                                }
                                break;

                            case TimeUnit.Months: // 1, 2, 3, 4 || Jan, Feb, Mar, Apr
                                MinorTick = Frequency * tickMulti;
                                MajorTick = MinorTick * 6;
                                for (int i = StartPt; i < StopPt; i++)
                                {
                                    DateTime time = m_BarTable.IndexToTime(i);
                                    if ((time.Month - 1) % MajorTick.Length == 0) AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("yyyy")));
                                    if ((time.Month - 1) % MinorTick.Length == 0) AxisX.TickList.CheckAdd(px, (Importance.Minor, time.ToString("MM")));
                                    px++;
                                }
                                break;

                            case TimeUnit.Weeks: // 1, 8, 15, 23, 30
                                MinorTick = Frequency * tickMulti;
                                MajorTick = new Frequency(TimeUnit.Months, 1);

                                break;

                            case TimeUnit.Days: // 1, 2, 3, 4, 5
                                if (tickMulti < 3)
                                {
                                    MinorTick = Frequency * tickMulti;
                                    MajorTick = new Frequency(TimeUnit.Weeks, 1);
                                    for (int i = StartPt; i < StopPt; i++)
                                    {
                                        DateTime time = m_BarTable.IndexToTime(i);
                                        DateTime last_time = m_BarTable.IndexToTime(i - 1);
                                        if (time.DayOfWeek < last_time.DayOfWeek)
                                            AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("MMM-dd"))); ///.WeekOfYear().ToString())); ;
                                        if (time.Day % MinorTick.Length == 0)
                                            AxisX.TickList.CheckAdd(px, (Importance.Minor, time.Day.ToString()));
                                        px++;
                                    }
                                }
                                else
                                {
                                    MinorTick = new Frequency(TimeUnit.Weeks, 1);
                                    MajorTick = new Frequency(TimeUnit.Months, 1);
                                    for (int i = StartPt; i < StopPt; i++)
                                    {
                                        DateTime time = m_BarTable.IndexToTime(i);
                                        DateTime last_time = m_BarTable.IndexToTime(i - 1);
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

                            case TimeUnit.Hours: // 09, 10, 11, ... 16
                                MinorTick = Frequency * tickMulti;
                                MajorTick = new Frequency(TimeUnit.Hours, 2);
                                for (int i = StartPt; i < StopPt; i++)
                                {
                                    DateTime time = m_BarTable.IndexToTime(i);
                                    DateTime last_time = m_BarTable.IndexToTime(i - 1);

                                    if (Frequency.Length == 1)
                                    {
                                        if (time.Month < last_time.Month)
                                            AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("yyyy")));
                                        else if (time.Month > last_time.Month)
                                            AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("MMM")));
                                        else if (time.Day > last_time.Day)
                                            AxisX.TickList.CheckAdd(px, (Importance.Major, "[" + time.ToString("%d") + "]"));
                                        else if (time.Hour % 2 == 0)
                                            AxisX.TickList.CheckAdd(px, (Importance.Minor, time.ToString("%H")));
                                    }
                                    else
                                    {
                                        if (time.Month < last_time.Month)
                                            AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("yyyy")));
                                        else if (time.Month > last_time.Month)
                                            AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("MMM")));
                                        else if (time.DayOfWeek < last_time.DayOfWeek)
                                            AxisX.TickList.CheckAdd(px, (Importance.Minor, time.ToString("%d")));
                                    }
                                    px++;
                                }
                                break;

                            case TimeUnit.Minutes: // How many minutes 
                                MinorTick = Frequency * 5;
                                MajorTick = new Frequency(TimeUnit.Minutes, 30);
                                for (int i = StartPt; i < StopPt; i++)
                                {
                                    DateTime time = m_BarTable.IndexToTime(i);
                                    DateTime last_time = m_BarTable.IndexToTime(i - 1);

                                    if (time.Hour > last_time.Hour)
                                        AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("HH:mm")));
                                    else if (time.Day > last_time.Day)
                                        AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("MMM-d")));

                                    if (time.Minute % 10 == 0) AxisX.TickList.CheckAdd(px, (Importance.Minor, time.ToString("HH:mm")));
                                    px++;
                                }
                                break;

                            case TimeUnit.Seconds:
                                MinorTick = Frequency * 10;
                                MajorTick = new Frequency(TimeUnit.Minutes, 30);
                                for (int i = StartPt; i < StopPt; i++)
                                {
                                    DateTime time = m_BarTable.IndexToTime(i);
                                    DateTime last_time = m_BarTable.IndexToTime(i - 1);

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

                            case TimeUnit.None:
                            default:
                                throw new("Invalid TimeInterval Type!");
                        }

                        if (ChartBounds.Width > RightBlankAreaWidth)
                        {
                            AxisX.IndexCount = IndexCount;
                            AxisX.Coordinate(ChartBounds.Width - RightBlankAreaWidth);

                            int ptY = ChartBounds.Top;
                            float totalY = TotalAreaHeightRatio;

                            if (AutoScaleFit)
                            {
                                foreach (Area ca in Areas)
                                {
                                    if (ca.Visible && ca.Enabled)
                                    {
                                        if (ca.HasXAxisBar)
                                        {
                                            ca.Bounds = new Rectangle(ChartBounds.X, ptY, ChartBounds.Width, (ChartBounds.Height * ca.HeightRatio / totalY - AxisXLabelHeight).ToInt32());
                                            ptY += ca.Bounds.Height + AxisXLabelHeight;
                                            ca.TimeLabelY = ca.Bounds.Bottom + AxisXLabelHeight / 2 + 1;
                                        }
                                        else
                                        {
                                            ca.Bounds = new Rectangle(ChartBounds.X, ptY, ChartBounds.Width, (ChartBounds.Height * ca.HeightRatio / totalY).ToInt32());
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

                Title = m_BarTable.Name + " | " + IndexCount + " Units | CTR: " + ChartRangePercent.ToString("0.##") + "% | " + LastTimeString;
            }
            else
            {
                Title = m_BarTable.Name + " | " + IndexCount + " Units | " + LastTimeString;
            }

            PerformLayout();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (ChartBounds.Width > 0 && ChartBounds.Height > 0)
            {
                if (m_BarTable is null)
                {
                    g.DrawString("Not configured", Main.Theme.FontBold, Main.Theme.GrayTextBrush, new Point(Bounds.Width / 2, Bounds.Height / 2), AppTheme.TextAlignCenter);
                }
                else
                {
                    if (!m_BarTable.ReadyToShow)
                    {
                        g.DrawString("Pacmio Chart | " + m_BarTable.Contract.Name + " | " + m_BarTable.Contract.FullName,
                            Main.Theme.TinyFont, Main.Theme.GrayTextBrush, new Point(ChartBounds.Left - 2, ChartBounds.Top - 5), AppTheme.TextAlignLeft);

                        if (DataCount < 1)
                            g.DrawString("No Data", Main.Theme.FontBold, Main.Theme.GrayTextBrush, new Point(Bounds.Width / 2, Bounds.Height / 2), AppTheme.TextAlignCenter);
                        else
                            g.DrawString("Preparing Data... Stand By.", Main.Theme.FontBold, Main.Theme.GrayTextBrush, new Point(Bounds.Width / 2, Bounds.Height / 2), AppTheme.TextAlignCenter);
                    }
                    else if (ReadyToShow)
                    {
                        lock (m_BarTable.DataLockObject)
                            lock (GraphicsLockObject)
                            {
                                g.DrawString("Pacmio Chart | " + m_BarTable.Contract.Name + " | " + m_BarTable.Contract.FullName + " | From " + m_BarTable.FirstTime + " to " + m_BarTable.LastTime,
                                    Main.Theme.TinyFont, Main.Theme.GrayTextBrush, new Point(ChartBounds.Left - 2, ChartBounds.Top - 5), AppTheme.TextAlignLeft);

                                foreach (var ic in BarAnalysisList.ChartBackgrounds.Where(n => n.ChartEnabled))
                                {
                                    ic.DrawBackground(g, this);
                                }

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

                                foreach (var ic in BarAnalysisList.ChartOverlays)
                                {
                                    ic.DrawOverlay(g, this);
                                }
                            }
                    }
                }
            }
        }
    }
}
