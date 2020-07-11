using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Xu;

namespace Pacmio.TradeCAD
{
    public partial class RemoteControlForm : Form
    {
        public RemoteControlForm()
        {
            InitializeComponent();
            Root.Form.Show();
        }

        private void BtnTestChart_Click(object sender, EventArgs e)
        {
            var siList = ContractList.GetOrFetch("AAPL", "US");
            Contract c = siList.First();

            BarTable bt_high = c.GetTableOld(BarFreq.Daily, BarType.Trades);
            bt_high.Reset(new Period(DateTime.MinValue, DateTime.Now), null, null);

            BarTable bt = c.GetTableOld(BarFreq.Minute, BarType.Trades);
            bt.Reset(new Period(DateTime.Now.AddDays(-20), DateTime.Now), null, null);



            Thread.Sleep(1000);

            //BarChartManager.GetForm(bt, BarChartManager.SampleChartConfig());
            BarChartList.GetForm(bt_high, BarChartList.SampleChartConfig());
        }

        private void BtnTestData_Click(object sender, EventArgs e)
        {
            var siList = ContractList.GetOrFetch("AAPL", "US");
            Contract c = siList.First();
            BarTable bt = c.GetTableOld(BarFreq.Minute, BarType.Trades);
            Period pd = new Period(DateTime.Now.AddDays(-3), DateTime.Now);

            bt.Reset(pd, null, null);
        }
    }
}
