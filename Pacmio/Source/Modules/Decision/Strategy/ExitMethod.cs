/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    public class ExitMethod
    {
        public string Name { get; set; }

        public bool IsDayTrade { get; set; }

        public double MaximumHoldingTimeInMs { get; set; }

        /// <summary>
        /// If value is 2, it means sell by half when the price target is met.
        /// </summary>
        public int MaximumScaling { get; set; }

        /// <summary>
        /// Bar Analysis vs Multi Time Frame
        /// </summary>
        protected Dictionary<(BarFreq BarFreq, DataType DataType), BarAnalysisSet> BarAnalysisSets { get; } = new();

        public virtual void ClearBarAnalysisSet() => BarAnalysisSets.Clear();

        public virtual BarAnalysisSet this[BarFreq BarFreq, DataType DataType = DataType.Trades]
        {
            get
            {
                if (BarAnalysisSets.ContainsKey((BarFreq, DataType)))
                    return BarAnalysisSets[(BarFreq, DataType)];
                else
                    return null;
            }
            set
            {
                if (value is BarAnalysisSet bas)
                    BarAnalysisSets[(BarFreq, DataType)] = new BarAnalysisSet(bas);
                else if (BarAnalysisSets.ContainsKey((BarFreq, DataType)))
                    BarAnalysisSets.Remove((BarFreq, DataType));
            }
        }
    }
}
