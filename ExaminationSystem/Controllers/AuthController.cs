using ExaminationSystem.BLL.Services.Interfaces;

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
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterVM model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(model.Map<RegisterDto>());

            if (!result.IsSuccess)
            {
                return BadRequest(ResponseViewModel<AuthVM>.Failure(result.ErrorCode,result.Message));
            }

            return Ok(ResponseViewModel<AuthVM>.Success(result.Data.Map<AuthVM>(), message:result.Message));

        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] TokenRequestVM model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.GetTokenAsync(model.Map<TokenRequestDto>());

            if (!result.IsSuccess)
            {
                return BadRequest(ResponseViewModel<AuthVM>.Failure(result.ErrorCode, result.Message));
            }

            return Ok(ResponseViewModel<AuthVM>.Success(result.Data.Map<AuthVM>(), message: result.Message));

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleVM model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.AddRoleAsync(model.Map<AddRoleDto>());

            if (!result.IsSuccess)
            {
                return BadRequest(ResponseViewModel<AddRoleVM>.Failure(result.ErrorCode, result.Message));
            }

            return Ok(ResponseViewModel<AddRoleVM>.Success(result.Data.Map<AddRoleVM>(), message: result.Message));

        }
    }
}
