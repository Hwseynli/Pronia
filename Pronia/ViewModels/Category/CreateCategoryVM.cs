namespace Pronia.ViewModels
{
	public class CreateCategoryVM
	{
		[Required,MinLength(3),MaxLength(40)]
		public string Name { get; set; }
	}
}

