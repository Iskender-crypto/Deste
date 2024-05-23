namespace Deste.Domain.Utils;

public static class CompleteUtils
{
    public  static IQueryable<TEntity> Complete<TEntity>(this IQueryable<TEntity> entities, Func<IQueryable<TEntity>, IQueryable<TEntity>> predicate)
    {
        return predicate(entities);
    }
}