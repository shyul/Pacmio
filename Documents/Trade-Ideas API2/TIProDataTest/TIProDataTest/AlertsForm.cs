using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TradeIdeas.TIProData;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// AlertForm Main class. Here one may enter an alert config
    /// strategy and receive updates within the "Results" window.
    /// </summary>
    public partial class AlertsForm : Form
    {
        /// <summary>
        /// Alert form class constructor
        /// </summary>
        public AlertsForm()
        {
            InitializeComponent();
        }

        private StreamingAlerts _streamingAlerts;

        private void startButton_Click(object sender, EventArgs e)
        {
            if (null != _streamingAlerts)
                _streamingAlerts.Stop();
            _streamingAlerts = Form1.Connection.StreamingAlertsManager.GetAlerts(configTextBox.Text);
            _streamingAlerts.StreamingAlertsData += new StreamingAlertsData(_streamingAlerts_StreamingAlertsData);
            _streamingAlerts.StreamingAlertsConfig += new StreamingAlertsConfig(_streamingAlerts_StreamingAlertsConfig);
            _streamingAlerts.Start();    
        }

        void _streamingAlerts_StreamingAlertsConfig(StreamingAlerts source)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _streamingAlerts_StreamingAlertsConfig(source); });
            else
            {
                if (source != _streamingAlerts)
                    resultsTextBox.AppendText("Ignoring StreamingAlertsConfig.\r\n");
                else
                    resultsTextBox.AppendText("Name = " + _streamingAlerts.WindowName + ", ShortForm = " + _streamingAlerts.Config + "\r\n");
            }
        }

        private int alertCount;
        private int messageCount;
        void _streamingAlerts_StreamingAlertsData(List<RowData> data, StreamingAlerts source)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _streamingAlerts_StreamingAlertsData(data, source); });
            else
            {
                if (source != _streamingAlerts)
                    resultsTextBox.AppendText("Ignoring StreamingAlertsData.\r\n");
                else
                {
                    alertCount += data.Count;
                    alertCountLabel.Text = "Alert Count:  " + alertCount;
                    messageCount++;
                    messageCountLabel.Text = "Message Count:  " + messageCount;
                    resultsTextBox.AppendText("Received " + data.Count + " alerts.\r\n");
                    foreach (RowData alert in data)
                    {
                        resultsTextBox.AppendText(alert.ToString());
                        resultsTextBox.AppendText("\r\n");
                    }
                }
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (null != _streamingAlerts)
                _streamingAlerts.Stop();
        }

        private void AlertsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (null != _streamingAlerts)
                _streamingAlerts.Stop();
        }
    }
}
