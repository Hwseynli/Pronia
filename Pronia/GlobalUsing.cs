global using System;
global using Pronia.Models.Base;
global using Pronia.Areas.AppAdmin.Models;
global using Pronia;
global using Pronia.Models;
global using Pronia.Utilities.Extensions;
global using Pronia.ViewModels;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Pronia.Areas.AppAdmin.DAL;

namespace Pronia
{
	public static class GlobalUsing
	{
        public static string ImageRoot { get; set; } = @"assets/images/website-images";
    }
}