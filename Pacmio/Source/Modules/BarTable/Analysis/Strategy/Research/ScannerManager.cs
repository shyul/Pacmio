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
                ExtraConfig = "form=1&omh=1&col_ver=1&show0=D_Symbol&show1=Price&show2=Float&show3=SFloat&show4=GUP&show5=TV&show6=EarningD&show7=Vol5&show8=STP&show9=RV&show10=D_Name&show11=RD&show12=FCP&show13=D_Sector&show14=",
            }; // &sort=MaxGUP

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
                //ExtraConfig = "O=2000000000000000000000000000001_1D_0&form=1&omh=1&col_ver=1&show0=D_Symbol&show1=D_Type&show2=D_Time&show3=D_Desc&show4=Price&show5=RV&show6=TV&show7=FCP&show8=Vol1&show9=Vol5&show10=PV&show11=Count&show12=Float&show13=SFloat&show14=D_Name",
                ExtraConfig = "O=2000000000000000000000000000001_1D_0&SL=X1o5&col_ver=1&show0=D_Symbol&show1=D_Type&show2=D_Time&show3=D_Desc&show4=Price&show5=RV&show6=TV&show7=FCP&show8=Vol1&show9=Vol5&show10=PV&show11=Count&show12=Float&show13=SFloat&show14=D_Name",
            };

            return Add(tal);
        }

        public static void Request_ScannerParameters() => IB.Client.SendRequest_ScannerParameters();

    }
}
