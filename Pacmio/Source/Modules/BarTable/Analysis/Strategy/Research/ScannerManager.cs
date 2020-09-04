/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public static class ScannerManager
    {
        public static List<Scanner> List { get; } = new List<Scanner>();

        public static void Start() => List.ForEach(n => n.Start());

        public static void Stop() => List.ForEach(n => n.Stop());

        public static void Clear() { Stop(); List.Clear(); }

        public static int Count => List.Count;

        public static Scanner Add(Scanner sc) 
        {
            if (List.Contains(sc))
                return List.Where(n => n == sc).First(); // as TIProData.TopListScanner;
            else
            {
                List.CheckAdd(sc);
                return sc;
            }
        }

        public static TIProData.TopListScanner AddTradeIdeasTopList(string name = "Gappers List", double minPrice = 1.5, double maxPrice = 25, double minVolume = 50e3, double minPercent = 5, double minATR = 0.25) 
        {
            double percent = Math.Abs(minPercent);

            TIProData.TopListScanner tls = new TIProData.TopListScanner(name)
            {
                Price = (minPrice, maxPrice),
                Volume = (minVolume, double.NaN),
                GapPercent = (percent, -percent),
                AverageTrueRange = (minATR, double.NaN),
                ExtraConfig = "form=1&sort=MaxGUP&omh=1&col_ver=1&show0=D_Symbol&show1=Price&show2=Float&show3=SFloat&show4=GUP&show5=TV&show6=EarningD&show7=Vol5&show8=STP&show9=RV&show10=D_Name&show11=RD&show12=FCP&show13=D_Sector&show14=",
            };

            return Add(tls) as TIProData.TopListScanner;
        }










        public static readonly ConcurrentDictionary<ScannerConfigOld, Dictionary<int, (int ConId, string Name, DateTime time)>> Old_List
            = new ConcurrentDictionary<ScannerConfigOld, Dictionary<int, (int ConId, string Name, DateTime time)>>();

        public static ScannerConfigOld GetOrAdd(ScannerConfigOld info)
        {
            if (!Old_List.ContainsKey(info))
            {
                if (Old_List.TryAdd(info, new Dictionary<int, (int ConId, string Name, DateTime time)>()))
                {
                    IB.Client.SendRequest_ScannerSubscription(info);
                }
            }

            if (!Old_List.ContainsKey(info))
            {
                return null;
            }
            else
            {
                return Old_List.Keys.Where(n => n == info).First();
            }
        }

        public static void Remove(ScannerConfigOld info)
        {
            if (Old_List.ContainsKey(info))
            {
                Old_List.TryRemove(info, out _);
                IB.Client.SendCancel_ScannerSubscription(info.RequestId);
            }
        }

        public static void CancelAll()
        {
            foreach (ScannerConfigOld info in Old_List.Keys)
            {
                IB.Client.SendCancel_ScannerSubscription(info.RequestId);
            }

            Old_List.Clear();
        }

        #region Data Requests

        public static void Request_ScannerParameters() => IB.Client.SendRequest_ScannerParameters();

        #endregion Data Requests

        /// <summary>
        /// Do not call any IB function in this method... or it is going to lock up.
        /// </summary>
        /// <param name="info"></param>
        public static void Updated(ScannerConfigOld info)
        {
            if (Old_List.ContainsKey(info))
            {
                //var clist = ;

                var conList = Old_List[info].Select(n => (n.Key, ContractList.GetOrFetch(n.Value.ConId))).OrderBy(n => n.Key);

                /*
                var names = clist.Values.Where(n => ContractList.GetList(n.ConId).Count() == 0).Select(n => n.Name).ToArray();

                Task t0 = new Task(() =>
                {
                    //string[] name_s = names; // new string[] { string.Join(",", names) };
                    ContractList.Download(names);
                });
                t0.Start();
                */
                Console.WriteLine("\nScanner Result: " + info.Code);
                foreach (var i in conList)
                {
                    if (i.Item2 is Contract c)
                    {
                        if (c is IBusiness it)
                        {
                            Console.WriteLine("Rank " + i.Key + ": " + c.Name + "\t" + "\t" + it.ISIN + "\t" + c.ExchangeName + "\t" + c.FullName);
                            if (!IB.Client.SubscriptionOverflow)
                                c.Request_MarketTicks("236,375");
                            /*
                            BarTable bt = c.GetOrAdd(BarFreq.Minute, BarType.Trades);

                            BarTableList.RequestAction.Enqueue(new Action(() => {
                                bt.Fetch(new Period(DateTime.Now.AddHours(8), DateTime.Now.AddDays(-7)), true);
                            }));
                            */



                            /*
                            BarFreq barFreq = SelectTestChartBarFreq.Text.ParseEnum<BarFreq>();
                            BarType barType = SelectTestChartBarType.Text.ParseEnum<BarType>();
                            Period pd = new Period(dateTimePickerTestChartStart.Value, dateTimePickerTestChartStop.Value);

                            BarTable bt = ActiveContract.GetOrAdd(barFreq, barType);
                            BarChartForm bcf = ConfigChart(bt);

                            bt.Fetch(pd, true);

                            ChartForms.Add(bcf);

                            Program.Ctf.AddForm(DockStyle.Fill, 0, bcf);*/
                        }
                        else
                        {
                            Console.WriteLine("Rank " + i.Key + ": " + c.Name + "\t" + "\t" + "NoISIN" + "\t" + c.ExchangeName + "\t" + c.FullName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Rank " + i.Key + ": " + Old_List[info][i.Key].Name + "\t" + Old_List[info][i.Key].ConId);
                    }
                }
                Console.WriteLine("Scanner Result End.\n\n");
            }
        }
    }
}
