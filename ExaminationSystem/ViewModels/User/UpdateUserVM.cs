using ExaminationSystem.Enums;
using ExaminationSystem.Enums.JWT_Role;

namespace ExaminationSystem.ViewModels.User
{
    public class UpdateUserVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
    }
}