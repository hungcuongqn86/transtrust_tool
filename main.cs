using MailKit.Security;
using OpenQA.Selenium.Chrome;
using System;
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
        public LogWriter logWriter;
        public static main thisForm;
        const SecureSocketOptions SslOptions = SecureSocketOptions.Auto;
        public IdleClient idleClient;
        public Task idleTask;

        private SamplesConfiguration _configuration;
        public main()
        {
            logWriter = new LogWriter("Open app...");
            InitializeComponent();
            thisForm = this;
            label1.Text = this.Configuration.TransperfectEmail;
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

        private void Config_btn_Click(object sender, EventArgs e)
        {
            ConfigurationForm configForm = new ConfigurationForm(this.Configuration);
            configForm.ShowDialog();
        }

        private void Loading(bool status)
        {
            btnAuto1.Enabled = status; 
        }

        private void BtnAuto1_Click(object sender, EventArgs e)
        {
            logWriter.LogWrite("btnAuto1_Click...");
            Loading(false);
            this.idleClient = new IdleClient(
                this.Configuration.Imap4Server, 993, SslOptions, 
                this.Configuration.Imap4UserName, 
                this.Configuration.Imap4Password,
                this.Configuration.TransperfectEmail,
                this.Configuration.TransperfectPass
                );
            this.idleTask = this.idleClient.RunAsync();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will close down the whole application. Confirm?", "Close Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private void Btn_stop2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
