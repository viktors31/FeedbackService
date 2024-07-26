using FeedbackAPI.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace FeedbackAPI.Services
{
    public interface ITokenService
    {
        string CreateToken(User user, List<IdentityRole<long>> roles);
    }
}
