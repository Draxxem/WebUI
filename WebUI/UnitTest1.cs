using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WebUi;
using WebUi.Model;

namespace WebUI
{
    public class Tests
    {
        IWebDriver driver;
        string url = "https://www.way2automation.com/angularjs-protractor/webtables/";
        Common common = new Common();

        [SetUp]
        public void startBrowser()
        {
            var dir = Directory.GetCurrentDirectory();
            driver = new ChromeDriver(dir);       
        }

        [Test]
        public void ValidateUserListTableAndAddUsers()
        {
            var content = common.ReadFile("TestData/UserTestData.json");
            var users = JsonConvert.DeserializeObject<User[]>(content);

            driver.Url = url;
            bool table = driver.FindElement(By.ClassName("smart-table")).Displayed;
            Assert.IsTrue(table);
        
            Base webTable = new Base(driver);

            users.ToList().ForEach(user =>
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
                User tester = new User();
                user.Trim();
                var data = Regex.Split(user, @"\s+");
                List<string> list = new List<string>();
                var listing = data.ToList();
                if (listing.Count >= 8)
                {
                    listing.RemoveAt(0);
                }
                if (listing.Count < 8)
                {
                    listing.Insert(3, "");
                }

                for (int i = 0; i < listing.Count; i++)
                {
                    list.Add(listing[i]);
                }

                if (list.Count > 0)
                {

                    tester.FirstName = list[0];
                    tester.Lastname = list[1];
                    tester.Username = list[2];
                    tester.Customer = list[3];
                    tester.Role = list[4];
                    tester.Email = list[5];
                    tester.Cell = list[6];
                    tester.Password = "";
                    userTableData.Add(tester);
                }
            });

            int count = 0;

            userTableData.ForEach(user =>
            {
                var y = users.ToList().Where(x => x.FirstName == user.FirstName);
                if (y.FirstOrDefault() != null)
                    count++;
            });

            Assert.IsTrue(count == users.Count());
        }

        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
        }
    }
}