namespace UserIdentityApplication.DTOs
{
    public class UserResponseDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
    }
}
