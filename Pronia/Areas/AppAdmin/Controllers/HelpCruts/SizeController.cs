namespace Pronia.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    public class SizeController : Controller
    {
        private readonly AppDbContext _context;

        public SizeController(AppDbContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            List<Size> sizes = await _context.Sizes.Include(c => c.Products).ToListAsync();
            return View(sizes);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSizeVM sizeVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = _context.Sizes.Any(c => c.Measure.Trim().ToLower() == sizeVM.Measure.Trim().ToLower());

            if (result)
            {
                ModelState.AddModelError("Measure", "Bu adda reng artiq movcuddur");
                return View();
            }
            //Size size = _mapper.Map<Size>(sizeVM);
            Size size = new Size
            {
                Measure = sizeVM.Measure.Trim()
            };
            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null) return BadRequest();
            Size existed = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            UpdateSizeVM sizeVM = new UpdateSizeVM
            {
                Measure = existed.Measure
            };
            return View(sizeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdateSizeVM sizeVM)
        {
            if (id == null) return BadRequest();
            Size existed = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            if (existed.Measure == sizeVM.Measure)
            {
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                return View(existed);
            }
            bool result = _context.Sizes.Any(c => c.Measure.Trim().ToLower() == sizeVM.Measure.Trim().ToLower() && c.Id != id);
            if (result)
            {
                ModelState.AddModelError("Measure", "Bu adda olcu artiq movcuddur");
                return View(existed);
            }
            existed.Measure = sizeVM.Measure.Trim();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();
            Size existed = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Sizes.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

