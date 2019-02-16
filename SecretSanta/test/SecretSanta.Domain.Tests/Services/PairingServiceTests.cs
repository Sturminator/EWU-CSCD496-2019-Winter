using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class PairingServiceTests : DatabaseServiceTests
    {
        [TestInitialize]
        public async Task Initialize()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var userService = new UserService(context);
                var groupService = new GroupService(context);

                var group = new Group
                {
                    Name = "TestGroup"
                };

                group = await groupService.AddGroup(group);

                string[] firstNames = { "Luke", "Han", "Chris", "John", "Mary" };
                string[] lastNames = {"Skywalker", "Solo", "Sturm", "Doe", "Jane"};

                for (int i = 0; i < 5; i++)
                {
                    User user = new User
                    {
                        FirstName = firstNames[i],
                        LastName = lastNames[i]
                    };

                    user = await userService.AddUser(user);

                    await groupService.AddUserToGroup(group.Id, user.Id);
                }
            }
        }

        [TestMethod]
        public async Task GeneratePairings()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var pairingService = new PairingService(context);
                List<Pairing> pairings = await pairingService.GeneratePairings(1);

                List<int> santaIds = new List<int>();

                foreach (Pairing pairing in pairings)
                {
                    if (!santaIds.Contains(pairing.SantaId)) santaIds.Add(pairing.SantaId);
                }

                foreach (var pairing in pairings)
                {
                    Assert.AreNotEqual<int>(pairing.SantaId, pairing.RecipientId);
                }

                Assert.IsNotNull(pairings);
                Assert.IsTrue(pairings.Count == 5);
                Assert.AreEqual(pairings.Count, santaIds.Count);
            }
        }
    }
}
