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

app.MapCompanyEndpoints();

await app.RunAsync();

public partial class Program { protected Program() { } }
