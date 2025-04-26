using System.ComponentModel.DataAnnotations;

namespace OA.Data
{
    public class User : BaseEntity
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        #region Navigation Properties

        #endregion
    }
}
