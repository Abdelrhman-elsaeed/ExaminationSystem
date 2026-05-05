using ExaminationSystem.DTOs.Auth;
using ExaminationSystem.Helper.JWT;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TokenGenerator _tokenGenerator;

        public AuthController(UserService userService, TokenGenerator tokenGenerator)
        {
            _userService = userService;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _userService.LoginAsync(request.Username, request.Password);

            if (user is null)
                return Unauthorized(new { Message = "Invalid credentials." });

            var token = _tokenGenerator.Generate(
                userId: user.Id,
                name: user.Name,
                role: user.Role.ToString()
            );

            return Ok(new LoginResponseDto(
                Token: token,
                Name: user.Name,
                Role: user.Role.ToString()
            ));
        }

    }
}
