namespace Pronia.ViewModels
{
	public class CreateProductVM
	{
        [Required,MinLength(3),MaxLength(50)]
        public string Name { get; set; }
        [Required, StringLength(2000)]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public Guid SkuId { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid ColorId { get; set; }
        [Required]
        public Guid SizeId { get; set; }
        [Required]
        public IFormFile MainPhoto { get; set; }
        [Required]
        public IFormFile HoverPhoto { get; set; }
        public List<IFormFile> Photos { get; set; }
        public List<Guid> TagIds { get; set; }
    }
}

