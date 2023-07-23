namespace Pronia.ViewModels
{
	public class UpdateProductVM
	{
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        [StringLength(200)]
        public string SKU { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ColorId { get; set; }
        public Guid SizeId { get; set; }
        public IFormFile MainPhoto { get; set; }
        public IFormFile HoverPhoto { get; set; }
        public List<IFormFile> Photos { get; set; }
        public List<Guid> TagIds { get; set; }
        public List<Guid> ProductInfoIds { get; set; }
        public List<ImageVM> ImageVMs { get; set; }
        public List<InfoVM> InfoVMs{ get; set; }
        public List<Guid> ImagesIds { get; set; }
    }

    public class ImageVM
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsPrimary { get; set; }
    }
    public class InfoVM
    {
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
    }
}

