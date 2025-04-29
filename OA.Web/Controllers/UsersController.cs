using Microsoft.AspNetCore.Mvc;
using OA.Data;
using OA.Service;
using OA.Web.Dtos;

namespace OA.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            _logger.LogInformation("Retrieving all users.");
            var users = _userService.GetUsers();
            _logger.LogInformation("Retrieved {UserCount} users.", users?.Count() ?? 0);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(long id)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", id);
            var user = _userService.GetUser(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return NotFound();
            }
            _logger.LogInformation("User with ID {UserId} found: {UserName}", id, user.UserName);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user creation.");
                return BadRequest(ModelState);
            }
            _logger.LogInformation("Creating new user with username: {UserName}", dto.UserName);

            var now = DateTime.UtcNow;

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Password = dto.Password,
                AddedDate = now,
                ModifiedDate = now
            };

            _userService.InsertUser(user);
            _logger.LogInformation("User created successfully with ID {UserId}", user.Id);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            });

        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(long id, [FromBody] UpdateUserDto dto)
        {
            _logger.LogInformation("Updating user with ID {UserId}", id);
            var existingUser = _userService.GetUser(id);
            if (existingUser == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for update.", id);
                return NotFound();
            }


            var now = DateTime.UtcNow;
            
            // Map updated fields
            existingUser.UserName = dto.UserName;
            existingUser.Email = dto.Email;
            existingUser.ModifiedDate = now;

            _userService.UpdateUser(existingUser);
            _logger.LogInformation("User with ID {UserId} updated successfully.", id);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(long id)
        {
            _logger.LogInformation("Deleting user with ID {UserId}", id);
            var existingUser = _userService.GetUser(id);
            if (existingUser == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for deletion.", id);
                return NotFound();
            }

            _userService.DeleteUser(id);
            _logger.LogInformation("User with ID {UserId} deleted successfully.", id);
            return NoContent();
        }
    }
}
