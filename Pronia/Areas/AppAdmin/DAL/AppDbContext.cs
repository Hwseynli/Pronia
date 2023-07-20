using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.Areas.AppAdmin.DAL
{
	public class AppDbContext:IdentityDbContext<AppUser>
	{
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ProductInfo> ProductInfos { get; set; }
    }
}

