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

        public static T Add<T>(T sc) where T : Scanner
        {
            if (List.Contains(sc))
                return List.Where(n => n == sc).First() as T;
            else
            {
                List.CheckAdd(sc);
                return sc;
            }
        }

        public static T Remove<T>(T sc) where T : Scanner
        {
            if (List.Contains(sc))
            {
                sc = List.Where(n => n == sc).First() as T;
                List.Remove(sc);
            }

            sc.Stop();
            return sc;
        }

        public static TIProData.TopListHandler AddTradeIdeasTopList(string name = "Gappers List", double minPrice = 1.5, double maxPrice = 25, double minVolume = 50e3, double minPercent = 5, double minATR = 0.25)
        {
            double percent = Math.Abs(minPercent);

            TIProData.TopListHandler tls = new TIProData.TopListHandler(name)
            {
                Price = (minPrice, maxPrice),
                Volume = (minVolume, double.NaN),
                GapPercent = (percent, -percent),
                AverageTrueRange = (minATR, double.NaN),
            };

            return Add(tls);
        }

        public static TIProData.AlertHandler AddTradeIdeasAlert()
        {
            TIProData.AlertHandler tal = new TIProData.AlertHandler("Low Float MOMO")
            {
                Price = (1, double.NaN),
                GainPercent = (1, double.NaN),
                Volume = (1e5, double.NaN),
                Volume5Days = (3e5, double.NaN),
                //NewHigh = 0,
                Float = (double.NaN, 25e6),
            };

            return Add(tal);
        }

        public static void Request_ScannerParameters() => IB.Client.SendRequest_ScannerParameters();

    }
}
