
namespace ExaminationSystem.BLL.DTOs.User
{
    public class UpdateUserDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }
    }
}