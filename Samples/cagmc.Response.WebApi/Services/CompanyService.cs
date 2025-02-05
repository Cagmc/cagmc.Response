using cagmc.Response.Core;
using cagmc.Response.WebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace cagmc.Response.WebApi.Services;

public interface ICompanyService
{
    Task<ListResponse<CompanyViewModel>> GetCompaniesAsync(CancellationToken cancellationToken = default);
    Task<CompanyViewModel?> GetCompanyAsync(int id, CancellationToken cancellationToken = default);
    Task<Response.Core.Response> CreateAsync(CreateCompanyModel model, CancellationToken cancellationToken = default);
    Task<Response.Core.Response> UpdateAsync(int id, UpdateCompanyModel model, CancellationToken cancellationToken = default);
    Task<Response.Core.Response> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

internal sealed class CompanyService(DbContext dbContext) : ICompanyService
{
    public async Task<ListResponse<CompanyViewModel>> GetCompaniesAsync(CancellationToken cancellationToken = default)
    {
        var query = dbContext.Set<Company>();
        
        var items = await query
            .Select(x => new CompanyViewModel
            {
                Id = x.Id,
                Name = x.Name,
                YearFounded = x.YearFounded
            }).ToListAsync(cancellationToken);
        
        return new ListResponse<CompanyViewModel>(items);
    }

    public async Task<CompanyViewModel?> GetCompanyAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await dbContext.Set<Company>()
            .Where(x => x.Id == id)
            .Select(x => new CompanyViewModel
            {
                Id = x.Id,
                Name = x.Name,
                YearFounded = x.YearFounded
            }).FirstOrDefaultAsync(cancellationToken);

        return item;
    }

    public async Task<Core.Response> CreateAsync(CreateCompanyModel model, CancellationToken cancellationToken = default)
    {
        var existsWithName = await dbContext.Set<Company>()
            .AnyAsync(x => x.Name == model.Name, cancellationToken);

        if (existsWithName)
        {
            return Core.Response.BadRequest;
        }
        
        var entity = new Company
        {
            Name = model.Name,
            YearFounded = model.YearFounded
        };

        dbContext.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Core.Response.Success;
    }

    public async Task<Core.Response> UpdateAsync(int id, UpdateCompanyModel model, CancellationToken cancellationToken = default)
    {
        var existsWithName = await dbContext.Set<Company>()
            .AnyAsync(x => x.Name == model.Name && x.Id != id, cancellationToken);

        if (existsWithName)
        {
            return Core.Response.BadRequest;
        }
        
        var entity = await dbContext.Set<Company>()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (entity is null)
        {
            return Core.Response.NotFound;
        }
        
        entity.Name = model.Name;
        entity.YearFounded = model.YearFounded;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Core.Response.Success;
    }

    public async Task<Core.Response> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Set<Company>()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (entity is null)
        {
            return Core.Response.NotFound;
        }
        
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Core.Response.Success;
    }
}

public sealed record CreateCompanyModel
{
    public required string Name { get; init; }
    public required int YearFounded { get; init; }
}

public sealed record UpdateCompanyModel
{
    public required string Name { get; init; }
    public required int YearFounded { get; init; }
}

public sealed record CompanyViewModel
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int YearFounded { get; init; }
}