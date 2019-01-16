using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
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

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
            _context.SaveChanges();
        }

        public void AddGroupMember(Group group, User user)
        {
            _context.Groups.Find(group).Users.Add(user);
            _context.SaveChanges();
        }

        public void RemoveGroupMember(Group group, User user)
        {
            _context.Groups.Find(group).Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
