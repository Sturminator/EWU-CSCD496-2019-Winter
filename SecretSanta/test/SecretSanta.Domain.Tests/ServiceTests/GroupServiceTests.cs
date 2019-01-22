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

        public Group CreateGroup(string title = "The Best Group")
        {

            Group group = new Group
            {
                Title = title,
                UserGroups = new List<UserGroup>()
            };

            return group;
        }

        public User CreateUser(string firstName = "Inigo", string lastName = "Montoya")
        {
            User user = new User
            {
                FirstName = firstName,
                LastName = lastName
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

                Assert.AreEqual<string>("The Best Group", persistedGroup.Title);
            }
        }

        [TestMethod]
        public void AddUserToGroup()
        {
            using (var context = new SecretSantaDbContext(Options))
            {
                GroupService gs = new GroupService(context);
                var group = CreateGroup();

                var persistedGroup = gs.AddGroup(group);

                UserService us = new UserService(context);
                var myUser = CreateUser();

                var persistedUser = us.AddUser(myUser);
            }

            using (var context = new SecretSantaDbContext(Options))
            {
                GroupService gs = new GroupService(context);
                var fetchedGroup = gs.Find(1);

                UserService us = new UserService(context);
                var fetchedUser = us.Find(1);

                var userGroup = CreateUserGroup(fetchedUser, fetchedGroup);

                var fetchedUserGroup = gs.AddGroupMember(userGroup, fetchedUser.Id);

                Assert.AreEqual<string>("Inigo", fetchedUser.FirstName);
                Assert.AreEqual<string>("The Best Group", fetchedGroup.Title);
                Assert.AreEqual<User>(fetchedUser, fetchedUserGroup.User);
                Assert.AreEqual<Group>(fetchedGroup, fetchedUserGroup.Group);
            }
        }

        [TestMethod]
        public void RemoveUserFromGroup()
        {
            using (var context = new SecretSantaDbContext(Options))
            {
                GroupService gs = new GroupService(context);
                var group = CreateGroup();

                var persistedGroup = gs.AddGroup(group);

                UserService us = new UserService(context);
                var myUser = CreateUser();

                var persistedUser = us.AddUser(myUser);
            }

            using (var context = new SecretSantaDbContext(Options))
            {
                GroupService gs = new GroupService(context);
                var fetchedGroup = gs.Find(1);

                UserService us = new UserService(context);
                var fetchedUser = us.Find(1);

                var userGroup = CreateUserGroup(fetchedUser, fetchedGroup);

                gs.AddGroupMember(userGroup, fetchedUser.Id);

                var fetchedUserGroup = gs.RemoveGroupMember(fetchedUser.Id, fetchedGroup.Id);

                Assert.AreEqual<string>("Inigo", fetchedUser.FirstName);
                Assert.AreEqual<string>("The Best Group", fetchedGroup.Title);
                Assert.IsNull(fetchedUserGroup.Group);
                Assert.AreEqual<int>(0, fetchedGroup.UserGroups.Count);
                Assert.AreEqual<int>(0, fetchedUser.UserGroups.Count);
            }
        }
    }
}
