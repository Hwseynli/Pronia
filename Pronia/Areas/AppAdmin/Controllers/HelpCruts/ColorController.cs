namespace Pronia.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;

        public ColorController(AppDbContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            List<Color> colors = await _context.Colors.Include(c => c.Products).ToListAsync();
            return View(colors);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateColorVM colorVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = _context.Colors.Any(c => c.Name.Trim().ToLower() == colorVM.Name.Trim().ToLower());

            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda reng artiq movcuddur");
                return View();
            }
            if (colorVM.Name.Capitalize == null)
            {
                ModelState.AddModelError("Name", "Duzgun ad daxil edin!");
                return View();
            }
            //Color color = _mapper.Map<Color>(colorVM);
            Color color = new Color
            {
                Name = colorVM.Name.Capitalize()
            };
            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null) return BadRequest();
            Color existed = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            UpdateColorVM colorVM = new UpdateColorVM
            {
                Name = existed.Name
            };
            return View(colorVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdateColorVM colorVM)
        {
            if (id == null) return BadRequest();
            Color existed = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            if (existed.Name == colorVM.Name)
            {
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                return View(existed);
            }
            bool result = _context.Colors.Any(c => c.Name.Trim().ToLower() == colorVM.Name.Trim().ToLower() && c.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda reng artiq movcuddur");
                return View(existed);
            }
            if (colorVM.Name.Capitalize() == null)
            {
                ModelState.AddModelError("Name", "Duzgun ad daxil edin!");
                return View();
            }

            existed.Name = colorVM.Name.Capitalize();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();
            Color existed = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Colors.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

