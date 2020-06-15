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
    public class SignalColumn : Column
    {
        public SignalColumn(string name) => Name = name;

        public SignalColumn(string name, string label)
        {
            Name = name;
            Label = label;
        }

        public int MaxEffectLength { get; set; } = 1;

        #region Graphics Properties

        public Color BullishColor { get; set; } = Color.Green;

        public Color BearishColor { get; set; } = Color.Red;

        #endregion Graphics Properties
    }
}
