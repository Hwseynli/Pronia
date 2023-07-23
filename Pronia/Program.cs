using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.AppAdmin.DAL;
using Pronia.Services;
using Pronia.Validators;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIdentity<AppUser, IdentityRole>(ops =>
{
    ops.Password.RequireNonAlphanumeric = false;
    ops.Password.RequiredLength = 0;
    ops.Password.RequireUppercase = true;
    ops.Password.RequireLowercase = true;
    ops.Password.RequireDigit = true;
    ops.Password.RequiredUniqueChars = 1;

    ops.User.RequireUniqueEmail = true;

    ops.Lockout.AllowedForNewUsers = true;
    ops.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(55);
    ops.Lockout.MaxFailedAccessAttempts = 3;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews()
     .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<RegistrUserValidator>())
    .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<LoginUserValidator>());

builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
builder.Services.AddDbContext<AppDbContext>(ops =>
{
    ops.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddScoped<LayoutService>();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();