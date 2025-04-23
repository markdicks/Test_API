using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OA.Data
{
    public class UserProfile : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Address { get; set; }

        [ForeignKey("Id")]
        public virtual User User { get; set; }

        #region Navigation Properties

        #endregion
    }
}
