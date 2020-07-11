/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Pacmio
{
    public static class ScannerManager
    {
        public static readonly ConcurrentDictionary<ScannerInfo, Dictionary<int, (int ConId, string Name, DateTime time)>> List
            = new ConcurrentDictionary<ScannerInfo, Dictionary<int, (int ConId, string Name, DateTime time)>>();

        public static ScannerInfo GetOrAdd(ScannerInfo info)
        {
            if (!List.ContainsKey(info))
            {
                if (List.TryAdd(info, new Dictionary<int, (int ConId, string Name, DateTime time)>()))
                {
                    IB.Client.SendRequest_ScannerSubscription(info);
                }
            }

            if (!List.ContainsKey(info))
            {
                return null;
            }
            else
            {
                return List.Keys.Where(n => n == info).First();
            }
        }

        public static void Remove(ScannerInfo info)
        {
            if (List.ContainsKey(info))
            {
                List.TryRemove(info, out _);
                IB.Client.SendCancel_ScannerSubscription(info.RequestId);
            }
        }

        public static void CancelAll()
        {
            foreach (ScannerInfo info in List.Keys)
            {
                IB.Client.SendCancel_ScannerSubscription(info.RequestId);
            }

            List.Clear();
        }

        #region Data Requests

        public static void Request_ScannerParameters() => IB.Client.SendRequest_ScannerParameters();

        #endregion Data Requests

        /// <summary>
        /// Do not call any IB function in this method... or it is going to lock up.
        /// </summary>
        /// <param name="info"></param>
        public static void Updated(ScannerInfo info)
        {
            if (List.ContainsKey(info))
            {
                //var clist = ;

                var conList = List[info].Select(n => (n.Key, ContractList.GetOrFetch(n.Value.ConId))).OrderBy(n => n.Key);

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
                        Console.WriteLine("Rank " + i.Key + ": " + List[info][i.Key].Name + "\t" + List[info][i.Key].ConId);
                    }
                }
                Console.WriteLine("Scanner Result End.\n\n");
            }
        }
    }
}
