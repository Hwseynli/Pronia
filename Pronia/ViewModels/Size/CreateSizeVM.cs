namespace Pronia.ViewModels
{
	public class CreateSizeVM
	{
		[Required, MinLength(1),MaxLength(100)]
		public string Measure { get; set; }
	}
}

