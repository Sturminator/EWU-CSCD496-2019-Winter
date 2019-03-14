using System;
using OpenQA.Selenium;

namespace SecretSanta.Web.UITests.Pages
{
    public class HomePage
    {
        public IWebDriver Driver { get; }
        public UsersPage UsersPage => new UsersPage(Driver);
        public IWebElement UsersLink => Driver.FindElement(By.LinkText("Users"));

        public HomePage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
    }
}
