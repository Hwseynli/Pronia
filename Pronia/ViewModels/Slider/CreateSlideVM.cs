namespace Pronia.ViewModels
{
	public class CreateSlideVM
	{
        [Required, MinLength(3), MaxLength(100)]
        public string Title { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string SubTitle { get; set; }
        [Required, MinLength(3), MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}

