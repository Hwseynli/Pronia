namespace Pronia.Models
{
	public class Product:BaseNameableEntity
	{
		[Required,StringLength(2000)]
		public string Description { get; set; }
		[Required]
		public decimal Price { get; set; }
		[Required]
		public int Count { get; set; }
		[Required,StringLength(500)]
		public string SKU { get; set; }
		public ICollection<ProductInfo> ProductInfos{ get; set; }
		public ICollection<ProductTag> ProductTags { get; set; }
        public ICollection<Image> Images { get; set; }
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
	}
}

