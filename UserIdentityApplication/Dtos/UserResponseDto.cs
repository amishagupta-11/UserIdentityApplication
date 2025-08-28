namespace UserIdentityApplication.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) used to return user details in responses.
    /// Includes username, email, and assigned roles.
    /// </summary>
    public class UserResponseDto
    {
        /// <summary>
        /// The username of the user.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// The email address of the user.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// A list of roles assigned to the user.
        /// </summary>
        public List<string>? Roles { get; set; }
    }
}
