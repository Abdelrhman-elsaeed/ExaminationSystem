using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.BLL.DTOs.Auth
{
    public record AuthDto(
    string Username,
    string Email,
    List<string> Roles,
    string Token,
    DateTime ExpiresOn);

}
