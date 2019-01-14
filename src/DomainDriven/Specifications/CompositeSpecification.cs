using System.Collections.Generic;

namespace DomainDriven.Specifications
{
    internal abstract class CompositeSpecification<T> : Specification<T>
    {
        protected readonly Specification<T> First;
        protected readonly Specification<T> Second;

        protected CompositeSpecification(Specification<T> first, Specification<T> second)
            : base(first)
        {
            First = first;
            Second = second;

            var unMetSpecs = new List<Specification<T>>();
            var composite = first as CompositeSpecification<T>;
            if (composite != null)
            {
                composite.UnMetSpecifications = Combine(unMetSpecs, composite);
            }

            composite = second as CompositeSpecification<T>;
            if (composite != null)
            {
                composite.UnMetSpecifications = Combine(unMetSpecs, composite);
            }

            UnMetSpecifications = unMetSpecs;
        }

        public List<Specification<T>> UnMetSpecifications { get; set; }

        private static List<Specification<T>> Combine(List<Specification<T>> unMetSpecs, CompositeSpecification<T> composite)
        {
            if (composite.UnMetSpecifications.Count > 0)
            {
                unMetSpecs.AddRange(composite.UnMetSpecifications);
            }

            return unMetSpecs;
        }

        protected bool FirstSatisfiedBy(T candidate)
        {
            var firstSatisfiedBy = First.IsSatisfiedBy(candidate);
            if (!firstSatisfiedBy)
            {
                if (!(First is CompositeSpecification<T>))
                {
                    UnMetSpecifications.Add(First);
                }
            }

            return firstSatisfiedBy;
        }

        protected bool SecondSatisfiedBy(T candidate)
        {
            var secondSatisfiedBy = Second.IsSatisfiedBy(candidate);
            if (!secondSatisfiedBy)
            {
                if (!(Second is CompositeSpecification<T>))
                {
                    UnMetSpecifications.Add(Second);
                }
            }

            return secondSatisfiedBy;
        }

        public override void BuildSatisfying(T candidate)
        {
            First.BuildSatisfying(candidate);
            Second.BuildSatisfying(candidate);
        }
    }
}