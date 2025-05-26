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
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("http://localhost:5173") // Your frontend URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Optional, if you send cookies/auth headers
    });
});

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
builder.Services.AddScoped<ICustomerAuthService, CustomerAuthService>();
builder.Services.AddScoped<IEmployeeAuthService, EmployeeAuthService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ICarBookingService, CarBookingService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<ITripPlanCarService, TripPlanCarService>();
builder.Services.AddScoped<ITripPlanService, TripPlanService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<ITripBookingService, TripBookingService>();


// Automapper
builder.Services.AddAutoMapper(
    typeof(BookingProfile),
    typeof(CarBookingProfile),
    typeof(CarProfile),
    typeof(CategoryProfile),
    typeof(PaymentProfile),
    typeof(PostProfile),
    typeof(RegionProfile),
    typeof(TripBookingProfile),
    typeof(TripPlanCarProfile),
    typeof(TripPlanProfile),
    typeof(TripProfile)
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

/////////////////        DEBUG       ///////////////////////////
///
/*
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });
*/
//////////////////////////////////////////

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentitySeed.SeedRolesAndAdmin(userManager, roleManager);
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:5173");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "*");
        context.Response.StatusCode = 200;
        return;
    }

    await next(context);
});

app.UseRouting();
app.UseCors("AllowAll"); 

app.UseAuthentication();
app.UseAuthorization();




app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();