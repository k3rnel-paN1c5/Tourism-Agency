using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Contexts;
/// <summary>
/// Represents the database context for ASP.NET Core Identity.
/// Manages user accounts, roles, and related identity data.
/// </summary>
public class IdentityAppDbContext : IdentityDbContext<User>
{

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityAppDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by this context.</param>
    public IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options)
        : base(options)
    {

    }
    /// <summary>
    /// Configures the schema needed for the Identity framework.
    /// </summary>
    /// <param name="builder">The builder used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}

