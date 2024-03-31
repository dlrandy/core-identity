using IdentityManager.Data;
using IdentityManager.Models;
using IdentityManager.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityManager.Services;
using IdentityManager.Constants;
using Microsoft.AspNetCore.Authorization;
using IdentityManager.Authorize;
using IdentityManager.Services.IServices;

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

builder.Services.AddScoped<INumberOfDaysForAccount, NumberOfDaysForAccount>();

builder.Services.AddScoped<IAuthorizationHandler, AdminWithOver1000DaysHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, FirstNameAuthHandler>();

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


builder.Services.AddAuthorization(opts =>
{

    opts.AddPolicy(SD.Admin, policy => policy.RequireRole(SD.Admin));
    opts.AddPolicy(SD.AdminAndUser, policy => policy.RequireRole(SD.Admin).RequireRole(SD.User));
    opts.AddPolicy(SD.AdminRole_CreateClaim, policy => policy.RequireRole(SD.Admin).RequireClaim(SD.Create, SD.True));
    opts.AddPolicy(SD.AdminRole_CreateEditDeleteClaim, policy => policy
                    .RequireRole(SD.Admin)
                    .RequireClaim(SD.Create, SD.True)
                    .RequireClaim(SD.Delete, SD.True)
                    .RequireClaim(SD.Edit, SD.True)
                 );
    opts.AddPolicy(SD.AdminRole_CreateEditDeleteClaim_OR_SuperAdminRole, policy => policy.RequireAssertion(context =>
    AdminRole_CreateEditDeleteClaim_OR_SuperAdminRole(context)
    ));
    opts.AddPolicy(SD.OnlySuperAdminChecker, p => p.Requirements.Add(new OnlySuperAdminChecker()));
    opts.AddPolicy(SD.AdminWithMoreThan1000Days, p => p.Requirements.Add(new AdminWithMoreThan1000DaysRequirement(1000)));
    opts.AddPolicy(SD.FirstNameAuth, p => p.Requirements.Add(new FirstNameAuthRequirement("test")));
});

// client value  3vy8Q~BAiTvB5q0zftCQthn.wOsP4ahL_LsEwcdG
// client  secret id  1bc83d78-11a7-4870-ac60-e196fd1317e9
// app id  be9cfba6-7481-4896-b56d-5b8dc4bc2302


builder.Services.AddAuthentication().AddMicrosoftAccount(opts => {

    opts.ClientId = "be9cfba6-7481-4896-b56d-5b8dc4bc2302";
    opts.ClientSecret = "3vy8Q~BAiTvB5q0zftCQthn.wOsP4ahL_LsEwcdG";
});
builder.Services.AddAuthentication().AddFacebook(opt =>
{
    opt.ClientId = "703446795330183";
    opt.ClientSecret = "4ade067b9326bbe9148a3c4f4f08c27e";
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


bool AdminRole_CreateEditDeleteClaim_OR_SuperAdminRole(AuthorizationHandlerContext context) {
    return (
        context.User.IsInRole(SD.Admin) && context.User.HasClaim(c => c.Type == SD.Create && c.Value == SD.True)
        && context.User.HasClaim(c => c.Type == SD.Edit && c.Value == SD.True)
        && context.User.HasClaim(c => c.Type == SD.Delete && c.Value == SD.True)
    )
    || context.User.IsInRole(SD.SuperAdmin);

}
