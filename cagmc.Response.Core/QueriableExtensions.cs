using System.Linq.Expressions;

namespace cagmc.Response.Core;

public static class QueriableExtensions
{
    public static IQueryable<T> OrderByAndPaginate<T>(
        this IQueryable<T> source,
        ListFilter filter)
    {
        if (filter.SortByColumn is not null && filter.IsAscending.HasValue)
        {
            source = filter.IsAscending.Value
                ? OrderBy(source, filter.SortByColumn)
                : OrderByDescending(source, filter.SortByColumn);
        }

        if (filter.PageIndex.HasValue && filter.PageSize.HasValue)
        {
            source = source
                .Skip((filter.PageIndex.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value);
        }
        
        return source;
    }

    public static IOrderedQueryable<T> OrderBy<T>(
        this IQueryable<T> source,
        string property)
    {
        return ApplyOrder(source, property, "OrderBy");
    }

    public static IOrderedQueryable<T> OrderByDescending<T>(
        this IQueryable<T> source,
        string property)
    {
        return ApplyOrder(source, property, "OrderByDescending");
    }

    public static IOrderedQueryable<T> ThenBy<T>(
        this IOrderedQueryable<T> source,
        string property)
    {
        return ApplyOrder(source, property, "ThenBy");
    }

    public static IOrderedQueryable<T> ThenByDescending<T>(
        this IOrderedQueryable<T> source,
        string property)
    {
        return ApplyOrder(source, property, "ThenByDescending");
    }

    private static IOrderedQueryable<T> ApplyOrder<T>(
        IQueryable<T> source,
        string property,
        string methodName)
    {
        var props = property.Split('.');
        var type = typeof(T);
        var arg = Expression.Parameter(type, "x");
        Expression expr = arg;
        foreach (var prop in props)
        {
            // use reflection (not ComponentModel) to mirror LINQ
            var pi = type.GetProperty(prop)!;
            expr = Expression.Property(expr, pi);
            type = pi.PropertyType;
        }

        var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
        var lambda = Expression.Lambda(delegateType, expr, arg);

        var result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                          && method.IsGenericMethodDefinition
                          && method.GetGenericArguments().Length == 2
                          && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), type)
            .Invoke(null, [source, lambda]);
        return (IOrderedQueryable<T>)result!;
    }
}