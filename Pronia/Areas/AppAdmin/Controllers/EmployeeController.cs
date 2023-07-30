namespace Pronia.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
    [AutoValidateAntiforgeryToken]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _context.Employees.Include(e => e.Position).ToListAsync();
            return View(employees);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _context.Positions.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM employee)
        {
            if (!ModelState.IsValid) return View();
            ViewBag.Positions = await _context.Positions.ToListAsync();
            bool result = await _context.Positions.AnyAsync(p => p.Id == employee.PositionId);
            if (!result)
            {
                ModelState.AddModelError("PositionId", "Bu id li position yoxdur");
                return View();
            }
            if (employee.Name.Capitalize==null)
            {
                ModelState.AddModelError("Name", "Duzgun deyer daxil edin!");
                return View();
            }
            if (employee.Surname.Capitalize == null)
            {
                ModelState.AddModelError("Surname", "Duzgun deyer daxil edin!");
                return View();
            }
            Employee employee1 = new Employee
            {
                Name = employee.Name.Capitalize(),
                Surname = employee.Surname.Capitalize(),
                PositionId = employee.PositionId
            };
            if (employee.Photo != null)
            {
                if (!employee.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Gonderilen file-nin tipi uygun deyil");
                    return View();
                }
                if (!employee.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Gonderilen file-nin hecmi 200 kb-den boyuk olmamalidir");
                    return View();
                }
                employee1.ImageUrl = await employee.Photo.CreateFileAsync(_env.WebRootPath, GlobalUsing.ImageRoot);
            }
            await _context.Employees.AddAsync(employee1);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id is null) return BadRequest();
            Employee existed = await _context.Employees.Include(e => e.Position).FirstOrDefaultAsync(c => c.Id == id);
            if (existed is null) return NotFound();
            ViewBag.Positions = await _context.Positions.ToListAsync();
            UpdateEmployeeVM employeeVM = new UpdateEmployeeVM
            {
                Name = existed.Name,
                ImageUrl = existed.ImageUrl,
                Surname = existed.Surname,
                PositionId = existed.PositionId
            };
            return View(employeeVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UpdateEmployeeVM employee)
        {
            if (id is null) return BadRequest();
            Employee existed = await _context.Employees.Where(e => e.Id == id).Include(e => e.Position).FirstOrDefaultAsync();
            if (existed is null) return NotFound();
            if (!ModelState.IsValid) return View(existed);
            ViewBag.Positions = await _context.Positions.ToListAsync();
            if (existed.PositionId != employee.PositionId)
            {
                bool result = await _context.Positions.AnyAsync(p => p.Id == employee.PositionId);
                if (!result)
                {
                    ModelState.AddModelError("PositionId", "Bu id li position yoxdur");
                    return View();
                }
                existed.PositionId = employee.PositionId;
            }
            if(employee.Name!=null && employee.Name.Capitalize!=null )existed.Name = employee.Name.Capitalize();
            if (employee.Surname!= null && employee.Surname.Capitalize()!=null) existed.Surname = employee.Surname.Capitalize();
            if (employee.Photo != null)
            {
                if (!employee.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi uygun deyil");
                    return View();
                }
                if (!employee.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "File hecmi 200 kb den cox olmamalidir");
                    return View();
                }
                existed.ImageUrl.DeleteFile(_env.WebRootPath, GlobalUsing.ImageRoot);
                existed.ImageUrl = await employee.Photo.CreateFileAsync(_env.WebRootPath, GlobalUsing.ImageRoot);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(s => s.Id == id);
            if (employee is null) return NotFound();
            employee.ImageUrl.DeleteFile(_env.WebRootPath,GlobalUsing.ImageRoot);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null) return BadRequest();
            Employee employee = await _context.Employees.Where(s => s.Id == id).Include(e=>e.Position).FirstOrDefaultAsync();
            if (employee is null) return NotFound();
            return View(employee);
        }
    }
}

