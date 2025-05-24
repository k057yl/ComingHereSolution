using ComingHereServer.Services;
using ComingHereShared.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ComingHereServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly ConfirmationCodeGenerator _codeGenerator;
        private readonly IMemoryCache _memoryCache;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 EmailService emailService,
                                 ConfirmationCodeGenerator codeGenerator,
                                 IMemoryCache memoryCache)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _codeGenerator = codeGenerator;
            _memoryCache = memoryCache;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var confirmationCode = _codeGenerator.GenerateCode();
            _memoryCache.Set($"ConfirmCode_{user.Id}", confirmationCode, TimeSpan.FromMinutes(10));

            await _emailService.SendConfirmationEmailAsync(user.Email, confirmationCode);

            return Ok(new { userId = user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!result.Succeeded)
                return Unauthorized();

            return Ok();
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
    }
}