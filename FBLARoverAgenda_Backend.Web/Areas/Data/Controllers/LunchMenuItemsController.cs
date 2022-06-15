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
using FBLARoverAgenda_Backend.Domain.Entities.LunchMenuItem;
using FBLARoverAgenda_Backend.Infrastructure.Persistence.DbContexts;

namespace FBLARoverAgenda_Backend.Web.Areas.Data.Controllers;

[Area("Data")]
[Authorize(Roles = "Admin")]
public class LunchMenuItemsController : BaseController<LunchMenuItemsController>
{
	public class LunchMenuItemsIndexViewModel 
	{
		[Key]            
	    public string Id { get; set; }
	    public string Name { get; set; }
	    public DateTime StartTime { get; set; }
	    public DateTime EndTime { get; set; }
	}

	private const string createBindingFields = "Id,Name,StartTime,EndTime";
    private const string editBindingFields = "Id,Name,StartTime,EndTime";
    private const string areaTitle = "Data";

    private readonly ApplicationDbContext _context;

    public LunchMenuItemsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Data/LunchMenuItems
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
			.Then("Manage LunchMenuItems");       
		
		// Fetch descriptive data from the index dto to build the datatables index
		var metadata = DatatableExtensions.GetDtMetadata<LunchMenuItemsIndexViewModel>();
		
		return View(metadata);
   }

    // GET: Data/LunchMenuItems/Details/5
    public async Task<IActionResult> Details(string id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage LunchMenuItems", "Index", "LunchMenuItems", new { Area = "Data" })
            .Then("LunchMenuItem Details");            

        if (id == null)
        {
            return NotFound();
        }

        var lunchMenuItem = await _context.LunchMenuItems
            .FirstOrDefaultAsync(m => m.Id == id);
        if (lunchMenuItem == null)
        {
            return NotFound();
        }

        return View(lunchMenuItem);
    }

    // GET: Data/LunchMenuItems/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage LunchMenuItems", "Index", "LunchMenuItems", new { Area = "Data" })
            .Then("Create LunchMenuItem");     

       return View();
	}

    // POST: Data/LunchMenuItems/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] LunchMenuItem lunchMenuItem)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage LunchMenuItems", "Index", "LunchMenuItemsController", new { Area = "Data" })
        .Then("Create LunchMenuItem");     
        
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);

        if (!ModelState.IsValid) return View(lunchMenuItem);
        _context.Add(lunchMenuItem);
        await _context.SaveChangesAsync();
            
        _toast.Success("Created successfully.");
            
        return RedirectToAction(nameof(Index));
    }

    // GET: Data/LunchMenuItems/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage LunchMenuItems", "Index", "LunchMenuItems", new { Area = "Data" })
        .Then("Edit LunchMenuItem");     

        if (id == null)
        {
            return NotFound();
        }

        var lunchMenuItem = await _context.LunchMenuItems.FindAsync(id);
        if (lunchMenuItem == null)
        {
            return NotFound();
        }
        

        return View(lunchMenuItem);
    }

    // POST: Data/LunchMenuItems/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind(editBindingFields)] LunchMenuItem lunchMenuItem)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage LunchMenuItems", "Index", "LunchMenuItems", new { Area = "Data" })
        .Then("Edit LunchMenuItem");  
    
        if (id != lunchMenuItem.Id)
        {
            return NotFound();
        }
        
        LunchMenuItem model = await _context.LunchMenuItems.FindAsync(id);

        if (model != null)
        {
            model.Name = lunchMenuItem.Name;
            model.StartTime = lunchMenuItem.StartTime;
            model.EndTime = lunchMenuItem.EndTime;
        }

        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(editBindingFields);

        if (!ModelState.IsValid) return View(lunchMenuItem);
        try
        {
            await _context.SaveChangesAsync();
            _toast.Success("Updated successfully.");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LunchMenuItemExists(lunchMenuItem.Id))
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

    // GET: Data/LunchMenuItems/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage LunchMenuItems", "Index", "LunchMenuItems", new { Area = "Data" })
        .Then("Delete LunchMenuItem");  

        if (id == null)
        {
            return NotFound();
        }

        var lunchMenuItem = await _context.LunchMenuItems
            .FirstOrDefaultAsync(m => m.Id == id);
        if (lunchMenuItem == null)
        {
            return NotFound();
        }

        return View(lunchMenuItem);
    }

    // POST: Data/LunchMenuItems/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var lunchMenuItem = await _context.LunchMenuItems.FindAsync(id);
        if (lunchMenuItem != null) _context.LunchMenuItems.Remove(lunchMenuItem);
        await _context.SaveChangesAsync();
        
        _toast.Success("LunchMenuItem deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool LunchMenuItemExists(string id)
    {
        return _context.LunchMenuItems.Any(e => e.Id == id);
    }


	[HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetLunchMenuItems(DtRequest request)
    {
        try
		{
			var query = _context.LunchMenuItems;
			var jsonData = await query.GetDatatableResponseAsync<LunchMenuItem, LunchMenuItemsIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating LunchMenuItems index json");
        }
        
        return StatusCode(500);
    }

}

