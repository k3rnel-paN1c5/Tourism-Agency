using DataAccess.Entities;
using DataAccess.Repositories.IRepositories;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Identity;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Controllers and Views
builder.Services.AddControllersWithViews();
// Database Contexts
builder.Services.AddDbContext<TourismAgencyDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDbContext<IdentityAppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Identity")));
    
// Register TourismAgencyDbContext as the default DbContext
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<TourismAgencyDbContext>());

// Repositories 
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IRepository<Employee, string>, Repository<Employee, string>>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<BusinessLogic.IServices.IEmployeeAuthService, BusinessLogic.Services.EmployeeAuthService>();
builder.Services.AddIdentity<User, IdentityRole>(
    options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<TourismAgencyDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>();



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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
