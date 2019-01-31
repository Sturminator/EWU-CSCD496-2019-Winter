using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    public class GroupService : IGroupService
    {
        private ApplicationDbContext DbContext { get; }

        public GroupService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Group AddGroup(Group group)
        {
            DbContext.Groups.Add(group);
            DbContext.SaveChanges();
            return group;
        }

        public Group UpdateGroup(Group group)
        {
            DbContext.Groups.Update(group);
            DbContext.SaveChanges();
            return group;
        }

        public Group RemoveGroup(Group group)
        {
            DbContext.Groups.Remove(group);
            DbContext.SaveChanges();

            return group;
        }

        public List<Group> FetchAll()
        {
            return DbContext.Groups.ToList();
        }

        public List<User> FetchAllUsersInGroup(int groupId)
        {
            return DbContext.Groups.Include(g => g.GroupUsers).SingleOrDefault(g => g.Id == groupId)
                .GroupUsers.Select(gu => gu.User).ToList();
        }

        public User AddUserToGroup(int groupId, User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            Group group = DbContext.Groups.Include(g => g.GroupUsers).SingleOrDefault(g => g.Id == groupId);

            group?.GroupUsers.Add(new GroupUser
            {
                GroupId = groupId,
                Group = group,
                UserId = user.Id,
                User = user
            });

            DbContext.SaveChanges();

            return user;
        }

        public User RemoveUserFromGroup(int groupId, User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            Group group = DbContext.Groups.Include(g => g.GroupUsers).SingleOrDefault(g => g.Id == groupId);

            GroupUser groupUser = group.GroupUsers.SingleOrDefault(gu => gu.UserId == user.Id);

            group?.GroupUsers.Remove(groupUser);

            DbContext.SaveChanges();

            return user;
        }
    }
}