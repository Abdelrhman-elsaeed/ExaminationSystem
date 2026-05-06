using ExaminationSystem.Enums;
using ExaminationSystem.Enums.JWT_Role;

namespace ExaminationSystem.DTOs.User
{
    public class AddUserDto
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}