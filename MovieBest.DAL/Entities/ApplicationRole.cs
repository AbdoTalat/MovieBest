using Microsoft.AspNetCore.Identity;

namespace MovieBest.DAL.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
