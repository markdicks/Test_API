using System.ComponentModel.DataAnnotations;

namespace OA.Web.Dtos
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserProfileDto UserProfile { get; set; }
    }

    public class UpdateUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public UpdateUserProfileDto UserProfile { get; set; }
    }

    public class UpdateUserProfileDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Address { get; set; }
    }


}
