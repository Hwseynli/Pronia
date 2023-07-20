namespace Pronia.Models
{
	public class Setting:BaseEntity
	{
		[Required,StringLength(1000)]
		public string Key { get; set; }
        [Required, StringLength(1000)]
        public string Value { get; set; }
		[StringLength(1000)]
		public string Link { get; set; }
	}
}

