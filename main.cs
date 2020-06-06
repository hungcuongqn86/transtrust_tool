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
using System.Runtime.CompilerServices;
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
        public static Imap4Client imap;
        public BackgroundWorker worker;
        ChromeDriver chromeDriver;
        string submissionId;
        string dashboardUrl = "https://dashboard.transperfect.com/";
        string tdcAvaliableUrl = "https://gl-tdcprod1.translations.com/PD/#userMenuAVAILABLE_SUBMISSION";
        string tptAvaliableUrl = "https://gl-tptprod1.transperfect.com/PD/#userMenuAVAILABLE_SUBMISSION";
        string avaliableUrl = "https://gl-tdcprod1.translations.com/PD/#userMenuAVAILABLE_SUBMISSION";

        private SamplesConfiguration _configuration;
        public main()
        {
            logWriter = new LogWriter("Open app...");
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
                try
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
                catch
                {
                    MessageBox.Show("Error, Not login email!");
                }
            }
        }

        public static void NewMessageReceived(object source, NewMessageReceivedEventArgs e)
        {
            Mailbox inbox = imap.SelectMailbox("inbox");
            ActiveUp.Net.Mail.Message message = inbox.Fetch.MessageObject(e.MessageCount);
            if (message.From.Email == "hungcuongqn86@gmail.com")
            {
                string[] subjectPath = message.Subject.Split('|');
                if(subjectPath.Length >= 3)
                {
                    if (subjectPath[2].Contains("Job Info to review"))
                    {
                        // avaliableUrl = "";
                        // autoget();
                    }
                }
            }

            // autoget();
            // imap4.StopIdle();
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
                logWriter.LogWrite("Error, Could not create directory for saving profiles!");
            }

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--user-data-dir=" + profilePath + "/" + imap4UserName);
            options.AddArgument("profile-directory=" + imap4UserName);
            options.AddArgument("disable-infobars");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--start-maximized");
            chromeDriver = new ChromeDriver(options);
            chromeDriver.Url = avaliableUrl;
            chromeDriver.Navigate();
            waitLoading();

            System.Threading.Thread.Sleep(5000);
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
                    System.Threading.Thread.Sleep(5000);
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
                    System.Threading.Thread.Sleep(2000);
                }
            }

            // button-1217 -- Close
            System.Threading.Thread.Sleep(5000);
            ReadOnlyCollection<IWebElement> buttonClose = chromeDriver.FindElements(By.XPath("//span[text()='Close' and contains(@id, 'btnInnerEl')]"));
            if (buttonClose.Count > 0)
            {
                IWebElement abuttonClose = buttonClose.First().FindElement(By.XPath("..")).FindElement(By.XPath("..")).FindElement(By.XPath(".."));
                string tbuttonClose = abuttonClose.TagName;
                if (tbuttonClose == "a")
                {
                    abuttonClose.Click();
                }
                waitLoading();
            }

            //select submission
            System.Threading.Thread.Sleep(3000);
            string xpath;
            if (String.IsNullOrEmpty(this.submissionId))
            {
                xpath = "//div[contains(@class, 'x-grid-cell-inner')]";
            }
            else
            {
                xpath = "//div[text()='" + this.submissionId + "' and contains(@class, 'x-grid-cell-inner')]";
            }
            
            ReadOnlyCollection<IWebElement> submission = chromeDriver.FindElements(By.XPath(xpath));
            if (submission.Count > 0)
            {
                submission.First().FindElement(By.XPath("..")).Click();
                logWriter.LogWrite("SubmissionID = " + submission.First().Text);
                waitLoading();
                System.Threading.Thread.Sleep(1000);

                ReadOnlyCollection<IWebElement> buttonpd_job_info_action = chromeDriver.FindElements(By.XPath("//span[text()='Job Info' and contains(@id, 'pd_job_info_action')]"));
                if (buttonpd_job_info_action.Count > 0)
                {
                    IWebElement abuttonpd_job_info_action = buttonpd_job_info_action.First().FindElement(By.XPath("..")).FindElement(By.XPath("..")).FindElement(By.XPath(".."));
                    string tbuttonpd_job_info_action = abuttonpd_job_info_action.TagName;
                    if (tbuttonpd_job_info_action == "a")
                    {
                        abuttonpd_job_info_action.Click();
                    }
                    waitLoading();
                    System.Threading.Thread.Sleep(5000);

                    // pdSubmissionBudgetJobInfoColumnRadioAccept
                    ReadOnlyCollection<IWebElement> pdSubmissionBudgetJobInfoColumnRadioAccept = chromeDriver.FindElements(By.XPath("//td[contains(@class, 'pdSubmissionBudgetJobInfoColumnRadioAccept')]"));
                    if (pdSubmissionBudgetJobInfoColumnRadioAccept.Count > 0)
                    {
                        pdSubmissionBudgetJobInfoColumnRadioAccept.First().Click();
                        waitLoading();
                    }

                    //checkbox-inputEl
                    ReadOnlyCollection<IWebElement> checkboxinputEl = chromeDriver.FindElements(By.XPath("//label[contains(@id, 'checkbox-') and contains(@id, '-boxLabelEl')]"));
                    if (checkboxinputEl.Count > 0)
                    {
                        checkboxinputEl.First().Click();
                        waitLoading();
                    }

                    //buttonSubmit
                    ReadOnlyCollection<IWebElement> buttonSubmit = chromeDriver.FindElements(By.XPath("//span[text()='Submit' and contains(@id, 'btnInnerEl')]"));
                    if (buttonSubmit.Count > 0)
                    {
                        IWebElement abuttonSubmit = buttonSubmit.First().FindElement(By.XPath("..")).FindElement(By.XPath("..")).FindElement(By.XPath(".."));
                        string tagname = abuttonSubmit.TagName;
                        if (tagname == "a")
                        {
                            // abuttonSubmit.Click();
                            logWriter.LogWrite("Submit...");
                        }
                        waitLoading();
                    }
                }
            }

            System.Threading.Thread.Sleep(5000);
            chromeDriver.Quit();
            logWriter.LogWrite("Done!");
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

        private void btnAuto1_Click(object sender, EventArgs e)
        {
            if (worker.IsBusy)
                worker.CancelAsync();

            worker.RunWorkerAsync();
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
            // this.submissionId = "0614938";
            logWriter.LogWrite("account1_start_btn_Click");
            autoget(this.Configuration.Imap4UserName, this.Configuration.TransperfectEmail, this.Configuration.TransperfectPass);
        }
    }
}
