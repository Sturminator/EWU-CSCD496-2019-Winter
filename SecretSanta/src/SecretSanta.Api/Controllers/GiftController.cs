using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _GiftService;

        public GiftController(IGiftService giftService)
        {
            _GiftService = giftService ?? throw new ArgumentNullException(nameof(giftService));
        }

        // GET api/Gift/5
        [HttpGet("{userId}")]
        public ActionResult<List<DTO.Gift>> GetGiftForUser(int userId)
        {
            if (userId <= 0)
                return NotFound();

            List<Gift> databaseUsers = _GiftService.GetGiftsForUser(userId);

            return databaseUsers.Select(x => new DTO.Gift(x)).ToList();
        }

        [HttpPost("{userId}")]
        public ActionResult<DTO.Gift> AddGift(int userId, DTO.Gift gift)
        {
            if (userId <= 0)
                return NotFound();

            if (gift == null)
                return BadRequest();

            var returnedGift = _GiftService.AddGiftToUser(userId, DTO.Gift.ToDomainEntity(gift));

            return new DTO.Gift(returnedGift);
        }

        [HttpPut("{userId}")]
        public ActionResult<DTO.Gift> UpdateGift(int userId, DTO.Gift gift)
        {
            if (userId <= 0)
                return NotFound();

            if (gift == null)
                return BadRequest();

            var returnedGift = _GiftService.UpdateGiftForUser(userId, DTO.Gift.ToDomainEntity(gift));

            return new DTO.Gift(returnedGift);
        }

        [HttpDelete]
        public ActionResult<DTO.Gift> DeleteGift(DTO.Gift gift)
        {
            if (gift == null)
                return BadRequest();

            var returnedGift = _GiftService.RemoveGift(DTO.Gift.ToDomainEntity(gift));

            return new DTO.Gift(returnedGift);
        }
    }
}
