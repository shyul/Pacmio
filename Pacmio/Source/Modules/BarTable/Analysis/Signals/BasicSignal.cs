/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public abstract class BasicSignal : IAnalysisSetting
    {
        public string Name { get; private set; }

        public abstract BarAnalysisSet BarAnalysisSet(BarFreq barFreq);

        public bool Equals(IAnalysisSetting other) => other is IAnalysisSetting ias && Name == ias.Name;
    }
}
