namespace FeedbackAPI.Models.Identity
{
    public class AuthResponse
    {
        public string Name { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
