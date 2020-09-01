using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeIdeas.TIProData;

namespace WindowsFormsApplication1
{
    public partial class GeneralInfoForm : Form
    {
        /// <summary>
        /// GeneralInfo class
        /// </summary>
        private GeneralInfo _generalInfo; 
        /// <summary>
        /// GeneralInfoForm constructor.
        /// </summary>
        public GeneralInfoForm()
        {
            InitializeComponent();
            _generalInfo = new GeneralInfo();
            _generalInfo.Received += new GeneralInfoReceived(_generalInfo_Received);
        }

        void _generalInfo_Received(GeneralInfo sender)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _generalInfo_Received(sender); });
            else if (!IsDisposed)
                lastReceivedabel.Text = "Message received: " + DateTime.Now;
        }

        private void requestUpdateButton_Click(object sender, EventArgs e)
        {
            lastRequestLabel.Text = "Requested: " + DateTime.Now;
            _generalInfo.RequestNow(Form1.Connection.SendManager);
        }

        private void getAlertInfoButton_Click(object sender, EventArgs e)
        {
            alertColorLabel.Text = "Color:  " + _generalInfo.GetAlertColor(alertInternalCodeTextBox.Text);
            alertDescriptionLabel.Text = "Description:  " + _generalInfo.GetAlertName(alertInternalCodeTextBox.Text);
            alertFilterLabel.Text = "Quality Filter:  " + _generalInfo.GetAlertFilter(alertInternalCodeTextBox.Text);
        }
    }
}
