using Microsoft.EntityFrameworkCore;

namespace cagmc.Response.WebApi.Infrastructure;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies { get; set; }
}

public sealed class Company
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int YearFounded { get; set; }
}