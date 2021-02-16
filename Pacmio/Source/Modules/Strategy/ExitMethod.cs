/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
        protected Dictionary<(BarFreq BarFreq, BarType BarType), BarAnalysisSet> BarAnalysisSets { get; } = new Dictionary<(BarFreq BarFreq, BarType BarType), BarAnalysisSet>();

        public virtual void ClearBarAnalysisSet() => BarAnalysisSets.Clear();

        public virtual BarAnalysisSet this[BarFreq BarFreq, BarType BarType = BarType.Trades]
        {
            get
            {
                if (BarAnalysisSets.ContainsKey((BarFreq, BarType)))
                    return BarAnalysisSets[(BarFreq, BarType)];
                else
                    return null;
            }
            set
            {
                if (value is BarAnalysisSet bas)
                    BarAnalysisSets[(BarFreq, BarType)] = new BarAnalysisSet(bas);
                else if (BarAnalysisSets.ContainsKey((BarFreq, BarType)))
                    BarAnalysisSets.Remove((BarFreq, BarType));
            }
        }
    }
}
