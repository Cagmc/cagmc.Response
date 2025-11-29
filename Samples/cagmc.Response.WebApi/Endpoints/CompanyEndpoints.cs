using cagmc.Response.Core;
using cagmc.Response.WebApi.Domain.Companies;
using cagmc.Response.WebApi.Services;

using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Builder;

public static class CompanyEndpoints
{
    public static WebApplication MapCompanyEndpoints(this WebApplication app)
    {
        app.MapGroup("/api/companies")
            .MapEndpoints()
            .WithTags("Companies")
            .MapOpenApi();
        
        return app;
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("", async (
                [FromQuery] string? search,
                [FromQuery] int? pageIndex, 
                [FromQuery] int pageSize,
                [FromQuery] bool? isAscending,
                [FromQuery] string? sortByColumn,
                [FromServices] ICompanyService companyService, CancellationToken cancellationToken) =>
            {
                var filter = new ListFilter
                {
                    Search = search,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    SortByColumn = sortByColumn,
                    IsAscending = isAscending
                };
                
                var response = await companyService.GetCompaniesAsync(filter, cancellationToken);
        
                return response;
            })
            .WithName("GetCompanies");
        
        groupBuilder.MapGet("/{id}",
            async ([FromRoute]int id,
                [FromServices]ICompanyService companyService, CancellationToken cancellationToken) =>
            {
                var response = await companyService.GetCompanyAsync(id, cancellationToken);
        
                if (response is null)
                {
                    return Results.NotFound(response);
                }
                
                return Results.Ok(response);
            })
            .WithName("GetCompany");
        
        groupBuilder.MapPost("",
            async (CreateCompanyModel model, ICompanyService companyService, CancellationToken cancellationToken) =>
            {
                var response = await companyService.CreateAsync(model, cancellationToken);
        
                if (response.Code == 400)
                {
                    return Results.BadRequest(response);
                }
        
                return Results.Ok(response);
            })
            .WithName("CreateCompany");
        
        groupBuilder.MapPut("/{id}",
            async (int id, UpdateCompanyModel model, ICompanyService companyService, CancellationToken cancellationToken) =>
            {
                var response = await companyService.UpdateAsync(id, model, cancellationToken);
                
                if (response.Code == 400)
                {
                    return Results.BadRequest(response);
                }
        
                if (response.Code == 404)
                {
                    return Results.NotFound(response);
                }
                
                return Results.Ok();
            })
            .WithName("UpdateCompany");
        
        groupBuilder.MapDelete("/{id}",
            async (int id, ICompanyService companyService, CancellationToken cancellationToken) =>
            {
                var response = await companyService.DeleteAsync(id, cancellationToken);
        
                if (response.Code == 404)
                {
                    return Results.NotFound(response);
                }
        
                return Results.Ok(response);
            })
            .WithName("DeleteCompany");
        
        return groupBuilder;
    }
}