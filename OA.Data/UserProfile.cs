using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OA.Data
{
    public class UserProfile : BaseEntity
    {
        [Key, ForeignKey("User")]
        public new long Id { get; set; }  /// "new" hides the inherited Id

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Address { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
