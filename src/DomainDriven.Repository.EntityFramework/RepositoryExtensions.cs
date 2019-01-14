using System;
using System.Linq;
using System.Linq.Expressions;

namespace DomainDriven.Repository
{
    public static class RepositoryExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property, string direction)
        {
            if (direction == "desc")
            {
                return ApplyOrder(source, property, "OrderByDescending");
            }
            return ApplyOrder(source, property, "OrderBy");
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IQueryable<T> source, string property, string direction)
        {
            if (direction == "desc")
            {
                return ApplyOrder(source, property, "ThenByDescending");
            }
            return ApplyOrder(source, property, "ThenBy");
        }

        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            var props = property.Split('.');
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (var prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = type.GetProperty(prop);
                if (pi != null)
                {
                    expr = Expression.Property(expr, pi);
                    type = pi.PropertyType;
                }
            }
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}