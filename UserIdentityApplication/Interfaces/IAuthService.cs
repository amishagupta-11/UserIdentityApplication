using System.Security.Claims;
using UserIdentityApplication.DTOs;

namespace UserIdentityApplication.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterUser(RegisterDto model);
        Task<string> LoginUser(LoginDto model);
        Task AddAdmin(RegisterDto dto, ClaimsPrincipal currentUser);
    }
}
