using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.BLL.Services.Interfaces
{
    public interface IAuthService
    {
         public Task<ResponseViewModel<AuthDto>> RegisterAsync(RegisterDto model, CancellationToken cancellationToken = default);
         public Task<ResponseViewModel<AuthDto>> GetTokenAsync(TokenRequestDto model, CancellationToken cancellationToken = default);
         public Task<ResponseViewModel<AddRoleDto>> AddRoleAsync(AddRoleDto model, CancellationToken cancellationToken = default);
    }
}
