using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Api.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserController_RequiresUserService()
        {
            new UserController(null);
        }

        [TestMethod]
        public void AddUser_ReturnsUserFromService()
        {
            var user = new User
            {
                Id = 3,
                FirstName = "John",
                LastName = "Doe"
            };
            var testService = new TestableUserService
            {
                ToReturn = user
            };

            var controller = new UserController(testService);

            ActionResult<DTO.User> result = controller.AddUser(new DTO.User(user));

            Assert.IsNotNull(testService.AddUser_User);
            Assert.AreEqual(user.Id, result.Value.Id);
            Assert.AreEqual(user.FirstName, result.Value.FirstName);
            Assert.AreEqual(user.LastName, result.Value.LastName);
        }

        [TestMethod]
        public void AddUser_RequiresNonNullUser()
        {
            var testService = new TestableUserService();
            var controller = new UserController(testService);

            ActionResult<DTO.User> result = controller.AddUser(null);
            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.IsNull(testService.AddUser_User);
        }

        [TestMethod]
        public void FetchAll_ReturnsUsersFromService()
        {
            var user = new User
            {
                Id = 3,
                FirstName = "John",
                LastName = "Doe"
            };
            var testService = new TestableUserService
            {
                ToReturnList = new List<User>
                {
                    user
                }
            };

            var controller = new UserController(testService);

            ActionResult<List<DTO.User>> result = controller.GetAllUsers();

            DTO.User resultUser = result.Value.Single();
            Assert.AreEqual(user.Id, resultUser.Id);
            Assert.AreEqual(user.FirstName, resultUser.FirstName);
            Assert.AreEqual(user.LastName, resultUser.LastName);
        }

        [TestMethod]
        public void RemoveUser_ReturnsUserFromService()
        {
            var user = new User
            {
                Id = 3,
                FirstName = "John",
                LastName = "Doe"
            };
            var testService = new TestableUserService
            {
                ToReturn = user
            };

            var controller = new UserController(testService);

            ActionResult<DTO.User> result = controller.DeleteUser(new DTO.User(user));

            Assert.IsNotNull(testService.RemoveUser_User);
            Assert.AreEqual(user.Id, result.Value.Id);
            Assert.AreEqual(user.FirstName, result.Value.FirstName);
            Assert.AreEqual(user.LastName, result.Value.LastName);
        }

        [TestMethod]
        public void RemoveUser_RequiresNonNullUser()
        {
            var testService = new TestableUserService();
            var controller = new UserController(testService);

            ActionResult<DTO.User> result = controller.DeleteUser(null);
            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.IsNull(testService.RemoveUser_User);
        }

        [TestMethod]
        public void UpdateUser_ReturnsUserFromService()
        {
            var user = new User
            {
                Id = 3,
                FirstName = "John",
                LastName = "Doe"
            };
            var testService = new TestableUserService
            {
                ToReturn = user
            };

            var controller = new UserController(testService);

            ActionResult<DTO.User> result = controller.UpdateUser(new DTO.User(user));

            Assert.IsNotNull(testService.UpdateUser_User);
            Assert.AreEqual(user.Id, result.Value.Id);
            Assert.AreEqual(user.FirstName, result.Value.FirstName);
            Assert.AreEqual(user.LastName, result.Value.LastName);
        }

        [TestMethod]
        public void UpdateUser_RequiresNonNullUser()
        {
            var testService = new TestableUserService();
            var controller = new UserController(testService);

            ActionResult<DTO.User> result = controller.UpdateUser(null);
            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.IsNull(testService.UpdateUser_User);
        }
    }
}
