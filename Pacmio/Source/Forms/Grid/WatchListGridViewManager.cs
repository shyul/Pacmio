/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;

namespace Pacmio
{
    public static class WatchListGridViewManager
    {
        public static void Add(WatchListGridView gv)
        {
            if (!List.Contains(gv))
            {
                List.Add(gv);
                Root.Form.AddForm(DockStyle.Fill, 0, gv);
            }
            gv.ReadyToShow = true;

        }

        /// <summary>
        /// TODO: change to WatchList -> GridViewLUT
        /// </summary>
        private static List<WatchListGridView> List { get; } = new List<WatchListGridView>();

        /// <summary>
        /// Update All UI Elements when a new tick comes in
        /// </summary>
        /// <param name="c"></param>
        /// 
        /*
        public static void UpdateUI(MarketData md)
        {
            Task.Run(() =>
            {
                List.Where(n => n.Contains(md)).ToList().ForEach(n => n.SetAsyncUpdateUI());
            });
        }*/

        public static Contract SelectedContract
        {
            get
            {
                if (DockCanvas.ActiveDockForm is WatchListGridView gv)
                {
                    return gv.SelectedContract;
                }
                return null;
            }
        }
    }
}

