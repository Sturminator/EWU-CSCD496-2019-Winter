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
    public class GroupsController : ControllerBase
    {
        private IGroupService GroupService { get; }
        private IMapper Mapper { get; }
        private ILogger<GroupsController> Logger { get; }

        public GroupsController(IGroupService groupService, IMapper mapper, ILogger<GroupsController> logger)
        {
            GroupService = groupService;
            Mapper = mapper;
            Logger = logger;
        }

        // GET api/group
        [HttpGet]
        public async Task<ActionResult<ICollection<GroupViewModel>>> GetGroups()
        {
            Logger.LogDebug($"Calling GroupService.FetchAll");
            var groups = await GroupService.FetchAll();

            Logger.LogInformation($"Success: Groups retrieved");
            return Ok(groups.Select(x => Mapper.Map<GroupViewModel>(x)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupViewModel>> GetGroup(int id)
        {
            Logger.LogDebug($"Calling GroupService.GetById with id {id}");
            var group = await GroupService.GetById(id);
            if (group == null)
            {
                Logger.LogError($"Error (404): Group could not be found with id {id}");
                return NotFound();
            }

            Logger.LogInformation($"Success: Group retrieved with id {id}");
            return Ok(Mapper.Map<GroupViewModel>(group));
        }

        // POST api/group
        [HttpPost]
        public async Task<ActionResult<GroupViewModel>> CreateGroup(GroupInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                Logger.LogError($"Error (400): Parameter viewModel cannot be null");
                return BadRequest();
            }

            Logger.LogDebug($"Calling GroupService.AddGroup with new group {viewModel.Name}");
            var createdGroup = await GroupService.AddGroup(Mapper.Map<Group>(viewModel));

            Logger.LogInformation($"Success: Group created. Returning new group of id {createdGroup.Id}");
            return CreatedAtAction(nameof(GetGroup), new { id = createdGroup.Id}, Mapper.Map<GroupViewModel>(createdGroup));
        }

        // PUT api/group/5
        [HttpPut]
        public async Task<ActionResult> UpdateGroup(int id, GroupInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                Logger.LogError($"Error (400): Parameter viewModel cannot be null");
                return BadRequest();
            }

            Logger.LogDebug($"Calling GroupService.GetById with id {id}");
            var group = await GroupService.GetById(id);
            if (group == null)
            {
                Logger.LogError($"Error (404): Group not found with id {id}");
                return NotFound();
            }

            Mapper.Map(viewModel, group);

            Logger.LogDebug($"Calling GroupService.UpdateGroup for group {group.Name}");
            await GroupService.UpdateGroup(group);

            Logger.LogInformation($"Success: Group updated.");
            return NoContent();
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            if (id <= 0)
            {
                Logger.LogError($"Error (400): Parameter id cannot be less than 1");
                return BadRequest("A group id must be specified");
            }

            Logger.LogDebug($"Calling GroupService.DeleteGroup with id {id}");
            if (await GroupService.DeleteGroup(id))
            {
                Logger.LogInformation($"Success: Group deleted.");
                return Ok();
            }

            Logger.LogError($"Error (404): Group not found with id {id}");
            return NotFound();
        }
    }
}
