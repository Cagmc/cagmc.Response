namespace cagmc.Response.WebApi.Domain.Companies;

public sealed class Company : EntityBase
{
    public required string Name { get; set; }
    public required int YearFounded { get; set; }

    public static Company Create(string name, int yearFounded)
    {
        return new Company
        {
            Name = name,
            YearFounded = yearFounded
        };
    }

    public Core.Response Update(string name, int yearFounded)
    {
        Name = name;
        YearFounded = yearFounded;
        
        return Core.Response.Success;
    }
}
