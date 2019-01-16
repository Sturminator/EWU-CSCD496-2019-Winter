using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Services
{
    class MessageService
    {
        private SecretSantaDbContext _context { get; set; }

        public MessageService(SecretSantaDbContext context)
        {
            _context = context;
        }

        public void StoreMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
        }
    }
}
