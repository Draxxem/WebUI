using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WebUi;
using WebUi.Model;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace WebUI
{
    [TestClass]
    public class Tests
    {
        IWebDriver driver;
        string url = "https://www.way2automation.com/angularjs-protractor/webtables/";
        Common common = new Common();

      [OneTimeSetUp]
        public void StartBrowser()
        {
            var dir = Directory.GetCurrentDirectory();
            driver = new ChromeDriver(dir);
            driver.Url = url;
        }

        [Test,Order(1)]
        public void NavigateToWebsiteAndVerifyThatYouAreOnCorrectWebsite()
        {      
            string currentURL = driver.Url.ToString();
            Assert.AreEqual(url, currentURL.ToString());
        }

        [Test, Order(2)]
        public void VerifyThatUserListTableIsDisplayed()
        {        
            bool table = driver.FindElement(By.ClassName("smart-table")).Displayed;
            Assert.IsTrue(table);
        }

        [Test, Order(3)]
        public void AddTwoUsersAndVerifyThatTwoUsersWereSuccessfullyAdded()
        {
            var currentDir = Path.GetDirectoryName(new Uri(typeof(Common).Assembly.CodeBase).LocalPath);
            var content = common.ReadFile(currentDir, "TestData/UserTestData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(content);

            Base webTable = new Base(driver);

            users.ForEach(user =>
            {
                webTable.Click_Add_User();
                bool addUserForm = driver.FindElement(By.ClassName("modal-body")).Displayed;
                Assert.IsTrue(addUserForm);
                webTable.CompleteUserForm(user);
            });

            var tableData = webTable.GetTableData();
            List<User> userTableData = new List<User>();
            tableData = Regex.Replace(tableData, "Company AAA", "CompanyAAA");
            tableData = Regex.Replace(tableData, "Company BBB", "CompanyBBB");
            var test = Regex.Split(tableData, "Edit");
            test.ToList().ForEach(user =>
            {
                if (user.Length < 1)
                    return;
                user.Trim();
                var data = Regex.Split(user, @"\s+");

                List<User> userData = new List<User>();
                var tester = new User { FirstName = data[0] != "" ? data[0] : data[1], Lastname = data[1], Username = data[2], Customer = data[3], Role = data[4], Email = data[5], Cell = data[6], Password = "" };

                userTableData.Add(tester);
            }
            );

            int count = 0;

            userTableData.ForEach(user =>
            {
                var y = users.ToList().Where(x => x.FirstName == user.FirstName.Trim());
                if (y.FirstOrDefault() != null)
                    count++;
            });

            Assert.IsTrue(count == users.Count());
        }

        [OneTimeTearDown]
        public void closeBrowser()
        {
            driver.Close();
        }
    }
}