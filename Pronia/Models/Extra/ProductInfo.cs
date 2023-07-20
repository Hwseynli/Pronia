namespace Pronia.Models
{
	public class ProductInfo:BaseNameableEntity
	{
		[Required,StringLength(3000)]
		public string Description { get; set; }
		[Required]
		public Guid ProductId { get; set; }
		[ForeignKey("ProductId")]
		public Product Product { get; set; }
	}
}

