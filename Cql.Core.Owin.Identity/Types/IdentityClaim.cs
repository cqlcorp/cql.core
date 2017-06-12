using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cql.Core.Owin.Identity.Types
{
    [Table("AspNetUserClaims", Schema = "dbo")]
    public class IdentityClaim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(1024)]
        public string ClaimType { get; set; }

        [Required]
        [StringLength(4000)]
        public string ClaimValue { get; set; }
    }
}
