using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Models
{
    public class UserGroup : Entity
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
