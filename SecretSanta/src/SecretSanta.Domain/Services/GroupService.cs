using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class GroupService
    {
        private SecretSantaDbContext _context { get; set; }

        public GroupService(SecretSantaDbContext context)
        {
            _context = context;
        }

        public Group AddGroup(Group group)
        {
            _context.Groups.Add(group);
            _context.SaveChanges();

            return group;
        }

        public UserGroup AddGroupMember(UserGroup ug, int groupId)
        {
            Group group = _context.Groups
                .Include(g => g.UserGroups)
                .SingleOrDefault(g => g.Id == groupId);

            group.UserGroups.Add(ug); 
            _context.SaveChanges();

            return ug;
        }

        public UserGroup RemoveGroupMember(int userId, int groupId)
        {
            Group group = _context.Groups
                .Include(g => g.UserGroups)
                .SingleOrDefault(g => g.Id == groupId);

            UserGroup userGroup = group.UserGroups.SingleOrDefault(ug => ug.UserId == userId);

            group.UserGroups.Remove(userGroup);
            _context.SaveChanges();

            return userGroup;
        }

        public Group Find(int id)
        {
            return _context.Groups
                .Include(g => g.UserGroups)
                .SingleOrDefault(g => g.Id == id);
        }
    }
}
