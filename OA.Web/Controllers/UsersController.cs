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

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(long id)
        {
            var user = _userService.GetUser(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var now = DateTime.UtcNow;

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Password = dto.Password,
                AddedDate = now,
                ModifiedDate = now,
                UserProfile = new UserProfile
                {
                    FirstName = dto.UserProfile.FirstName,
                    LastName = dto.UserProfile.LastName,
                    Address = dto.UserProfile.Address,
                    AddedDate = now,
                    ModifiedDate = now
                }
            };

            _userService.InsertUser(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                UserProfile = new UserProfileDto
                {
                    FirstName = user.UserProfile.FirstName,
                    LastName = user.UserProfile.LastName,
                    Address = user.UserProfile.Address
                }
            });

        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(long id, [FromBody] UpdateUserDto dto)
        {
            var existingUser = _userService.GetUser(id);
            if (existingUser == null)
                return NotFound();


            var now = DateTime.UtcNow;
            
            // Map updated fields
            existingUser.UserName = dto.UserName;
            existingUser.Email = dto.Email;
            existingUser.ModifiedDate = now;

            if (existingUser.UserProfile != null && dto.UserProfile != null)
            {
                existingUser.UserProfile.FirstName = dto.UserProfile.FirstName;
                existingUser.UserProfile.LastName = dto.UserProfile.LastName;
                existingUser.UserProfile.Address = dto.UserProfile.Address;
                existingUser.UserProfile.ModifiedDate = now;
            }

            _userService.UpdateUser(existingUser);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(long id)
        {
            var existingUser = _userService.GetUser(id);
            if (existingUser == null)
                return NotFound();

            _userService.DeleteUser(id);
            return NoContent();
        }
    }
}
