using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System.Linq;

namespace SecretSanta.Domain.Services
{
    public class GroupService
    {
        private SecretSantaDbContext DbContext { get; set; }

        public GroupService(SecretSantaDbContext context)
        {
            DbContext = context;
        }

        public Group AddGroup(Group group)
        {
            DbContext.Groups.Add(group);
            DbContext.SaveChanges();

            return group;
        }

        public UserGroup AddGroupMember(UserGroup ug, int groupId)
        {
            Group group = DbContext.Groups
                .Include(g => g.UserGroups)
                .SingleOrDefault(g => g.Id == groupId);

            group.UserGroups.Add(ug);
            DbContext.SaveChanges();

            return ug;
        }

        public UserGroup RemoveGroupMember(int userId, int groupId)
        {
            Group group = DbContext.Groups
                .Include(g => g.UserGroups)
                .SingleOrDefault(g => g.Id == groupId);

            UserGroup userGroup = group.UserGroups.SingleOrDefault(ug => ug.UserId == userId);

            group.UserGroups.Remove(userGroup);
            DbContext.SaveChanges();

            return userGroup;
        }

        public Group Find(int id)
        {
            return DbContext.Groups
                .Include(g => g.UserGroups)
                .SingleOrDefault(g => g.Id == id);
        }
    }
}
