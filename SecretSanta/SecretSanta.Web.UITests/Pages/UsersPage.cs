using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace SecretSanta.Web.UITests.Pages
{
    public class UsersPage
    {
        public IWebDriver Driver { get; }
        public const string Slug = "Users";
        public IWebElement AddUserLink => Driver.FindElement(By.LinkText("Add User"));

        public UsersPage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        public List<string> UserList
        {
            get
            {
                var elements = Driver.FindElements(By.CssSelector("h1 + ul > li"));

                return elements
                    .Select(x =>
                    {
                        var text = x.Text;
                        if (text.EndsWith(" Edit Delete"))
                        {
                            text = text.Substring(0, text.Length - " Edit Delete".Length);
                        }

                        return text;
                    })
                    .ToList();
            }
        }

        public IWebElement GetEditLink(string username)
        {
            ReadOnlyCollection<IWebElement> userElements =
                Driver.FindElements(By.CssSelector("h1 + ul > li"));

            return userElements.Single(x => x.Text.StartsWith(username)).FindElement(By.CssSelector("a.button"));
        }

        public IWebElement GetDeleteLink(string username)
        {
            IReadOnlyCollection<IWebElement> deleteLinks =
                Driver.FindElements(By.CssSelector("a.is-danger"));

            return deleteLinks.Single(x => x.GetAttribute("onclick").EndsWith($"{username}?')"));
        }
    }
}
