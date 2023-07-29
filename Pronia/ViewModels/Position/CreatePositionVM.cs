namespace Pronia.ViewModels
{
	public class CreatePositionVM
	{
		[Required,MinLength(3),MaxLength(100)]
		public string Name { get; set; }
	}
}

