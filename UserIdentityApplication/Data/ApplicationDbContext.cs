using Microsoft.EntityFrameworkCore;
using UserIdentityApplication.Constants;
using UserIdentityApplication.Models;

namespace UserIdentityApplication.Data
{
    /// <summary>
    /// Represents the Entity Framework Core database context for the application.
    /// Provides access to Users, Roles, and UserRoles entities.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">Database context options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        /// <summary>
        /// Represents the Users table in the database.
        /// </summary>
        public DbSet<Users> Users { get; set; }

        /// <summary>
        /// Represents the Roles table in the database.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Represents the mapping between Users and Roles (many-to-many relationship).
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Configures entity relationships, keys, and seeds initial data for Roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure entity mappings.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define composite key for UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Seed data (optional for development)
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = Guid.NewGuid(), Name = RolesConstant.ADMIN },
                new Role { Id = Guid.NewGuid(), Name = RolesConstant.USER }
            );
        }
    }
}
