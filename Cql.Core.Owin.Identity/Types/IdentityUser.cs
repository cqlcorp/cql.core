using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNet.Identity;

namespace Cql.Core.Owin.Identity.Types
{
    [Table("AspNetUsers", Schema = "dbo")]
    public class IdentityUser : IUser<int>, IUserId
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public IdentityUser()
        {
            Guid = Guid.NewGuid();
        }

        /// <summary>
        /// Constructor that takes user name as argument
        /// </summary>
        /// <param name="userName"></param>
        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        public static IdentityUser CreateFromEmail(string email)
        {
            return new IdentityUser(email)
            {
                Email = email
            };
        }

        /// <summary>
        /// Email
        /// </summary>
        [MaxLength(255)]
        public string Email { get; set; }

        /// <summary>
        /// True if the email is confirmed, default is false
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// The salted/hashed form of the user password
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// A random value (salt) that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string SecurityStamp { get; set; }

        /// <summary>
        /// PhoneNumber for the user
        /// </summary>
        [MaxLength(255)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// True if the phone number is confirmed, default is false
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Is two factor enabled for the user
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// DateTimeOffset when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public DateTimeOffset? LockoutEndDate { get; set; }

        /// <summary>
        /// Is lockout enabled for this user
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// Used to record failures for the purposes of lockout
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        [Key]
        public int Id { get; set; }

        public Guid Guid { get; set; }

        /// <summary>
        /// User's name
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string UserName { get; set; }

        int IUserId.UserId {
            get { return Id; }
            set { Id = value; }
        }

        public DateTime? AccessRevokedDate { get; set; }

        public int? AccessRevokedBy { get; set; }
    }
}
