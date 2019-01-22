using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.ServiceTests
{
    [TestClass]
    public class PairingServiceTests
    {
        private SqliteConnection SqliteConnection { get; set; }
        private DbContextOptions<SecretSantaDbContext> Options { get; set; }

        public Pairing CreatePairing(string firstNameSanta = "Luke", string lastNameSanta = "Skywalker", string firstNameRecipient = "Han",
            string lastNameRecipient = "Solo")
        {
            User santa = new User
            {
                FirstName = firstNameSanta,
                LastName = lastNameSanta
            };

            User recipient = new User
            {
                FirstName = firstNameRecipient,
                LastName = lastNameRecipient
            };

            Pairing pairing = new Pairing
            {
                Santa = santa,
                Recipient = recipient
            };

            return pairing;
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
        public void AddPairing()
        {
            using (var context = new SecretSantaDbContext(Options))
            {
                PairingService ps = new PairingService(context);
                var pairing = CreatePairing();

                var persistedPairing = ps.AddPairing(pairing);

                Assert.AreEqual<string>("Luke", persistedPairing.Santa.FirstName);
                Assert.AreEqual<string>("Han", persistedPairing.Recipient.FirstName);
            }
        }
    }
}
