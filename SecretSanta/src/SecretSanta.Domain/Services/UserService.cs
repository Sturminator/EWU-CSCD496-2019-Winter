using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System.Linq;

namespace SecretSanta.Domain.Services
{
    public class UserService
    {
        private SecretSantaDbContext DbContext { get; set; }

        public UserService(SecretSantaDbContext context)
        {
            DbContext = context;
        }

        public User AddUser(User user)
        {
            DbContext.Users.Add(user);
            DbContext.SaveChanges();

            return user;
        }

        public User UpdateUser(User user)
        {
            DbContext.Users.Update(user);
            DbContext.SaveChanges();

            return user;
        }

        public User Find(int id)
        {
            return DbContext.Users
                .Include(u => u.UserGroups)
                .SingleOrDefault(u => u.Id == id);
        }
    }
}
