using FBLARoverAgenda_Backend.Domain.Entities.Identity;

namespace FBLARoverAgenda_Backend.Infrastructure.Common.Settings.Services;

public class RedactorService
{
    public static ApplicationUser REDACT(ApplicationUser user)
    {
        user.PasswordHash = "[REDACTED]";
        user.ConcurrencyStamp = "[REDACTED]";
        user.LockoutEnabled = false;
        user.LockoutEnd = null;
        user.AccessFailedCount = 0;
        user.ConcurrencyStamp = "[REDACTED]";
        user.Claims = null;
        return user;
    }
}