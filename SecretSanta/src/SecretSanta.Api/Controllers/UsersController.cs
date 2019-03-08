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
    public class UsersController : ControllerBase
    {
        private IUserService UserService { get; }
        private IMapper Mapper { get; }
        private ILogger<UsersController> Logger { get; }

        public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> logger)
        {
            UserService = userService;
            Mapper = mapper;
            Logger = logger;
        }

        // GET api/User
        [HttpGet]
        public async Task<ActionResult<ICollection<UserViewModel>>> GetAllUsers()
        {
            Logger.LogDebug($"Calling UserService.FetchAll");
            var users = await UserService.FetchAll();

            Logger.LogInformation($"Success: Users retrieved");
            return Ok(users.Select(x => Mapper.Map<UserViewModel>(x)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> GetUser(int id)
        {
            Logger.LogDebug($"Calling UserService.GetById with id {id}");
            var fetchedUser = await UserService.GetById(id);

            if (fetchedUser == null)
            {
                Logger.LogError($"Error (404): User could not be found with id {id}");
                return NotFound();
            }

            Logger.LogInformation($"Success: User retrieved with id {id}");
            return Ok(Mapper.Map<UserViewModel>(fetchedUser));
        }

        // POST api/User
        [HttpPost]
        public async Task<ActionResult<UserViewModel>> CreateUser(UserInputViewModel viewModel)
        {
            if (User == null)
            {
                Logger.LogError($"Error (400): Parameter viewModel cannot be null");
                return BadRequest();
            }

            Logger.LogDebug($"Calling UserService.AddUser with new user {viewModel.FirstName} {viewModel.LastName}");
            var createdUser = await UserService.AddUser(Mapper.Map<User>(viewModel));

            Logger.LogInformation($"Success: User created. Returning new user of id {createdUser.Id}");
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, Mapper.Map<UserViewModel>(createdUser));
        }

        // PUT api/User/5
        [HttpPut]
        public async Task<ActionResult> UpdateUser(int id, UserInputViewModel viewModel)
        {
            if (viewModel == null)
            {
                Logger.LogError($"Error (400): Parameter viewModel cannot be null");
                return BadRequest();
            }

            Logger.LogDebug($"Calling UserService.GetById with id {id}");
            var fetchedUser = await UserService.GetById(id);

            if (fetchedUser == null)
            {
                Logger.LogError($"Error (404): User not found with id {id}");
                return NotFound();
            }

            Mapper.Map(viewModel, fetchedUser);

            Logger.LogDebug($"Calling UserService.UpdateUser for user {fetchedUser.FirstName} {fetchedUser.LastName}");
            await UserService.UpdateUser(fetchedUser);

            Logger.LogInformation($"Success: User updated.");
            return NoContent();
        }

        // DELETE api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if (id <= 0)
            {
                Logger.LogError($"Error (400): Parameter id cannot be less than 1");
                return BadRequest("A User id must be specified");
            }

            if (await UserService.DeleteUser(id))
            {
                Logger.LogInformation($"Success: User deleted.");
                return Ok();
            }

            Logger.LogError($"Error (404): User not found with id {id}");
            return NotFound();
        }
    }
}
