namespace Pronia.Models
{
	public class Image:BaseNameableEntity
	{
		[Required]
		public string ImgUrl { get; set; }
		public bool? IsPramery { get; set; } = false;
		[Required]
		public Guid ProductId { get; set; }
		[ForeignKey("ProductId")]
		public Product Product { get; set; }
	}
}

