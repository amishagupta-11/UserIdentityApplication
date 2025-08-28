using System.ComponentModel.DataAnnotations;

namespace UserIdentityApplication.Models
{
    /// <summary>
    /// Represents a role within the application (e.g., Admin, User).
    /// Roles define access and authorization levels for users.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Unique identifier for the role.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the role (e.g., "Admin", "User").
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        /// <summary>
        /// Navigation property representing the relationship 
        /// between roles and users through the <see cref="UserRole"/> mapping table.
        /// </summary>
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
