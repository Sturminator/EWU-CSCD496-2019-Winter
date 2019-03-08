using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftsController : ControllerBase
    {
        private IGiftService GiftService { get; }
        private IMapper Mapper { get; }
        private ILogger Logger { get; }

        public GiftsController(IGiftService giftService, IMapper mapper, ILogger logger)
        {
            GiftService = giftService;
            Mapper = mapper;
            Logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<GiftViewModel>> GetGift(int id)
        {
            Logger.LogDebug($"Calling GiftService.GetGift with id {id}");
            var gift = await GiftService.GetGift(id);

            if (gift == null)
            {
                Logger.LogError($"Error (404): Gift could not be found with id {id}");
                return NotFound();
            }

            Logger.LogInformation($"Success: Gift found with id {id}");
            return Ok(Mapper.Map<GiftViewModel>(gift));
        }

        [HttpPost]
        public async Task<ActionResult<GiftViewModel>> CreateGift(GiftInputViewModel viewModel)
        {
            Logger.LogDebug($"Calling GiftService.AddGift with gift {viewModel.Title}");
            var createdGift = await GiftService.AddGift(Mapper.Map<Gift>(viewModel));

            Logger.LogInformation($"Success: Gift created. Returning new gift of id {createdGift.Id}");
            return CreatedAtAction(nameof(GetGift), new { id = createdGift.Id }, Mapper.Map<GiftViewModel>(createdGift));
        }

        // GET api/Gift/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ICollection<GiftViewModel>>> GetGiftsForUser(int userId)
        {
            if (userId <= 0)
            {
                Logger.LogError($"Error (404): UserId {userId} cannot be less than 1");
                return NotFound();
            }

            Logger.LogDebug($"Calling GiftService.GetGiftsForUser with userId {userId}");
            List<Gift> databaseUsers = await GiftService.GetGiftsForUser(userId);

            Logger.LogInformation($"Success: Gifts retrieved for user with userId {userId}");
            return Ok(databaseUsers.Select(x => Mapper.Map<GiftViewModel>(x)).ToList());
        }
    }
}
