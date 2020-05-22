/// ***************************************************************************
/// Shared Libraries and Utilities
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
    public class SignalDatum
    {
        /// <summary>
        /// The Name of the signal, used as identifier for different signal types
        /// </summary>
        public string Name { get; set; }

        public double Score => ScoreList[ScorePt];

        /// <summary>
        /// Score with lasting effect
        /// </summary>
        public List<double> ScoreList { get; } = new List<double>();

        public int ScorePt { get; set; } = 0;

        #region Graphics Properties

        public Color Color { get; set; }

        // Custom Graphics Shapes can be drawn from here.

        #endregion Graphics Properties
    }
}
