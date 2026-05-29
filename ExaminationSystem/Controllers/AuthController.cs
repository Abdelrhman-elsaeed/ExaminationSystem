using ExaminationSystem.BLL.Services.Interfaces;
using ExaminationSystem.BLL.DTOs.Auth;
using ExaminationSystem.BLL.ViewModels;
using ExaminationSystem.BLL.ViewModels.Auth;
using ExaminationSystem.BLL.AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterVM model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(model.Map<RegisterDto>(), cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(ResponseViewModel<AuthVM>.Failure(result.ErrorCode,result.Message));
            }

            return Ok(ResponseViewModel<AuthVM>.Success(result.Data.Map<AuthVM>(), message:result.Message));
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] TokenRequestVM model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.GetTokenAsync(model.Map<TokenRequestDto>(), cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(ResponseViewModel<AuthVM>.Failure(result.ErrorCode, result.Message));
            }

            return Ok(ResponseViewModel<AuthVM>.Success(result.Data.Map<AuthVM>(), message: result.Message));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleVM model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.AddRoleAsync(model.Map<AddRoleDto>(), cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(ResponseViewModel<AddRoleVM>.Failure(result.ErrorCode, result.Message));
            }

            return Ok(ResponseViewModel<AddRoleVM>.Success(result.Data.Map<AddRoleVM>(), message: result.Message));
        }
    }
}
