using Microsoft.AspNetCore.Identity;

namespace FBLARoverAgenda_Backend.Domain.Entities.Identity
{
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
