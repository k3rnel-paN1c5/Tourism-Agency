using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.IRepositories;
using Domain.Entities;
using Application.IServices.Auth;
using Application.IServices.UseCases;
using Application.Services.Auth;
using Application.Services.UseCases;
using Application.MappingProfiles;
using Infrastructure.Contexts;
using Infrastructure.DataSeeders;
using Infrastructure.Repositories;
using Application.IServices.UseCases.Post;
using Application.Services.UseCases.Post;



using System;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Controllers and Views
builder.Services.AddControllersWithViews();

builder.Services.AddControllers();


// Database Contexts
builder.Services.AddDbContext<TourismAgencyDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddDbContext<IdentityAppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Identity")));

// Register TourismAgencyDbContext as the default DbContext
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<TourismAgencyDbContext>());

builder.Services.AddAuthorization();
// Repositories 
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IRepository<Employee, string>, Repository<Employee, string>>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddIdentity<User, IdentityRole>(
    options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<IdentityAppDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>();



// Services
builder.Services.AddScoped<ICarBookingService, CarBookingService>();
builder.Services.AddScoped<IEmployeeAuthService, EmployeeAuthService>();
builder.Services.AddScoped<ICustomerAuthService, CustomerAuthService>();
builder.Services.AddScoped<IPostService, PostService>();

// Automapper
builder.Services.AddAutoMapper(
    typeof(CarBookingProfile),
    typeof(PostProfile) // ✅ Add PostProfile file
);

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var userManager = services.GetRequiredService<UserManager<User>>();
//     var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//     await IdentitySeed.SeedRolesAndAdmin(userManager, roleManager);
// }

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
