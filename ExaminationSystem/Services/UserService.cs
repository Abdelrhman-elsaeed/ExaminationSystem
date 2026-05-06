using ExaminationSystem.DTOs.Auth;
using ExaminationSystem.DTOs.User;
using ExaminationSystem.Enums;
using ExaminationSystem.Helper.AutoMapper;
using ExaminationSystem.Models;
using ExaminationSystem.Repo;
using ExaminationSystem.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExaminationSystem.Services
{
    public class UserService
    {
        private readonly GenericRepository<User> _userRepo;

        public UserService(GenericRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<ResponseViewModel<UserAuthResultDto>> LoginAsync(LoginRequestDto model)
        {
            if (model is null)
                return ResponseViewModel<UserAuthResultDto>.Failure(ErrorCode.InvalidCredentials, "Invalid login input");

            var user = await _userRepo.Get(u => u.Username == model.Username && u.Deleted == false).FirstOrDefaultAsync();

            if (user is null)
                return ResponseViewModel<UserAuthResultDto>.Failure(ErrorCode.UserNotFound, "User not found");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            if (!isPasswordValid)
                return ResponseViewModel<UserAuthResultDto>.Failure(ErrorCode.InvalidCredentials, "Password is not correct");

            return ResponseViewModel<UserAuthResultDto>.Success(new UserAuthResultDto(user.ID, user.Name, user.Role.ToString()), ErrorCode.None, "Login success");
        }

        public async Task<bool> IsExistAsync(int id)
        {
            return await _userRepo.AnyAsync(u => u.ID == id && u.Deleted == false);
        }

        public async Task<ResponseViewModel<bool>> AddAsync(AddUserDto model)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                return ResponseViewModel<bool>.Failure(ErrorCode.AddUserFail, "Invalid user input");

            var newUserModel = model.Map<User>();
            newUserModel.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            
            var result = await _userRepo.AddAsync(newUserModel);

            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.AddUserFail, "Failed to add user");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "User added successfully");
        }

        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateUserDto model)
        {
            if (model is null || model.ID <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.UserNotFound, "Invalid user input");

            var isExist = await IsExistAsync(model.ID);
            if (!isExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.UserNotFound, "User not found");

            var updateModel = model.Map<User>();
            
            var propertiesToUpdate = new List<string> { nameof(User.Name), nameof(User.Username), nameof(User.Role) };

            if (!string.IsNullOrEmpty(model.Password))
            {
                updateModel.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                propertiesToUpdate.Add(nameof(User.PasswordHash));
            }

            var result = await _userRepo.UpdateInclude(updateModel, propertiesToUpdate.ToArray());

            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.UpdateUserFail, "Failed to update user");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "User updated successfully");
        }

        public async Task<ResponseViewModel<bool>> DeleteAsync(int id)
        {
            if (id <= 0)
                return ResponseViewModel<bool>.Failure(ErrorCode.UserNotFound, "Invalid user id");

            var isExist = await IsExistAsync(id);
            if (!isExist)
                return ResponseViewModel<bool>.Failure(ErrorCode.UserNotFound, "User not found");

            var result = await _userRepo.DeleteAsync(id);
            if (!result)
                return ResponseViewModel<bool>.Failure(ErrorCode.DeleteUserFail, "Failed to delete user");

            return ResponseViewModel<bool>.Success(true, ErrorCode.None, "User deleted successfully");
        }

        public async Task<ResponseViewModel<IEnumerable<UserDto>>> GetAllAsync()
        {
            var users = await _userRepo.GetAll().ToListAsync();
            
            var result = users.Map<IEnumerable<UserDto>>();

            return ResponseViewModel<IEnumerable<UserDto>>.Success(result, ErrorCode.None, "Users retrieved successfully");
        }

        public async Task<ResponseViewModel<UserDto>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return ResponseViewModel<UserDto>.Failure(ErrorCode.UserNotFound, "Invalid user id");

            var user = await _userRepo.GetByIdAsync(id);

            if (user is null)
                return ResponseViewModel<UserDto>.Failure(ErrorCode.UserNotFound, "User not found");

            var result = user.Map<UserDto>();
            return ResponseViewModel<UserDto>.Success(result, ErrorCode.None, "User retrieved successfully");
        }
    }
}
