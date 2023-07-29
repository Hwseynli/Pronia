namespace Pronia.ViewModels
{
	public class CreateEmployeeVM
	{
        [Required,MinLength(3),MaxLength(30)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(40)]
        public string Surname { get; set; }
        [Required]
        public Guid PositionId { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}

