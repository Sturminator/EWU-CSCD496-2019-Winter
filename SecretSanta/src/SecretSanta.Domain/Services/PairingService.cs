using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class PairingService
    {
        private SecretSantaDbContext _context;

        public PairingService(SecretSantaDbContext context)
        {
            _context = context;
        }

        public void AddPairing(Pairing pairing)
        {
            _context.Pairings.Add(pairing);
            _context.SaveChanges();
        }
    }
}
