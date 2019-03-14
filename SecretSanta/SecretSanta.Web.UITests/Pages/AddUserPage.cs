using System;
using System.Linq;
using OpenQA.Selenium;

namespace SecretSanta.Web.UITests.Pages
{
    public class AddUserPage
    {
        public const string Slug = UsersPage.Slug + "/Add";
        public IWebDriver Driver { get; }
        public IWebElement FirstNameTextBox => Driver.FindElement(By.Id("FirstName"));
        public IWebElement LastNameTextBox => Driver.FindElement(By.Id("LastName"));

        public AddUserPage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        public IWebElement SubmitButton =>
            Driver
                .FindElements(By.CssSelector("button.is-primary"))
                .Single(x => x.Text == "Submit");
    }
}
