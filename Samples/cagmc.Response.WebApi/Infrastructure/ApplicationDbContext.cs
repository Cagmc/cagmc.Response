using cagmc.Response.WebApi.Domain.Companies;

using Microsoft.EntityFrameworkCore;

namespace cagmc.Response.WebApi.Infrastructure;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies { get; set; }
}
