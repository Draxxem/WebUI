using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using WebUi.Model;
using System.Threading;

namespace WebUI
{
   public class Base
    {
        private IWebDriver driver;
        private readonly Random _random = new Random();
        //private AddUserPageContext addUser;

        public Base(IWebDriver _driver)
        {
            this.driver = _driver;
            //this.addUser = new AddUserPageContext(this.driver);
        }

        IWebElement txtFirstName => driver.FindElement(By.Name("FirstName"));
        IWebElement txtLastName => driver.FindElement(By.Name("LastName"));
        IWebElement txtUserName => driver.FindElement(By.Name("UserName"));
        IWebElement txtPassword => driver.FindElement(By.Name("Password"));
        IWebElement radioCustomerA => driver.FindElement(By.CssSelector("input[value = '15']"));
        IWebElement radioCustomerB => driver.FindElement(By.CssSelector("input[value = '16']"));
        IWebElement selectRole => driver.FindElement(By.Name("RoleId"));
        IWebElement txtEmail => driver.FindElement(By.Name("Email"));
        IWebElement txtCell => driver.FindElement(By.Name("Mobilephone"));
        IWebElement saveBtn => driver.FindElement(By.XPath("/html/body/div[3]/div[3]/button[2]"));
        IWebElement table => driver.FindElement(By.XPath("/html/body/table/tbody"));
        IWebElement btnAddUser => driver.FindElement(By.XPath("//button[text()=' Add User']"));

        public void Click_Add_User()
        {
            btnAddUser.Click();
        }
        public void CompleteUserForm(User user)
        {
            ClearForm();

            txtFirstName.SendKeys(user.FirstName);
            txtLastName.SendKeys(user.Lastname);

            txtUserName.SendKeys(user.Username + RandomNumber(1,9999));

            txtPassword.SendKeys(user.Password);

            try
            {
                if (user.Customer.Equals("Company AAA"))
                {
                    radioCustomerA.Click();
                }
                else if (user.Customer.Equals("Company BBB"))
                {
                    radioCustomerB.Click();
                }
                else
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Company does not exist");
                }
            }
            catch (AssertFailedException e)
            {
                Console.WriteLine(e);
            }

            selectRole.SendKeys(user.Role);
            txtEmail.SendKeys(user.Email);
            txtCell.SendKeys(user.Cell);
            Thread.Sleep(2000);
            saveBtn.Click();
            Thread.Sleep(2000);
        }

        public void ClearForm()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtUserName.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            txtCell.Clear();
        }

        public string GetTableData()
        {
            return table.Text;
        }
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
