/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// Condition Filter: yield: push into lower time frame BarTable
    /// what does it do? Calculate(BarAnalysisPointer bap)

    /// </summary>
    public abstract class Filter : Indicator
    {
        public Filter()
        {
            // Prepare the BarAnalysis needed in this filter
        }

        // also list the BarAnalysis needed here

        public virtual (BarFreq Freq, BarType Type) LowerTimeFrame { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            /// 1. Finish a group of BarAnalysis
            /// 2. Yield signal score and storage them in the BarAnalysisSetData
            /// 3. Yeild important levels and trend lines !! and storage them in the BarAnalysisSetData ->> accessible to lower time frame indicators
            /// 4. IF Signal Score is met: Load the lower time frame BarTable from StrategyManager.BarTableSet with Period setttings
            /// 5. Run Lower Time Frame Calculate
            /// 

            /// Filter does not yield trading result, only push into lower time frame analysis
        }


    }
}
