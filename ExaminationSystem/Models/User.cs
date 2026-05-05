using ExaminationSystem.Enums.JWT_Role;
using System.Data;

namespace ExaminationSystem.Models
{
    public class User : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; }
    }
}
