/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;
using Pacmio;
using Pacmio.TIProData;

namespace TestClient
{
    public class PositionUpdate : IDataConsumer
    {
        public PositionUpdate(IEnumerable<Button> buttons) 
        {
            Buttons = buttons;
            AccountPositionManager.PositionDataProvider.AddDataConsumer(this);
        }

        public IEnumerable<Button> Buttons { get; }

        public void DataIsUpdated(IDataProvider provider)
        {
            HashSet<string> pslist = new();

            AccountPositionManager.Positions.OrderByDescending(n => n.Cost).Select(n => n.Contract.Name).RunEach(n => pslist.CheckAdd(n));

            int pt = 0;
            foreach (var btn in Buttons)
            {
                if (pt < pslist.Count)
                {
                    btn.Invoke(() => btn.Text = pslist.ElementAt(pt));
                }
                pt++;
            }
        }

        public void Dispose()
        {
            AccountPositionManager.PositionDataProvider.RemoveDataConsumer(this);
        }
    }
}
