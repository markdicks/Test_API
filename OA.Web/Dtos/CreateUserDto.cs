using System.ComponentModel.DataAnnotations;

namespace OA.Web.Dtos
{
    public class CreateUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public UserProfileDto? UserProfile { get; set; }
    }

    public class UpdateUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string? Password { get; set; }

        public UserProfileDto? UserProfile { get; set; }
    }
}
