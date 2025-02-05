using cagmc.Response.WebApi.Infrastructure;
using cagmc.Response.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DbContext, ApplicationDbContext>(options => options.UseInMemoryDatabase("SampleDb"));
builder.Services.AddScoped<ICompanyService, CompanyService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

const string companyTag = "Companies";
app.MapGet("/api/companies", async (ICompanyService companyService, CancellationToken cancellationToken) =>
    {
        var response = await companyService.GetCompaniesAsync(cancellationToken);
        
        return response;
    })
    .WithTags(companyTag)
    .WithName("GetCompanies")
    .WithOpenApi();

app.MapGet("/api/companies/{id}",
    async (int id, ICompanyService companyService, CancellationToken cancellationToken) =>
    {
        var response = await companyService.GetCompanyAsync(id, cancellationToken);

        if (response is null)
        {
            return Results.NotFound(response);
        }
        
        return Results.Ok(response);
    })
    .WithTags(companyTag)
    .WithName("GetCompany")
    .WithOpenApi();

app.MapPost("/api/companies",
    async (CreateCompanyModel model, ICompanyService companyService, CancellationToken cancellationToken) =>
    {
        var response = await companyService.CreateAsync(model, cancellationToken);

        if (response.Code == 400)
        {
            return Results.BadRequest(response);
        }

        return Results.Ok(response);
    })
    .WithTags(companyTag)
    .WithName("CreateCompany")
    .WithOpenApi();

app.MapPut("/api/companies/{id}",
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
    .WithTags(companyTag)
    .WithName("UpdateCompany")
    .WithOpenApi();

app.MapDelete("/api/companies/{id}",
    async (int id, ICompanyService companyService, CancellationToken cancellationToken) =>
    {
        var response = await companyService.DeleteAsync(id, cancellationToken);

        if (response.Code == 404)
        {
            return Results.NotFound(response);
        }

        return Results.Ok(response);
    })
    .WithTags(companyTag)
    .WithName("DeleteCompany")
    .WithOpenApi();

await app.RunAsync();
