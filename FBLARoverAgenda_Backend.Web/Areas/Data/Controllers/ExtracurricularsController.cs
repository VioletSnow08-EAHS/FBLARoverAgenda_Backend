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
public class ExtracurricularsController : BaseController<ExtracurricularsController>
{
	public class ExtracurricularsIndexViewModel 
	{
		[Key]            
	    public string Id { get; set; }
	    public string Name { get; set; }
	    public string Description { get; set; }
	    public string MeetingDates { get; set; }
	    public string TeacherId { get; set; }
	}

	private const string createBindingFields = "Id,Name,Description,MeetingDates,TeacherId";
    private const string editBindingFields = "Id,Name,Description,MeetingDates,TeacherId";
    private const string areaTitle = "Data";

    private readonly ApplicationDbContext _context;

    public ExtracurricularsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Data/Extracurriculars
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
			.Then("Manage Extracurriculars");       
		
		// Fetch descriptive data from the index dto to build the datatables index
		var metadata = DatatableExtensions.GetDtMetadata<ExtracurricularsIndexViewModel>();
		
		return View(metadata);
   }

    // GET: Data/Extracurriculars/Details/5
    public async Task<IActionResult> Details(string id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Extracurriculars", "Index", "Extracurriculars", new { Area = "Data" })
            .Then("Extracurricular Details");            

        if (id == null)
        {
            return NotFound();
        }

        var extracurricular = await _context.Extracurriculars
                .Include(e => e.Teacher)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (extracurricular == null)
        {
            return NotFound();
        }

        return View(extracurricular);
    }

    // GET: Data/Extracurriculars/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Extracurriculars", "Index", "Extracurriculars", new { Area = "Data" })
            .Then("Create Extracurricular");     

        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id");
       return View();
	}

    // POST: Data/Extracurriculars/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] Extracurricular extracurricular)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Extracurriculars", "Index", "ExtracurricularsController", new { Area = "Data" })
        .Then("Create Extracurricular");     
        
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);           

        if (ModelState.IsValid)
        {
            extracurricular.Id = Guid.NewGuid().ToString();
            _context.Add(extracurricular);
            await _context.SaveChangesAsync();
            
            _toast.Success("Created successfully.");
            
                return RedirectToAction(nameof(Index));
            }
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", extracurricular.TeacherId);
        return View(extracurricular);
    }

    // GET: Data/Extracurriculars/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Extracurriculars", "Index", "Extracurriculars", new { Area = "Data" })
        .Then("Edit Extracurricular");     

        if (id == null)
        {
            return NotFound();
        }

        var extracurricular = await _context.Extracurriculars.FindAsync(id);
        if (extracurricular == null)
        {
            return NotFound();
        }
        
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", extracurricular.TeacherId);

        return View(extracurricular);
    }

    // POST: Data/Extracurriculars/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind(editBindingFields)] Extracurricular extracurricular)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Extracurriculars", "Index", "Extracurriculars", new { Area = "Data" })
        .Then("Edit Extracurricular");  
    
        if (id != extracurricular.Id)
        {
            return NotFound();
        }
        
        Extracurricular model = await _context.Extracurriculars.FindAsync(id);

        model.Name = extracurricular.Name;
        model.Description = extracurricular.Description;
        model.MeetingDates = extracurricular.MeetingDates;
        model.TeacherId = extracurricular.TeacherId;
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
                if (!ExtracurricularExists(extracurricular.Id))
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
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", extracurricular.TeacherId);
        return View(extracurricular);
    }

    // GET: Data/Extracurriculars/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Extracurriculars", "Index", "Extracurriculars", new { Area = "Data" })
        .Then("Delete Extracurricular");  

        if (id == null)
        {
            return NotFound();
        }

        var extracurricular = await _context.Extracurriculars
                .Include(e => e.Teacher)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (extracurricular == null)
        {
            return NotFound();
        }

        return View(extracurricular);
    }

    // POST: Data/Extracurriculars/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var extracurricular = await _context.Extracurriculars.FindAsync(id);
        _context.Extracurriculars.Remove(extracurricular);
        await _context.SaveChangesAsync();
        
        _toast.Success("Extracurricular deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool ExtracurricularExists(string id)
    {
        return _context.Extracurriculars.Any(e => e.Id == id);
    }


	[HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetExtracurriculars(DtRequest request)
    {
        try
		{
			var query = _context.Extracurriculars.Include(e => e.Teacher);
			var jsonData = await query.GetDatatableResponseAsync<Extracurricular, ExtracurricularsIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating Extracurriculars index json");
        }
        
        return StatusCode(500);
    }

}

