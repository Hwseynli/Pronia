namespace Pronia.ViewModels
{
	public class UpdateEmployeeVM
	{
        [StringLength(30)]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        [StringLength(40)]
        public string Surname { get; set; }
        public Guid PositionId { get; set; }
        public IFormFile Photo { get; set; }
    }
}

