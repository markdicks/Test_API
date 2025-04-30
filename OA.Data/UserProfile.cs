using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OA.Data
{
    public class UserProfile : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Address { get; set; }

        // Foreign Key
        public long UserId { get; set; }

        // Navigation back to User
        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual User User { get; set; }

        #region Navigation Properties

        #endregion
    }
}
