using System.ComponentModel.DataAnnotations;

namespace UserIdentityApplication.Models
{
    /// <summary>
    /// Represents an application user with authentication and authorization details.
    /// Each user can have multiple roles through the <see cref="UserRole"/> mapping.
    /// </summary>
    public class Users
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The username chosen by the user.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Username { get; set; }

        /// <summary>
        /// The email address of the user (must be unique and valid).
        /// </summary>
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// The hashed password of the user.
        /// </summary>
        [Required]
        public string? Password { get; set; }

        /// <summary>
        /// The date and time when the user account was created.
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Navigation property representing the relationship 
        /// between the user and assigned roles.
        /// </summary>
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
