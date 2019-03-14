using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SecretSanta.Web.UITests.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SecretSanta.Web.UITests
{
    [TestClass]
    public class UsersPageTests
    {
        protected IWebDriver Driver { get; set; }
        private const string RootUrl = "https://localhost:44332/";

        [TestInitialize]
        public void Init()
        {
            Driver = new ChromeDriver(Path.GetFullPath("."));
        }

        [TestCleanup]
        public void Cleanup()
        {       
            Driver.Quit();
            Driver.Dispose();
        }

        [TestMethod]
        public void CanNavigateToUsersPage()
        {
            Driver.Navigate().GoToUrl(RootUrl);
            var homePage = new HomePage(Driver);
            homePage.UsersLink.Click();

            Assert.IsTrue(Driver.Url.EndsWith(UsersPage.Slug));
        }

        [TestMethod]
        public void CanNavigateToAddUsersPage()
        {
            Driver.Navigate().GoToUrl(new Uri(new Uri(RootUrl), UsersPage.Slug));
            var usersPage = new UsersPage(Driver);
            usersPage.AddUserLink.Click();

            Assert.IsTrue(Driver.Url.EndsWith(AddUserPage.Slug));
        }

        [TestMethod]
        public void CanNavigateToEditUserPage()
        {
            Driver.Navigate().GoToUrl(new Uri(new Uri(RootUrl), UsersPage.Slug));
            var usersPage = new UsersPage(Driver);
            usersPage.AddUserLink.Click();

            string firstName = "First Name" + Guid.NewGuid().ToString("N");
            string lastName = "Last Name" + Guid.NewGuid().ToString("N");
            string fullName = firstName + " " + lastName;

            CreateUser(RootUrl, firstName, lastName);

            IWebElement editLink = usersPage.GetEditLink(fullName);
            var editPage = new EditUserPage(Driver);

            editLink.Click();

            Assert.AreEqual<string>(firstName, editPage.FirstNameTextBox.GetAttribute("value"));
            Assert.AreEqual<string>(lastName, editPage.LastNameTextBox.GetAttribute("value"));
        }

        [TestMethod]
        public void CanAddUser()
        {
            string firstName = "First Name" + Guid.NewGuid().ToString("N");
            string lastName = "Last Name" + Guid.NewGuid().ToString("N");
            string fullName = firstName + " " + lastName;

            UsersPage up = CreateUser(RootUrl, firstName, lastName);

            Assert.IsTrue(Driver.Url.EndsWith(UsersPage.Slug));
            List<string> users = up.UserList;
            Assert.IsTrue(users.Contains(fullName));
        }

        [TestMethod]
        public void CanEditUser()
        {
            string firstName = "First Name" + Guid.NewGuid().ToString("N");
            string lastName = "Last Name" + Guid.NewGuid().ToString("N");
            string fullName = firstName + " " + lastName;

            UsersPage up = CreateUser(RootUrl, firstName, lastName);

            up.GetEditLink(fullName).Click();

            EditUserPage eup = new EditUserPage(Driver);

            eup.FirstNameTextBox.Clear();
            eup.LastNameTextBox.Clear();

            string firstNameEdit = "First Name" + Guid.NewGuid().ToString("N");
            string lastNameEdit = "Last Name" + Guid.NewGuid().ToString("N");
            string fullNameEdit = firstNameEdit + " " + lastNameEdit;

            eup.FirstNameTextBox.SendKeys(firstNameEdit);
            eup.LastNameTextBox.SendKeys(lastNameEdit);
            eup.SubmitButton.Click();

            List<string> users = up.UserList as List<string>;
            Assert.IsTrue(users.Contains(fullNameEdit));
            Assert.IsFalse(users.Contains(fullName));
        }

        [TestMethod]
        public void CanDeleteUser()
        {
            string firstName = "First Name" + Guid.NewGuid().ToString("N");
            string lastName = "Last Name" + Guid.NewGuid().ToString("N");
            string fullName = firstName + " " + lastName;

            UsersPage up = CreateUser(RootUrl, firstName, lastName);

            IWebElement deleteLink = up.GetDeleteLink(fullName);
            deleteLink.Click();

            Driver.SwitchTo().Alert().Accept(); // handle alert      
            List<string> groupNames = up.UserList;
            Assert.IsFalse(groupNames.Contains(fullName));
        }

        private UsersPage CreateUser(string rootUrl, string firstName, string lastName)
        {
            Driver.Navigate().GoToUrl(new Uri(new Uri(rootUrl), UsersPage.Slug));
            var page = new UsersPage(Driver);
            page.AddUserLink.Click();

            var addUserPage = new AddUserPage(Driver);

            addUserPage.FirstNameTextBox.SendKeys(firstName);
            addUserPage.LastNameTextBox.SendKeys(lastName);
            addUserPage.SubmitButton.Click();
            return page;
        }
    }
}
