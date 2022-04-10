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
using FBLARoverAgenda_Backend.Domain.Entities.FAQ;
using FBLARoverAgenda_Backend.Infrastructure.Persistence.DbContexts;

namespace FBLARoverAgenda_Backend.Web.Areas.Data.Controllers;

[Area("Data")]
[Authorize(Roles = "Admin")]
public class FAQsController : BaseController<FAQsController>
{
	public class FAQsIndexViewModel 
	{
		[Key]            
	    public string Id { get; set; }
	    public string Question { get; set; }
	    public string Answer { get; set; }
	}

	private const string createBindingFields = "Id,Question,Answer";
    private const string editBindingFields = "Id,Question,Answer";
    private const string areaTitle = "Data";

    private readonly ApplicationDbContext _context;

    public FAQsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Data/FAQs
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
			.Then("Manage FAQs");       
		
		// Fetch descriptive data from the index dto to build the datatables index
		var metadata = DatatableExtensions.GetDtMetadata<FAQsIndexViewModel>();
		
		return View(metadata);
   }

    // GET: Data/FAQs/Details/5
    public async Task<IActionResult> Details(string id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage FAQs", "Index", "FAQs", new { Area = "Data" })
            .Then("FAQ Details");            

        if (id == null)
        {
            return NotFound();
        }

        var fAQ = await _context.FAQs
            .FirstOrDefaultAsync(m => m.Id == id);
        if (fAQ == null)
        {
            return NotFound();
        }

        return View(fAQ);
    }

    // GET: Data/FAQs/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage FAQs", "Index", "FAQs", new { Area = "Data" })
            .Then("Create FAQ");     

       return View();
	}

    // POST: Data/FAQs/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] FAQ fAQ)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage FAQs", "Index", "FAQsController", new { Area = "Data" })
        .Then("Create FAQ");     
        
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);           

        if (ModelState.IsValid)
        {
            fAQ.Id = Guid.NewGuid().ToString();
            _context.Add(fAQ);
            await _context.SaveChangesAsync();
            
            _toast.Success("Created successfully.");
            
                return RedirectToAction(nameof(Index));
            }
        return View(fAQ);
    }

    // GET: Data/FAQs/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage FAQs", "Index", "FAQs", new { Area = "Data" })
        .Then("Edit FAQ");     

        if (id == null)
        {
            return NotFound();
        }

        var fAQ = await _context.FAQs.FindAsync(id);
        if (fAQ == null)
        {
            return NotFound();
        }
        

        return View(fAQ);
    }

    // POST: Data/FAQs/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind(editBindingFields)] FAQ fAQ)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage FAQs", "Index", "FAQs", new { Area = "Data" })
        .Then("Edit FAQ");  
    
        if (id != fAQ.Id)
        {
            return NotFound();
        }
        
        FAQ model = await _context.FAQs.FindAsync(id);

        model.Question = fAQ.Question;
        model.Answer = fAQ.Answer;
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
                if (!FAQExists(fAQ.Id))
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
        return View(fAQ);
    }

    // GET: Data/FAQs/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage FAQs", "Index", "FAQs", new { Area = "Data" })
        .Then("Delete FAQ");  

        if (id == null)
        {
            return NotFound();
        }

        var fAQ = await _context.FAQs
            .FirstOrDefaultAsync(m => m.Id == id);
        if (fAQ == null)
        {
            return NotFound();
        }

        return View(fAQ);
    }

    // POST: Data/FAQs/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var fAQ = await _context.FAQs.FindAsync(id);
        _context.FAQs.Remove(fAQ);
        await _context.SaveChangesAsync();
        
        _toast.Success("FAQ deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool FAQExists(string id)
    {
        return _context.FAQs.Any(e => e.Id == id);
    }


	[HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetFAQs(DtRequest request)
    {
        try
		{
			var query = _context.FAQs;
			var jsonData = await query.GetDatatableResponseAsync<FAQ, FAQsIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating FAQs index json");
        }
        
        return StatusCode(500);
    }

}

