namespace Cql.Core.Owin.Identity.Types
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RefreshTokens", Schema = "dbo")]
    public class RefreshToken
    {
        [Required]
        [MaxLength(128)]
        public string ClientId { get; set; }

        public DateTime ExpiresUtc { get; set; }

        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        public DateTime IssuedUtc { get; set; }

        [Required]
        [MaxLength(4000)]
        public string ProtectedTicket { get; set; }

        [Required]
        [MaxLength(255)]
        public string Subject { get; set; }
    }
}
