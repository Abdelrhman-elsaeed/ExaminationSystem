using ExaminationSystem.DTOs.Auth;
using ExaminationSystem.Enums;
using ExaminationSystem.Models;
using ExaminationSystem.Repo;
using ExaminationSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Services
{
    public class UserService
    {
        private readonly GenericRepository<User> _userRepo;

        public UserService(GenericRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<ResponseViewModel<UserAuthResultDto>> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepo.Get(u => u.Username == dto.Username && !u.Deleted).FirstOrDefaultAsync();

            if (user is null)
                return ResponseViewModel<UserAuthResultDto>.Failure(ErrorCode.UserNotFound, "User Not Found.");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isPasswordValid)
                return ResponseViewModel<UserAuthResultDto>.Failure(ErrorCode.InvalidCredentials, "Password is not Correct.");

            return ResponseViewModel<UserAuthResultDto>.Success(new UserAuthResultDto(user.ID, user.Name, user.Role.ToString()),message:"Login Success");
        }
    }
}
