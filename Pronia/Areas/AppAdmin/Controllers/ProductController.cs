﻿namespace Pronia.Areas.AppAdmin.Controllers
{
    [Area("AppAdmin")]
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
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
               .Include(p => p.Images
               .Where(pi => pi.IsPrimary == true))
               .Include(p => p.Category)
               .Include(p => p.SKU)
               .Include(p => p.Color)
               .Include(p => p.Size)
               .Include(p => p.ProductTags)
               .ThenInclude(pt => pt.Tag)
               .ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Skus = await _context.Skus.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Skus = await _context.Skus.ToListAsync();
            bool result = await _context.Categories.AnyAsync(c => c.Id == productVM.CategoryId);
            if (!result)
            {
                ModelState.AddModelError("CategoryId", "Bu id-li category movcud deyil");
                return View();
            }
            bool resultcol = await _context.Colors.AnyAsync(c => c.Id == productVM.ColorId);
            if (!resultcol)
            {
                ModelState.AddModelError("ColorId", "Bu id-li reng movcud deyil");
                return View();
            }
            bool resultsku = await _context.Skus.AnyAsync(c => c.Id == productVM.SkuId);
            if (!resultsku)
            {
                ModelState.AddModelError("SkuId", "Bu id-li sku movcud deyil");
                return View();
            }
            bool resultsize = await _context.Sizes.AnyAsync(c => c.Id == productVM.SizeId);
            if (!resultsize)
            {
                ModelState.AddModelError("SizeId", "Bu id-li olcu movcud deyil");
                return View();
            }
            if (productVM.Count < 0 || productVM.Count > 2147483647)
            {
                ModelState.AddModelError("Count", "Duzgun reqem daxil edin");
                return View();
            }
            Product product = new Product
            {
                CreateOn=DateTime.Now,
                Name = productVM.Name,
                Description = productVM.Description,
                Price = productVM.Price,
                Count = productVM.Count,
                SkuId = productVM.SkuId,
                CategoryId = productVM.CategoryId,
                ColorId = productVM.ColorId,
                SizeId = productVM.SizeId,
                ProductTags = new List<ProductTag>(),
                Images = new List<Image>()
            };
            if (productVM.TagIds != null && productVM.TagIds.Count > 0)
            {
                foreach (Guid tagId in productVM.TagIds)
                {
                    bool tagResult = await _context.Tags.AnyAsync(t => t.Id == tagId);
                    if (!tagResult)
                    {
                        ModelState.AddModelError("TagIds", $"{tagId} id-li Tag movcud deyil");
                        return View();
                    }
                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId,
                        Product = product
                    };
                    product.ProductTags.Add(productTag);
                }
            }
            if (productVM.MainPhoto != null)
            {
                if (!productVM.MainPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "File tipi uygun deyil");
                    return View();
                }
                if (!productVM.MainPhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("MainPhoto", "File olcusu uygun deyil");
                    return View();
                }
                Image mainImage = new Image
                {
                    Name = "ProductPhoto",
                    ImgUrl = await productVM.MainPhoto.CreateFileAsync(_env.WebRootPath, GlobalUsing.ImageRoot),
                    IsPrimary = true,
                    Product = product
                };
                product.Images.Add(mainImage);
            }
            if (productVM.HoverPhoto != null)
            {
                if (!productVM.HoverPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("HoverPhoto", "File tipi uygun deyil");
                    return View();
                }
                if (!productVM.HoverPhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("HoverPhoto", "File olcusu uygun deyil");
                    return View();
                }
                Image hoverImage = new Image
                {
                    Name = "ProductPhoto",
                    ImgUrl = await productVM.HoverPhoto.CreateFileAsync(_env.WebRootPath, GlobalUsing.ImageRoot),
                    IsPrimary = false,
                    Product = product
                };
                product.Images.Add(hoverImage);
            }
            if (productVM.Photos.Count > 0 && productVM.Photos != null)
            {
                TempData["PhotoError"] = "";
                foreach (IFormFile photo in productVM.Photos)
                {
                    if (photo != null)
                    {
                        if (!photo.CheckFileType("image/"))
                        {
                            TempData["PhotoError"] += $"{photo.FileName} file tipi uygun deyil\t";
                            continue;
                        }
                        if (!photo.CheckFileSize(200))
                        {
                            TempData["PhotoError"] += $"{photo.FileName} file olcusu uygun deyil\t";
                            continue;
                        }
                        Image addImage = new Image
                        {
                            Name = "ProductPhotos",
                            ImgUrl = await photo.CreateFileAsync(_env.WebRootPath, GlobalUsing.ImageRoot),
                            IsPrimary = null,
                            Product = product
                        };
                        product.Images.Add(addImage);
                    }
                }
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null) return BadRequest();
            Product product = await _context.Products.Where(p => p.Id == id).Include(p => p.Images).Include(p => p.ProductTags).FirstOrDefaultAsync();
            if (product is null) return NotFound();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Skus = await _context.Skus.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            UpdateProductVM productVM = new UpdateProductVM
            {
                Name = product.Name,
                Description = product.Description,
                SkuId = product.SkuId,
                Count = product.Count,
                Price = product.Price,
                CategoryId = product.CategoryId,
                ColorId = product.ColorId,
                SizeId = product.SizeId,
                TagIds = product.ProductTags.Select(pt => pt.TagId).ToList(),
            };
            productVM = MapImages(productVM, product);
            return View(productVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id,UpdateProductVM productVM)
        {
            if (id == null) return BadRequest();
            Product existed = await _context.Products.Where(p => p.Id == id).Include(p => p.Images).Include(p => p.ProductTags).Include(p=>p.Color).Include(p => p.Category).Include(p => p.Size).Include(p => p.SKU).FirstOrDefaultAsync();
            if (existed is null) return NotFound();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Skus = await _context.Skus.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            productVM = MapImages(productVM, existed);
            if (!ModelState.IsValid) return View(productVM);
            if (!await _context.Categories.AnyAsync(c => c.Id == productVM.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Bele bir category yoxdur");
                return View(productVM);
            }
            existed.CategoryId = productVM.CategoryId;
            if (!await _context.Colors.AnyAsync(c => c.Id == productVM.ColorId))
            {
                ModelState.AddModelError("ColorId", "Bele bir reng yoxdur");
                return View(productVM);
            }
            existed.ColorId = productVM.ColorId;
            if (!await _context.Skus.AnyAsync(c => c.Id == productVM.SkuId))
            {
                ModelState.AddModelError("SkuId", "Bele bir sku yoxdur");
                return View(productVM);
            }
            existed.SkuId = productVM.SkuId;
            if (!await _context.Sizes.AnyAsync(c => c.Id == productVM.SizeId))
            {
                ModelState.AddModelError("SizeId", "Bele bir olcu yoxdur");
                return View(productVM);
            }
            existed.SizeId = productVM.SizeId;
            if (productVM.Price > 0) existed.Price = productVM.Price;
            if (productVM.Description != null && productVM.Description.Length > 2001) existed.Description = productVM.Description;
            if (productVM.Name != null && productVM.Name.Length > 50) existed.Name = productVM.Name;
            if (productVM.Count >= 0) existed.Count = productVM.Count;
            if (productVM.TagIds is null)
            {
                ModelState.AddModelError("TagIds", "En azi 1 tag secin");
                return View(productVM);
            }
            if (productVM.MainPhoto != null)
            {
                if (!productVM.MainPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("MainPhoto", "Sheklin novu uygun deyil");
                    return View(productVM);
                }
                if (!productVM.MainPhoto.CheckFileSize(2000))
                {
                    ModelState.AddModelError("MainPhoto", "Sheklin olcusu uygun deyil");
                    return View(productVM);
                }
                Image mainImage = existed.Images.FirstOrDefault(pi => pi.IsPrimary == true);
                if (mainImage != null)
                {
                    mainImage.ImgUrl.DeleteFile(_env.WebRootPath, GlobalUsing.ImageRoot);
                    existed.Images.Remove(mainImage);
                }
                Image roomImage = new Image
                {
                    Name = "MainPhoto",
                    ProductId = existed.Id,
                    ImgUrl = await productVM.MainPhoto.CreateFileAsync(_env.WebRootPath, GlobalUsing.ImageRoot),
                    IsPrimary = true
                };
                existed.Images.Add(roomImage);
            }
            if (productVM.HoverPhoto != null)
            {
                if (!productVM.HoverPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("HoverPhoto", "Sheklin novu uygun deyil");
                    return View(productVM);
                }
                if (!productVM.HoverPhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("HoverPhoto", "Sheklin olcusu uygun deyil");
                    return View(productVM);
                }
                var mainImage = existed.Images.FirstOrDefault(pi => pi.IsPrimary == true);
                mainImage.ImgUrl.DeleteFile(_env.WebRootPath, GlobalUsing.ImageRoot);
                existed.Images.Remove(mainImage);
                Image roomImage = new Image
                {
                    Name = "HoverPhoto",
                    ProductId = existed.Id,
                    ImgUrl = await productVM.HoverPhoto.CreateFileAsync(_env.WebRootPath, GlobalUsing.ImageRoot),
                    IsPrimary = false
                };
                existed.Images.Add(roomImage);
            }
            if (existed.Images != null && existed.Images.Count > 0)
            {
                    List<Image> removeImageList = existed.Images.Where(pi => !productVM.ImagesIds.Contains(pi.Id) && pi.IsPrimary == null).ToList();
                    foreach (Image pImage in removeImageList)
                    {
                        pImage.ImgUrl.DeleteFile(_env.WebRootPath, GlobalUsing.ImageRoot);
                        existed.Images.Remove(pImage);
                    }
                if (productVM.Photos != null && productVM.Photos.Count > 0)
                {
                    TempData["PhotoError"] = "";
                    foreach (IFormFile photo in productVM.Photos)
                    {
                        if (!photo.CheckFileType("image/"))
                        {
                            TempData["PhotoError"] += $"{photo.FileName} file tipi uygun deyil\t";
                            continue;
                        }
                        if (!photo.CheckFileSize(2000))
                        {
                            TempData["PhotoError"] += $"{photo.FileName} file olcusu uygun deyil\t";
                            continue;
                        }
                        Image addImage = new Image
                        {
                            Name = "Exist picture",
                            ImgUrl = await photo.CreateFileAsync(_env.WebRootPath, GlobalUsing.ImageRoot),
                            IsPrimary = null,
                            ProductId = existed.Id
                        };

                        existed.Images.Add(addImage);
                    }
                }
            }
            List<Guid> createList = productVM.TagIds.Where(t => !existed.ProductTags.Any(pt => pt.TagId == t)).ToList();
            if (createList != null && createList.Count > 0)
            {
                foreach (Guid tagId in createList)
                {
                    bool tagResult = await _context.Tags.AnyAsync(pt => pt.Id == tagId);
                    if (!tagResult)
                    {
                        ModelState.AddModelError("TagIds", "Bele tag movcud deyil");
                        productVM = MapImages(productVM, existed);
                        return View(productVM);
                    }
                    ProductTag productTag = new ProductTag
                    {
                        ProductId = existed.Id,
                        TagId = tagId
                    };
                    existed.ProductTags.Add(productTag);
                }
                List<ProductTag> removeList = existed.ProductTags.Where(pt => !productVM.TagIds.Contains(pt.TagId)).ToList();
                _context.ProductTags.RemoveRange(removeList);
            }
            existed.UpdateOn = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null) return BadRequest();
            Product existed = await _context.Products.Include(r => r.Images).FirstOrDefaultAsync(r => r.Id == id);
            if (existed == null) return NotFound();
            if (existed.Images.Count > 0)
            {
                foreach (var item in existed.Images)
                {
                    item.ImgUrl.DeleteFile(_env.WebRootPath, GlobalUsing.ImageRoot);
                    _context.Images.Remove(item);
                }
            }
            _context.Products.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return BadRequest();
            Product product = await _context.Products.Where(s => s.Id == id).Include(e => e.Color).Include(p => p.Category).Include(p => p.SKU).Include(p => p.ProductTags).Include(p => p.ProductInfos).Include(p => p.Size).FirstOrDefaultAsync();
            if (product == null) return NotFound();
            return View(product);
        }
        public UpdateProductVM MapImages(UpdateProductVM productVM, Product product)
        {
            productVM.ImageVMs = new List<ImageVM>();
            foreach (Image image in product.Images)
            {
                ImageVM imageVM = new ImageVM
                {
                    Id = image.Id,
                    Name = image.Name,
                    ImageUrl = image.ImgUrl,
                    IsPrimary = image.IsPrimary
                };
                productVM.ImageVMs.Add(imageVM);
            }
            return productVM;
        }
    }
}

