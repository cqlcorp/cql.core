using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cql.Core.Owin.Identity.Types
{
    [Table("Clients", Schema = "dbo")]
    public class Client
    {
        [Key]
        [MaxLength(255)]
        public string Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Secret { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int ApplicationType { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public int RefreshTokenLifeTime { get; set; }

        [MaxLength(4000)]
        public string AllowedOrigin { get; set; }
    }
}
