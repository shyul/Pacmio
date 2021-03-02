/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.ComponentModel;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    [Serializable, DataContract]
    public class StockData : MarketData
    {
        public override void Initialize(Contract c)
        {
            base.Initialize(c);
            RTLastTime = DateTime.MinValue;
            RTLastPrice = -1;
        }

        [DataMember]
        public bool IsFilteredRTStream { get; set; } = true;

        [IgnoreDataMember]
        public DateTime RTLastTime { get; private set; } = DateTime.MinValue;

        [IgnoreDataMember]
        public double RTLastPrice { get; private set; } = -1;

        // TODO: 
        // Queue the Tape Here

        public void InboundLiveTick(DateTime time, double price, double size)
        {
            if (time > RTLastTime)
            {
                RTLastTime = time;

                if (double.IsNaN(price))
                {
                    price = RTLastPrice;

                    // Even tick
                }
                else
                {
                    if (price > RTLastPrice)
                    {
                        // Is an advancing tick
                    }
                    else
                    {
                        // Is a declining tick
                    }

                    RTLastPrice = price;
                }

                if (price > 0)
                {
                    lock (DataConsumers)
                    {
                        Parallel.ForEach(DataConsumers.Where(n => n is BarTable b && b.IsLive).Select(n => n as BarTable), bt =>
                        {
                            if (bt.BarFreq < BarFreq.Daily)// || bt.LastTime == time.Date)
                            {
                                bt.AddPriceTick(time, price, size);
                            }
                            else if (bt.BarFreq >= BarFreq.Daily)// && bt.LastTime < time.Date)
                            {
                                DateTime date = time.Date;
                                //Console.WriteLine(">>> [[[[ Received for " + bt.ToString() + " | LastTime = " + bt.LastTime.Date + " | time = " + time.Date);
                                if (bt.LastTime.Date < date)
                                {
                                    // Also check the Stock Data time stamp here! Is it current???
                                    if (bt.Status == TableStatus.Ready && (!double.IsNaN(Open)) && (!double.IsNaN(High)) && (!double.IsNaN(Low)) && (!double.IsNaN(LastPrice)) && (!double.IsNaN(Volume)))
                                    {
                                        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> [[[[ Adding new candle: " + date + " | " + Open + " | " + High + " | " + Low + " | " + LastPrice + " | " + Volume);
                                        Bar b = new Bar(bt, date, Open, High, Low, LastPrice, Volume);
                                        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> [[[[ Adding new candle: " + b.DataSourcePeriod.Stop);
                                        bt.MergeFromSmallerBar(b);
                                        Console.WriteLine("bt.LastBar.DataSourcePeriod.Stop = " + bt.LastBar.DataSourcePeriod.Stop);
                                        Console.WriteLine("bt.LastBar.DataSourcePeriod.Stop = " + bt.LastBar.DataSourcePeriod.Stop);

                                    }
                                }
                                else if (bt.LastTime == date)
                                {
                                    bt.AddPriceTick(time, price, size);
                                }

                                //Console.WriteLine("bt.Status = " + bt.Status);
                            }
                        });
                    }
                }
            }
        }

        [DataMember]
        public double MarketCap { get; set; } = double.NaN;

        [DataMember]
        public double FloatShares { get; set; } = double.NaN;

        [DataMember]
        public double ShortPercent { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("S.Shares"), GridColumnOrder(18), GridRenderer(typeof(NumberGridRenderer), 80)]
        public double ShortableShares { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Short"), GridColumnOrder(17), GridRenderer(typeof(NumberGridRenderer), 60)]
        public double ShortStatus { get; set; } = double.NaN;

        [DataMember]
        public bool EnableNews { get; set; } = true;

        /// <summary>
        /// Be aware of toggling changes
        /// </summary>
        [DataMember]
        public bool EnableShortableShares { get; set; } = true;

        [DataMember]
        public bool FilteredTicks { get; set; } = true;

        public override bool StartTicks()
        {
            FilteredTicks = true;
            EnableNews = true;
            EnableShortableShares = true;
            return IB.Client.SendRequest_MarketData(this);
        }



        // Tape

        // Position in Range

        // L2



        // News






        // Social Media
    }
}
