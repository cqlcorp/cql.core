// ***********************************************************************
// Assembly         : Cql.Core.Owin.Identity
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="RefreshToken.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Owin.Identity.Types
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class RefreshToken.
    /// </summary>
    [Table("RefreshTokens", Schema = "dbo")]
    public class RefreshToken
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        [Required]
        [MaxLength(128)]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the expires UTC.
        /// </summary>
        /// <value>The expires UTC.</value>
        public DateTime ExpiresUtc { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the issued UTC.
        /// </summary>
        /// <value>The issued UTC.</value>
        public DateTime IssuedUtc { get; set; }

        /// <summary>
        /// Gets or sets the protected ticket.
        /// </summary>
        /// <value>The protected ticket.</value>
        [Required]
        [MaxLength(4000)]
        public string ProtectedTicket { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        [Required]
        [MaxLength(255)]
        public string Subject { get; set; }
    }
}
