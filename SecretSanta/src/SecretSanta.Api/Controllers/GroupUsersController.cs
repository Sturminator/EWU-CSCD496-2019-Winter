using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    public class GroupUsersController : ControllerBase
    {
        private IGroupService GroupService { get; }
        private ILogger Logger { get; }

        public GroupUsersController(IGroupService groupService, ILogger logger)
        {
            GroupService = groupService;
            Logger = logger;
        }

        [HttpPut("{groupId}")]
        public async Task<ActionResult> AddUserToGroup(int groupId, int userId)
        {
            if (groupId <= 0)
            {
                Logger.LogError($"Error (400): Parameter groupId cannot be less than 1");
                return BadRequest();
            }

            if (userId <= 0)
            {
                Logger.LogError($"Error (400): Parameter userId cannot be less than 1");
                return BadRequest();
            }

            Logger.LogDebug($"Calling GroupService.AddUserToGroup with groupId {groupId} and userId {userId}");
            if (await GroupService.AddUserToGroup(groupId, userId))
            {
                Logger.LogInformation($"Success: User added to group {groupId} with userId {userId}");
                return Ok();
            }

            Logger.LogError($"Error (404): Group could not be found with groupId {groupId}");
            return NotFound();
        }

        [HttpDelete("{groupId}")]
        public async Task<ActionResult> RemoveUserFromGroup(int groupId, int userId)
        {
            if (groupId <= 0)
            {
                Logger.LogError($"Error (400): Parameter groupId cannot be less than 1");
                return BadRequest();
            }

            if (userId <= 0)
            {
                Logger.LogError($"Error (400): Parameter userId cannot be less than 1");
                return BadRequest();
            }

            Logger.LogDebug($"Calling GroupService.RemoveUserFromGroup with groupId {groupId} and userId {userId}");
            if (await GroupService.RemoveUserFromGroup(groupId, userId))
            {
                Logger.LogInformation($"Success: User removed from group {groupId} with userId {userId}");
                return Ok();
            }

            Logger.LogError($"Error (404): Group could not be found with groupId {groupId}");
            return NotFound();
        }
    }
}
