using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class User : IdentityUser
    {
        public string Nickname { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public bool IsOnline { get; set; }
    }
}
