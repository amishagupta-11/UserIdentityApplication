using System.ComponentModel.DataAnnotations.Schema;

namespace UserIdentityApplication.Models
{
    /// <summary>
    /// Represents the many-to-many relationship between <see cref="Users"/> and <see cref="Role"/>.
    /// A user can have multiple roles, and a role can belong to multiple users.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// The unique identifier of the user associated with this role.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The unique identifier of the role associated with this user.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Navigation property linking to the user entity.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public Users? User { get; set; }

        /// <summary>
        /// Navigation property linking to the role entity.
        /// </summary>
        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }
    }
}
