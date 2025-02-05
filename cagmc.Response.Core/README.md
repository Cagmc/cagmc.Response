# C# response classes

Return a proper response instead of an exception or simple bool.

- bool indicator
- int response code
- optional message

## Example

```
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
```