
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SecretSanta.Web.Api;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace SecretSanta.Web.Tests
{
    [TestClass]
    public class GiftPageTests
    {
        [NotNull]
        public TestContext? TestContext { get; set; }

        [NotNull]
        private IWebDriver? Driver { get; set; }

        static string AppURL { get; } = "https://localhost:44394/";
        static string ApiURL { get; } = "https://localhost:44388";
        private static Process? ApiHostProcess { get; set; }
        private static Process? WebHostProcess { get; set; }


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var path = testContext.DeploymentDirectory;
            string apiPath = Path.GetFullPath(Path.Combine(path, @"../../../../../src/SecretSanta.Api/SecretSanta.Api.csproj"));
            string webPath = Path.GetFullPath(Path.Combine(path, @"../../../../../src/SecretSanta.Web/SecretSanta.Web.csproj"));

            Console.WriteLine(apiPath);
            Console.WriteLine(webPath);
            
            ApiHostProcess = Process.Start("dotnet.exe", $"run -p {apiPath} --urls={ApiURL}");
            WebHostProcess = Process.Start("dotnet.exe", $"run -p {webPath} --urls={AppURL}");

            Console.WriteLine("did the process start?");
        }

        [ClassCleanup]
        public static void ClassCleanUp()
        {
            ApiHostProcess?.Close();
            WebHostProcess?.Close();
        }

        [TestInitialize]
        public void TestInitialize()
        {

            string browser = "Chrome";
            switch (browser)
            {
                case "Chrome":
                    Driver = new ChromeDriver();
                    break;
                default:
                    Driver = new ChromeDriver();
                    break;
            }
            Driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 10);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
        }

        private async Task<User> AddUser()
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ApiURL);
            UserClient client = new UserClient(httpClient);
            UserInput user = new UserInput
            {
                FirstName = "Kyle",
                LastName = "Smith"
            };

            return await client.PostAsync(user);
        }


        [TestMethod]
        public async Task GiftPage_AddGift_Success()
        {
            var user = await AddUser();

            Driver.Navigate().GoToUrl(new Uri(AppURL + "Gifts"));
            Driver.Manage().Window.Maximize();

            var GetGiftList = Driver.FindElements(By.TagName("tr"));

            var CreateGiftBtn = Driver.FindElement(By.CssSelector("body > section > div > div > button"));
            CreateGiftBtn.Click();
            var Inputs = Driver.FindElements(By.ClassName("input"));
            Assert.IsTrue(Inputs.Count == 3);
            Inputs[0].SendKeys("A gift");
            Inputs[1].SendKeys("Look at this description");
            Inputs[2].SendKeys("www.Awebsite.com");
            SelectElement Select = new SelectElement(Driver.FindElement(By.TagName("select")));
            Assert.IsTrue(Select.Options.Count > 0);
            Select.SelectByValue(user.Id.ToString());
            var SubmitBtn = Driver.FindElement(By.Id("submit"));
            SubmitBtn.Click();
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("body > section > div > div > button")));
            var UpdatedGiftList = Driver.FindElements(By.TagName("tr"));

            //Assert.IsTrue(GetGiftList.Count < UpdatedGiftList.Count);
            Screenshot image = ((ITakesScreenshot) Driver).GetScreenshot();
            image.SaveAsFile("../../../ScreenShot.png", ScreenshotImageFormat.Png);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Driver.Quit();
        }


    }
}
