
using Microsoft.AspNetCore.Identity;


namespace ExaminationSystem.DAL.Models
{
    public class User : IdentityUser
    {


        public string FirstName { get; set; }



        public string LastName { get; set; }
    }
}
