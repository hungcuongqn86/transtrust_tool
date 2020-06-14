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
        public static main thisForm;
        public static Imap4Client imap1;
        public static Imap4Client imap2;
        public BackgroundWorker worker1;
        public BackgroundWorker worker2;
        ChromeDriver chromeDriver;
        string submissionId;
        string dashboardUrl = "https://dashboard.transperfect.com/";
        string tdcAvaliableUrl = "https://gl-tdcprod1.translations.com/PD/#userMenuAVAILABLE_SUBMISSION";
        string tptAvaliableUrl = "https://gl-tptprod1.transperfect.com/PD/#userMenuAVAILABLE_SUBMISSION";
        string avaliableUrl = "https://gl-tdcprod1.translations.com/PD/#userMenuAVAILABLE_SUBMISSION";
        public static string proSender = "noreply@translations.com";

        private SamplesConfiguration _configuration;
        public main()
        {
            logWriter = new LogWriter("Open app...");
            InitializeComponent();
            thisForm = this;
            InitializeSample();
            label1.Text = this.Configuration.TransperfectEmail;
            label2.Text = this.Configuration.TransperfectEmail2;
        }

        protected void InitializeSample()
        {
            worker1 = new BackgroundWorker();
            worker2 = new BackgroundWorker();
            worker1.DoWork += new DoWorkEventHandler(StartIdleProcess1);
            worker2.DoWork += new DoWorkEventHandler(StartIdleProcess2);
        }

        private void StartIdleProcess1(object sender, DoWorkEventArgs e)
        {
            if (imap1 != null && imap1.IsConnected)
            {
                imap1.StopIdle();
                imap1.Disconnect();
            }
            if (this.Configuration.Imap4Server != null)
            {
                try
                {
                    imap1 = new Imap4Client();
                    imap1.NewMessageReceived += new NewMessageReceivedEventHandler(NewMessageReceived1);
                    //worker.ReportProgress(1, "Connection...");
                    imap1.ConnectSsl(this.Configuration.Imap4Server, 993);
                    //worker.ReportProgress(0, "Login...");
                    imap1.Login(this.Configuration.Imap4UserName, this.Configuration.Imap4Password);
                    //worker.ReportProgress(0, "Select 'inbox'...");
                    imap1.SelectMailbox("inbox");
                    //worker.ReportProgress(0, "Start idle...");
                    logWriter.LogWrite("imap1.StartIdle ...");
                    imap1.StartIdle();
                }
                catch
                {
                    logWriter.LogWrite("Error, Not login email!"); 
                    MessageBox.Show("Error, Not login email!");
                }
            }
        }

        private void StartIdleProcess2(object sender, DoWorkEventArgs e)
        {
            if (imap2 != null && imap2.IsConnected)
            {
                imap2.StopIdle();
                imap2.Disconnect();
            }
            if (this.Configuration.Imap4Server2 != null)
            {
                try
                {
                    imap2 = new Imap4Client();
                    imap2.NewMessageReceived += new NewMessageReceivedEventHandler(NewMessageReceived2);
                    //worker.ReportProgress(1, "Connection...");
                    imap2.ConnectSsl(this.Configuration.Imap4Server2, 993);
                    //worker.ReportProgress(0, "Login...");
                    imap2.Login(this.Configuration.Imap4UserName2, this.Configuration.Imap4Password2);
                    //worker.ReportProgress(0, "Select 'inbox'...");
                    imap2.SelectMailbox("inbox");
                    //worker.ReportProgress(0, "Start idle...");
                    logWriter.LogWrite("imap2.StartIdle ...");
                    imap2.StartIdle();
                }
                catch
                {
                    MessageBox.Show("Error, Not login email!");
                }
            }
        }

        public static void NewMessageReceived1(object source, NewMessageReceivedEventArgs e)
        {
            thisForm.logWriter.LogWrite("NewMessageReceived1---");
            using (var client = new Imap4Client())
            {
                try
                {
                    client.ConnectSsl(thisForm.Configuration.Imap4Server, 993);
                    client.Login(thisForm.Configuration.Imap4UserName, thisForm.Configuration.Imap4Password);
                    Mailbox inbox = client.SelectMailbox("inbox");
                    if (inbox.MessageCount > 0)
                    {
                        ActiveUp.Net.Mail.Message message = inbox.Fetch.MessageObject(inbox.MessageCount);
                        thisForm.logWriter.LogWrite(string.Format("{3} Subject: {0} From :{1} Message Body {2}"
                                        , message.Subject, message.From.Email, message.BodyText, inbox.MessageCount.ToString("00000")));
                        if (message.From.Email == proSender)
                        {
                            string[] subjectPath = message.Subject.Split('|');
                            if (subjectPath.Length >= 3)
                            {
                                if (subjectPath[2].Contains("Job Info to review"))
                                {
                                    // string submissionId
                                    string[] submissionPath = subjectPath[1].Trim().Split(' ');
                                    if (submissionPath.Length >= 2)
                                    {
                                        thisForm.runAuto(message.Subject,
                                            subjectPath[0].Trim(),
                                            submissionPath[1].Trim(),
                                            thisForm.Configuration.Imap4UserName,
                                            thisForm.Configuration.TransperfectEmail,
                                            thisForm.Configuration.TransperfectPass);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        thisForm.logWriter.LogWrite("NewMessageReceived1: There is no message in the imap4 account");
                    }
                }
                catch (Exception ex)
                {
                    thisForm.logWriter.LogWrite(ex.Message);
                }
            }
        }

        public static void NewMessageReceived2(object source, NewMessageReceivedEventArgs e)
        {
            thisForm.logWriter.LogWrite("NewMessageReceived2---");
            using (var client = new Imap4Client())
            {
                try
                {
                    client.ConnectSsl(thisForm.Configuration.Imap4Server2, 993);
                    client.Login(thisForm.Configuration.Imap4UserName2, thisForm.Configuration.Imap4Password2);
                    Mailbox inbox = client.SelectMailbox("inbox");
                    if (inbox.MessageCount > 0)
                    {
                        ActiveUp.Net.Mail.Message message = inbox.Fetch.MessageObject(inbox.MessageCount);
                        thisForm.logWriter.LogWrite(string.Format("{3} Subject: {0} From :{1} Message Body {2}"
                                        , message.Subject, message.From.Email, message.BodyText, inbox.MessageCount.ToString("00000")));
                        if (message.From.Email == proSender)
                        {
                            string[] subjectPath = message.Subject.Split('|');
                            if (subjectPath.Length >= 3)
                            {
                                if (subjectPath[2].Contains("Job Info to review"))
                                {
                                    // string submissionId
                                    string[] submissionPath = subjectPath[1].Trim().Split(' ');
                                    if (submissionPath.Length >= 2)
                                    {
                                        thisForm.runAuto(message.Subject,
                                            subjectPath[0].Trim(),
                                            submissionPath[1].Trim(),
                                            thisForm.Configuration.Imap4UserName2,
                                            thisForm.Configuration.TransperfectEmail2,
                                            thisForm.Configuration.TransperfectPass2);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        thisForm.logWriter.LogWrite("NewMessageReceived2: There is no message in the imap4 account");
                    }
                }
                catch (Exception ex)
                {
                    thisForm.logWriter.LogWrite(ex.Message);
                }
            }
        }

        private void runAuto(string subject, string endpoint, string submission, string imap4UserName, string transperfectEmail, string transperfectPass)
        {
            string msg = "Email " + imap4UserName + ", Subject: " + subject;
            logWriter.LogWrite(msg);
            submissionId = submission;
            if (endpoint == "TDC-PD")
            {
                avaliableUrl = tdcAvaliableUrl;
            }
            if (endpoint == "TPT-PD")
            {
                avaliableUrl = tptAvaliableUrl;
            }
            autoget(imap4UserName, transperfectEmail, transperfectPass);
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
                logWriter.LogWrite("------------------------------------------------");
                return;
            }

            try
            {
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
            }
            catch
            {
                logWriter.LogWrite("Error, Could not open profiles!");
                logWriter.LogWrite("------------------------------------------------");
                return;
            }
            // If nologin
            string url = chromeDriver.Url;
            if (url.Contains("gl-tdcprod1.translations.com/PD/login") || url.Contains("gl-tptprod1.transperfect.com/PD/login"))
            {
                // confirm login
                ReadOnlyCollection<IWebElement> eloginwithemailbutton = chromeDriver.FindElements(By.Id("loginwithemail-button"));
                if (eloginwithemailbutton.Count > 0)
                {
                    eloginwithemailbutton.First().Click();
                    waitLoading();
                    System.Threading.Thread.Sleep(5000);
                    logWriter.LogWrite("login with email...");
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
                    System.Threading.Thread.Sleep(2000);
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
                System.Threading.Thread.Sleep(2000);
                if (ePasswords.Count > 0)
                {
                    ePasswords.First().Clear(); ;
                    ePasswords.First().SendKeys(pass);
                    System.Threading.Thread.Sleep(2000);
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

                logWriter.LogWrite("login...");
            }

            url = chromeDriver.Url;
            if (url.Contains("sso.transperfect.com/Account/Login"))
            {
                logWriter.LogWrite("Login fall!");
                chromeDriver.Quit();
                return;
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
                waitLoading();
                System.Threading.Thread.Sleep(1000);

                ReadOnlyCollection<IWebElement> buttonpd_job_info_action = chromeDriver.FindElements(By.XPath("//span[text()='Job Info' and contains(@id, 'pd_job_info_action')]"));
                if (buttonpd_job_info_action.Count > 0)
                {
                    logWriter.LogWrite("SubmissionID = " + submission.First().Text);
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
                            abuttonSubmit.Click();
                            logWriter.LogWrite("Submit...");
                        }
                        waitLoading();
                    }

                    // button -- Close
                    System.Threading.Thread.Sleep(5000);
                    ReadOnlyCollection<IWebElement> buttonClose1 = chromeDriver.FindElements(By.XPath("//span[text()='Close' and contains(@id, 'btnInnerEl')]"));
                    if (buttonClose1.Count > 0)
                    {
                        IWebElement abuttonClose1 = buttonClose1.First().FindElement(By.XPath("..")).FindElement(By.XPath("..")).FindElement(By.XPath(".."));
                        string tbuttonClose1 = abuttonClose1.TagName;
                        if (tbuttonClose1 == "a")
                        {
                            abuttonClose1.Click();
                        }
                        waitLoading();
                    }

                    // button -- Close
                    System.Threading.Thread.Sleep(5000);
                    ReadOnlyCollection<IWebElement> buttonClose2 = chromeDriver.FindElements(By.XPath("//span[text()='Close' and contains(@id, 'btnInnerEl')]"));
                    if (buttonClose2.Count > 0)
                    {
                        IWebElement abuttonClose2 = buttonClose2.First().FindElement(By.XPath("..")).FindElement(By.XPath("..")).FindElement(By.XPath(".."));
                        string tbuttonClose2 = abuttonClose2.TagName;
                        if (tbuttonClose2 == "a")
                        {
                            abuttonClose2.Click();
                        }
                        waitLoading();
                    }
                }
                else
                {
                    logWriter.LogWrite("Not submission!");
                }
            }
            else
            {
                logWriter.LogWrite("Not submission!");
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

        private void loading(bool status)
        {
            btnAuto1.Enabled = status;
            btnAuto2.Enabled = status;
            account1_start_btn.Enabled = status;
            account1TptRun.Enabled = status;
            account2_start_btn.Enabled = status;
            account2TptRun.Enabled = status;
        }

        private void btnAuto1_Click(object sender, EventArgs e)
        {
            if (worker1.IsBusy)
                worker1.CancelAsync();

            worker1.RunWorkerAsync();
            logWriter.LogWrite("btnAuto1_Click...");
            loading(false);
        }

        private void btnAuto2_Click(object sender, EventArgs e)
        {
            if (worker2.IsBusy)
                worker2.CancelAsync();

            worker2.RunWorkerAsync();
            logWriter.LogWrite("btnAuto2_Click...");
            loading(false);
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
            loading(false);
            logWriter.LogWrite("------------------------------------------------");
            logWriter.LogWrite("account1_start_btn_Click");
            avaliableUrl = tdcAvaliableUrl;
            autoget(this.Configuration.Imap4UserName, this.Configuration.TransperfectEmail, this.Configuration.TransperfectPass);
            loading(true);
        }

        private void account2_start_btn_Click(object sender, EventArgs e)
        {
            // this.submissionId = "0614938";
            loading(false);
            logWriter.LogWrite("------------------------------------------------");
            logWriter.LogWrite("account2_start_btn_Click");
            avaliableUrl = tdcAvaliableUrl;
            autoget(this.Configuration.Imap4UserName2, this.Configuration.TransperfectEmail2, this.Configuration.TransperfectPass2);
            loading(true);
        }

        private void account1TptRun_Click(object sender, EventArgs e)
        {
            // this.submissionId = "0614938";
            loading(false);
            logWriter.LogWrite("------------------------------------------------");
            logWriter.LogWrite("account1TptRun_Click");
            avaliableUrl = tptAvaliableUrl;
            autoget(this.Configuration.Imap4UserName, this.Configuration.TransperfectEmail, this.Configuration.TransperfectPass);
            loading(true);
        }

        private void account2TptRun_Click(object sender, EventArgs e)
        {
            // this.submissionId = "0614938";
            loading(false);
            logWriter.LogWrite("------------------------------------------------");
            logWriter.LogWrite("account2TptRun_Click");
            avaliableUrl = tptAvaliableUrl;
            autoget(this.Configuration.Imap4UserName2, this.Configuration.TransperfectEmail2, this.Configuration.TransperfectPass2);
            loading(true);
        }
    }
}
