using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.ServiceTests
{
    [TestClass]
    public class UserServiceTests
    {
        private SqliteConnection SqliteConnection { get; set; }
        private DbContextOptions<SecretSantaDbContext> Options { get; set; }

        public User CreateUser()
        {
            User user = new User
            {
                FirstName = "Inigo",
                LastName = "Montoya"
            };

            return user;
        }

        [TestInitialize]
        public void OpenConnection()
        {
            SqliteConnection = new SqliteConnection("DataSource=:memory:");
            SqliteConnection.Open();

            Options = new DbContextOptionsBuilder<SecretSantaDbContext>()
                .UseSqlite(SqliteConnection)
                .Options;

            using (var context = new SecretSantaDbContext(Options))
            {
                context.Database.EnsureCreated();
            }
        }

        [TestCleanup]
        public void CloseConnection()
        {
            SqliteConnection.Close();
        }

        [TestMethod]
        public void AddUser()
        {
            using (var context = new SecretSantaDbContext(Options))
            {
                UserService us = new UserService(context);
                var myUser = CreateUser();

                var persistedUser = us.AddUser(myUser);

                Assert.AreEqual("Inigo", persistedUser.FirstName);
            }
        }

        [TestMethod]
        public void FindUser()
        {
            // arrange
            using (var context = new SecretSantaDbContext(Options))
            {
                UserService us = new UserService(context);
                var myUser = CreateUser();

                var persistedUser = us.AddUser(myUser);
            }

            // act
            using (var context = new SecretSantaDbContext(Options))
            {
                UserService us = new UserService(context);
                var fetchedUser = us.Find(1);

                // assert
                Assert.AreEqual("Inigo", fetchedUser.FirstName);
            }
        }

        [TestMethod]
        public void UpdateUser()
        {
            // arrange
            using (var context = new SecretSantaDbContext(Options))
            {
                User user = CreateUser();
                var us = new UserService(context);

                us.AddUser(user);
            }

            // act
            using (var context = new SecretSantaDbContext(Options))
            {
                var us = new UserService(context);
                User user = us.Find(1);
                user.FirstName = "Chris";

                us.UpdateUser(user);
            }

            // find
            using (var context = new SecretSantaDbContext(Options))
            {
                var us = new UserService(context);
                User user = us.Find(1);

                Assert.AreEqual("Chris", user.FirstName);
            }
        }
    }
}
