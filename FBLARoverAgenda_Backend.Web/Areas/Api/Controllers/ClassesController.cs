using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FBLARoverAgenda_Backend.Domain.Entities.Class;
using FBLARoverAgenda_Backend.Domain.Entities.FAQ;
using FBLARoverAgenda_Backend.Domain.Entities.Identity;
using FBLARoverAgenda_Backend.Infrastructure.Common.Extensions;
using FBLARoverAgenda_Backend.Infrastructure.Common.Settings.Services;
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
public class ClassesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET
    [HttpGet]
    public async Task<ApiResponse> Index(string classId)
    {
        var userId = _userManager.GetUserId(User);
        var @class = await _context.Classes.Where(x => x.UserId == userId && x.Id == classId).Include(x => x.Teacher)
            .Include(x => x.User).FirstOrDefaultAsync();
        if (@class != null)
        {
            @class.User = RedactorService.REDACT(@class.User);
            return await Task.FromResult<ApiResponse>(new ApiResponse(HttpStatusCode.OK, @class));
        }
        else return await Task.FromResult<ApiResponse>(new ApiResponse(HttpStatusCode.BadRequest));
    }

    // GET
    [HttpGet]
    public async Task<ApiResponse> All()
    {
        var classes = await _context.Classes.Include(x => x.Teacher).Include(x => x.User).ToListAsync();
        foreach (var @class in classes)
        {
            @class.User = RedactorService.REDACT(@class.User);
        }

        return await Task.FromResult(new ApiResponse(HttpStatusCode.OK, classes));
    }
    

    [HttpPut]
    public async Task<ApiResponse> Edit(string classId, Class newModel)
    {
        var userId = _userManager.GetUserId(User);

        var @class = await _context.Classes.Where(x => x.Id == classId && x.UserId == userId).FirstOrDefaultAsync();
        if (@class == null) return new ApiResponse(HttpStatusCode.BadRequest);


        @class.Name = newModel.Name ?? @class.Name;
        @class.Description = newModel.Description ?? @class.Description;
        @class.Location = newModel.Location ?? @class.Location;
        @class.StartTime = newModel.StartTime == DateTime.MinValue ? @class.StartTime : newModel.StartTime;
        @class.EndTime = newModel.EndTime == DateTime.MinValue ? @class.EndTime : newModel.EndTime;
        @class.Color = newModel.Color ?? @class.Color;
        @class.TeacherId = newModel.TeacherId ?? @class.TeacherId;
        @class.RecurrenceRule = newModel.RecurrenceRule ?? @class.RecurrenceRule;
        @class.UserId = newModel.UserId ?? @class.UserId;

        // Queryables

        var teacher = await _context.Teachers.Where(x => x.Id == @class.TeacherId).FirstOrDefaultAsync();
        var user = await _context.Users.Where(x => x.Id == @class.UserId).FirstOrDefaultAsync();
        @class.Teacher = teacher;
        @class.User = user;


        // Check if user input is valid
       // if (!ModelState.IsValid) return new ApiResponse(HttpStatusCode.BadRequest, newModel);
        _context.Update(@class);
        await _context.SaveChangesAsync();

        return new ApiResponse(HttpStatusCode.OK, @class);
    }

    [HttpDelete]
    public async Task<ApiResponse> Delete(string classId)
    {
        var userId = _userManager.GetUserId(User);
        var @class = await _context.Classes.Where(x => x.Id == classId && x.UserId == userId).FirstOrDefaultAsync();

        if (@class == null) return new ApiResponse(HttpStatusCode.BadRequest, "Class does not exist.");
        
        _context.Remove(@class);
        await _context.SaveChangesAsync();

        return new ApiResponse(HttpStatusCode.OK);
    }
}