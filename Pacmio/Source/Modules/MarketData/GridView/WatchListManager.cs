/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;

namespace Pacmio
{
    public static class WatchListManager
    {
        public static void Add(MarketDataGridView gv)
        {
            if (!List.Contains(gv))
            {
                List.Add(gv);
            }
            gv.ReadyToShow = true;
            Root.Form.AddForm(DockStyle.Fill, 0, gv);
        }

        private static List<MarketDataGridView> List { get; } = new List<MarketDataGridView>();

        /// <summary>
        /// Update All UI Elements when a new tick comes in
        /// </summary>
        /// <param name="c"></param>
        public static void UpdateUI(MarketData md)
        {
            Task.Run(() =>
            {
                List.Where(n => n.Contains(md)).ToList().ForEach(n => n.SetAsyncUpdateUI());
            });
        }

        public static Contract SelectedContract
        {
            get
            {
                if (DockCanvas.ActiveDockForm is MarketDataGridView gv)
                {
                    return gv.SelectedContract;
                }
                return null;
            }
        }


    }









}

