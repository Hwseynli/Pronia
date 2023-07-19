using Microsoft.AspNetCore.Identity;
using System;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddIdentity<AppUser, IdentityRole>(ops =>
//{
//    ops.Password.RequireNonAlphanumeric = true;
//    ops.Password.RequiredLength = 0;
//    ops.Password.RequireUppercase = true;
//    ops.Password.RequireLowercase = true;
//    ops.Password.RequireDigit = true;
//    ops.Password.RequiredUniqueChars = 1;

//    ops.User.RequireUniqueEmail = true;

//    ops.Lockout.AllowedForNewUsers = true;
//    ops.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(55);
//    ops.Lockout.MaxFailedAccessAttempts = 3;
//}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

// Add services to the container.

//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews();
//builder.Services.AddControllersWithViews()
//     .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<RegistrUserValidator>())
//    .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<LoginUserValidator>());


//builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
//builder.Services.AddDbContext<AppDbContext>(ops =>
//{
//    ops.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
//});
//builder.Services.AddScoped<LayoutService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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

