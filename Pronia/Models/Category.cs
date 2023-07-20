namespace Pronia.Models
{
	public class Category:BaseNameableEntity
	{
		public ICollection<Product> Products { get; set; }
	}
}