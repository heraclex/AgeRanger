using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;
using System.Diagnostics;

namespace AgeRange.AcceptanceTest.BrowerTests
{
    [TestClass]
    public class BrowerTestBase
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(BrowerTestBase));
        protected IWebDriver webDriver;

        [TestInitialize]
        public void Setup()
        {
            this.InitWebDriver();
            this.webDriver.Navigate().GoToUrl(ConfigurationManager.AppSettings["acceptanceTestUrl"]);
            this.webDriver.Manage().Window.Maximize();
        }

        [TestCleanup]
        public void TearDown()
        {
            this.webDriver.Close();
            this.webDriver.Dispose();
        }

        protected void WaitForElementVisible(By by, int timeOutInSeconds)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(by));
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
                this.logger.InfoFormat("Time elapsed: {0}", stopwatch.Elapsed.Seconds);
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        // Should be refactor using Factory Methods
        private void InitWebDriver()
        {            
            var browerTest = ConfigurationManager.AppSettings["brower:Name"];
            switch (browerTest)
            {
                case "Chrome":
                    this.webDriver = new ChromeDriver();
                    break;
                case "IE":
                    this.webDriver = new InternetExplorerDriver();
                    break;
                case "Firefox":
                    this.webDriver = new FirefoxDriver();
                    break;
                default:
                    // using phantomjs 
                    break;
            }
        }
    }
}
