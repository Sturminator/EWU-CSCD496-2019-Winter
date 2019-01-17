using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System.Collections.Generic;

namespace SecretSanta.Domain.Tests.ServiceTests
{
    [TestClass]
    public class GroupServiceTests
    {
        private SqliteConnection SqliteConnection { get; set; }
        private DbContextOptions<SecretSantaDbContext> Options { get; set; }

        public Group CreateGroup()
        {

            Group group = new Group
            {
                Title = "The Best Group",
                UserGroups = new List<UserGroup>()
        };

            return group;
        }

        public User CreateUser()
        {
            User user = new User
            {
                FirstName = "Inigo",
                LastName = "Montoya"
            };

            return user;
        }

        public UserGroup CreateUserGroup(User user, Group group)
        {
            UserGroup ug = new UserGroup
            {
                GroupId = group.Id,
                Group = group,
                UserId = user.Id,
                User = user
            };

            return ug;
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
        public void GroupCreation()
        {
            using (var context = new SecretSantaDbContext(Options))
            {
                GroupService gs = new GroupService(context);
                var group = CreateGroup();

                var persistedGroup = gs.AddGroup(group);

                Assert.AreEqual("The Best Group", persistedGroup.Title);
            }
        }

        [TestMethod]
        public void AddUserToGroup()
        {
            // arrange
            using (var context = new SecretSantaDbContext(Options))
            {
                GroupService gs = new GroupService(context);
                var group = CreateGroup();

                var persistedGroup = gs.AddGroup(group);

                UserService us = new UserService(context);
                var myUser = CreateUser();

                var persistedUser = us.AddUser(myUser);
            }

            // act
            using (var context = new SecretSantaDbContext(Options))
            {
                GroupService gs = new GroupService(context);
                var fetchedGroup = gs.Find(1);

                UserService us = new UserService(context);
                var fetchedUser = us.Find(1);

                var userGroup = CreateUserGroup(fetchedUser, fetchedGroup);

                var fetchedUserGroup = gs.AddGroupMember(userGroup, fetchedUser.Id);

                // assert
                Assert.AreEqual("Inigo", fetchedUser.FirstName);
                Assert.AreEqual("The Best Group", fetchedGroup.Title);
                Assert.AreEqual(fetchedUser, fetchedUserGroup.User);
                Assert.AreEqual(fetchedGroup, fetchedUserGroup.Group);
            }
        }

        [TestMethod]
        public void RemoveUserFromGroup()
        {
            // arrange
            using (var context = new SecretSantaDbContext(Options))
            {
                GroupService gs = new GroupService(context);
                var group = CreateGroup();

                var persistedGroup = gs.AddGroup(group);

                UserService us = new UserService(context);
                var myUser = CreateUser();

                var persistedUser = us.AddUser(myUser);
            }

            // act
            using (var context = new SecretSantaDbContext(Options))
            {
                GroupService gs = new GroupService(context);
                var fetchedGroup = gs.Find(1);

                UserService us = new UserService(context);
                var fetchedUser = us.Find(1);

                var userGroup = CreateUserGroup(fetchedUser, fetchedGroup);

                gs.AddGroupMember(userGroup, fetchedUser.Id);

                var fetchedUserGroup = gs.RemoveGroupMember(fetchedUser.Id, fetchedGroup.Id);

                // assert
                Assert.AreEqual("Inigo", fetchedUser.FirstName);
                Assert.AreEqual("The Best Group", fetchedGroup.Title);
                Assert.IsNull(fetchedUserGroup.Group);
                Assert.AreEqual(0, fetchedGroup.UserGroups.Count);
                Assert.AreEqual(0, fetchedUser.UserGroups.Count);
            }
        }
    }
}
