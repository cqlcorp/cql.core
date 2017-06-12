using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cql.Core.Owin.Identity.Types
{
    [Table("RefreshTokens", Schema = "dbo")]
    public class RefreshToken
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(128)]
        public string ClientId { get; set; }

        public DateTime IssuedUtc { get; set; }

        public DateTime ExpiresUtc { get; set; }

        [Required]
        [MaxLength(4000)]
        public string ProtectedTicket { get; set; }
    }
}
