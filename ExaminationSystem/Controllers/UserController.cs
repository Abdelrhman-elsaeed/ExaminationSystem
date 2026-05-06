using ExaminationSystem.DTOs.User;
using ExaminationSystem.Helper.AutoMapper;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels;
using ExaminationSystem.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExaminationSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserVM model)
        {
            var addUserDto = model.Map<AddUserDto>();
            var result = await _userService.AddAsync(addUserDto);

            if (result.IsSuccess)
                return Ok(result);
            else
                return NotFound(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update(UpdateUserVM model)
        {
            var updateUserDto = model.Map<UpdateUserDto>();
            var result = await _userService.UpdateAsync(updateUserDto);

            if (result.IsSuccess)
                return Ok(result);
            else
                return NotFound(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);

            if (result.IsSuccess)
                return Ok(result);
            else
                return NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();

            if (result.IsSuccess)
            {
                var usersVm = result.Data.Map<IEnumerable<UserVM>>();
                return Ok(ResponseViewModel<IEnumerable<UserVM>>.Success(usersVm, result.ErrorCode, result.Message));
            }
            else
            {
                return NotFound(ResponseViewModel<IEnumerable<UserVM>>.Failure(result.ErrorCode, result.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetByIdAsync(id);

            if (result.IsSuccess)
            {
                var userVm = result.Data.Map<UserVM>();
                return Ok(ResponseViewModel<UserVM>.Success(userVm, result.ErrorCode, result.Message));
            }
            else
            {
                return NotFound(ResponseViewModel<UserVM>.Failure(result.ErrorCode, result.Message));
            }
        }
    }
}
