using System.Linq.Expressions;

namespace Deste.Domain.Utils;

public static class SortingUtils
{
    public static IQueryable<TEntity> Sort<TEntity>(this IQueryable<TEntity> entities, string? orderName = "Id", string? orderDirection = "ASC")
    {
        if (!entities.Any()) return entities;
        switch (orderDirection)
        {
            case "ASC": return entities.OrderBy(orderName ?? "Id");
            case "DESC": return entities.OrderByDescending(orderName ?? "Id");
            default: throw new ArgumentOutOfRangeException();
        }
    }
    
    private static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName)
    {
        var entityType = typeof(TSource);

        var propertyInfo = entityType.GetProperty(propertyName);
        if (propertyInfo == null) throw new NullReferenceException();
        var arg = Expression.Parameter(entityType, "x");
        var property = Expression.Property(arg, propertyName);
        var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

        var enumerableType = typeof(Queryable);
        if (enumerableType == null) throw new NullReferenceException();
        var method = enumerableType.GetMethods()
            .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
            .Where(m =>
            {
                var parameters = m.GetParameters().ToList();
                return parameters.Count == 2;
            }).Single();
            
        var genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);
            
        var newQuery = (IOrderedQueryable<TSource>)genericMethod
            .Invoke(genericMethod, new object[] { query, selector })!;
            
        return newQuery;
    }

    private static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> query, string propertyName)
    {
        var entityType = typeof(TSource);

        var propertyInfo = entityType.GetProperty(propertyName);
        if (propertyInfo == null) throw new NullReferenceException();
        var arg = Expression.Parameter(entityType, "x");
        var property = Expression.Property(arg, propertyName);
        var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

        var enumarableType = typeof(Queryable);
        if (enumarableType == null) throw new NullReferenceException();
        var method = enumarableType.GetMethods()
            .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
            .Where(m =>
            {
                var parameters = m.GetParameters().ToList();
                return parameters.Count == 2;//overload that has 2 parameters
            }).Single();
            
        var genericMethod = method
            .MakeGenericMethod(entityType, propertyInfo.PropertyType);
            
        var newQuery = (IOrderedQueryable<TSource>)genericMethod
            .Invoke(genericMethod, new object[] { query, selector })!;
        return newQuery;
    }
    
}