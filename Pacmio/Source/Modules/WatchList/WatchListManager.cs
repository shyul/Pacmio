/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;

namespace Pacmio
{
    public static class WatchListManager
    {
        private static Dictionary<string, WatchList> NameToListLUT { get; } = new Dictionary<string, WatchList>();

        public static List<WatchList> List
        {
            get
            {
                lock (NameToListLUT)
                    return NameToListLUT.Values.ToList();
            }
        }

        public static int Count
        {
            get
            {
                lock (NameToListLUT)
                    return NameToListLUT.Count;
            }
        }

        public static T Add<T>(T wt) where T : WatchList
        {
            WatchList result = null;

            lock (NameToListLUT)
            {
                if (NameToListLUT.ContainsKey(wt.Name))
                {
                    result = NameToListLUT[wt.Name];
                    if (!(result is T))
                    {
                        if (result is DynamicWatchList dwt)
                            dwt.Stop();

                        NameToListLUT.Remove(result.Name);
                        NameToListLUT.Add(wt.Name, wt);
                        result = wt;
                    }
                }
                else
                {
                    result = wt;
                    NameToListLUT.Add(result.Name, result);
                }
            }

            UpdateTime = DateTime.Now;
            OnUpdateHandler?.Invoke(0, UpdateTime, "");
            return result as T;
        }

        public static WatchList Remove(WatchList wt)
        {
            WatchList result = null;
            lock (NameToListLUT)
            {
                if (NameToListLUT.ContainsKey(wt.Name))
                {
                    result = NameToListLUT[wt.Name];
                    if (result is DynamicWatchList dwt) dwt.Stop();
                    NameToListLUT.Remove(wt.Name);
                }
            }
            UpdateTime = DateTime.Now;
            OnUpdateHandler?.Invoke(0, UpdateTime, "");
            return result;
        }

        public static void Clear()
        {
            lock (NameToListLUT)
            {
                Stop();
                NameToListLUT.Clear();
            }

            UpdateTime = DateTime.Now;
            OnUpdateHandler?.Invoke(0, UpdateTime, "");
        }

        public static List<T> WatchListByType<T>() where T : WatchList => List.Where(n => n is T list).Select(n => n as T).ToList();

        public static void Start() => WatchListByType<DynamicWatchList>().ForEach(n => n.Start());

        public static void Stop() => WatchListByType<DynamicWatchList>().ForEach(n => n.Stop());

        public static DateTime UpdateTime { get; private set; }

        public static event StatusEventHandler OnUpdateHandler;

    }
}
