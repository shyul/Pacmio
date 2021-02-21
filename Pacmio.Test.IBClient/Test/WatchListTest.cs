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
using Pacmio;
using Pacmio.TIProData;

namespace TestClient
{
    public static class WatchListTest
    {
        public static TopListHandler AddTradeIdeasTopList(string name = "Gappers List", double minPrice = 1.5, double maxPrice = 25, double minVolume = 50e3, double minPercent = 5, double minATR = 0.25)
        {
            double percent = Math.Abs(minPercent);

          TopListHandler tls = new TopListHandler(name)
            {
                Price = (minPrice, maxPrice),
                Volume = (minVolume, double.NaN),
                GapPercent = (percent, -percent),
                AverageTrueRange = (minATR, double.NaN),
            };

            return WatchListManager.Add(tls);
        }

        public static AlertHandler AddTradeIdeasAlert()
        {
            AlertHandler tal = new AlertHandler("Low Float MOMO")
            {
                Price = (1, double.NaN),
                GainPercent = (1, double.NaN),
                Volume = (1e5, double.NaN),
                Volume5Days = (3e5, double.NaN),
                //NewHigh = 0,
                Float = (double.NaN, 25e6),
            };

            return WatchListManager.Add(tal);
        }
    }
}
