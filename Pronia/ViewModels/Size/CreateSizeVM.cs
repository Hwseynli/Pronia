namespace Pronia.ViewModels
{
	public class CreateSizeVM
	{
		[Required, MinLength(1),MaxLength(50)]
		public string Measure { get; set; }
	}
}

