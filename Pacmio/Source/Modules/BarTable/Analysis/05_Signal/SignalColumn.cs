/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************
/// 
using System;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class SignalColumn : DatumColumn
    {
        public SignalColumn(BarAnalysis source, string name)
            : base(source.Name + "_Signal_" + name, typeof(SignalDatum))
        {
            Source = source;
        }

        public SignalColumn(SignalAnalysis source, Type datumType)
            : base(source.Name + "_Signal", datumType)
        {
            Source = source;
        }

        public BarAnalysis Source { get; }

        public Color BullishColor
        {
            get
            {
                return BullishTheme.ForeColor;
            }
            set
            {
                BullishTheme.ForeColor = value;
                BullishTheme.FillColor = value.Opaque(64);
                BullishTheme.EdgeColor = value.Opaque(255);
            }
        }

        public Color BearishColor
        {
            get
            {
                return BearishTheme.ForeColor;
            }
            set
            {
                BearishTheme.ForeColor = value;
                BearishTheme.FillColor = value.Opaque(64);
                BearishTheme.EdgeColor = value.Opaque(255);
            }
        }

        public ColorTheme BullishTheme { get; set; } = new();

        public ColorTheme BearishTheme { get; set; } = new();
    }
}
