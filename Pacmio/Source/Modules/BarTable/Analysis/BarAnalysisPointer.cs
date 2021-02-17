/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Pacmio
{
    public sealed class BarAnalysisPointer
    {
        public BarAnalysisPointer(BarTable bt, BarAnalysis ba)
        {
            Table = bt;
            Analysis = ba;
        }

        public BarTable Table { get; private set; }

        public BarAnalysis Analysis { get; private set; }

        public int Count => Table.Count;

        /// <summary>
        /// Valid data start pointer
        /// </summary>
        public int StartPt { get; set; } = 0;

        /// <summary>
        /// Pointer for calculated data end
        /// </summary>
        public int StopPt { get; set; } = 0;

        /// <summary>
        /// Check if the this analysis is up to date
        /// </summary>
        public bool IsUpToDate => StartPt >= Count;
    }
}
