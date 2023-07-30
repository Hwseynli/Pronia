using AutoMapper;
namespace Pronia.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    public class SkuController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SkuController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<Sku> skus = await _context.Skus.Include(c => c.Products).ToListAsync();
            return View(skus);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSkuVM skuVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = _context.Skus.Any(c => c.Name.Trim().ToLower() == skuVM.Name.Trim().ToLower());

            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda sku artiq movcuddur");
                return View();
            }
            if (skuVM.Name.Capitalize == null)
            {
                ModelState.AddModelError("Name", "Duzgun ad daxil edin!");
                return View();
            }
            //Sku sku = _mapper.Map<Sku>(skuVM);
            Sku sku = new Sku
            {
                Name = skuVM.Name.Capitalize()
            };
            await _context.Skus.AddAsync(sku);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null) return BadRequest();
            Sku existed = await _context.Skus.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            UpdateSkuVM skuVM = new UpdateSkuVM
            {
                Name = existed.Name
            };
            return View(skuVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdateSkuVM skuVM)
        {
            if (id == null) return BadRequest();
            Sku existed = await _context.Skus.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            if (existed.Name == skuVM.Name)
            {
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                return View(existed);
            }
            bool result = _context.Skus.Any(c => c.Name.Trim().ToLower() == skuVM.Name.Trim().ToLower() && c.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda sku artiq movcuddur");
                return View(existed);
            }
            if (skuVM.Name.Capitalize() == null)
            {
                ModelState.AddModelError("Name", "Duzgun ad daxil edin!");
                return View();
            }

            existed.Name = skuVM.Name.Capitalize();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();
            Sku existed = await _context.Skus.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();
            _context.Skus.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

