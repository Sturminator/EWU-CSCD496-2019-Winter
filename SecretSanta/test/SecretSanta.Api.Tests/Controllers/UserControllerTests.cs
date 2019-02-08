using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Services.Interfaces;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public void CreateUser_ReturnsCreatedUser()
        {
            var user = new UserInputViewModel   
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var service = new Mock<IUserService>();
            service.Setup(x => x.AddUser(It.Is<Domain.Models.User>(u => u.FirstName == u.FirstName)))
                .Returns(new Domain.Models.User
                {
                    Id = 2,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                })
                .Verifiable();

            var controller = new UserController(service.Object, Mapper.Instance);

            var result = controller.Post(user);

            var resultValue = (UserViewModel)((OkObjectResult)result).Value;

            Assert.AreEqual(2, resultValue.Id);
            Assert.AreEqual("John", resultValue.FirstName);
            Assert.AreEqual("Doe", resultValue.LastName);
            service.VerifyAll();
        }

        [TestMethod]
        public void CreateUser_RequiresFirstName()
        {
            var user = new UserInputViewModel
            {
                FirstName = null,
                LastName = "Doe"
            };

            var service = new Mock<IUserService>();
            service.Setup(x => x.AddUser(It.Is<Domain.Models.User>(u => u.FirstName != null)))
                .Returns(new Domain.Models.User
                {
                    Id = 2,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                })
                .Verifiable();

            var controller = new UserController(service.Object, Mapper.Instance);

            var result = controller.Post(user);

            var resultValue = (UserViewModel)((OkObjectResult)result).Value;

            Assert.IsNull(resultValue);
        }
    }
}
