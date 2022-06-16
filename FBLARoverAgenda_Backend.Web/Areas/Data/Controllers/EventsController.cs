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
using FBLARoverAgenda_Backend.Domain.Entities.Calendaric;
using FBLARoverAgenda_Backend.Infrastructure.Persistence.DbContexts;

namespace FBLARoverAgenda_Backend.Web.Areas.Data.Controllers;

[Area("Data")]
[Authorize(Roles = "Admin")]
public class EventsController : BaseController<EventsController>
{
	public class EventsIndexViewModel 
	{
		[Key]            
	    public string Id { get; set; }
	    public DateTime StartTime { get; set; }
	    public DateTime EndTime { get; set; }
	    public string Subject { get; set; }
	    public string Color { get; set; }
	    public string RecurrenceRule { get; set; }
	    public string UserId { get; set; }
	}

	private const string createBindingFields = "Id,StartTime,EndTime,Subject,Color,RecurrenceRule,UserId";
    private const string editBindingFields = "Id,StartTime,EndTime,Subject,Color,RecurrenceRule,UserId";
    private const string areaTitle = "Data";

    private readonly ApplicationDbContext _context;

    public EventsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Data/Events
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
			.Then("Manage Events");       
		
		// Fetch descriptive data from the index dto to build the datatables index
		var metadata = DatatableExtensions.GetDtMetadata<EventsIndexViewModel>();
		
		return View(metadata);
   }

    // GET: Data/Events/Details/5
    public async Task<IActionResult> Details(string id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Events", "Index", "Events", new { Area = "Data" })
            .Then("Event Details");            

        if (id == null)
        {
            return NotFound();
        }

        var @event = await _context.Events
                .Include(x => x.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }

    // GET: Data/Events/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Events", "Index", "Events", new { Area = "Data" })
            .Then("Create Event");     

        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
       return View();
	}

    // POST: Data/Events/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] Event @event)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Events", "Index", "EventsController", new { Area = "Data" })
        .Then("Create Event");     
        
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);           

        if (ModelState.IsValid)
        {
            @event.Id = Guid.NewGuid().ToString();
            _context.Add(@event);
            await _context.SaveChangesAsync();
            
            _toast.Success("Created successfully.");
            
                return RedirectToAction(nameof(Index));
            }
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", @event.UserId);
        return View(@event);
    }

    // GET: Data/Events/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Events", "Index", "Events", new { Area = "Data" })
        .Then("Edit Event");     

        if (id == null)
        {
            return NotFound();
        }

        var @event = await _context.Events.FindAsync(id);
        if (@event == null)
        {
            return NotFound();
        }
        
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", @event.UserId);

        return View(@event);
    }

    // POST: Data/Events/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind(editBindingFields)] Event @event)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Events", "Index", "Events", new { Area = "Data" })
        .Then("Edit Event");  
    
        if (id != @event.Id)
        {
            return NotFound();
        }
        
        Event model = await _context.Events.FindAsync(id);

        model.StartTime = @event.StartTime;
        model.EndTime = @event.EndTime;
        model.Subject = @event.Subject;
        model.Color = @event.Color;
        model.RecurrenceRule = @event.RecurrenceRule;
        model.UserId = @event.UserId;
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
                if (!EventExists(@event.Id))
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
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", @event.UserId);
        return View(@event);
    }

    // GET: Data/Events/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Events", "Index", "Events", new { Area = "Data" })
        .Then("Delete Event");  

        if (id == null)
        {
            return NotFound();
        }

        var @event = await _context.Events
                .Include(x => x.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }

    // POST: Data/Events/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var @event = await _context.Events.FindAsync(id);
        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();
        
        _toast.Success("Event deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool EventExists(string id)
    {
        return _context.Events.Any(e => e.Id == id);
    }


	[HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetEvents(DtRequest request)
    {
        try
		{
			var query = _context.Events.Include(x => x.User);
			var jsonData = await query.GetDatatableResponseAsync<Event, EventsIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating Events index json");
        }
        
        return StatusCode(500);
    }

}

