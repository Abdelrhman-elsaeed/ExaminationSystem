using ExaminationSystem.BLL.Helper.JWT;
using ExaminationSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExaminationSystem.BLL.Services.Implementaiton
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public AuthService(UserManager<User> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value; 
        }

        private async Task<JwtSecurityToken> CreateJwtTokenAsync(User user)
        {

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials
            );

            return token;
        }

        public async Task<ResponseViewModel<AuthDto>> RegisterAsync(RegisterDto model, CancellationToken cancellationToken = default)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                return ResponseViewModel<AuthDto>.Failure(ErrorCode.EmailAlreadyRegistered, "Email is already registered");
            }

            if (await _userManager.FindByNameAsync(model.Username) is not null)
            {
                return ResponseViewModel<AuthDto>.Failure(ErrorCode.UsernameAlreadyRegistered, "Username is already registered");
            }

            var user = model.Map<User>();
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseViewModel<AuthDto>.Failure(ErrorCode.RegisterationFail, errors);
            }

            

            // Default role is Student
            await _userManager.AddToRoleAsync(user, "Student");
            var jwtSecurityToken = await CreateJwtTokenAsync(user);

            var authDto = new AuthDto(
                Username: user.UserName,
                Email: user.Email,
                Roles: new List<string> { "User" },
                Token: new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExpiresOn: jwtSecurityToken.ValidTo
            );

            return ResponseViewModel<AuthDto>.Success(authDto,message:"User registered successfully");
        }
        public async Task<ResponseViewModel<AuthDto>> GetTokenAsync(TokenRequestDto model, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return ResponseViewModel<AuthDto>.Failure(ErrorCode.InvalidCredentials, "Invalid Email or Password");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var jwtSecurityToken = await CreateJwtTokenAsync(user);

            var authDto = new AuthDto(
                Username: user.UserName,
                Email: user.Email,
                Roles: roles.ToList(),
                Token: new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExpiresOn: jwtSecurityToken.ValidTo
            );

            return ResponseViewModel<AuthDto>.Success(authDto, ErrorCode.None, "Login successful");
        }
        public async Task<ResponseViewModel<AddRoleDto>> AddRoleAsync(AddRoleDto model, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user is null)
            {
                return ResponseViewModel<AddRoleDto>.Failure(ErrorCode.UserNotFound, "Invalid User ID");
            }

            string roleName = model.Role.ToString();

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return ResponseViewModel<AddRoleDto>.Failure(ErrorCode.RoleNotFound, "Invalid Role");
            }

            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                return ResponseViewModel<AddRoleDto>.Failure(ErrorCode.RoleAssignedBefore, "User is already assigned to this role");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseViewModel<AddRoleDto>.Failure(ErrorCode.AddRoleFail, errors);
            }

            return ResponseViewModel<AddRoleDto>.Success(model,message:"Role assigned successfully");
        }

    }
}
