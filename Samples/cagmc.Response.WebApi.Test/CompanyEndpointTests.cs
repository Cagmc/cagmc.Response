using System.Net;
using System.Net.Http.Json;
using cagmc.Response.Core;
using cagmc.Response.WebApi.Infrastructure;
using cagmc.Response.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace cagmc.Response.WebApi.Test;

public class CompanyEndpointTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetCompaniesAsync()
    {
        // Arrange
        var client = factory.CreateClient();
        List<Company> companies = 
        [
            new() { Id = 1, Name = "Company 1", YearFounded = 2021 },
            new() { Id = 2, Name = "Company 2", YearFounded = 2022 }
        ];

        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
            
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
            
            dbContext.AddRange(companies);
            await dbContext.SaveChangesAsync();
        }

        // Act
        var httpResponse = await client.GetAsync("/api/companies");
        
        // Assert
        httpResponse.EnsureSuccessStatusCode();
        
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        
        var listResponse = await httpResponse.Content.ReadFromJsonAsync<ListResponse<CompanyViewModel>>();

        Assert.NotNull(listResponse);
        Assert.NotNull(listResponse.Data);
        Assert.NotEmpty(listResponse.Data);
        Assert.Equal(companies.Count, listResponse.Data.Count);
        Assert.Equal(companies[0].Id, listResponse.Data[0].Id);
        Assert.Equal(companies[0].Name, listResponse.Data[0].Name);
        Assert.Equal(companies[0].YearFounded, listResponse.Data[0].YearFounded);
        Assert.Equal(companies[1].Id, listResponse.Data[1].Id);
        Assert.Equal(companies[1].Name, listResponse.Data[1].Name);
        Assert.Equal(companies[1].YearFounded, listResponse.Data[1].YearFounded);
    }
    
    [Fact]
    public async Task GetCompanyAsync()
    {
        // Arrange
        var client = factory.CreateClient();
        Company company = new() { Id = 1, Name = "Company 1", YearFounded = 2021 };
        
        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
            
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
            
            dbContext.Add(company);
            await dbContext.SaveChangesAsync();
        }
        
        // Act
        var httpResponse = await client.GetAsync("/api/companies/1");
        
        // Assert
        httpResponse.EnsureSuccessStatusCode();
        
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        
        var companyResponse = await httpResponse.Content.ReadFromJsonAsync<CompanyViewModel>();
        
        Assert.NotNull(companyResponse);
        Assert.Equal(company.Id, companyResponse.Id);
        Assert.Equal(company.Name, companyResponse.Name);
        Assert.Equal(company.YearFounded, companyResponse.YearFounded);
    }
    
    [Fact]
    public async Task CreateAsync()
    {
        // Arrange
        var client = factory.CreateClient();
        CreateCompanyModel model = new() { Name = "Company 1", YearFounded = 2021 };
        
        // Act
        var httpResponse = await client.PostAsJsonAsync("/api/companies", model);
        
        // Assert
        httpResponse.EnsureSuccessStatusCode();

        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
            
            var company = await dbContext.Set<Company>()
                .Where(x => x.Name == model.Name)
                .FirstOrDefaultAsync();
            
            Assert.NotNull(company);
            Assert.Equal(model.Name, company.Name);
            Assert.Equal(model.YearFounded, company.YearFounded);
        }
    }
    
    [Fact]
    public async Task UpdateAsync()
    {
        // Arrange
        var client = factory.CreateClient();
        Company company = new() { Id = 1, Name = "Company 1", YearFounded = 2021 };
    
        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
    
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
    
            dbContext.Add(company);
            await dbContext.SaveChangesAsync();
        }
    
        UpdateCompanyModel updateModel = new() { Name = "Updated Company", YearFounded = 2025 };
    
        // Act
        var httpResponse = await client.PutAsJsonAsync($"/api/companies/{company.Id}", updateModel);
    
        // Assert
        httpResponse.EnsureSuccessStatusCode();
    
        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
    
            var updatedCompany = await dbContext.Set<Company>()
                .FirstOrDefaultAsync(x => x.Id == company.Id);
    
            Assert.NotNull(updatedCompany);
            Assert.Equal(updateModel.Name, updatedCompany.Name);
            Assert.Equal(updateModel.YearFounded, updatedCompany.YearFounded);
        }
    }

    [Fact]
    public async Task DeleteAsync()
    {
        // Arrange
        var client = factory.CreateClient();
        Company company = new() { Id = 1, Name = "Company 1", YearFounded = 2021 };

        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            dbContext.Add(company);
            await dbContext.SaveChangesAsync();
        }

        // Act
        var httpResponse = await client.DeleteAsync($"/api/companies/{company.Id}");

        // Assert
        httpResponse.EnsureSuccessStatusCode();

        await using (var scope = factory.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

            var deletedCompany = await dbContext.Set<Company>()
                .FirstOrDefaultAsync(x => x.Id == company.Id);

            Assert.Null(deletedCompany);
        }
    }
}