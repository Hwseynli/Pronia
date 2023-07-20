using Microsoft.AspNetCore.Identity;
namespace Pronia.Areas.AppAdmin.Models
{
	public class AppUser:IdentityUser
	{
		[Required,MinLength(3),MaxLength(100)]
		public string FullName { get; set; }
		[Required,StringLength(25)]
		public string Gender { get; set; }
		[Required]
		public byte Age { get; set; }
		[StringLength(2000)]
		public string UserImgUrl { get; set; } = "usernoimg.jpeg";
    }
}

