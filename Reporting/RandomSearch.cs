using System;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using Reporting.Properties;

namespace Reporting
{
    [TestFixture, Parallelizable(ParallelScope.Fixtures)]
    public class RandomSearch : ReportGenerator
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            this.driver = new ChromeDriver();
            this.driver.Manage().Window.Maximize();
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        }

        [TestAttribute()]
        [Category("Search Engine")]
        [Category("bvt")]
        [Author("anammalu")]
        public void GoogleSearch()
        {
            this.driver.Navigate().GoToUrl(Resources.Google);
            this.driver.FindElement(By.XPath(Resources.Query)).SendKeys("Extent Reports");
            this.driver.FindElement(By.XPath(Resources.Query)).SendKeys(Keys.Enter);
            //    driver.FindElement(By.XPath(Resources.Submit)).Click();

            IWebElement element = this.driver.FindElement(By.XPath(Resources.SearchResults));
            Assert.IsTrue(element.Displayed);
        }

        [TestAttribute()]
        [CategoryAttribute("Search Engine")]
        [CategoryAttribute("priority=1")]
        [Author("anammalu")]
        public void NegativeTestForBingSearch()
        {
            this.driver.Navigate().GoToUrl(Resources.Bing);
            this.driver.FindElement(By.XPath(Resources.Query)).SendKeys("Extent Reports");
            this.driver.FindElement(By.XPath(Resources.Query)).SendKeys(Keys.Enter);
            //    driver.FindElement(By.XPath(Resources.Submit)).Click();

            IWebElement element = this.driver.FindElement(By.XPath(Resources.SearchResults));
            Assert.IsFalse(element.Displayed);
        }

        [TearDown]
        public void TearDown()
        {
            this.driver.Close();
        }
    }
}
