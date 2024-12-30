using System.ComponentModel.DataAnnotations;

namespace UserIdentityApplication.Models
{
    public class Users
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Username { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        // Navigation Property
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
