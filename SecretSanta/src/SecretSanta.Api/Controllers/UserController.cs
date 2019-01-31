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
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService userService)
        {
            _UserService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost]
        public ActionResult<DTO.User> AddUser(DTO.User user)
        {
            if (user == null)
                return BadRequest();

            var returnedUser = _UserService.AddUser(DTO.User.ToDomainEntity(user));

            return new DTO.User(returnedUser);
        }

        [HttpPut]
        public ActionResult<DTO.User> UpdateUser(DTO.User user)
        {
            if (user == null)
                return BadRequest();

            var returnedUser = _UserService.UpdateUser(DTO.User.ToDomainEntity(user));

            return new DTO.User(returnedUser);
        }

        [HttpGet]
        public ActionResult<List<DTO.User>> GetAllUsers()
        {
            List<User> databaseUsers = _UserService.FetchAll();

            return databaseUsers.Select(x => new DTO.User(x)).ToList();
        }
    }
}
