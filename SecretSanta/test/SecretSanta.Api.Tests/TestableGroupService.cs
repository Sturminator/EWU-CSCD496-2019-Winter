using System.Collections.Generic;
using SecretSanta.Domain.Services;
using SecretSanta.Domain.Models;

namespace SecretSanta.Api.Tests
{
    public class TestableGroupService : IGroupService
    {
        public Group ToReturnGroup { get; set; }
        public User ToReturnUser { get; set; }
        public List<Group> ToReturnGroupList { get; set; }
        public List<User> ToReturnUserList { get; set; }
        public Group AddGroup_Group { get; set; }
        public int AddUserToGroup_GroupId { get; set; }
        public User AddUserToGroup_User{ get; set; }
        public int FetchAllUsersInGroup_GroupId { get; set; }
        public Group RemoveGroup_Group { get; set; }
        public int RemoveUserFromGroup_GroupId { get; set; }
        public User RemoveUserFromGroup_User { get; set; }
        public Group UpdateGroup_Group { get; set; }

        public Group AddGroup(Group group)
        {
            AddGroup_Group = group;

            return ToReturnGroup;
        }

        public User AddUserToGroup(int groupId, User user)
        {
            AddUserToGroup_GroupId = groupId;
            AddUserToGroup_User = user;

            return ToReturnUser;
        }

        public List<Group> FetchAll()
        {
            return ToReturnGroupList;
        }

        public List<User> FetchAllUsersInGroup(int groupId)
        {
            FetchAllUsersInGroup_GroupId = groupId;

            return ToReturnUserList;
        }

        public Group RemoveGroup(Group group)
        {
            RemoveGroup_Group = group;

            return ToReturnGroup;
        }

        public User RemoveUserFromGroup(int groupId, User user)
        {
            RemoveUserFromGroup_GroupId = groupId;
            RemoveUserFromGroup_User = user;

            return ToReturnUser;
        }

        public Group UpdateGroup(Group group)
        {
            UpdateGroup_Group = group;

            return ToReturnGroup;
        }
    }
}
