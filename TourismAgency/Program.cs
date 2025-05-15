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
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Controllers and Views
builder.Services.AddControllersWithViews();
// builder.Services.AddControllers();


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

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// Services
builder.Services.AddScoped<IEmployeeAuthService, EmployeeAuthService>();
builder.Services.AddScoped<ICustomerAuthService, CustomerAuthService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<ITripPlanService, TripPlanService>();
builder.Services.AddScoped<ITripPlanCarService, TripPlanCarService>();
builder.Services.AddScoped<ITripBookingService, TripBookingService>();
builder.Services.AddScoped<IBookingService, BookingService>();
// Automapper
builder.Services.AddAutoMapper(
    typeof(CarBookingProfile),
    typeof(RegionProfile),
    typeof(TripProfile),
    typeof(TripPlanProfile),
    typeof(TripPlanCarProfile),
    typeof(TripBookingProfile),
    typeof(PostProfile)
);

var configuration = builder.Configuration;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
            )
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"message\": \"Unauthorized. Token is missing or invalid.\"}");
            }
        };
    });
builder.Services.AddAuthorization();


builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tourism Agency API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your token: Bearer {your token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    // Only include API controllers (those with [ApiController])
    c.DocInclusionPredicate((docName, description) =>
    {
        if (description.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controllerActionDescriptor)
        {
            return controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(true)
                .Any(attr => attr is ApiControllerAttribute);
        }
        return false;
    });
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TourismAgency API V1");
        c.RoutePrefix = "swagger"; // Makes it available at /swagger
    });
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

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthorization();




app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
