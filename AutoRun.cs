using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Keys = OpenQA.Selenium.Keys;

namespace transtrusttool
{
    public class AutoRun : IDisposable
    {
        public LogWriter logWriter;
        public ChromeDriver chromeDriver;
        public string submissionId;
        // string dashboardUrl = "https://dashboard.transperfect.com/";
        public string tdcAvaliableUrl = "https://gl-tdcprod1.translations.com/PD/#userMenuAVAILABLE_SUBMISSION";
        public string tptAvaliableUrl = "https://gl-tptprod1.transperfect.com/PD/#userMenuAVAILABLE_SUBMISSION";
        public string avaliableUrl = "https://gl-tdcprod1.translations.com/PD/#userMenuAVAILABLE_SUBMISSION";
        public bool working = false;
        public string email;
        public string pass;
        private bool popup = false;

        public AutoRun(string imap4UserName, string email, string pass)
        {
            this.email = email;
            this.pass = pass;

            logWriter = new LogWriter("AutoRun ...");
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

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--user-data-dir=" + profilePath + "/" + imap4UserName);
            options.AddArgument("profile-directory=" + imap4UserName);
            options.AddArgument("disable-infobars");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--start-maximized");
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            chromeDriver = new ChromeDriver(driverService, options);

            // Test
            // avaliableUrl = tptAvaliableUrl;
            // chromeDriver.Navigate().GoToUrl(avaliableUrl);
            // Login();
        }

        private void TimerEventProcessor(Object myObject,
                                        EventArgs myEventArgs)
        {
            if (working == false)
            {
                chromeDriver.Navigate().Refresh();

                WaitAjaxLoading(By.Id("errorMessage-title"));
                ReadOnlyCollection<IWebElement> msgErrorBox = chromeDriver.FindElements(By.Id("errorMessage-title"));
                if (msgErrorBox.Count > 0)
                {
                    ReadOnlyCollection<IWebElement> buttonClose = chromeDriver.FindElements(By.Id("errorMessage-closeButton"));
                    if (buttonClose.Count > 0)
                    {
                        string tbuttonClose = buttonClose.First().TagName;
                        if (tbuttonClose == "button")
                        {
                            buttonClose.First().Click();
                        }
                        WaitLoading();
                        Login();
                    }
                }

                ReadOnlyCollection<IWebElement> sessionTerminatedErrorBox = chromeDriver.FindElements(By.XPath("//div[text()='Session terminated' and contains(@id, 'innerCt')]"));
                if (sessionTerminatedErrorBox.Count > 0)
                {
                    ReadOnlyCollection<IWebElement> buttonClose = chromeDriver.FindElements(By.XPath("//span[text()='Close' and contains(@id, 'btnInnerEl')]"));
                    if (buttonClose.Count > 0)
                    {
                        IWebElement abuttonClose = buttonClose.First().FindElement(By.XPath("..")).FindElement(By.XPath("..")).FindElement(By.XPath(".."));
                        string tbuttonClose = abuttonClose.TagName;
                        if (tbuttonClose == "a")
                        {
                            abuttonClose.Click();
                        }
                        WaitLoading();
                        Login();
                    }
                }

                // If logout
                ReadOnlyCollection<IWebElement> loginForm = chromeDriver.FindElements(By.Id("loginForm"));
                if (loginForm.Count > 0)
                {
                    Login();
                }

                // button-1217 -- Close
                popup = false;
                WaitAjaxLoading(By.XPath("//span[text()='Close' and contains(@id, 'btnInnerEl')]"));
                ReadOnlyCollection<IWebElement> buttonClosepp = chromeDriver.FindElements(By.XPath("//span[text()='Close' and contains(@id, 'btnInnerEl')]"));
                if (buttonClosepp.Count > 0)
                {
                    popup = true;
                    IWebElement abuttonClose = buttonClosepp.First().FindElement(By.XPath("..")).FindElement(By.XPath("..")).FindElement(By.XPath(".."));
                    string tbuttonClose = abuttonClose.TagName;
                    if (tbuttonClose == "a")
                    {
                        abuttonClose.Click();
                    }
                    WaitLoading();
                }
            }
        }

