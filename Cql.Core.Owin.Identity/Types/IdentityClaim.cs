namespace Cql.Core.Owin.Identity.Types
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AspNetUserClaims", Schema = "dbo")]
    public class IdentityClaim
    {
        [Required]
        [StringLength(1024)]
        public string ClaimType { get; set; }

        [Required]
        [StringLength(4000)]
        public string ClaimValue { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
