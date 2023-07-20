namespace Pronia.Models
{
	public class Tag:BaseNameableEntity
	{
		public ICollection<ProductTag> ProductTags { get; set; }
	}
}

