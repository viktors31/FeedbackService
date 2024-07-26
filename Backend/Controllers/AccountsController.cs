using System.IdentityModel.Tokens.Jwt;
using FeedbackAPI.Data;
using FeedbackAPI.Data.Entities;
using FeedbackAPI.Extensions;
using FeedbackAPI.Models.Identity;
using FeedbackAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackAPI.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly FeedbackDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AccountsController(ITokenService tokenService, FeedbackDbContext context, UserManager<User> userManager, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            //Это костыль для создания пользователя.
            /*var myUser = new User { UserName = "Victor", Name = "Victor" };
            var Result = await _userManager.CreateAsync(myUser, "VictorSQL24_");
            var addedUser = await _context.Users.FirstOrDefaultAsync(x => x.Name == "Victor");
            var R2 = await _userManager.AddToRoleAsync(addedUser, "Admin");
            _context.SaveChanges();*/
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //Ищем пользователя
            var managedUser = await _userManager.FindByNameAsync(request.Name);
            if (managedUser == null)
                return BadRequest("Bad credentials");
            //Проверяем пароль
            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
            if (!isPasswordValid)
                return BadRequest("Bad credentials");

            var user = _context.Users.FirstOrDefault(u => u.Name == request.Name);
            if (user is null)
                return Unauthorized();

            var roleIds = await _context.UserRoles.Where(r => r.UserId == user.Id).Select(x => x.RoleId).ToListAsync();
            var roles = _context.Roles.Where(x => roleIds.Contains(x.Id)).ToList();

            var accessToken = _tokenService.CreateToken(user, roles);
            user.RefreshToken = _configuration.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());

            await _context.SaveChangesAsync();

            return Ok(new AuthResponse
            {
                Name = user.Name!,
                Token = accessToken,
                RefreshToken = user.RefreshToken
            });
            //return Ok(R2);
        }
    }
}
