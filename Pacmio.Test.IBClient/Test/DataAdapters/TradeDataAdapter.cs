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
using Pacmio.IB;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;

namespace TestClient
{
    public class TradeDataAdapter : IDataConsumer
    {
        public TradeDataAdapter() 
        {
            ExecutionManager.DataProvider.AddDataConsumer(this);
        }

        public void DataIsUpdated(IDataProvider provider)
        {
            Task.Run(() =>
            {
                Console.WriteLine("Trade History is updated. " + ExecutionManager.Count);





            });
        }

        public void Dispose()
        {
            RemoveDataSource();
        }

        public void RemoveDataSource()
        {
            ExecutionManager.DataProvider.RemoveDataConsumer(this);
        }
    }
}
