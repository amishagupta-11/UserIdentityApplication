namespace UserIdentityApplication.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) used for user login requests.
    /// Carries the username and password credentials.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// The username of the user attempting to log in.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// The password of the user attempting to log in.
        /// </summary>
        public string? Password { get; set; }
    }
}
