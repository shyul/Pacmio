/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Xu;
using Xu.GridView;
using System.Threading.Tasks;

namespace Pacmio
{
    public static class MarketDataManager
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

        private static readonly List<MarketDataGridView> List = new List<MarketDataGridView>();

        /// <summary>
        /// Update All UI Elements when a new tick comes in
        /// </summary>
        /// <param name="c"></param>
        public static void UpdateUI(Contract c)
        {
            Task.Run(() =>
            {
                List.Where(n => n.MarketDataTable.Contains(c.MarketData)).ToList().ForEach(n => n.SetRefreshUI());
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CleanUp() 
        {
        
        }

        public static Contract SelectedContract
        {
            get
            {
                if(DockCanvas.ActiveDockForm is MarketDataGridView gv) 
                {
                    return gv.SelectMarketData.Contract;
                
                }

                return null;
            }

        }
    }
}

