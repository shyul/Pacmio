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

        public static IEnumerable<WatchList> List
        {
            get
            {
                lock (NameToListLUT)
                    return NameToListLUT.Values.ToArray();
            }
        }





        public static IEnumerable<InteractiveBrokerWatchList> GetInteractiveBrokerWatchList()
        {
            lock (NameToListLUT)
                return NameToListLUT.Values.Where(n => n is InteractiveBrokerWatchList list).Select(n => n as InteractiveBrokerWatchList).ToArray();
        }

        public static InteractiveBrokerWatchList GetInteractiveBrokerWatchList(int requestId)
        {
            return GetInteractiveBrokerWatchList().Where(n => n.RequestId == requestId).FirstOrDefault();
        }




    }
}
