/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class SingleData : BarAnalysis, ISignalAnalysis
    {
        public SingleData() { }

        public void Config<T>(T analysis) where T : BarAnalysis, ISingleData
        {
            Column = analysis.Result_Column;

            string label = "[ " + analysis.Name + " ]";

            Name = GetType().Name + label;
            Description = GetType().Name + " " + label;
            GroupName = Description + ": ";
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode();

        public NumericColumn Column { get; private set; }

        public ObjectColumn Signal_Column { get; private set; }

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }

        public ColorTheme BullishTheme { get; } = new ColorTheme(Color.Teal, Color.Teal.Opaque(64));

        public ColorTheme BearishTheme { get; } = new ColorTheme(Color.Orange, Color.Peru.Opaque(64));
    }
}
