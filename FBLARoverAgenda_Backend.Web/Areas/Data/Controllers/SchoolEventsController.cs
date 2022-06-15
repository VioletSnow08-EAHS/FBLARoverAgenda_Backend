using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FBLARoverAgenda_Backend.Domain.Entities.SchoolEvent;
using FBLARoverAgenda_Backend.Infrastructure.Common.Extensions;
using FBLARoverAgenda_Backend.Infrastructure.Persistence.DbContexts;
using FBLARoverAgenda_Backend.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoverCore.BreadCrumbs.Services;
using RoverCore.Datatables.DTOs;
using RoverCore.Datatables.Extensions;

namespace FBLARoverAgenda_Backend.Web.Areas.Data.Controllers;

[Area("Data")]
[Authorize(Roles = "Admin")]
public class SchoolEventsController : BaseController<SchoolEventsController>
{
	public class SchoolEventsIndexViewModel 
	{
		[Key]            
	    public int Id { get; set; }
	    public string Name { get; set; }
	    public string Description { get; set; }
	    public DateTime StartTime { get; set; }
	    public DateTime EndTime { get; set; }
	}

	private const string createBindingFields = "Id,Name,Description,StartTime,EndTime";
    private const string editBindingFields = "Id,Name,Description,StartTime,EndTime";
    private const string areaTitle = "Data";

    private readonly ApplicationDbContext _context;

    public SchoolEventsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Data/SchoolEvents
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
			.Then("Manage School Events");       
		
		// Fetch descriptive data from the index dto to build the datatables index
		var metadata = DatatableExtensions.GetDtMetadata<SchoolEventsIndexViewModel>();
		
		return View(metadata);
   }

    // GET: Data/SchoolEvents/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage School Events", "Index", "SchoolEvents", new { Area = "Data" })
            .Then("School Event Details");            

        if (id == null)
        {
            return NotFound();
        }

        var schoolEvent = await _context.SchoolEvents
            .FirstOrDefaultAsync(m => m.Id == id);
        if (schoolEvent == null)
        {
            return NotFound();
        }

        return View(schoolEvent);
    }

    // GET: Data/SchoolEvents/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage School Events", "Index", "SchoolEvents", new { Area = "Data" })
            .Then("Create School Event");     

       return View();
	}

    // POST: Data/SchoolEvents/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] SchoolEvent schoolEvent)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage School Events", "Index", "SchoolEventsController", new { Area = "Data" })
        .Then("Create School Event");     
        
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);           

        if (ModelState.IsValid)
        {
            _context.Add(schoolEvent);
            await _context.SaveChangesAsync();
            
            _toast.Success("Created successfully.");
            
                return RedirectToAction(nameof(Index));
        }
        return View(schoolEvent);
    }

    // GET: Data/SchoolEvents/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage School Events", "Index", "SchoolEvents", new { Area = "Data" })
        .Then("Edit School Event");     

        if (id == null)
        {
            return NotFound();
        }

        var schoolEvent = await _context.SchoolEvents.FindAsync(id);
        if (schoolEvent == null)
        {
            return NotFound();
        }
        

        return View(schoolEvent);
    }

    // POST: Data/SchoolEvents/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(editBindingFields)] SchoolEvent schoolEvent)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage School Events", "Index", "SchoolEvents", new { Area = "Data" })
        .Then("Edit School Event");  
    
        if (id != schoolEvent.Id)
        {
            return NotFound();
        }
        
        SchoolEvent model = await _context.SchoolEvents.FindAsync(id);

        if (model != null)
        {
            model.Name = schoolEvent.Name;
            model.Description = schoolEvent.Description;
            model.StartTime = schoolEvent.StartTime;
            model.EndTime = schoolEvent.EndTime;
        }

        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(editBindingFields);

        if (!ModelState.IsValid) return View(schoolEvent);
        try
        {
            await _context.SaveChangesAsync();
            _toast.Success("Updated successfully.");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SchoolEventExists(schoolEvent.Id))
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

    // GET: Data/SchoolEvents/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage School Events", "Index", "SchoolEvents", new { Area = "Data" })
        .Then("Delete School Event");  

        if (id == null)
        {
            return NotFound();
        }

        var schoolEvent = await _context.SchoolEvents
            .FirstOrDefaultAsync(m => m.Id == id);
        if (schoolEvent == null)
        {
            return NotFound();
        }

        return View(schoolEvent);
    }

    // POST: Data/SchoolEvents/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var schoolEvent = await _context.SchoolEvents.FindAsync(id);
        if (schoolEvent != null) _context.SchoolEvents.Remove(schoolEvent);
        await _context.SaveChangesAsync();
        
        _toast.Success("School Event deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool SchoolEventExists(int id)
    {
        return _context.SchoolEvents.Any(e => e.Id == id);
    }


	[HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetSchoolEvents(DtRequest request)
    {
        try
		{
			var query = _context.SchoolEvents;
			var jsonData = await query.GetDatatableResponseAsync<SchoolEvent, SchoolEventsIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating School Events index json");
        }
        
        return StatusCode(500);
    }

}

