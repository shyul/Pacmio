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
    /// <summary>
    /// OddsMakerForm allows user to configure the OddsMaker
    /// This has the same functionality of TIPro. Results show
    /// in the large lower panel.
    /// </summary>
    public partial class OddsMakerForm : Form
    {
        
        private OddsMaker _oddsMaker;
        private string _debugText;
        private bool xmlMode;
        /// <summary>
        /// OddsMakerForm constructor
        /// </summary>
        public OddsMakerForm()
        {
            InitializeComponent();
            locationComboBox.SelectedIndex = 0;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (null != _oddsMaker)
                _oddsMaker.Stop();
        }
        private void startButton_Click(object sender, EventArgs e)
        {
            if (null != _oddsMaker)
                _oddsMaker.Stop();
            OddsMakerRequest request;
            try
            {
                request.DaysOfTest = Convert.ToInt32(testTextBox.Text);
                request.SkipDays = Convert.ToInt32(skipTextBox.Text);
                request.EntryCondition = entryConditionTextBox.Text;
                request.EntryTimeEnd = Convert.ToInt32(entryTimeStopMinutesTextBox.Text);
                request.EntryTimeStart = Convert.ToInt32(entryTimeStartMinutesTextBox.Text);
              
                switch (locationComboBox.SelectedIndex)
                {
                    case 1:
                        request.Location = SelectedLocation.Canada;
                        break;
                    case 0:
                    default:
                        request.Location = SelectedLocation.US;
                        break;
                }

                if (profitTargetCheckBox.Checked)
                    request.ProfitTarget = Convert.ToDouble(profitTargetTextBox.Text);
                else
                    request.ProfitTarget = null;
                if (stopLossCheckBox.Checked)
                    request.StopLoss = Convert.ToInt32(stopLossTextBox.Text);
                else
                    request.StopLoss = null;
                request.StopLossWiggle = wiggleCheckBox.Checked;
                request.SuccessDirectionUp = plusRadioButton.Checked;
                request.SuccessMinMove = Convert.ToDouble(SuccessValueTextBox.Text);
                request.SuccessTypePercent = PercentRadioButton.Checked;
                request.TimeoutMinutes = Convert.ToInt32(timeoutMinutesTextBox.Text);
                request.AtCloseDays = Convert.ToInt32(atCloseDaysTextBox.Text);
                request.AtOpenDays = Convert.ToInt32(atOpenDaysTextBox.Text);
                request.BeforeCloseMinutes = Convert.ToInt32(beforeCloseMinutesTextBox.Text);
                if (minutesAfterEntryRadioButton.Checked)
                {
                    request.TimeoutType = TimeoutType.MinutesAfterEntry;
                }
                else if (minutesBeforeCloseRadioButton.Checked)
                {
                    request.TimeoutType = TimeoutType.Close;
                }
                else if (atOpenDaysRadioButton.Checked)
                {
                    request.TimeoutType = TimeoutType.Open;
                }
                else if (afterCloseDaysRadioButton.Checked)
                {
                    request.TimeoutType = TimeoutType.FutureClose;
                }
                else 
                request.TimeoutType = TimeoutType.MinutesAfterEntry;
#pragma warning disable 612
                request.ShowDebugInfo = debugModeCheckBox.Checked;
#pragma warning restore 612
                request.XmlMode = xmlCheckBox.Checked;
                xmlMode = request.XmlMode;

                request.ExitConditionAlert = exitConditionAlertTextBox.Text;
                request.ExitConditionTrailingStop = Convert.ToDouble(exitConditionTrailingStopTextBox.Text);
                if (percentTrailingRadioButton.Checked)
                    request.ExitConditionType = ExitConditionType.TrailingPercent;
                else if (barsTrailingRadioButton.Checked)
                    request.ExitConditionType = ExitConditionType.TrailingBars;
                else if (AlertExitRadioButton.Checked)
                    request.ExitConditionType = ExitConditionType.Alert;
                else
                    request.ExitConditionType = ExitConditionType.None;
                request.RequestCsvFile = getCsvFileCheckBox.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            _oddsMaker = Form1.Connection.OddsMakerManager.GetOddsMaker(request);
            _oddsMaker.OddsMakerStatus += new OddsMakerStatus(_oddsMaker_OddsMakerStatus);
            _oddsMaker.OddsMakerProgress += new OddsMakerProgress(_oddsMaker_OddsMakerProgress);
            _oddsMaker.OddsMakerProgressXml += new OddsMakerProgressXml(_oddsMaker_OddsMakerProgressXml);
            _oddsMaker.OddsMakerDebug += new OddsMakerDebug(_oddsMaker_OddsMakerDebug);
            _oddsMaker.OddsMakerCSV += new OddsMakerCSV(_oddsMaker_OddsMakerCSV);
            _oddsMaker.Start();
        }

        void _oddsMaker_OddsMakerCSV(string csv, OddsMaker sender)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _oddsMaker_OddsMakerCSV(csv, sender); });
            else
            {
                csvTextBox.Text = csv;
            }
        }

        void _oddsMaker_OddsMakerProgressXml(System.Xml.XmlNode progress, OddsMaker sender)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _oddsMaker_OddsMakerProgressXml(progress, sender); });
            else
            {
                progressRichTextBox.Text = progress.OuterXml;
            }
        }

        void _oddsMaker_OddsMakerStatus(OddsMaker sender)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _oddsMaker_OddsMakerStatus(sender); });
            else
            {
                if (sender.OddsMakerDisposition == OddsMakerDisposition.Done)
                {
                    debugResultTextBox.Text = _debugText;
                }
                else
                {
                    debugResultTextBox.Text = _debugText = "";
                    csvTextBox.Text = "";
                }
            }
        }

        void _oddsMaker_OddsMakerProgress(string progress, OddsMaker sender)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _oddsMaker_OddsMakerProgress(progress, sender); });
            else
            {
                try
                {
                    progressRichTextBox.Rtf = progress;
                }
                catch
                {

                }
            }
        }

        void _oddsMaker_OddsMakerDebug(string debug, OddsMaker sender)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate {_oddsMaker_OddsMakerDebug(debug, sender); });
            else
            {
                if (_debugText.Length > 0)
                    _debugText += Environment.NewLine;
                _debugText += debug;
            }
        }

    }
}
