using ActiveUp.Net.Mail;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ChromeDriver chromeDriver;

        private SamplesConfiguration _configuration;
        public main()
        {
            InitializeComponent();
            InitializeSample();
            label1.Text = this.Configuration.TransperfectEmail;
            label2.Text = this.Configuration.TransperfectEmail2;
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

        private void autoget(string imap4UserName, string email, string pass)
        {
            string profilePath = "C:/profile";
            try
            {
                bool exists = System.IO.Directory.Exists(profilePath);
                if (!exists)
                    System.IO.Directory.CreateDirectory(profilePath);
            }
            catch
            {
                MessageBox.Show("Error, Could not create directory for saving profiles!");
            }

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--user-data-dir=" + profilePath + "/" + imap4UserName);
            options.AddArgument("profile-directory=" + imap4UserName);
            options.AddArgument("disable-infobars");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--start-maximized");
            chromeDriver = new ChromeDriver(options);
            // chromeDriver.Url = "https://dashboard.transperfect.com/";
            chromeDriver.Url = "https://gl-tdcprod1.translations.com/PD/#userMenuAVAILABLE_SUBMISSION";
            chromeDriver.Navigate();
            waitLoading();

            System.Threading.Thread.Sleep(2000);
            waitLoading();
            // If nologin
            string url = chromeDriver.Url;
            if (url.Contains("gl-tdcprod1.translations.com/PD/login"))
            {
                // confirm login
                ReadOnlyCollection<IWebElement> eloginwithemailbutton = chromeDriver.FindElements(By.Id("loginwithemail-button"));
                if (eloginwithemailbutton.Count > 0)
                {
                    eloginwithemailbutton.First().Click();
                    waitLoading();
                    System.Threading.Thread.Sleep(2000);
                }
            }

            url = chromeDriver.Url;
            if (url.Contains("sso.transperfect.com/Account/Login"))
            {
                // SendKeys email
                ReadOnlyCollection<IWebElement> eEmails = chromeDriver.FindElements(By.Id("Email"));
                if (eEmails.Count > 0)
                {
                    eEmails.First().Clear(); ;
                    eEmails.First().SendKeys(email);
                }

                // SendKeys password /Password
                ReadOnlyCollection<IWebElement> ePasswords = chromeDriver.FindElements(By.Id("Password"));
                if (ePasswords.Count == 0)
                {
                    // SubmitLogin
                    ReadOnlyCollection<IWebElement> SubmitLogin = chromeDriver.FindElements(By.Id("SubmitLogin"));
                    if (SubmitLogin.Count > 0)
                    {
                        SubmitLogin.First().Click();
                        waitLoading();
                        System.Threading.Thread.Sleep(2000);
                        ePasswords = chromeDriver.FindElements(By.Id("Password"));
                    }
                }

                if (ePasswords.Count > 0)
                {
                    ePasswords.First().Clear(); ;
                    ePasswords.First().SendKeys(pass);
                }

                // SubmitLogin2
                System.Threading.Thread.Sleep(2000);
                ReadOnlyCollection<IWebElement> SubmitLogin2 = chromeDriver.FindElements(By.Id("SubmitLogin"));
                if (SubmitLogin2.Count > 0)
                {
                    SubmitLogin2.First().Click();
                    waitLoading();
                }
            }


        }

        private void waitLoading()
        {
            // wait loading
            WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(30));
            Func<IWebDriver, bool> waitLoading = new Func<IWebDriver, bool>((IWebDriver Web) =>
            {
                try
                {
                    IWebElement alertE = Web.FindElement(By.Id("abccuongnh"));
                    return false;
                }
                catch
                {
                    return true;
                }
            });

            try
            {
                wait.Until(waitLoading);
            }
            catch { }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnAuto1_Click(object sender, EventArgs e)
        {

        }

        private void btnAuto2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will close down the whole application. Confirm?", "Close Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private void account1_start_btn_Click(object sender, EventArgs e)
        {
            autoget(this.Configuration.Imap4UserName, this.Configuration.TransperfectEmail, this.Configuration.TransperfectPass);
        }
    }
}
