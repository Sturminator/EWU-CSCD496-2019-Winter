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
    public class GroupControllerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupController_RequiresGroupService()
        {
            new GroupController(null);
        }

        [TestMethod]
        public void AddGroup_ReturnsGroupFromService()
        {
            var group = new Group
            {
                Id = 3,
                Name = "Test Group"
                
            };
            var testService = new TestableGroupService
            {
                ToReturnGroup = group
            };

            var controller = new GroupController(testService);

            ActionResult<DTO.Group> result = controller.AddGroup(new DTO.Group(group));

            Assert.IsNotNull(testService.AddGroup_Group);
            Assert.AreEqual(group.Id, result.Value.Id);
            Assert.AreEqual(group.Name, result.Value.Name);
        }

        [TestMethod]
        public void AddGroup_RequiresNonNullGroup()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);

            ActionResult<DTO.Group> result = controller.AddGroup(null);
            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.IsNull(testService.AddGroup_Group);
        }

        [TestMethod]
        public void UpdateGroup_ReturnsGroupFromService()
        {
            var group = new Group
            {
                Id = 3,
                Name = "Test Group"

            };
            var testService = new TestableGroupService
            {
                ToReturnGroup = group
            };

            var controller = new GroupController(testService);

            ActionResult<DTO.Group> result = controller.UpdateGroup(new DTO.Group(group));

            Assert.IsNotNull(testService.UpdateGroup_Group);
            Assert.AreEqual(group.Id, result.Value.Id);
            Assert.AreEqual(group.Name, result.Value.Name);
        }

        [TestMethod]
        public void UpdateGroup_RequiresNonNullGroup()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);

            ActionResult<DTO.Group> result = controller.UpdateGroup(null);
            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.IsNull(testService.UpdateGroup_Group);
        }

        [TestMethod]
        public void RemoveGroup_ReturnsGroupFromService()
        {
            var group = new Group
            {
                Id = 3,
                Name = "Test Group"

            };
            var testService = new TestableGroupService
            {
                ToReturnGroup = group
            };

            var controller = new GroupController(testService);

            ActionResult<DTO.Group> result = controller.DeleteGroup(new DTO.Group(group));

            Assert.IsNotNull(testService.RemoveGroup_Group);
            Assert.AreEqual(group.Id, result.Value.Id);
            Assert.AreEqual(group.Name, result.Value.Name);
        }

        [TestMethod]
        public void RemoveGroup_RequiresNonNullGroup()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);

            ActionResult<DTO.Group> result = controller.DeleteGroup(null);
            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.IsNull(testService.RemoveGroup_Group);
        }

        [TestMethod]
        public void FetchAll_ReturnsGroupsFromService()
        {
            var group = new Group
            {
                Id = 3,
                Name = "Test Group"

            };
            var testService = new TestableGroupService
            {
                ToReturnGroupList = new List<Group>
                {
                    group
                }
            };

            var controller = new GroupController(testService);

            ActionResult<List<DTO.Group>> result = controller.GetAllGroups();

            DTO.Group resultGroup = result.Value.Single();
            Assert.AreEqual(group.Id, resultGroup.Id);
            Assert.AreEqual(group.Name, resultGroup.Name);
        }

        [TestMethod]
        public void FetchAllUsersInGroup_ReturnsUsersFromService()
        {
            var user = new User
            {
                Id = 3,
                FirstName = "John",
                LastName = "Doe"

            };
            var testService = new TestableGroupService
            {
                ToReturnUserList = new List<User>
                {
                    user
                }
            };

            var controller = new GroupController(testService);

            ActionResult<List<DTO.User>> result = controller.GetAllUsersInGroup(3);

            DTO.User resultUser = result.Value.Single();
            Assert.AreEqual(user.Id, resultUser.Id);
            Assert.AreEqual(user.FirstName, resultUser.FirstName);
            Assert.AreEqual(user.LastName, resultUser.LastName);
        }

        [TestMethod]
        public void FetchAllUsersInGroup_RequiresPositiveGroupId()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);

            ActionResult<List<DTO.User>> result = controller.GetAllUsersInGroup(-1);

            Assert.IsTrue(result.Result is NotFoundResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.FetchAllUsersInGroup_GroupId);
        }

        [TestMethod]
        public void AddUserToGroup_ReturnsUsersFromService()
        {
            var user = new User
            {
                Id = 3,
                FirstName = "John",
                LastName = "Doe"

            };

            var testService = new TestableGroupService
            {
                ToReturnUser = user
            };

            var controller = new GroupController(testService);

            ActionResult<DTO.User> result = controller.AddUserToGroup(3, new DTO.User(user));

            Assert.IsNotNull(testService.AddUserToGroup_User);
            Assert.AreEqual(3, testService.AddUserToGroup_GroupId);
            Assert.AreEqual(user.Id, result.Value.Id);
            Assert.AreEqual(user.FirstName, result.Value.FirstName);
            Assert.AreEqual(user.LastName, result.Value.LastName);
        }

        [TestMethod]
        public void AddUserToGroup_UserNotFound()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);

            var user = new User
            {
                Id = 3,
                FirstName = "John",
                LastName = "Doe"

            };

            ActionResult<DTO.User> result = controller.AddUserToGroup(3, new DTO.User(user));

            Assert.IsTrue(result.Result is NotFoundResult);
        }

        [TestMethod]
        public void AddUserToGroup_RequiresPositiveGroupId()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);

            var user = new User
            {
                Id = 3,
                FirstName = "John",
                LastName = "Doe"

            };

            ActionResult<DTO.User> result = controller.AddUserToGroup(-1, new DTO.User(user));

            Assert.IsTrue(result.Result is NotFoundResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.AddUserToGroup_GroupId);
        }

        [TestMethod]
        public void AddUserToGroup_RequiresNonNullUser()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);

            ActionResult<DTO.User> result = controller.AddUserToGroup(3, null);

            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.IsNull(testService.AddUserToGroup_User);
        }

        [TestMethod]
        public void RemoveUserFromGroup_ReturnsUsersFromService()
        {
            var user = new User
            {
                Id = 3,
                FirstName = "John",
                LastName = "Doe"

            };

            var testService = new TestableGroupService
            {
                ToReturnUser = user
            };

            var controller = new GroupController(testService);

            ActionResult<DTO.User> result = controller.DeleteUserFromGroup(3, new DTO.User(user));

            Assert.IsNotNull(testService.RemoveUserFromGroup_User);
            Assert.AreEqual(3, testService.RemoveUserFromGroup_GroupId);
            Assert.AreEqual(user.Id, result.Value.Id);
            Assert.AreEqual(user.FirstName, result.Value.FirstName);
            Assert.AreEqual(user.LastName, result.Value.LastName);
        }

        [TestMethod]
        public void RemoveUserFromGroup_UserNotFound()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);

            var user = new User
            {
                Id = 3,
                FirstName = "John",
                LastName = "Doe"

            };

            ActionResult<DTO.User> result = controller.DeleteUserFromGroup(3, new DTO.User(user));

            Assert.IsTrue(result.Result is NotFoundResult);
        }

        [TestMethod]
        public void RemoveUserFromGroup_RequiresPositiveGroupId()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);

            var user = new User
            {
                Id = 3,
                FirstName = "John",
                LastName = "Doe"

            };

            ActionResult<DTO.User> result = controller.DeleteUserFromGroup(-1, new DTO.User(user));

            Assert.IsTrue(result.Result is NotFoundResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.RemoveUserFromGroup_GroupId);
        }

        [TestMethod]
        public void RemoveUserFromGroup_RequiresNonNullUser()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);

            ActionResult<DTO.User> result = controller.DeleteUserFromGroup(3, null);

            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.IsNull(testService.RemoveUserFromGroup_User);
        }
    }
}
