/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using Xu;

namespace Pacmio
{
    public abstract class SignalAnalysis : BarAnalysis
    {
        public SignalColumn Column_Result { get; protected set; }

        public Color BullishColor
        {
            get => Column_Result.BullishColor;
            set => Column_Result.BullishColor = value;
        }

        public Color BearishColor
        {
            get => Column_Result.BearishColor;
            set => Column_Result.BearishColor = value;
        }

        public ColorTheme BullishTheme
        {
            get => Column_Result.BullishTheme;
            set => Column_Result.BullishTheme = value;
        }

        public ColorTheme BearishTheme
        {
            get => Column_Result.BearishTheme;
            set => Column_Result.BearishTheme = value;
        }
    }
}
