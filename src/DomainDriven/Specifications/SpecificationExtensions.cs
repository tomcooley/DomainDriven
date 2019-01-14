using System;
using System.Linq;
using System.Linq.Expressions;

namespace DomainDriven.Specifications
{
    internal static class SpecificationExtensions
    {
        public static CompositeSpecification<T> And<T>(this Specification<T> first, Specification<T> second)
        {
            return new AndSpecification<T>(first, second);
        }

        public static CompositeSpecification<T> Or<T>(this Specification<T> first, Specification<T> second)
        {
            return new OrSpecification<T>(first, second);
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new
            {
                f,
                s = second.Parameters[i]
            }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }
    }
}