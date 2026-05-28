using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExaminationSystem.BLL.ViewModels.Auth
{
    public class AddRoleVM
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
