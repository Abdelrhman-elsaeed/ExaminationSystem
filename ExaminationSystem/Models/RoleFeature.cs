using ExaminationSystem.Enums;
using ExaminationSystem.Enums.JWT_Role;

namespace ExaminationSystem.Models
{
    public class RoleFeature : BaseModel
    {
        public Role Role { get; set; }
        public Feature Feature { get; set; }
    }
}
