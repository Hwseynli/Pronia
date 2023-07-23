namespace Pronia.ViewModels
{
	public class CreateProductVM
	{
        [Required,MinLength(3),MaxLength(50)]
        public string Name { get; set; }
        [Required, StringLength(2000)]
        public string Description { get; set; }
        public ICollection<Guid> ProductInfoIds { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Count { get; set; }
        [Required, StringLength(200)]
        public string SKU { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Required]
        public Guid ColorId { get; set; }
        [ForeignKey("ColorId")]
        public Color Color { get; set; }
        [Required]
        public Guid SizeId { get; set; }
        [ForeignKey("SizeId")]
        public Size Size { get; set; }
        [Required]
        public IFormFile MainPhoto { get; set; }
        [Required]
        public IFormFile HoverPhoto { get; set; }
        public List<IFormFile> Photos { get; set; }
        public List<Guid> TagIds { get; set; }
    }
}

