namespace Cql.Core.Owin.Identity.Types
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Clients", Schema = "dbo")]
    public class Client
    {
        [Required]
        public bool Active { get; set; }

        [MaxLength(4000)]
        public string AllowedOrigin { get; set; }

        [Required]
        public int ApplicationType { get; set; }

        [Key]
        [MaxLength(255)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int RefreshTokenLifeTime { get; set; }

        [Required]
        [MaxLength(255)]
        public string Secret { get; set; }
    }
}
