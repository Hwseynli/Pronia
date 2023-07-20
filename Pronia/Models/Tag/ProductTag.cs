namespace Pronia.Models
{
	public class ProductTag:BaseEntity
	{
		[Required]
		public Guid ProductId { get; set; }
		[ForeignKey("ProductId")]
        public Product Product { get; set; }
		[Required]
        public Guid TagId { get; set; }
		[ForeignKey("TagId")]
		public Tag Tag { get; set; }
	}
}

