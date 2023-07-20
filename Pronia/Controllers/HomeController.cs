using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers;

public class HomeController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}

