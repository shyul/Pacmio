using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TradeIdeas.TIProData;
using TradeIdeas.TIProData.Configuration;
using TradeIdeas.ServerConnection;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// This is the Form1 class. This is the gateway form
    /// by which the user logs in and selects the desired option
    /// (e.g. Toplist,Alert,History,...etc
    /// </summary>
    public partial class Form1 : Form
    {
        static private ConnectionMaster _connectionMaster;
        /// <summary>
        /// get ConnectionMaster object.
        /// </summary>
        static public ConnectionMaster Connection { get { return _connectionMaster; } }
        private string messagePreview;
        /// <summary>
        /// Form1 class constructor
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            // It's best to create this at the very beginning, rather than waiting until
            // you log in.  If this is null, other windows will have trouble and could
            // cause null pointer exceptions.  If we are not logged in and not connected,
            // but this object exists, most messages will be safely ignored.
            _connectionMaster = new ConnectionMaster();
            _connectionMaster.PingManager.PingUpdate += _pingManager_PingUpdate;
            _connectionMaster.LoginManager.AccountStatusUpdate += _LoginManager_AccountStatus;
            _connectionMaster.ConnectionBase.Preview += _ConnectionBase_Preview;
            _connectionMaster.ConnectionBase.ConnectionStatusUpdate += _ConnectionBase_ConnectionStatusUpdate;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            _connectionMaster.LoginManager.Username = usernameTextBox.Text;
            _connectionMaster.LoginManager.Password = passwordTextBox.Text;
            if (tcpIpRadioButton.Checked)
                _connectionMaster.ConnectionBase.ConnectionFactory =
                    new TcpIpConnectionFactory(hostTextBox.Text, Convert.ToInt32(portTextBox.Text));
            else
                _connectionMaster.ConnectionBase.ConnectionFactory =
                    new HttpConnectionFactory(urlTextBox.Text);
        }

        SymbolListsMasterForm _symbolListsMasterForm;
        private void symbolListsButton_Click(object sender, EventArgs e)
        {
            if (_symbolListsMasterForm == null)
                _symbolListsMasterForm = new SymbolListsMasterForm();
            _symbolListsMasterForm.Show();
            _symbolListsMasterForm.WindowState = FormWindowState.Normal;
            _symbolListsMasterForm.BringToFront();
        }

        private void configWindowButton_Click(object sender, EventArgs e)
        {
            new ConfigWindow(ConfigurationType.Alerts).Show();
        }

        private void alertWindowButton_Click(object sender, EventArgs e)
        {
            new AlertsForm().Show();
        }

        private void historyWindowButton_Click(object sender, EventArgs e)
        {
            new HistoryForm().Show();
        }

        private void topListButton_Click(object sender, EventArgs e)
        {
            new TopListForm().Show();
        }

        private void topListConfigButton_Click(object sender, EventArgs e)
        {
            new ConfigWindow(ConfigurationType.TopList).Show();
        }

        private void imagesButton_Click(object sender, EventArgs e)
        {
            new ImageTest(_connectionMaster.ImageCacheManager).Show();
        }

        private void oddsMakerButton_Click(object sender, EventArgs e)
        {
            new OddsMakerForm().Show();
        }
        void _pingManager_PingUpdate(TimeSpan ping)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _pingManager_PingUpdate(ping); });
            else
            {
                pingLabel.Text = "Ping:  " + ping.TotalMilliseconds.ToString() + "ms";
            }
        }
        void _LoginManager_AccountStatus(LoginManager source, AccountStatusArgs args)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _LoginManager_AccountStatus(source, args); });
            else
            {
                accountStatusLabel.Text = "Account Status:  " + args.accountStatus.ToString();
                if (null != args.nextPayment)
                    nextPaymentLabel.Text = "Next Payment:  " + args.nextPayment.ToString();
                else
                    nextPaymentLabel.Text = "Next Payment: Right Now!!!";
                oddsMakerLabel.Text = "OddsMaker Remaining:  " + args.oddsmakerAvailable.ToString();
            }
        }
        void _ConnectionBase_Preview(ConnectionBase source, PreviewArgs args)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _ConnectionBase_Preview(source, args); });
            else
            {
                messagePreview = "GoodMessage:  " + args.goodMessage.ToString() + Environment.NewLine;
                if (args.goodMessage)
                {
                    messagePreview += Encoding.ASCII.GetString(args.messageBody);
                }
            }
        }

        void _ConnectionBase_ConnectionStatusUpdate(ConnectionBase source, ConnectionStatusCallbackArgs args)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { _ConnectionBase_ConnectionStatusUpdate(source, args); });
            else
            {
                statusLabel.Text = "Status:  " + args.message;
            }
        }

        private void previewTimer_Tick(object sender, EventArgs e)
        {
            previewTextBox.Text = messagePreview;
        }

        private void generalInfoButton_Click(object sender, EventArgs e)
        {
            new GeneralInfoForm().Show();
        }
    }
}
