﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Xu;
using System.Threading.Tasks;
using System.Threading;
using Pacmio;

namespace Pacmio.TradeCAD
{
    internal sealed partial class PacMain : MosaicForm
    {
        #region Ctor
        public PacMain() : base(Root.SHOW_PACMIO)
        {
            SuspendLayout();

            HelpLink = "https://github.com/shyul/Pacmio/wiki";
            Icon = Pacmio.Properties.Resources.Pacman;

            //IsRibbonShrink = true;

            RibbonButton rbtn_1 = new RibbonButton(c_IBHistorial, 0, Importance.Major);

            RibbonPane rbtpane_IBClient = new RibbonPane("IBClient Test", 0);
            rbtpane_IBClient.Add(rbtn_1);

            RibbonTabItem rbtIBClient = new RibbonTabItem("IBClient");
            rbtIBClient.Add(rbtpane_IBClient, 0);

            Ribbon.AddRibbonTab(rbtIBClient);

            GC.Collect(GC.MaxGeneration);
            GC.WaitForPendingFinalizers();

            EnableOutputPanel();

            ResumeLayout(false); // false
            PerformLayout();
        }

        #endregion

        #region Data Components

        private readonly Command c_IBHistorial = new Command()
        {
            //Enabled = false,
            Label = "Historical Data Request Awseome with more Awesome!!",

            IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() { { IconType.Normal, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Xu.Properties.Resources.Database_16 },
                    { new Size(24, 24), Xu.Properties.Resources.Database_24 },
                    { new Size(32, 32), Xu.Properties.Resources.Database_32 }
                } } },
            //Action = () => { TestPublicHistoricalData(); },
        };

        public void EnableOutputPanel()
        {
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

        }

        #endregion
    }
}
