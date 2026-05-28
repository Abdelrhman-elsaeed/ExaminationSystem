using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.BLL.Services.Interfaces
{
    public interface IAuthService
    {
         public Task<ResponseViewModel<AuthDto>> RegisterAsync(RegisterDto model);
         public Task<ResponseViewModel<AuthDto>> GetTokenAsync(TokenRequestDto model);
         public Task<ResponseViewModel<AddRoleDto>> AddRoleAsync(AddRoleDto model);
    }
}
