using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class UserService
    {
        private SecretSantaDbContext _context { get; set; }

        public UserService(SecretSantaDbContext context)
        {
            _context = context;
        }

        public User AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        public User Find(int id)
        {
            return _context.Users
                .Include(u => u.Gifts)
                .Include(u => u.UserGroups)
                .SingleOrDefault(u => u.Id == id);
        }
    }
}
