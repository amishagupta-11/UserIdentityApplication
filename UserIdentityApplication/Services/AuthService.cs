using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserIdentityApplication.Data;
using UserIdentityApplication.DTOs;
using UserIdentityApplication.Helpers;
using UserIdentityApplication.Interfaces;
using UserIdentityApplication.Models;

namespace UserIdentityApplication.Services
{
    public class AuthService:IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<UserResponseDto> RegisterUser(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("User already exists");

            var user = new Users
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                CreatedDate=DateTime.Now
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


            return new UserResponseDto { Username = user.Username, Email = user.Email, Roles = new List<string> { defaultRole.Name } };
        }

        public async Task<string> LoginUser(LoginDto dto)
        {
            var user = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || dto.Password!= user.Password){
                throw new Exception("Invalid credentials");
            }

            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
            return JwtHelper.GenerateToken(user, roles, _configuration);
        }

        public async Task AddAdmin(RegisterDto dto, ClaimsPrincipal currentUser)
        {
            // Check if the current user is an admin
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
                // Register the new admin if the user doesn't exist
                user = new Users
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    Password = dto.Password,
                    CreatedDate=DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole == null)
            {
                throw new Exception("Admin role not found.");            }


            // Check if the user already has the Admin role
            var isAlreadyAdmin = await _context.UserRoles
                .AnyAsync(ur => ur.UserId==user.Id && ur.RoleId == adminRole.Id);

            if (isAlreadyAdmin)
            {
                throw new Exception("This user is already an admin.");
            }

            // Assign the Admin role
            var addAdminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole == null)
            {
                throw new Exception("Admin role not found.");
            }

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
