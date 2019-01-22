﻿namespace SecretSanta.Domain.Models
{
    public class Message : Entity
    {
        public User ToUser { get; set; }
        public User FromUser { get; set; }
        public string MessageText { get; set; }
    }
}
