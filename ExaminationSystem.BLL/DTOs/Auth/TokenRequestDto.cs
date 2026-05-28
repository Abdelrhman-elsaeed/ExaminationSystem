using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExaminationSystem.BLL.DTOs.Auth
{
    public class TokenRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
