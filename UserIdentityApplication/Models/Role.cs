using System.ComponentModel.DataAnnotations;

namespace UserIdentityApplication.Models
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        // Navigation Property
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
