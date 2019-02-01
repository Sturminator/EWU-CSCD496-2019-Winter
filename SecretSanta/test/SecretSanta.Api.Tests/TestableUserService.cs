using System.Collections.Generic;
using SecretSanta.Domain.Services;
using SecretSanta.Domain.Models;

namespace SecretSanta.Api.Tests
{
    public class TestableUserService : IUserService
    {
        public List<User> ToReturnList { get; set; }
        public User ToReturn { get; set; }
        public User AddUser_User { get; set; }
        public User RemoveUser_User { get; set; }
        public User UpdateUser_User { get; set; }

        public User AddUser(User user)
        {
            AddUser_User = user;
            return ToReturn;
        }

        public List<User> FetchAll()
        {
            return ToReturnList;
        }

        public User RemoveUser(User user)
        {
            RemoveUser_User = user;
            return ToReturn;
        }

        public User UpdateUser(User user)
        {
            UpdateUser_User = user;
            return ToReturn;
        }
    }
}
