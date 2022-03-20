using Microsoft.AspNetCore.Identity;

namespace FBLARoverAgenda_Backend.Domain.Entities.Identity
{
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
