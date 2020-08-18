/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Drawing;
using Xu;

namespace Pacmio
{
    public sealed partial class PacmioForm : MosaicForm
    {
        public PacmioForm() : base(Root.SHOW_PACMIO)
        {
            SuspendLayout();

            HelpLink = "https://github.com/shyul/Pacmio/wiki";
            Icon = Pacmio.Properties.Resources.Pacman;
            /*
            RibbonButton rbtn_1 = new RibbonButton(c_IBHistorial, 0, Importance.Major);
            RibbonPane rbtpane_IBClient = new RibbonPane("IBClient Test", 0);
            rbtpane_IBClient.Add(rbtn_1);
            RibbonTabItem rbtIBClient = new RibbonTabItem("IBClient");
            rbtIBClient.Add(rbtpane_IBClient, 0);
            Ribbon.AddRibbonTab(rbtIBClient);
            */
            IsRibbonShrink = true;
            Width = 1500;
            Height = 1200;

            ResumeLayout(false); // false
            PerformLayout();
        }
    }
}
