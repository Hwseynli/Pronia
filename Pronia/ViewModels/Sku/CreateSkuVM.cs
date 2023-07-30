namespace Pronia.ViewModels
{
	public class CreateSkuVM
	{
		[Required,MinLength(3),MaxLength(100)]
		public string Name { get; set; }
	}
}

