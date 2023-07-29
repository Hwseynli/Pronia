namespace Pronia.Models
{
	public class Employee:BaseNameableEntity
	{
        [Required, MinLength(3), MaxLength(50)]
        public string Surname { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public Guid PositionId { get; set; }
        [ForeignKey("PositionId")]
        public Position Position { get; set; }
    }
}

