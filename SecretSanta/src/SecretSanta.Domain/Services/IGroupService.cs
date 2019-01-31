using System;
using System.Collections.Generic;
using System.Text;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    public interface IGroupService
    {
        Group AddGroup(Group group);

        Group UpdateGroup(Group group);

        Group RemoveGroup(Group group);

        List<Group> FetchAll();

        List<User> FetchAllUsersInGroup(int groupId);

        User AddUserToGroup(int groupId, User user);

        User RemoveUserFromGroup(int groupId, User user);
    }
}
