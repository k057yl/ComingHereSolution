using ComingHereServer.Services;
using ComingHereShared.DTO;
using ComingHereShared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ComingHereServer.Controllers
{
    [ApiController]
    [Route("api/account")]

    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailService _emailService;
        private readonly ConfirmationCodeGenerator _codeGenerator;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 EmailService emailService,
                                 ConfirmationCodeGenerator codeGenerator,
                                 IMemoryCache memoryCache,
                                 IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _codeGenerator = codeGenerator;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var confirmationCode = _codeGenerator.GenerateCode();
            _memoryCache.Set($"ConfirmCode_{user.Id}", confirmationCode, TimeSpan.FromMinutes(10));

            await _emailService.SendConfirmationEmailAsync(user.Email, confirmationCode);

            return Ok(new { userId = user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            Console.WriteLine("User roles: " + string.Join(", ", roles));

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var keyString = _configuration["Jwt:Key"];
            var keyBytes = Convert.FromBase64String(keyString);
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                email = user.Email,
                roles = roles
            });
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return Ok(new { Email = User.Identity.Name });
            }
            return Unauthorized();
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmEmailWithCode([FromBody] ConfirmEmailDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return NotFound("Пользователь не найден");

            if (_memoryCache.TryGetValue($"ConfirmCode_{dto.UserId}", out string expectedCode))
            {
                if (dto.Code == expectedCode)
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    _memoryCache.Remove($"ConfirmCode_{dto.UserId}");

                    return Ok("Email подтверждён");
                }

                return BadRequest("Неверный код");
            }

            return BadRequest("Срок действия кода истёк или он не найден");
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _userManager.Users.Select(u => new { u.Id, u.UserName, u.Email }).ToList();
            return Ok(users);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailOnlyDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return Ok();

            var code = _codeGenerator.GenerateCode();
            _memoryCache.Set($"ResetCode_{user.Email}", code, TimeSpan.FromMinutes(15));
            await _emailService.SendResetCodeAsync(user.Email, code);

            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return BadRequest("User not found");

            if (!_memoryCache.TryGetValue($"ResetCode_{user.Email}", out string expectedCode))
                return BadRequest("Code expired");

            if (dto.Code != expectedCode)
                return BadRequest("Wrong code");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            _memoryCache.Remove($"ResetCode_{user.Email}");
            return Ok();
        }
    }
}