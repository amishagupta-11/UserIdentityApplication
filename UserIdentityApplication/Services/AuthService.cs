using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserIdentityApplication.Data;
using UserIdentityApplication.DTOs;
using UserIdentityApplication.Helpers;
using UserIdentityApplication.Interfaces;
using UserIdentityApplication.Models;

namespace UserIdentityApplication.Services
{
    /// <summary>
    /// Provides authentication and authorization services, including user registration,
    /// login with JWT token generation, and admin management.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing users and roles.</param>
        /// <param name="configuration">The application configuration (used for JWT settings).</param>
        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Registers a new user with default "User" role.
        /// </summary>
        /// <param name="dto">The registration details.</param>
        /// <returns>A <see cref="UserResponseDto"/> containing registered user information.</returns>
        /// <exception cref="Exception">Thrown if the user already exists or default role is missing.</exception>
        public async Task<UserResponseDto> RegisterUser(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("User already exists");

            var user = new Users
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                CreatedDate = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (defaultRole == null)
                throw new Exception("Default role not found");

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = defaultRole.Id
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Username = user.Username,
                Email = user.Email,
                Roles = new List<string> { defaultRole.Name }
            };
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token if credentials are valid.
        /// </summary>
        /// <param name="dto">The login details (username and password).</param>
        /// <returns>A JWT token string if login is successful.</returns>
        /// <exception cref="Exception">Thrown if credentials are invalid.</exception>
        public async Task<string> LoginUser(LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null || dto.Password != user.Password)
            {
                throw new Exception("Invalid credentials");
            }

            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
            return JwtHelper.GenerateToken(user, roles, _configuration);
        }

        /// <summary>
        /// Allows an admin user to create or assign the Admin role to another user.
        /// </summary>
        /// <param name="dto">The registration details of the new or existing user.</param>
        /// <param name="currentUser">The currently logged-in user attempting the action.</param>
        /// <exception cref="UnauthorizedAccessException">Thrown if the current user is not an admin.</exception>
        /// <exception cref="Exception">Thrown if the Admin role is missing or the user is already an admin.</exception>
        public async Task AddAdmin(RegisterDto dto, ClaimsPrincipal currentUser)
        {
            // Verify current user is an admin
            var currentUserRoles = currentUser.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!currentUserRoles.Contains("Admin"))
            {
                throw new UnauthorizedAccessException("Only admins can add a new admin.");
            }

            // Check if the user already exists
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                // Register a new user if they don't exist
                user = new Users
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    Password = dto.Password, // ⚠️ Should be hashed in production
                    CreatedDate = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole == null)
            {
                throw new Exception("Admin role not found.");
            }

            // Ensure the user is not already an admin
            var isAlreadyAdmin = await _context.UserRoles
                .AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == adminRole.Id);

            if (isAlreadyAdmin)
            {
                throw new Exception("This user is already an admin.");
            }

            // Assign the Admin role
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = adminRole.Id
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
        }
    }
}
