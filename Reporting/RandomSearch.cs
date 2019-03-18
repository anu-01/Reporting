using System;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using Reporting.Properties;

namespace Reporting
{
    [TestFixture]
    internal partial class RandomSearch
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            this.driver = new ChromeDriver();
            this.driver.Manage().Window.Maximize();
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.Category("Search Engine")]
        [NUnit.Framework.Category("bvt")]
        [NUnit.Framework.Author("anammalu")]
        public void GoogleSearch()
        {
            this.driver.Navigate().GoToUrl(Resources.Google);
            this.driver.FindElement(By.XPath(Resources.Query)).SendKeys("Extent Reports");
            this.driver.FindElement(By.XPath(Resources.Query)).SendKeys(Keys.Enter);
            //    driver.FindElement(By.XPath(Resources.Submit)).Click();

            IWebElement element = this.driver.FindElement(By.XPath(Resources.SearchResults));
            Assert.IsTrue(element.Displayed);
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.CategoryAttribute("Search Engine")]
        [NUnit.Framework.CategoryAttribute("priority=1")]
        [NUnit.Framework.Author("anammalu")]
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
