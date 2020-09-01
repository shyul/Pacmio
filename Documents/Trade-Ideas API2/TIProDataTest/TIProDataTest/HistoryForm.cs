using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using TradeIdeas.TIProData;

namespace WindowsFormsApplication1
{
    public partial class HistoryForm : Form
    {

        private History _history;
        /// <summary>
        /// HistoryForm constructor. The History window allows the
        /// user to take a snapshot of activity of a selected strategy
        /// within a specific time frame.
        /// </summary>
        public HistoryForm()
        {
            InitializeComponent();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = true;
            stopButton.Enabled = true;
            if (null != _history)
                _history.Stop();
            HistoryRequest request;
            request.EndTime = null;
            request.Config = configTextBox.Text;
            if (startDateTimePicker.Checked)
                request.StartTime = startDateTimePicker.Value;
            else
                request.StartTime = null;
            if (endDateTimePicker.Checked)
                request.EndTime = endDateTimePicker.Value;
            else
                request.EndTime = null;
            _history = Form1.Connection.HistoryManager.GetHistory(request);
            _history.HistoryStatus += new HistoryStatus(_history_HistoryStatus);
            _history.HistoryData += new HistoryData(_history_HistoryData);
        }

        private int _alertCount;
        private int _messageCount;
        /// <summary>
        /// gathers the history data for the specified timeframe
        /// </summary>
        /// <param name="alerts"> RowData objects</param>
        /// <param name="sender"> History object</param>
        void _history_HistoryData(List<RowData> alerts, History sender)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _history_HistoryData(alerts, sender); });
            else
            {
                _alertCount += alerts.Count;
                _messageCount++;
                statusLabel.Text = String.Format("Alert count:  {0}, Message count:  {1}", _alertCount, _messageCount);
                StringBuilder newText = new StringBuilder();
                newText.Append("Received ");
                newText.Append(alerts.Count);
                newText.Append(" alerts.\r\n");
                foreach (RowData alert in alerts)
                {
                    newText.Append(alert.ToString());
                    newText.Append("\r\n");
                }
                resultTextBox.AppendText(newText.ToString());
            }                
        }

        void _history_HistoryStatus(History sender)
        {
            // Note:  This could be a reentrant call from the GUI.  Start() or Stop() could
            // call this delegate.
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _history_HistoryStatus(sender); });
            else
                // This wasn't really made for a log file.  It was made to show you the current status.
                // There's nothing to say that the status didn't change when we weren't looking.  Even
                // if we were in the same thread, we might have that issue.
                resultTextBox.AppendText("Status:  " + _history.HistoryDisposition.ToString() + "\r\n");
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (!_history.CanStart())
            {   // Ideally we might gray out the button.
                SystemSounds.Exclamation.Play();
                resultTextBox.AppendText("Cannot start at this time.\r\n");
            }
            else
                _history.Start();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {   // This is always safe. (Aside from a null pointer!)
            _history.Stop();
        }

        private void HistoryForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (null != _history)
                // Give up the resources.
                _history.Stop();
        }

    }
}
