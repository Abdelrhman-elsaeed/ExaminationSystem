using Azure;
using ExaminationSystem.DTOs.Auth;
using ExaminationSystem.Helper.JWT;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels;
using ExaminationSystem.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
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
        public async Task<ActionResult> Login([FromBody] LoginRequestVM request)
        {
            var loginDto = new LoginRequestDto(request.Username, request.Password);

            var result = await _userService.LoginAsync(loginDto);

            if (!result.IsSuccess)
                return Unauthorized(result);

            //Generate Token
            var token = _tokenGenerator.Generate(
                userId: result.Data!.Id,
                name: result.Data.Name,
                role: result.Data.Role);

            return Ok(ResponseViewModel<LoginResponseVM>.Success (new LoginResponseVM(token, result.Data.Name, result.Data.Role)));
        }

    }
}
