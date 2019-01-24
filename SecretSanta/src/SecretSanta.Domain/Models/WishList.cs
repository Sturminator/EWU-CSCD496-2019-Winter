using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Models
{
    public class Wishlist : Entity
    {
        public User User { get; set; }
        public List<Gift> Gifts { get; set; }
    }
}
