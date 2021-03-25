/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Pacmio
{
    public sealed class BarAnalysisSetPointer
    {
        public BarAnalysisSetPointer(BarTable bt, BarAnalysisSet bas)
        {
            Table = bt;
            AnalysisSet = bas;
        }

        public BarTable Table { get; private set; }

        public BarAnalysisSet AnalysisSet { get; private set; }

        public int LastCalculateIndex { get; set; } = -1;

        /// <summary>
        /// Returns the Last Bar in the Table. Null is the BarTable is empty.
        /// </summary>
        public Bar LastBar => LastCalculateIndex < 0 ? null : Table[LastCalculateIndex];

        public Bar LastBar_1 => LastCalculateIndex < 1 ? null : Table[LastCalculateIndex - 1];
    }
}
