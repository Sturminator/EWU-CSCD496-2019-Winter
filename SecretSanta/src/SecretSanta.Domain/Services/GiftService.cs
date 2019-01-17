using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Gift AddGift(Gift gift)
        {
            _context.Gifts.Add(gift);
            _context.SaveChanges();

            return gift;
        }

        public Gift UpdateGift(Gift gift)
        {
            _context.Gifts.Update(gift);
            _context.SaveChanges();

            return gift;
        }

        public Gift DeleteGift(Gift gift)
        {
            _context.Gifts.Remove(gift);
            _context.SaveChanges();

            return gift;
        }

        public Gift Find(int id)
        {
            return _context.Gifts
                .Include(g => g.User)
                .SingleOrDefault(g => g.Id == id);
        }
    }
}
