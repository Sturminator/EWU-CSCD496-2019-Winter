using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class PairingControllerTests
    {
        public List<Pairing> CreatePairings()
        {
            return new List<Pairing>
            {
                new Pairing {Id = 1, SantaId = 1, RecipientId = 2},
                new Pairing {Id = 2, SantaId = 2, RecipientId = 3},
                new Pairing {Id = 3, SantaId = 3, RecipientId = 4},
                new Pairing {Id = 4, SantaId = 4, RecipientId = 5},
                new Pairing {Id = 5, SantaId = 5, RecipientId = 1}
            };
        }

        [TestMethod]
        public async Task GeneratePairings_RequiresPositiveId()
        {
            var service = new Mock<IPairingService>(MockBehavior.Strict);
            var controller = new PairingController(service.Object, Mapper.Instance);

            var result = await controller.GeneratePairings(-1);

            Assert.IsTrue(result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task GeneratePairings_ReturnResult()
        {
            var pairings = CreatePairings();

            var service = new Mock<IPairingService>();
            service.Setup(x => x.GeneratePairings(It.IsAny<int>()))
                .ReturnsAsync(pairings)
                .Verifiable();

            var controller = new PairingController(service.Object, Mapper.Instance);
            var result = await controller.GeneratePairings(1) as CreatedResult;

            var returnPairings = result?.Value as List<PairingViewModel>;

            Assert.IsNotNull(returnPairings);

            Assert.AreEqual(1, returnPairings[0].Id);
            Assert.AreEqual(1, returnPairings[0].SantaId);
            Assert.AreEqual(2, returnPairings[0].RecipientId);

            Assert.AreEqual(2, returnPairings[1].Id);
            Assert.AreEqual(2, returnPairings[1].SantaId);
            Assert.AreEqual(3, returnPairings[1].RecipientId);

            Assert.AreEqual(3, returnPairings[2].Id);
            Assert.AreEqual(3, returnPairings[2].SantaId);
            Assert.AreEqual(4, returnPairings[2].RecipientId);

            Assert.AreEqual(4, returnPairings[3].Id);
            Assert.AreEqual(4, returnPairings[3].SantaId);
            Assert.AreEqual(5, returnPairings[3].RecipientId);

            Assert.AreEqual(5, returnPairings[4].Id);
            Assert.AreEqual(5, returnPairings[4].SantaId);
            Assert.AreEqual(1, returnPairings[4].RecipientId);

            service.VerifyAll();
        }
    }
}
