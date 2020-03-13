
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
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
            if (testContext is null) throw new ArgumentNullException(nameof(testContext));

            ApiHostProcess = Process.Start("dotnet", $@"run -p ..\..\..\..\..\src\SecretSanta.Api\SecretSanta.Api.csproj --urls={ApiURL}");
            WebHostProcess = Process.Start("dotnet", $@"run -p ..\..\..\..\..\src\SecretSanta.Web\SecretSanta.Web.csproj --urls={AppURL}");

            ApiHostProcess.WaitForExit(10000);
        }

        [ClassCleanup]
        public static void ClassCleanUp()
        {
            ApiHostProcess?.CloseMainWindow();
            ApiHostProcess?.Close();
            WebHostProcess?.CloseMainWindow();
            WebHostProcess?.Close();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            chromeOptions.AcceptInsecureCertificates = true;

            Driver = new ChromeDriver("./",chromeOptions);
            Driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 15);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(15);
        }

        private async Task<User> AddUser()
        {
            using HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = 
                (sender, cert, chain, sslPolicyErrors) => { return true; };
            using HttpClient httpClient = new HttpClient(clientHandler);
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
            User user = await AddUser();
            Driver.Navigate().GoToUrl(new Uri(AppURL + "Gifts"));
            Driver.Manage().Window.Maximize();
            string title = "A gift";
            string description = "Look at this description";
            string website = "www.Awebsite.com";

            IWebElement CreateGiftBtn = Driver.FindElement(By.CssSelector("body > section > div > div > button"));
            CreateGiftBtn.Click();
            ReadOnlyCollection<IWebElement> Inputs = Driver.FindElements(By.ClassName("input"));
            Assert.IsTrue(Inputs.Count == 3);
            Inputs[0].SendKeys(title);
            Inputs[1].SendKeys(description);
            Inputs[2].SendKeys(website);
            SelectElement Select = new SelectElement(Driver.FindElement(By.TagName("select")));
            Assert.IsTrue(Select.Options.Count > 0);
            Select.SelectByValue(user.Id.ToString());
            IWebElement SubmitBtn = Driver.FindElement(By.Id("submit"));
            SubmitBtn.Click();
            Thread.Sleep(8000);

            ReadOnlyCollection<IWebElement> UpdatedGiftList = Driver.FindElements(By.TagName("tr"));
            
            Assert.IsTrue(UpdatedGiftList.Count > 0);
            Screenshot ss = ((ITakesScreenshot)Driver).GetScreenshot();
            string path = Directory.GetCurrentDirectory() + "Screenshot.png";
            ss.SaveAsFile(path);
            TestContext.AddResultFile(path); 
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Driver?.Quit();
        }


    }
}
