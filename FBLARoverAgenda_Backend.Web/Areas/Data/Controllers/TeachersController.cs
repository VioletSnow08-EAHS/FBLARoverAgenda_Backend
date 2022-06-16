using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoverCore.BreadCrumbs.Services;
using RoverCore.Datatables.DTOs;
using RoverCore.Datatables.Extensions;
using FBLARoverAgenda_Backend.Web.Controllers;
using FBLARoverAgenda_Backend.Infrastructure.Common.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FBLARoverAgenda_Backend.Domain.Entities.Teacher;
using FBLARoverAgenda_Backend.Infrastructure.Persistence.DbContexts;

namespace FBLARoverAgenda_Backend.Web.Areas.Data.Controllers;

[Area("Data")]
[Authorize(Roles = "Admin")]
public class TeachersController : BaseController<TeachersController>
{
	public class TeachersIndexViewModel 
	{
		[Key]            
	    public string Id { get; set; }
	    public string FirstName { get; set; }
	    public string LastName { get; set; }
	    public string Email { get; set; }
	}

	private const string createBindingFields = "Id,FirstName,LastName,Email";
    private const string editBindingFields = "Id,FirstName,LastName,Email";
    private const string areaTitle = "Teachers";

    private readonly ApplicationDbContext _context;

    public TeachersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Teachers/Teachers
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
			.Then("Manage Teachers");       
		
		// Fetch descriptive data from the index dto to build the datatables index
		var metadata = DatatableExtensions.GetDtMetadata<TeachersIndexViewModel>();
		
		return View(metadata);
   }

    // GET: Teachers/Teachers/Details/5
    public async Task<IActionResult> Details(string id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Teachers", "Index", "Teachers", new { Area = "Teachers" })
            .Then("Teacher Details");            

        if (id == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (teacher == null)
        {
            return NotFound();
        }

        return View(teacher);
    }

    // GET: Teachers/Teachers/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Teachers", "Index", "Teachers", new { Area = "Teachers" })
            .Then("Create Teacher");     

       return View();
	}

    // POST: Teachers/Teachers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] Teacher teacher)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Teachers", "Index", "TeachersController", new { Area = "Teachers" })
        .Then("Create Teacher");     
        
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);           

        if (ModelState.IsValid)
        {
            teacher.Id = Guid.NewGuid().ToString();
            _context.Add(teacher);
            await _context.SaveChangesAsync();
            
            _toast.Success("Created successfully.");
            
                return RedirectToAction(nameof(Index));
            }
        return View(teacher);
    }

    // GET: Teachers/Teachers/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Teachers", "Index", "Teachers", new { Area = "Teachers" })
        .Then("Edit Teacher");     

        if (id == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null)
        {
            return NotFound();
        }
        

        return View(teacher);
    }

    // POST: Teachers/Teachers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind(editBindingFields)] Teacher teacher)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Teachers", "Index", "Teachers", new { Area = "Teachers" })
        .Then("Edit Teacher");  
    
        if (id != teacher.Id)
        {
            return NotFound();
        }
        
        Teacher model = await _context.Teachers.FindAsync(id);

        model.FirstName = teacher.FirstName;
        model.LastName = teacher.LastName;
        model.Email = teacher.Email;
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(editBindingFields);           

        if (ModelState.IsValid)
        {
            try
            {
                await _context.SaveChangesAsync();
                _toast.Success("Updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(teacher.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(teacher);
    }

    // GET: Teachers/Teachers/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Teachers", "Index", "Teachers", new { Area = "Teachers" })
        .Then("Delete Teacher");  

        if (id == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (teacher == null)
        {
            return NotFound();
        }

        return View(teacher);
    }

    // POST: Teachers/Teachers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();
        
        _toast.Success("Teacher deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool TeacherExists(string id)
    {
        return _context.Teachers.Any(e => e.Id == id);
    }


	[HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetTeachers(DtRequest request)
    {
        try
		{
			var query = _context.Teachers;
			var jsonData = await query.GetDatatableResponseAsync<Teacher, TeachersIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating Teachers index json");
        }
        
        return StatusCode(500);
    }

}

