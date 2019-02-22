﻿using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests
{
    public class TestableGiftService : IGiftService
    {
        public List<Gift> ToReturn { get; set; }
        public int GetGiftsForUser_UserId { get; set; }

        public async Task<List<Gift>> GetGiftsForUser(int userId)
        {
            GetGiftsForUser_UserId = userId;
            return await Task.FromResult(ToReturn);
        }
    }
}
