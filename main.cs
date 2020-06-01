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
            /*if (worker.IsBusy)
                worker.CancelAsync();

            worker.RunWorkerAsync();*/

            autoget();
        }

        private void autoget()
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
            options.AddArgument("--user-data-dir=" + profilePath + "/" + this.Configuration.Imap4UserName);
            options.AddArgument("profile-directory=" + this.Configuration.Imap4UserName);
            options.AddArgument("disable-infobars");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--start-maximized");
            chromeDriver = new ChromeDriver(options);
            chromeDriver.Url = "https://dashboard.transperfect.com/";
            chromeDriver.Navigate();
            waitLoading();

            System.Threading.Thread.Sleep(2000);
            waitLoading();
            // If nologin
            string url = chromeDriver.Url;
            if (url.Contains("https://sso.transperfect.com/Account/Login"))
            {
                // SendKeys email
                ReadOnlyCollection<IWebElement> eEmails = chromeDriver.FindElements(By.Id("Email"));
                if (eEmails.Count > 0)
                {
                    string email = "nnmaika@vt.edu";
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
                        ePasswords = chromeDriver.FindElements(By.Id("Password"));
                    }
                }

                if (ePasswords.Count > 0)
                {
                    string pass = "Studyintheus2012**";
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
            else
            {

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
    }
}
