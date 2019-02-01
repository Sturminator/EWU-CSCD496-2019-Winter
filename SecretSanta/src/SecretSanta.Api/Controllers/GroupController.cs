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
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _GroupService;

        public GroupController(IGroupService groupService)
        {
            _GroupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }

        [HttpPost]
        public ActionResult<DTO.Group> AddGroup(DTO.Group group)
        {
            if (group == null)
                return BadRequest();

            var returnedGroup = _GroupService.AddGroup(DTO.Group.ToDomainEntity(group));

            return new DTO.Group(returnedGroup);
        }

        [HttpPut]
        public ActionResult<DTO.Group> UpdateGroup(DTO.Group group)
        {
            if (group == null)
                return BadRequest();

            var returnedGroup = _GroupService.UpdateGroup(DTO.Group.ToDomainEntity(group));

            return new DTO.Group(returnedGroup);
        }

        [HttpDelete]
        public ActionResult<DTO.Group> DeleteGroup(DTO.Group group)
        {
            if (group == null)
                return BadRequest();

            var returnedGroup = _GroupService.RemoveGroup(DTO.Group.ToDomainEntity(group));

            return new DTO.Group(returnedGroup);
        }

        [HttpGet]
        public ActionResult<List<DTO.Group>> GetAllGroups()
        {
            List<Group> databaseGroups = _GroupService.FetchAll();

            return databaseGroups.Select(x => new DTO.Group(x)).ToList();
        }

        [HttpGet("{groupId}")]
        public ActionResult<List<DTO.User>> GetAllUsersInGroup(int groupId)
        {
            if (groupId <= 0)
                return NotFound();

            List<User> databaseGroupUsers = _GroupService.FetchAllUsersInGroup(groupId);

            return databaseGroupUsers.Select(x => new DTO.User(x)).ToList();
        }

        [HttpPost("{groupId}")]
        public ActionResult<DTO.User> AddUserToGroup(int groupId, DTO.User user)
        {
            if (groupId <= 0)
                return NotFound();

            if (user == null)
                return BadRequest();

            var returnedUser = _GroupService.AddUserToGroup(groupId, DTO.User.ToDomainEntity(user));

            if (returnedUser == null)
                return NotFound();

            return new DTO.User(returnedUser);
        }

        [HttpDelete("{groupId}")]
        public ActionResult<DTO.User> DeleteUserFromGroup(int groupId, DTO.User user)
        {
            if (groupId <= 0)
                return NotFound();

            if (user == null)
                return BadRequest();

            var returnedUser = _GroupService.RemoveUserFromGroup(groupId, DTO.User.ToDomainEntity(user));

            if (returnedUser == null)
                return NotFound();

            return new DTO.User(returnedUser);
        }

    }
}
