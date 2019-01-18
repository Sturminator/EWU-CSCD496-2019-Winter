using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.ServiceTests
{
    [TestClass]
    public class MessageServiceTests
    {
        private SqliteConnection SqliteConnection { get; set; }
        private DbContextOptions<SecretSantaDbContext> Options { get; set; }

        public Message CreateMessage(string firstNameToUser = "Luke", string lastNameToUser = "Skywalker", string firstNameFromUser = "Han",
            string lastNameFromUser = "Solo", string messageText = "Thanks for the blaster, Luke!")
        {
            User toUser = new User
            {
                FirstName = firstNameToUser,
                LastName = lastNameToUser
            };

            User fromUser = new User
            {
                FirstName = firstNameFromUser,
                LastName = lastNameFromUser
            };

            Message message = new Message
            {
                ToUser = toUser,
                FromUser = fromUser,
                MessageText = messageText
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

                Assert.AreEqual<string>("Luke", persistedMessage.ToUser.FirstName);
                Assert.AreEqual<string>("Han", persistedMessage.FromUser.FirstName);
                Assert.AreEqual<string>("Thanks for the blaster, Luke!", persistedMessage.MessageText);
            }
        }
    }
}
