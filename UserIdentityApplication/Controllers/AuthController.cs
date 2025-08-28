using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserIdentityApplication.DTOs;
using UserIdentityApplication.Interfaces;

namespace UserIdentityApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user with the provided details.
        /// </summary>
        /// <param name="dto">The registration data transfer object.</param>
        /// <returns>Action result with registration response.</returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var response = await _authService.RegisterUser(dto);
            return Ok(response);
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token on successful login.
        /// </summary>
        /// <param name="dto">The login data transfer object.</param>
        /// <returns>Action result containing the JWT token.</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.LoginUser(dto);
            return Ok(new { Token = token });
        }

        /// <summary>
        /// Allows an Admin user to create another Admin account.
        /// </summary>
        /// <param name="dto">The registration data transfer object for the new admin.</param>
        /// <returns>Action result indicating success or failure.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddAdmin([FromBody] RegisterDto dto)
        {
            try
            {
                await _authService.AddAdmin(dto, User);
                return Ok("Admin added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
