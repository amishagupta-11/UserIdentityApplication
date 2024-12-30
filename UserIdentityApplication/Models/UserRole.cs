using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace UserIdentityApplication.Models
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users? User { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }
    }
}
