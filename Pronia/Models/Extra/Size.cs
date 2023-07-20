namespace Pronia.Models
{
	public class Size:BaseEntity
	{
		[Required,StringLength(200)]
		public string Measure { get; set; }
		public ICollection<Product> Products { get; set; }
	}
}

