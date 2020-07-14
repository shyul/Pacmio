/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;

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

        //public int MaxEffectLength { get; set; } = 1;

        #region Graphics Properties

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
        } //= Color.Green;

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
        } //= Color.Red;

        public ColorTheme BullishTheme { get; } = new ColorTheme();

        public ColorTheme BearishTheme { get; } = new ColorTheme();

        #endregion Graphics Properties
    }
}
