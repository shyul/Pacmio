using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pacmio.Utility
{
    public partial class ProgressForm : Form
    {
        public Progress<int> Progress;
        public CancellationTokenSource CancellationTS = new CancellationTokenSource();

        public ProgressForm()
        {
            InitializeComponent();
            Progress = new Progress<int>(percent =>
            {
                ProgressBarMain.Value = percent;
            });
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            CancellationTS.Cancel();

            Close();
        }
    }
}
