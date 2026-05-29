using ExaminationSystem.BLL.Services.Interfaces;

namespace ExaminationSystem.Controllers
{
    [Authorize(Roles = "Admin,Instructor")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserVM model, CancellationToken cancellationToken)
        {
            var addUserDto = model.Map<AddUserDto>();
            var result = await _userService.AddAsync(addUserDto, cancellationToken);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<UserVM>.Success(result.Data.Map<UserVM>(), result.ErrorCode, result.Message));

            return NotFound(ResponseViewModel<UserVM>.Failure(result.ErrorCode, result.Message));
        }

        [HttpPatch]
        public async Task<IActionResult> Update(UpdateUserVM model, CancellationToken cancellationToken)
        {
            var updateUserDto = model.Map<UpdateUserDto>();
            var result = await _userService.UpdateAsync(updateUserDto, cancellationToken);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<UserVM>.Success(result.Data.Map<UserVM>(), result.ErrorCode, result.Message));

            return NotFound(ResponseViewModel<UserVM>.Failure(result.ErrorCode, result.Message));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var result = await _userService.DeleteAsync(id, cancellationToken);

            if (result.IsSuccess)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _userService.GetAllAsync(cancellationToken);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<IEnumerable<UserVM>>.Success(result.Data.Map<IEnumerable<UserVM>>(), result.ErrorCode, result.Message));

            return NotFound(ResponseViewModel<IEnumerable<UserVM>>.Failure(result.ErrorCode, result.Message));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            var result = await _userService.GetByIdAsync(id, cancellationToken);

            if (result.IsSuccess)
                return Ok(ResponseViewModel<UserVM>.Success(result.Data.Map<UserVM>(), result.ErrorCode, result.Message));

            return NotFound(ResponseViewModel<UserVM>.Failure(result.ErrorCode, result.Message));
        }
    }
}
