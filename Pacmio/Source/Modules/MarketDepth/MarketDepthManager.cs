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
    public static class MarketDepthManager
    {
        public const int MaximumParallelMarketDepthCount = 3;

        public static void Add(Contract c)
        {

            if (List.Where(n => n.Contract == c).Count() < 1)
            {
                IB.Client.SendRequest_MarketDepth(c);
                var gv = new MarketDepthGridView(c);
                List.Add(gv);
                gv.ReadyToShow = true;
                Root.Form.AddForm(DockStyle.Fill, 0, gv);
            }

        }



        private static List<MarketDepthGridView> List { get; } = new List<MarketDepthGridView>();


        public static void UpdateUI(Contract c)
        {
            Task.Run(() =>
            {
                List.Where(n => n.Contract == c).ToList().ForEach(n => n.UpdateGrid());
            });
        }
    }
}
