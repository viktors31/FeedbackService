using FeedbackAPI.Data.Entities;
using FeedbackAPI.Extensions;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace FeedbackAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user, List<IdentityRole<long>> role)
        {
            var token = user
                .CreateClaims(role)
                .CreateJwtToken(_configuration);
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}
