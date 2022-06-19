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
public class LunchMenuItemsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public LunchMenuItemsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Gets a specific lunchmenuitem.
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ApiResponse> Index(string itemId)
    {
        var item = await _context.LunchMenuItems.Where(x => x.Id == itemId).FirstOrDefaultAsync();
        return item == null
            ? new ApiResponse(HttpStatusCode.BadRequest, itemId, "LunchMenuItem not found.")
            : new ApiResponse(HttpStatusCode.OK, item);
    }
    /// <summary>
    /// Gets all lunchmenuitems.
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public async Task<ApiResponse> All()
    {
        var list = await _context.LunchMenuItems.ToListAsync();
        return new ApiResponse(HttpStatusCode.OK, list);
    }
}