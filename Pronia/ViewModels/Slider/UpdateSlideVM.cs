﻿namespace Pronia.ViewModels
{
	public class UpdateSlideVM
	{
        public string Image { get; set; }
        public IFormFile Photo { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}