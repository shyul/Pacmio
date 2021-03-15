/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;

namespace Pacmio
{
    public class SignalColumn : DatumColumn, IOrdered
    {
        public SignalColumn(string name, SignalColumnType type = SignalColumnType.Filter) : base(name, typeof(SignalDatum))
        {
            Type = type;
        }

        public SignalColumn(string name, string label, SignalColumnType type = SignalColumnType.Filter) : base(name, label, typeof(SignalDatum))
        {
            Type = type;
        }

        public SignalColumnType Type { get; }

        public bool Enabled { get; set; } = true;

        public int Order { get; set; } = 0;

        public Importance Importance { get; set; } = Importance.None;

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

        public ColorTheme BullishTheme { get; } = new ColorTheme();

        public ColorTheme BearishTheme { get; } = new ColorTheme();

        #endregion Graphics Properties
    }

    public enum SignalColumnType : int
    {
        Filter = 0,

        Entry = 10,

        ExitLong = 11,

        ExitShort = 12
    }
}
