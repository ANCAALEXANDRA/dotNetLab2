using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class EndToEndTest
    {
        private IWebDriver _driver;

        [SetUp]
        public void SetupDriver()
        {
            _driver = new ChromeDriver("C:\\drivers");
        }


        [TearDown]
        public void CloseBrowser()
        {
            _driver.Close();
        }

        [Test]
        public void LoginAndViewMovie()
        {
            _driver.Url = "http://localhost:4200/login";


          
            var emailInput = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-login/ion-content/div/form/ion-item[1]/ion-input/input"));
            emailInput.SendKeys("anca@gmail.ro");
            System.Threading.Thread.Sleep(1000);

            var password = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-login/ion-content/div/form/ion-item[2]/ion-input/input"));
            System.Threading.Thread.Sleep(4000);
            password.SendKeys("ancaANCA12!");

            var loginBtn = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-login/ion-content/div/form/ion-button"));
            System.Threading.Thread.Sleep(800); 
            loginBtn.Click();
            System.Threading.Thread.Sleep(7000);

            _driver.FindElement(By.XPath("//ion-list"));
            System.Threading.Thread.Sleep(4000);
            Assert.Pass();

        }

    }
}