using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FBLARoverAgenda_Backend.Domain.Entities.FAQ;
using FBLARoverAgenda_Backend.Domain.Entities.Identity;
using FBLARoverAgenda_Backend.Infrastructure.Persistence.DbContexts;
using FBLARoverAgenda_Backend.Web.Areas.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FBLARoverAgenda_Backend.Web.Areas.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Produces("application/json")]
[Area("Api")]
public class TeachersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public TeachersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Gets a specific teacher.
    /// </summary>
    /// <param name="teacherId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ApiResponse> Index(string teacherId)
    {
        var item = await _context.Teachers.Where(x => x.Id == teacherId).FirstOrDefaultAsync();
        return item == null
            ? new ApiResponse(HttpStatusCode.BadRequest, teacherId, "Teacher not found.")
            : new ApiResponse(HttpStatusCode.OK, item);
    }
    /// <summary>
    /// Gets all teachers.
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public async Task<ApiResponse> All()
    {
        var list = await _context.Teachers.ToListAsync();
        return new ApiResponse(HttpStatusCode.OK, list);
    }
}