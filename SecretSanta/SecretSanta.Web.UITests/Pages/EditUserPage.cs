using OpenQA.Selenium;
using System;
using System.Linq;

namespace SecretSanta.Web.UITests.Pages
{
    public class EditUserPage
    {
        IWebDriver Driver { get; }
        public IWebElement FirstNameTextBox => Driver.FindElement(By.Id("FirstName"));
        public IWebElement LastNameTextBox => Driver.FindElement(By.Id("LastName"));

        public EditUserPage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        public IWebElement SubmitButton =>
            Driver
                .FindElements(By.CssSelector("button.is-primary"))
                .Single(x => x.Text == "Submit");
    }
}
