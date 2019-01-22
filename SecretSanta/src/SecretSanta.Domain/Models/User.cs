using System.Collections.Generic;

namespace SecretSanta.Domain.Models
{
    public class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
    }
}
