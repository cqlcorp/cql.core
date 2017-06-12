using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNet.Identity;

namespace Cql.Core.Owin.Identity.Types
{
    [Table("AspNetRoles", Schema = "dbo")]
    public class IdentityRole : IRole<int>
    {
        public IdentityRole() {}

        public IdentityRole(string name) : this()
        {
            Name = name;
        }

        public IdentityRole(string name, int id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// Role ID
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
