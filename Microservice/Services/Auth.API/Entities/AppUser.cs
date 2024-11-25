using Microsoft.AspNetCore.Identity;

namespace Auth.API.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
