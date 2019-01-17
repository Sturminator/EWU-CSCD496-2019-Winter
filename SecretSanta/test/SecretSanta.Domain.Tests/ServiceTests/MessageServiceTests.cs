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
    public class MessageServiceTests
    {
        private SqliteConnection SqliteConnection { get; set; }
        private DbContextOptions<SecretSantaDbContext> Options { get; set; }

        public Message CreateMessage()
        {
            User user = new User
            {
                FirstName = "Luke",
                LastName = "Skywalker"
            };

            User user2 = new User
            {
                FirstName = "Han",
                LastName = "Solo"
            };

            Message message = new Message
            {
                ToUser = user,
                FromUser = user2,
                MessageText = "Thanks for the blaster, Luke!"
            };

            return message;
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
        public void StoreMessage()
        {
            using (var context = new SecretSantaDbContext(Options))
            {
                MessageService ms = new MessageService(context);
                var message = CreateMessage();

                var persistedMessage = ms.StoreMessage(message);

                Assert.AreEqual("Luke", persistedMessage.ToUser.FirstName);
                Assert.AreEqual("Han", persistedMessage.FromUser.FirstName);
                Assert.AreEqual("Thanks for the blaster, Luke!", persistedMessage.MessageText);
            }
        }
    }
}
