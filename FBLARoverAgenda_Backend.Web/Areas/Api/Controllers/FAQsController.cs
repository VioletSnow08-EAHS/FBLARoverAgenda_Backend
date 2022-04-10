using System.Collections.Generic;
using System.Linq;
using FBLARoverAgenda_Backend.Domain.Entities.FAQ;
using FBLARoverAgenda_Backend.Infrastructure.Persistence.DbContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBLARoverAgenda_Backend.Web.Areas.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Produces("application/json")]
[Area("Api")]
public class FAQsController : Controller
{
    private readonly ApplicationDbContext _context;

    public FAQsController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET
    public List<FAQ> Index()
    {
        return _context.FAQs.ToList();
    }
}