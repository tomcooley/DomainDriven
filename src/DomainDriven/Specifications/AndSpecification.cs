using System.Linq.Expressions;

namespace DomainDriven.Specifications
{
    internal class AndSpecification<T> : CompositeSpecification<T>
    {
        public AndSpecification(Specification<T> first, Specification<T> second)
            : base(first, second)
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
            return FirstSatisfiedBy(candidate) && SecondSatisfiedBy(candidate);
        }

        public override Expression<System.Func<T, bool>> IsSatisfied()
        {
            return First.IsSatisfied().And(Second.IsSatisfied());
        }

        #endregion
    }
}