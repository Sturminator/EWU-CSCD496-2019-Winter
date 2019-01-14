using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Models
{
    public class Message : Entity
    {
        public Pairing Pairing { get; set; }
        public string MessageText { get; set; }
    }
}
