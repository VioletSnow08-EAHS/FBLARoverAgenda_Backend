using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBLARoverAgenda_Backend.Domain.DTOs.Authentication;
using FBLARoverAgenda_Backend.Domain.Entities.Identity;

namespace FBLARoverAgenda_Backend.Infrastructure.Authentication.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
        Task<ApplicationUser?> GetById(string id);
    }
}