        private bool Login()
        {
            try
            {
                string url = chromeDriver.Url;
                if (url.Contains("sso.transperfect.com/Account/Login"))
                {
                    // SendKeys email
                    string inputStr = "Email";
                    WaitAjaxLoading(By.Id(inputStr));
                    IWebElement input = chromeDriver.FindElement(By.Id(inputStr));
                    input.Clear(); ;
                    input.SendKeys(email);
                    Delay(500);

                    // SendKeys password /Password
                    inputStr = "Password";
                    WaitAjaxLoading(By.Id(inputStr));
                    input = chromeDriver.FindElement(By.Id(inputStr));
                    input.Clear(); ;
                    input.SendKeys(pass);
                    Delay(500);

                    // SubmitLogin2
                    WaitAjaxLoading(By.Id("SubmitLogin"));
                    ReadOnlyCollection<IWebElement> SubmitLogin = chromeDriver.FindElements(By.Id("SubmitLogin"));
                    if (SubmitLogin.Count > 0)
                    {
                        SubmitLogin.First().Click();
                        WaitLoading();
                    }
                }

                WaitLoading();
                url = chromeDriver.Url;
                if (url.Contains("sso.transperfect.com/Account/Login"))
                {
                    logWriter.LogWrite("Login fall!");
                }

                //-- Close
                WaitAjaxLoading(By.XPath("//span[text()='Close' and contains(@id, 'btnInnerEl')]"));
                ReadOnlyCollection<IWebElement> buttonClose = chromeDriver.FindElements(By.XPath("//span[text()='Close' and contains(@id, 'btnInnerEl')]"));
                if (buttonClose.Count > 0)
                {
                    popup = true;
                    IWebElement abuttonClose = buttonClose.First().FindElement(By.XPath("..")).FindElement(By.XPath("..")).FindElement(By.XPath(".."));
                    string tbuttonClose = abuttonClose.TagName;
                    if (tbuttonClose == "a")
                    {
                        abuttonClose.Click();
                    }
                    logWriter.LogWrite("Check login - Close popup ...");
                    WaitLoading();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            if (chromeDriver != null)
            {
                chromeDriver.Quit();
            }
        }

        public void RunAuto(string endpoint, string submission)
        {
            working = true;
            // string msg = "Email " + imap4UserName + ", Subject: " + subject;
            this.logWriter.LogWrite(String.Format("RunAuto: {0} - {1}", endpoint, submission));
            submissionId = submission;
            if (endpoint == "TDC-PD")
            {
                avaliableUrl = tdcAvaliableUrl;
            }
            if (endpoint == "TPT-PD")
            {
                avaliableUrl = tptAvaliableUrl;
            }

            chromeDriver.Navigate().GoToUrl(avaliableUrl);
            WaitLoading();
            Autoget();
            working = false;
        }

        public void Autoget()
        {
            Login();
            try
            {
                //select submission
                string xpath;
                if (String.IsNullOrEmpty(this.submissionId))
                {
                    xpath = "//div[contains(@class, 'x-grid-cell-inner')]";
                }
                else
                {
                    xpath = "//div[contains(text(),'" + this.submissionId + "') and contains(@class, 'x-grid-cell-inner')]";
                }

                WaitAjaxLoading(By.XPath(xpath));
                ReadOnlyCollection<IWebElement> submission = chromeDriver.FindElements(By.XPath(xpath));

                if (submission.Count > 0)
                {
                    submission.First().FindElement(By.XPath("..")).Click();
                    string btnJobInfo = "//span[text()='Job Info' and contains(@id, 'pd_job_info_action')]";
                    WaitAjaxLoading(By.XPath(btnJobInfo));
                    ReadOnlyCollection<IWebElement> buttonpd_job_info_action = chromeDriver.FindElements(By.XPath(btnJobInfo));
                    if (buttonpd_job_info_action.Count > 0)
                    {
                        logWriter.LogWrite("SubmissionID = " + submission.First().Text);
                        IWebElement abuttonpd_job_info_action = buttonpd_job_info_action.First().FindElement(By.XPath("..")).FindElement(By.XPath("..")).FindElement(By.XPath(".."));
                        string tbuttonpd_job_info_action = abuttonpd_job_info_action.TagName;
                        if (tbuttonpd_job_info_action == "a")
                        {
                            abuttonpd_job_info_action.Click();
                        }
                        WaitLoading();

                        // pdSubmissionBudgetJobInfoColumnRadioAccept
                        WaitAjaxLoading(By.XPath("//div[contains(@class, 'x-grid-radio-col')]"));
                        ReadOnlyCollection<IWebElement> pdSubmissionBudgetJobInfoColumnRadioAccept = chromeDriver.FindElements(By.XPath("//div[contains(@class, 'x-grid-radio-col')]"));
                        if (pdSubmissionBudgetJobInfoColumnRadioAccept.Count > 0)
                        {
                            pdSubmissionBudgetJobInfoColumnRadioAccept.First().FindElement(By.XPath("..")).FindElement(By.XPath("..")).Click();
                            WaitLoading();
                        }

                        //checkbox-inputEl
                        WaitAjaxLoading(By.XPath("//label[contains(@id, 'checkbox-') and contains(@id, '-boxLabelEl')]"));
                        ReadOnlyCollection<IWebElement> checkboxinputEl = chromeDriver.FindElements(By.XPath("//label[contains(@id, 'checkbox-') and contains(@id, '-boxLabelEl')]"));
                        if (checkboxinputEl.Count > 0)
                        {
                            checkboxinputEl.First().Click();
                            WaitLoading();
                        }

                        //buttonSubmit
                        WaitAjaxLoading(By.XPath("//span[text()='Submit' and contains(@id, 'btnInnerEl')]"));
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
                            WaitLoading();
                        }

                        // button -- Close
                        WaitAjaxLoading(By.XPath("//span[text()='Close' and contains(@id, 'btnInnerEl')]"));
                        ReadOnlyCollection<IWebElement> buttonClose1 = chromeDriver.FindElements(By.XPath("//span[text()='Close' and contains(@id, 'btnInnerEl')]"));
                        if (buttonClose1.Count > 0)
                        {
                            IWebElement abuttonClose1 = buttonClose1.Last().FindElement(By.XPath("..")).FindElement(By.XPath("..")).FindElement(By.XPath(".."));
                            string tbuttonClose1 = abuttonClose1.TagName;
                            if (tbuttonClose1 == "a")
                            {
                                abuttonClose1.Click();
                            }
                            WaitLoading();
                        }
                    }
                    else
                    {
                        logWriter.LogWrite("Not show Job Info!");
                    }
                }
                else
                {
                    logWriter.LogWrite("Not submission!");
                }

                System.Threading.Thread.Sleep(10000);
                logWriter.LogWrite("Done!");
            }
            catch { }
        }

        private static void Delay(int Time_delay)
        {
            int i = 0;
            System.Timers.Timer _delayTimer = new System.Timers.Timer();
            _delayTimer.Interval = Time_delay;
            _delayTimer.AutoReset = false; //so that it only calls the method once
            _delayTimer.Elapsed += (s, args) => i = 1;
            _delayTimer.Start();
            while (i == 0) { };
        }

        private void WaitAjaxLoading(By byFinter)
        {
            // wait loading
            WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(5));
            Func<IWebDriver, bool> waitLoading = new Func<IWebDriver, bool>((IWebDriver Web) =>
            {
                try
                {
                    ReadOnlyCollection<IWebElement> alertE = Web.FindElements(byFinter);
                    if (alertE.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            });

            try
            {
                wait.Until(waitLoading);
            }
            catch { }
        }

        private void WaitLoading()
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
