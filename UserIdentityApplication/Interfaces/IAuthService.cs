using System.Security.Claims;
using UserIdentityApplication.DTOs;

namespace UserIdentityApplication.Interfaces
{
    /// <summary>
    /// Defines authentication and authorization operations such as 
    /// user registration, login, and admin management.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user with the provided details.
        /// </summary>
        /// <param name="model">The registration data transfer object.</param>
        /// <returns>A <see cref="UserResponseDto"/> containing registered user details.</returns>
        Task<UserResponseDto> RegisterUser(RegisterDto model);

        /// <summary>
        /// Authenticates a user and generates a JWT token upon successful login.
        /// </summary>
        /// <param name="model">The login data transfer object.</param>
        /// <returns>A JWT token string if login is successful.</returns>
        Task<string> LoginUser(LoginDto model);

        /// <summary>
        /// Allows an admin to create another admin account.
        /// </summary>
        /// <param name="dto">The registration data transfer object for the new admin.</param>
        /// <param name="currentUser">The current authenticated user making the request.</param>
        Task AddAdmin(RegisterDto dto, ClaimsPrincipal currentUser);
    }
}
