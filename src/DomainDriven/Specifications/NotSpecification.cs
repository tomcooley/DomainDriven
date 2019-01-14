using System;
using System.Linq;
using System.Linq.Expressions;

namespace DomainDriven.Specifications
{
    internal class NotSpecification<T> : Specification<T>
    {
        internal NotSpecification(Specification<T> inner)
            : base(inner)
        {
        }

        #region Overrides of Specification<T>

        /// <summary>
        /// Determines if an existing object satisfies the specification criteria
        /// </summary>
        /// <param name="candidate">The object to compare to the specification criteria.</param>
        /// <returns>true if the specification is satisfied, otherwise false</returns>
        public override bool IsSatisfiedBy(T candidate)
        {
            return !IsSatisfiedBy(candidate);
        }

        public override Expression<Func<T, bool>> IsSatisfied()
        {
            var param = Expression.Parameters.Single();

            var exp = Expression<Func<T, bool>>.Not(Expression.Body);

            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(exp, param);
        }

        #endregion
    }
}