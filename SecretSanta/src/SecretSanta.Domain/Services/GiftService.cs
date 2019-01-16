using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class GiftService
    {
        private SecretSantaDbContext _context { get; set; }

        public GiftService(SecretSantaDbContext context)
        {
            _context = context;
        }

        public void AddGift(Gift gift, User user)
        {
            _context.Users.Find(user).Gifts.Add(gift);
            _context.SaveChanges();
        }

        public void UpdateGift(Gift gift, User user)
        {
            DeleteGift(gift, user);
            AddGift(gift, user);
        }

        public void DeleteGift(Gift gift, User user)
        {
            _context.Users.Find(user).Gifts.Remove(gift);
            _context.SaveChanges();
        }
    }
}
