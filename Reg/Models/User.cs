using Microsoft.AspNetCore.Identity;

namespace Store.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
