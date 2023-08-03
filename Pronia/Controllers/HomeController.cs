using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.AppAdmin.DAL;

namespace Pronia.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    public HomeController(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        HomeVM homeVM = new HomeVM
        {
            Employees=await _context.Employees.Where(e=>e.Position.Name=="Customer").Take(3).Include(e=>e.Position).ToListAsync(),
            Sliders = await _context.Sliders.OrderBy(s=>s.Order).ToListAsync(),
            Products = await _context.Products.Include(p => p.Images).Include(p => p.Category).ToListAsync(),
        };
        return View(homeVM);
    }
}

