using Microsoft.AspNetCore.Identity;

namespace Exam.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
