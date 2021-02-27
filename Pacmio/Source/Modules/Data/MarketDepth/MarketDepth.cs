/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    public class MarketDepth : IDataProvider
    {
        public MarketDepth(Contract c)
        {
            Contract = c;
        }

        public Contract Contract { get; }

        #region Options

        public int NumberOfRows { get; set; } = 20;

        public bool IsSmartDepth { get; set; } = true;

        public ICollection<(string, string)> IbOptions { get; set; } = null;

        public bool IsLive => RequestId > 0;

        public int RequestId { get; set; } = -1;

        public DateTime StartTime { get; set; } = DateTime.MinValue;

        #endregion Options

        private Dictionary<int, MarketDepthDatum> DepthToDatumLUT { get; } = new Dictionary<int, MarketDepthDatum>();

        public MarketDepthDatum[] List
        {
            get
            {
                lock (DepthToDatumLUT)
                {
                    return DepthToDatumLUT.OrderBy(n => n.Key).Select(n => n.Value).Take(NumberOfRows).ToArray();
                }
            }
        }

        public MarketDepthDatum this[int depth]
        {
            get
            {
                lock (DepthToDatumLUT)
                {
                    if (!DepthToDatumLUT.ContainsKey(depth))
                        DepthToDatumLUT[depth] = new MarketDepthDatum(depth);

                    return DepthToDatumLUT[depth];
                }
            }
            set
            {
                lock (DepthToDatumLUT)
                {
                    DepthToDatumLUT[depth] = value;
                }
            }
        }

        #region Data Provider

        public DateTime UpdateTime { get; protected set; } = DateTime.MinValue;

        public List<IDataConsumer> DataConsumers { get; } = new List<IDataConsumer>();

        public bool AddDataConsumer(IDataConsumer idk)
        {
            return DataConsumers.CheckAdd(idk);
        }

        public bool RemoveDataConsumer(IDataConsumer idk)
        {
            if (idk is DockForm df) df.ReadyToShow = false;
            return DataConsumers.CheckRemove(idk);
        }

        public void Updated()
        {
            UpdateTime = DateTime.Now;
            DataConsumers.ForEach(n => n.DataIsUpdated(this));
        }

        #endregion Data Provider
    }
}
