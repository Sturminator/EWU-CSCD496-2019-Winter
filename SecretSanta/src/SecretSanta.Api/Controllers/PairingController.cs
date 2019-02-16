using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PairingController : ControllerBase
    {
        private IPairingService PairingService { get; }
        private IMapper Mapper { get; }

        public PairingController(IPairingService pairingService, IMapper mapper)
        {
            PairingService = pairingService;
            Mapper = mapper;
        }

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> GeneratePairings(int groupId)
        {
            if (groupId < 1)
                return BadRequest("No ID given.");


            List<Pairing> pairings = await PairingService.GeneratePairings(groupId);

            if (pairings.Count < 1 || pairings == null)
                return BadRequest("Issue generating pairings.");

            return Created($"/Pairing/{groupId}", pairings.Select(p => Mapper.Map<PairingViewModel>(p)).ToList());
        }
    }
}
