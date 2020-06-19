/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    public class ContractGridStripe : GridStripe
    {
        public ContractColumn Column { get; set; }

        public override void Draw(Graphics g, Rectangle bound, ITable table, int index)
        {
            if(table is IContractTable ct) 
            {
                Contract value = ct[index, Column];
                g.DrawString(value.ToString(), Main.Theme.Font, Theme.ForeBrush, bound.Location);
            }
        }
    }
}
