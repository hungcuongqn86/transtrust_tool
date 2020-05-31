using ActiveUp.Net.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using transtrusttool.Utils;

namespace transtrusttool
{
    public partial class main : Form
    {
        public static Imap4Client imap;
        public BackgroundWorker worker;

        private SamplesConfiguration _configuration;
        public main()
        {
            InitializeComponent();
            InitializeSample();
        }

        protected void InitializeSample()
        {
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(StartIdleProcess);
        }

        private void StartIdleProcess(object sender, DoWorkEventArgs e)
        {
            if (imap != null && imap.IsConnected)
            {
                imap.StopIdle();
                imap.Disconnect();
            }
            if (this.Configuration.Imap4Server != null)
            {
                imap = new Imap4Client();
                imap.NewMessageReceived += new NewMessageReceivedEventHandler(NewMessageReceived);
                //worker.ReportProgress(1, "Connection...");
                imap.ConnectSsl(this.Configuration.Imap4Server, 993);
                //worker.ReportProgress(0, "Login...");
                imap.Login(this.Configuration.Imap4UserName, this.Configuration.Imap4Password);
                //worker.ReportProgress(0, "Select 'inbox'...");
                imap.SelectMailbox("inbox");
                //worker.ReportProgress(0, "Start idle...");
                imap.StartIdle();
            }
        }

        public static void NewMessageReceived(object source, NewMessageReceivedEventArgs e)
        {
            MessageBox.Show("New message received :" + e.MessageCount);
            //imap4.StopIdle();
        }

        public SamplesConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new SamplesConfiguration();

                    string configFullPath = Utils.Common.GetImagePath(Assembly.GetExecutingAssembly().Location) + @"\" + _configuration.FileName;

                    if (File.Exists(configFullPath))
                    {
                        TextReader reader = new StreamReader(configFullPath);
                        XmlSerializer serialize = new XmlSerializer(typeof(SamplesConfiguration));
                        _configuration = (SamplesConfiguration)serialize.Deserialize(reader);
                        reader.Close();
                    }
                    else
                    {
                        _configuration.SetDefaultValue();

                        ConfigurationForm configForm = new ConfigurationForm(this.Configuration);
                        configForm.ShowDialog();
                    }
                }
                return _configuration;
            }
            set
            {
                _configuration = value;
            }
        }

        private void config_btn_Click(object sender, EventArgs e)
        {
            ConfigurationForm configForm = new ConfigurationForm(this.Configuration);
            configForm.ShowDialog();
        }

        private void start_btn_Click(object sender, EventArgs e)
        {
            if (worker.IsBusy)
                worker.CancelAsync();

            worker.RunWorkerAsync();
        }
    }
}
