namespace Pronia.Areas.AppAdmin.Controllers
{

    [Area("ProniaAdmin")]
    [AutoValidateAntiforgeryToken]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Images
                .Where(pi => pi.IsPrimary == true))
                .Include(p => p.Category)
                .Include(p=>p.Color)
                .Include(p=>p.Size)
                .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag)
                .ToListAsync();

            return View(products);
        }
    }

}