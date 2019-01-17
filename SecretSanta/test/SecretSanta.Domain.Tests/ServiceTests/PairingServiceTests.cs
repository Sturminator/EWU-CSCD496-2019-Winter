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

        public Pairing CreatePairing()
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

            Pairing pairing = new Pairing
            {
                Santa =  user,
                Recipient = user2
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

                Assert.AreEqual("Luke", persistedPairing.Santa.FirstName);
                Assert.AreEqual("Han", persistedPairing.Recipient.FirstName);
            }
        }
    }
}
