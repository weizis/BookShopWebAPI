namespace API1.DTOs.Auth
{
    public class TokenDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
