namespace Pronia.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;
        public PositionController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Position> positions = await _context.Positions.Include(c => c.Employees).ToListAsync();
            return View(positions);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionVM positionVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = _context.Positions.Any(c => c.Name.Trim().ToLower() == positionVM.Name.Trim().ToLower());

            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda categoriya artiq movcuddur");
                return View();
            }
            Position position = new Position
            {
                Name = positionVM.Name
            };
            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null) return BadRequest();
            Position existed = await _context.Positions.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            return View(existed);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdatePositionVM positionVM)
        {
            if (id == null) return BadRequest();
            Position existed = await _context.Positions.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            if (existed.Name == positionVM.Name)
            {
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                return View(existed);
            }
            bool result = _context.Positions.Any(c => c.Name.Trim().ToLower() == positionVM.Name.Trim().ToLower() && c.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda categoriya artiq movcuddur");
                return View(existed);
            }
            existed.Name = positionVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();
            Position existed = await _context.Positions.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Positions.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }

}

