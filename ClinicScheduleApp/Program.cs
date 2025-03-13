using Microsoft.EntityFrameworkCore;

using ClinicScheduleApp.DataAccess;
using Microsoft.AspNetCore.Identity;
using ClinicScheduleApp.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connStr = builder.Configuration.GetConnectionString("FinalExamDb");
builder.Services.AddDbContext<ClinicScheduleDbContext>(options => options.UseSqlServer(connStr));

builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            // Configure password requirements
            options.Password.RequireDigit = true; // Require at least 1 digit
            options.Password.RequiredLength = 6; // Require at least 6 characters
            options.Password.RequireNonAlphanumeric = true; // Require at least one special character
        })
        .AddEntityFrameworkStores<ClinicScheduleDbContext>()
        .AddDefaultTokenProviders();

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

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    await ClinicScheduleDbContext.CreateAdminUser(scope.ServiceProvider);
}

app.Run();
