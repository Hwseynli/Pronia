namespace Pronia.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
           ICollection<Slider> sliders = await _context.Sliders.ToListAsync();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!slideVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Gonderilen file-nin tipi uygun deyil");
                return View();
            }
            if (!slideVM.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Gonderilen file-nin hecmi 200 kb-den boyuk olmamalidir");
                return View();
            }
            if (slideVM.Order < 1)
            {
                ModelState.AddModelError("Order", "Duzgun ve musbet deyer daxil edin!");
                return View();
            }
            Slider slide = new Slider
            {
                Title = slideVM.Title.Trim().ToString(),
                SubTitle = slideVM.SubTitle.Trim().ToString(),
                Description = slideVM.Description.Trim().ToString(),
                Order=slideVM.Order
            };
            slide.ImgUrl = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, @"assets/images/website-images");
            await _context.Sliders.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id is null) return BadRequest();
            Slider slide = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null) return NotFound();
            UpdateSlideVM slideVM = new UpdateSlideVM
            {
                Title = slide.Title,
                SubTitle = slide.SubTitle,
                Order = slide.Order,
                Description = slide.Description,
                Image = slide.ImgUrl
            };
            return View(slideVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdateSlideVM slideVM)
        {
            if (id == null) return BadRequest();
            Slider existed = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (existed == null) return NotFound();
            if (slideVM.Photo != null)
            {
                if (!slideVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi uygun deyil");
                    return View();
                }
                if (!slideVM.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "File hecmi 200 kb den cox olmamalidir");
                    return View();
                }
                existed.ImgUrl.DeleteFile(_env.WebRootPath, @"assets/images/website-images");
                existed.ImgUrl = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, @"assets/images/website-images");
            }
            existed.Order = slideVM.Order;
            existed.Title = slideVM.Title;
            existed.SubTitle = slideVM.SubTitle;
            existed.Description = slideVM.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();
            Slider slide = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null) return NotFound();
            slide.ImgUrl.DeleteFile(_env.WebRootPath, @"assets/images/website-images");
            _context.Sliders.Remove(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return BadRequest();
            Slider slide = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null) return NotFound();
            return View(slide);
        }
    }
}

