using ExaminationSystem.BLL.DTOs.Auth;
using ExaminationSystem.BLL.DTOs.User;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.BLL.Services.Interfaces;
using ExaminationSystem.BLL.ViewModels;
using ExaminationSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Implementaiton
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;

        public UserService(UserManager<User> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        public async Task<bool> IsExistAsync(string id, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user != null;
        }

        public async Task<ResponseViewModel<UserDto>> AddAsync(AddUserDto model, CancellationToken cancellationToken = default)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                return ResponseViewModel<UserDto>.Failure(ErrorCode.AddUserFail, "Invalid user input");

            var newUserModel = model.Map<User>();

            var result = await _userManager.CreateAsync(newUserModel, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseViewModel<UserDto>.Failure(ErrorCode.AddUserFail, $"Failed to add user: {errors}");
            }

            var addRoleDto = new AddRoleDto { UserId = newUserModel.Id, Role = model.Role };
            await _authService.AddRoleAsync(addRoleDto);

            return ResponseViewModel<UserDto>.Success(newUserModel.Map<UserDto>(), ErrorCode.None, "User added successfully");
        }

        public async Task<ResponseViewModel<UserDto>> UpdateAsync(UpdateUserDto model, CancellationToken cancellationToken = default)
        {
            if (model is null || string.IsNullOrEmpty(model.ID))
                return ResponseViewModel<UserDto>.Failure(ErrorCode.UserNotFound, "Invalid user input");

            var user = await _userManager.FindByIdAsync(model.ID);
            if (user == null)
                return ResponseViewModel<UserDto>.Failure(ErrorCode.UserNotFound, "User not found");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.Username;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseViewModel<UserDto>.Failure(ErrorCode.UpdateUserFail, $"Failed to update user: {errors}");
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (!passResult.Succeeded)
                {
                    return ResponseViewModel<UserDto>.Failure(ErrorCode.UpdateUserFail, "Failed to update user password");
                }
            }

            var addRoleDto = new AddRoleDto { UserId = user.Id, Role = model.Role };
            await _authService.AddRoleAsync(addRoleDto);

            return ResponseViewModel<UserDto>.Success(user.Map<UserDto>(), ErrorCode.None, "User updated successfully");
        }

        public async Task<ResponseViewModel<bool>> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
                return ResponseViewModel<bool>.Failure(ErrorCode.UserNotFound, "Invalid user id");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return ResponseViewModel<bool>.Failure(ErrorCode.UserNotFound, "User not found");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return ResponseViewModel<bool>.Failure(ErrorCode.DeleteUserFail, "Failed to delete user");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "User deleted successfully");
        }

        public async Task<ResponseViewModel<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var users = await _userManager.Users.ToListAsync();

            var result = users.Map<IEnumerable<UserDto>>();

            return ResponseViewModel<IEnumerable<UserDto>>.Success(result, ErrorCode.None, "Users retrieved successfully");
        }

        public async Task<ResponseViewModel<UserDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
                return ResponseViewModel<UserDto>.Failure(ErrorCode.UserNotFound, "Invalid user id");

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return ResponseViewModel<UserDto>.Failure(ErrorCode.UserNotFound, "User not found");

            var result = user.Map<UserDto>();
            return ResponseViewModel<UserDto>.Success(result, ErrorCode.None, "User retrieved successfully");
        }
    }
}
