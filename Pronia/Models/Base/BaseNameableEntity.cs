namespace Pronia.Models.Base
{
	public abstract class BaseNameableEntity:BaseEntity
	{
		[Required,MinLength(3),MaxLength(100)]
		public string Name { get; set; }
	}
}