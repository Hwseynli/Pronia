namespace Pronia.Models
{
	public class Color:BaseNameableEntity
	{
		public ICollection<Product> Products { get; set; }
	}
}

