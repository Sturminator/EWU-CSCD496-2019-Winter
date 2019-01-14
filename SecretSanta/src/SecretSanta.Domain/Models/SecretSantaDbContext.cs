using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Models
{
    public class SecretSantaDbContext : DbContext
    {
        public SecretSantaDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Pairing> Pairings { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
