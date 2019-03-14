using System;
using OpenQA.Selenium;

namespace SecretSanta.Web.UITests.Pages
{
    public class HomePage
    {
        public IWebDriver Driver { get; }

        public HomePage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        public IWebElement GroupsLink => Driver.FindElement(By.CssSelector("a[href=\"/Groups\"]"));
        public IWebElement UsersLink => Driver.FindElement(By.CssSelector("a[href=\"/Users\"]"));

    }
}
