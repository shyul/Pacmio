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

        public static Dictionary<Contract, MarketDepth> ContractMarketDepthLUT { get; } = new Dictionary<Contract, MarketDepth>();

        public static MarketDepth GetOrCreateMarketDepth(this Contract c)
        {
            lock (ContractMarketDepthLUT)
            {
                if (!ContractMarketDepthLUT.ContainsKey(c))
                    ContractMarketDepthLUT[c] = new MarketDepth(c);

                return ContractMarketDepthLUT[c];
            }
        }

        public static MarketDepth StartMarketDepth(this Contract c)
        {
            while (IB.Client.ActiveMarketDepth.Count > MaximumParallelMarketDepthCount - 1)
            {
                MarketDepth mdt_to_cancel = IB.Client.ActiveMarketDepth.Values.OrderBy(n => n.StartTime).FirstOrDefault();
                Cancel(mdt_to_cancel);
            }

            var mdt = GetOrCreateMarketDepth(c);
            Start(mdt);
            return mdt;
        }

        public static MarketDepth CancelMarketDepth(this Contract c)
        {
            var mdt = GetOrCreateMarketDepth(c);
            Cancel(mdt);
            return mdt;
        }

        private static void Start(MarketDepth mdt) => IB.Client.SendRequest_MarketDepth(mdt);

        private static void Cancel(MarketDepth mdt) => IB.Client.SendCancel_MarketDepth(mdt.RequestId);

        public static MarketDepthGridView EnableMarketDepthGridView(this Contract c) 
        {
            // if 
            MarketDepth mdt = c.StartMarketDepth();

            return new MarketDepthGridView(mdt);

            /*
                if (List.Where(n => n.Contract == c).Count() < 1)
                {
                    IB.Client.SendRequest_MarketDepth(c);
                    var gv = new MarketDepthGridView(c);
                    List.Add(gv);
                    gv.ReadyToShow = true;
                    Root.Form.AddForm(DockStyle.Fill, 0, gv);
                }
            */
        }
    }
}
