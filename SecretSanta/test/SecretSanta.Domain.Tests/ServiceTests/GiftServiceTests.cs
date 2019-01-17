using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System.Collections.Generic;

namespace SecretSanta.Domain.Tests.ServiceTests
{
    [TestClass]
    public class GiftServiceTests
    {
        private SqliteConnection SqliteConnection { get; set; }
        private DbContextOptions<SecretSantaDbContext> Options { get; set; }

        public Gift CreateGift()
        {
            User user = new User
            {
                FirstName = "Inigo",
                LastName = "Montoya"
            };

            Gift gift = new Gift
            {
                Title = "Test Gift",
                OrderOfImportance = 1,
                URL = "www.testURL.com",
                Description = "This is a test gift",
                User = user
            };

            return gift;
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
        public void AddGift()
        {
            using (var context = new SecretSantaDbContext(Options))
            {
                GiftService gs = new GiftService(context);
                var gift = CreateGift();

                var persistedGift = gs.AddGift(gift);

                Assert.AreEqual("Test Gift", persistedGift.Title);
                Assert.AreEqual("Inigo", persistedGift.User.FirstName);
            }
        }

        [TestMethod]
        public void UpdateGift()
        {
            
            using (var context = new SecretSantaDbContext(Options))
            {
                Gift gift = CreateGift();
                var gs = new GiftService(context);

                gs.AddGift(gift);
            }

            
            using (var context = new SecretSantaDbContext(Options))
            {
                var gs = new GiftService(context);
                Gift gift = gs.Find(1);
                gift.Title = "New & Improved Gift";

                gs.UpdateGift(gift);
            }

            
            using (var context = new SecretSantaDbContext(Options))
            {
                var gs = new GiftService(context);
                Gift gift = gs.Find(1);

                Assert.AreEqual("New & Improved Gift", gift.Title);
            }
        }

        [TestMethod]
        public void DeleteGift()
        {
            using (var context = new SecretSantaDbContext(Options))
            {
                GiftService gs = new GiftService(context);
                var gift = CreateGift();

                var persistedGift = gs.AddGift(gift);
            }

            using (var context = new SecretSantaDbContext(Options))
            {
                var gs = new GiftService(context);
                Gift gift = gs.Find(1);

                Gift persistedGift = gs.DeleteGift(gift);

                Assert.AreEqual("Test Gift", persistedGift.Title);
            }
        }
    }
}
