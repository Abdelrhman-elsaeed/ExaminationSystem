using System;
using System.Collections.Generic;

using System.Text;

namespace ExaminationSystem.BLL.DTOs.Auth
{
    public class AddRoleDto
    {

        public string UserId { get; set; }
        public Role Role { get; set; } 
    }
}
