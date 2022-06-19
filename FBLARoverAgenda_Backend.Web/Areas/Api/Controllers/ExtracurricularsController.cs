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
public class ExtracurricularsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ExtracurricularsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Gets a specific extracurricular.
    /// </summary>
    /// <param name="extracurricularId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ApiResponse> Index(string extracurricularId)
    {
        var item = await _context.Extracurriculars.Where(x => x.Id == extracurricularId).FirstOrDefaultAsync();
        return item == null
            ? new ApiResponse(HttpStatusCode.BadRequest, extracurricularId, "Extracurricular not found.")
            : new ApiResponse(HttpStatusCode.OK, item);
    }
    /// <summary>
    /// Gets all extracurriculars.
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public async Task<ApiResponse> All()
    {
        var list = await _context.Extracurriculars.ToListAsync();
        return new ApiResponse(HttpStatusCode.OK, list);
    }
}