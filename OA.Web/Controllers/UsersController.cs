using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        [HttpGet("username/{username}")]
        public IActionResult GetUserByUsername(string username)
        {
            _logger.LogInformation("Fetching user with username {UserName}", username);
            var user = _userService.GetUserByUsername(username);
            if (user == null)
            {
                _logger.LogWarning("User with username {UserName} not found", username);
                return NotFound();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password                
            };

            if (user.Profile != null)
            {
                userDto.UserProfile = new UserProfileDto
                {
                    FirstName = user.Profile.FirstName,
                    LastName = user.Profile.LastName,
                    Address = user.Profile.Address
                };
            }
            _logger.LogInformation("User {UserName} found with ID: {UserID}", username, user.Id);
            return Ok(userDto);

        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user creation.");
                return BadRequest(ModelState);
            }
            _logger.LogInformation("Attempting to create user with username: {UserName}", dto.UserName);

            // Check for duplicate username
            var existingUser = _userService.GetUsers().FirstOrDefault(u => u.UserName == dto.UserName);
            if (existingUser != null)
            {
                _logger.LogWarning("User creation failed. Username '{UserName}' already exists.", dto.UserName);
                return Conflict(new { message = $"Username '{dto.UserName}' is already taken." });
            }

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
            _logger.LogInformation("Updating user with ID {UserId}. Request payload: {RequestPayload}", id, JsonConvert.SerializeObject(dto));

            var existingUser = _userService.GetUser(id);
            if (existingUser == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for update.", id);
                return NotFound();
            }

            var now = DateTime.UtcNow;

            // Validate the request (you can add specific validation logic here)
            if (dto.UserProfile != null && string.IsNullOrWhiteSpace(dto.UserProfile.FirstName) && string.IsNullOrWhiteSpace(dto.UserProfile.LastName))
            {
                _logger.LogWarning("User profile update request for user ID {UserId} has invalid user profile.", id);
                return BadRequest("User profile cannot be empty.");
            }

            // Update scalar fields
            if (!string.IsNullOrWhiteSpace(dto.UserName))
            {
                existingUser.UserName = dto.UserName;
                _logger.LogInformation("Updated UserName for user ID {UserId}.", id);
            }

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                existingUser.Email = dto.Email;
                _logger.LogInformation("Updated Email for user ID {UserId}.", id);
            }

            // Update profile fields individually
            if (dto.UserProfile != null)
            {
                if (existingUser.Profile == null)
                    existingUser.Profile = new UserProfile();

                existingUser.Profile.FirstName = dto.UserProfile.FirstName ?? existingUser.Profile.FirstName;
                existingUser.Profile.LastName = dto.UserProfile.LastName ?? existingUser.Profile.LastName;
                existingUser.Profile.Address = dto.UserProfile.Address ?? existingUser.Profile.Address;

                _logger.LogInformation("Updated Profile fields for user ID {UserId}. FirstName: {FirstName}, LastName: {LastName}, Address: {Address}",
                    id, existingUser.Profile.FirstName, existingUser.Profile.LastName, existingUser.Profile.Address);
            }

            existingUser.ModifiedDate = now;

            try
            {
                _userService.UpdateUser(existingUser);
                _logger.LogInformation("User with ID {UserId} updated successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating user with ID {UserId}: {ExceptionMessage}", id, ex.Message);
                return StatusCode(500, "Internal server error");
            }

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
