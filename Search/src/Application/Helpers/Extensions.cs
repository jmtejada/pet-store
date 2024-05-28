using Microsoft.EntityFrameworkCore;
using PetStore.Search.Application.Common;
using System.Linq.Expressions;

namespace PetStore.Search.Application.Helpers
{
    public static class EFExtensions
    {
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string propertyName, string propertyDirection) where TEntity : class
        {
            var queryExpr = source.Expression;
            var command = propertyDirection.ToUpper().Equals("DESC") ? "OrderByDescending" : "OrderBy";

            var type = typeof(TEntity);
            var property = type.GetProperties()
                .Where(item => item.Name.ToLower() == propertyName.ToLower())
                .FirstOrDefault();

            if (property == null)
                return source;

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            queryExpr = Expression.Call(
                type: typeof(Queryable),
                methodName: command,
                typeArguments: new Type[] { type, property.PropertyType },
                queryExpr,
                Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<TEntity>(queryExpr); ;
        }

        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, string? propertyName, object? propertyValue) where TEntity : class
        {
            if (string.IsNullOrEmpty(propertyName) || propertyValue == null || (propertyValue is string stringValue && string.IsNullOrWhiteSpace(stringValue)))
            {
                return source;
            }

            var type = typeof(TEntity);
            var property = type.GetProperties()
                .Where(item => item.Name.ToLower() == propertyName.ToLower())
                .FirstOrDefault();

            if (property == null)
                return source;

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var constant = Expression.Constant(propertyValue);
            var equality = Expression.Equal(propertyAccess, constant);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equality, parameter);

            return source.Where(lambda);
        }

        public static async Task<PagedResult<TEntity>> GetPagedResultAsync<TEntity>(this IQueryable<TEntity> source, int pageSize, int currentPage) where TEntity : class
        {
            var rows = source.Count();
            var results = await source
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<TEntity>
            {
                CurrentPage = currentPage,
                PageCount = (int)Math.Ceiling((double)rows / pageSize),
                PageSize = pageSize,
                Results = results,
                RowsCount = rows
            };
        }
    }
}