using ExaminationSystem.Enums;
using ExaminationSystem.Enums.JWT_Role;

namespace ExaminationSystem.DTOs.User
{
    public class UserDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
    }
}