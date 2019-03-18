// <copyright file="ReportGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace Reporting
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using AventStack.ExtentReports;
    using AventStack.ExtentReports.Reporter;
    using AventStack.ExtentReports.Reporter.Configuration;

    using NUnit.Framework;
    using NUnit.Framework.Interfaces;

    /// <summary>
    /// http://extentreports.com/docs/versions/4/net/
    /// </summary>
    internal partial class RandomSearch
    {        
        private static readonly NameValueCollection ReportSettings = ConfigurationManager.GetSection("reporting") as NameValueCollection;
        private static readonly bool Enabled = ReportSettings != null && bool.Parse(ReportSettings["Enabled"]);
        private static readonly bool ShowSteps = Enabled && bool.Parse(ReportSettings["ShowSteps"]);
        private static readonly bool DarkTheme = Enabled && bool.Parse(ReportSettings["DarkTheme"]);
        private static ExtentReports extent;
        private static ExtentKlovReporter klov;
        private static ExtentTest extentTest;
        private static Process cmd;


        [OneTimeSetUp()]
        public static void ClassInitialize()
        {
            try
            {
                if (Enabled)
                {
                    string currentdir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string style = "body {font-family: 'Segoe UI';}";
                    ExtentHtmlReporter htmlReporter = new ExtentHtmlReporter(currentdir);
                    htmlReporter.Config.Theme = DarkTheme ? Theme.Dark : Theme.Standard;
                    htmlReporter.Config.CSS = style;
                    extent = new ExtentReports();
                    extent.AddSystemInfo("Host", Environment.MachineName);
                    extent.AddSystemInfo("Env", ConfigurationManager.AppSettings["Env"]);
                    extent.AddSystemInfo("User", Environment.UserName);

                    StartKlovServer(currentdir);
                    
                    klov = new ExtentKlovReporter();
                    klov.InitMongoDbConnection("localhost", 27017);
                    klov.InitKlovServerConnection("http://localhost:8443");
                    klov.ProjectName = "Random Search";
                    klov.AnalysisStrategy = AnalysisStrategy.Class;

                    extent.AttachReporter(klov, htmlReporter);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        [OneTimeTearDown]
        public static void ClassCleanUp()
        {
            try
            {
                if (Enabled)
                {
                    extent.Flush();
                    FlushServer();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        [SetUp]
        public void BeforeTest()
        {
            extentTest = extent.CreateTest(TestContext.CurrentContext.Test.ClassName);
            var categories = TestContext.CurrentContext.Test.Properties["Category"];
            AddMetadata(extentTest, categories, TestContext.CurrentContext.Test.Properties.Get("Author").ToString());
        }

        [TearDown]
        public void AfterTest()
        {
            Status logstatus;
            TestStatus status = TestContext.CurrentContext.Result.Outcome.Status;
            string stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                ? ""
                : string.Format("<pre>{0}</pre>", TestContext.CurrentContext.Result.Message);

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    break;
                default:
                    logstatus = Status.Pass;
                    break;
            }

            var node = extentTest.CreateNode(TestContext.CurrentContext.Test.MethodName);
            node.Log(logstatus, "Test Execution status - " + logstatus + stacktrace);
        }

        private static void StartKlovServer(string currentdir)
        {
            // Initialize the mongodb and klov server connection
            cmd = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = $@"{ReportSettings["MongoDb"]} {currentdir}\klovServer";
            startInfo.FileName = $@"{currentdir}\setup.bat";
            cmd.StartInfo = startInfo;
            cmd.Start();
            Thread.Sleep(20000);
        }

        private static void FlushServer()
        {
            cmd.WaitForExit();
        }

        private static void AddMetadata(ExtentTest node, IEnumerable<object> categories, string author)
        {
            try
            {
                node.AssignCategory(categories.Cast<string>().ToArray());
                node.AssignAuthor(author);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
            }
        }
    }
}
