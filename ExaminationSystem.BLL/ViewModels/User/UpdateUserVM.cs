
namespace ExaminationSystem.BLL.ViewModels.User
{
    public class UpdateUserVM
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }
    }
}