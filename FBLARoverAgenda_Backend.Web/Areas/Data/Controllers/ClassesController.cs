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
using FBLARoverAgenda_Backend.Domain.Entities.Class;
using FBLARoverAgenda_Backend.Infrastructure.Persistence.DbContexts;

namespace FBLARoverAgenda_Backend.Web.Areas.Data.Controllers;

[Area("Data")]
[Authorize(Roles = "Admin")]
public class ClassesController : BaseController<ClassesController>
{
	public class ClassesIndexViewModel 
	{
		[Key]            
	    public string Id { get; set; }
	    public string Name { get; set; }
	    public string Description { get; set; }
	    public string Location { get; set; }
	    public DateTime StartTime { get; set; }
	    public DateTime EndTime { get; set; }
	    public string Color { get; set; }
	    public string TeacherId { get; set; }
	    public string RecurrenceRule { get; set; }
	    public string UserId { get; set; }
	}

	private const string createBindingFields = "Id,Name,Description,Location,StartTime,EndTime,Color,TeacherId,RecurrenceRule,UserId";
    private const string editBindingFields = "Id,Name,Description,Location,StartTime,EndTime,Color,TeacherId,RecurrenceRule,UserId";
    private const string areaTitle = "Data";

    private readonly ApplicationDbContext _context;

    public ClassesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Data/Classes
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
			.Then("Manage Classes");       
		
		// Fetch descriptive data from the index dto to build the datatables index
		var metadata = DatatableExtensions.GetDtMetadata<ClassesIndexViewModel>();
		
		return View(metadata);
   }

    // GET: Data/Classes/Details/5
    public async Task<IActionResult> Details(string id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Classes", "Index", "Classes", new { Area = "Data" })
            .Then("Class Details");            

        if (id == null)
        {
            return NotFound();
        }

        var @class = await _context.Classes
                .Include(x => x.Teacher)
                .Include(x => x.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (@class == null)
        {
            return NotFound();
        }

        return View(@class);
    }

    // GET: Data/Classes/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Classes", "Index", "Classes", new { Area = "Data" })
            .Then("Create Class");     

        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id");
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
       return View();
	}

    // POST: Data/Classes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] Class @class)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Classes", "Index", "ClassesController", new { Area = "Data" })
        .Then("Create Class");     
        
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);           

        if (ModelState.IsValid)
        {
            @class.Id = Guid.NewGuid().ToString();
            _context.Add(@class);
            await _context.SaveChangesAsync();
            
            _toast.Success("Created successfully.");
            
                return RedirectToAction(nameof(Index));
        }
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", @class.TeacherId);
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", @class.UserId);
        return View(@class);
    }

    // GET: Data/Classes/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Classes", "Index", "Classes", new { Area = "Data" })
        .Then("Edit Class");     

        if (id == null)
        {
            return NotFound();
        }

        var @class = await _context.Classes.FindAsync(id);
        if (@class == null)
        {
            return NotFound();
        }
        
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", @class.TeacherId);
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", @class.UserId);

        return View(@class);
    }

    // POST: Data/Classes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind(editBindingFields)] Class @class)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Classes", "Index", "Classes", new { Area = "Data" })
        .Then("Edit Class");  
    
        if (id != @class.Id)
        {
            return NotFound();
        }
        
        Class model = await _context.Classes.FindAsync(id);

        if (model != null)
        {
            model.Name = @class.Name;
            model.Description = @class.Description;
            model.Location = @class.Location;
            model.StartTime = @class.StartTime;
            model.EndTime = @class.EndTime;
            model.Color = @class.Color;
            model.TeacherId = @class.TeacherId;
            model.RecurrenceRule = @class.RecurrenceRule;
            model.UserId = @class.UserId;
        }

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
                if (!ClassExists(@class.Id))
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
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", @class.TeacherId);
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", @class.UserId);
        return View(@class);
    }

    // GET: Data/Classes/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Classes", "Index", "Classes", new { Area = "Data" })
        .Then("Delete Class");  

        if (id == null)
        {
            return NotFound();
        }

        var @class = await _context.Classes
                .Include(x => x.Teacher)
                .Include(x => x.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (@class == null)
        {
            return NotFound();
        }

        return View(@class);
    }

    // POST: Data/Classes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var @class = await _context.Classes.FindAsync(id);
        if (@class != null) _context.Classes.Remove(@class);
        await _context.SaveChangesAsync();
        
        _toast.Success("Class deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool ClassExists(string id)
    {
        return _context.Classes.Any(e => e.Id == id);
    }


	[HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetClasses(DtRequest request)
    {
        try
		{
			var query = _context.Classes.Include(x => x.Teacher).Include(x => x.User);
			var jsonData = await query.GetDatatableResponseAsync<Class, ClassesIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating Classes index json");
        }
        
        return StatusCode(500);
    }

}

