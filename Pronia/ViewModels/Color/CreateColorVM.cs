namespace Pronia.ViewModels
{
	public class CreateColorVM
	{
        [Required, MinLength(3), MaxLength(40)]
        public string Name { get; set; }
    }
}

