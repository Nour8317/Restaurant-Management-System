using Microsoft.AspNetCore.Identity;

namespace Restorant.Models
{
    public class ApplicationUser : IdentityUser
    {
        List<Order> Orders {  get; set; }

    }
}
