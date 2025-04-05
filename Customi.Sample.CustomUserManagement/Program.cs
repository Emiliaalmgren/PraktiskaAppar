using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Customi.Sample.CustomUserManagement.Data;
using Customi.Sample.CustomUserManagement.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("CustomiSampleCustomUserManagementContextConnection") ?? throw new InvalidOperationException("Connection string 'CustomiSampleCustomUserManagementContextConnection' not found.");;

builder.Services.AddDbContext<CustomiSampleCustomUserManagementContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<CustomiSampleCustomUserManagementContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
