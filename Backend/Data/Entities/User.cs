using Microsoft.AspNetCore.Identity;

namespace FeedbackAPI.Data.Entities
{
    public class User : IdentityUser<long>
    {
        public string Name { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
