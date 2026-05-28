using System;
using System.Collections.Generic;
using System.Text;

namespace ExaminationSystem.BLL.ViewModels.Auth
{
    public record AuthVM(
    string Username,
    string Email,
    List<string> Roles,
    string Token,
    DateTime ExpiresOn);
}
