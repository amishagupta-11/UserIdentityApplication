namespace UserIdentityApplication.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) used for user registration requests.
    /// Contains username, email, and password details.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// The username chosen by the user during registration.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// The email address provided by the user.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// The password chosen by the user during registration.
        /// </summary>
        public string? Password { get; set; }
    }
}
