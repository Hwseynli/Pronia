namespace Pronia.Models
{
	public class Sku:BaseNameableEntity
	{
		public ICollection<Product> Products { get; set; }
	}
}

