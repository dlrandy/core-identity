using IdentityManager.Data;
using IdentityManager.Models;
using IdentityManager.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddControllers()

builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(
      builder.Configuration.GetConnectionString("DefaultConnection")
      )
);

builder.Services.AddIdentity<ApplicationUser,IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddFluentEmail(builder.Configuration);
builder.Services.ConfigureApplicationCookie(opt => {

    opt.AccessDeniedPath = new PathString("/Account/NoAccess");
});
builder.Services.AddScoped<IEmailService, EmailService>();
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<IdentityOptions>(opt =>
{
    //opt.Password.RequireDigit = false;
    //opt.Password.RequireLowercase = false;
    //opt.Password.RequireNonAlphanumeric = false;
    opt.Lockout.MaxFailedAccessAttempts = 2;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(40);
    opt.SignIn.RequireConfirmedEmail = false;

});
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapControllers();

app.Run();

