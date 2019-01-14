using System;
using System.Linq.Expressions;

namespace DomainDriven.Specifications
{
    /// <summary>
    /// Defines criteria to satisfy a specification. Specifications are used
    /// to encapsulate criteria logic that is applied to one or more domain
    /// objects of a given type. Sometimes this criteria is best defined via 
    /// properties and methods on the class. But in many cases, it is best to 
    /// separate this concern and acheive greater reuse.
    /// <remarks>
    /// A concrete implementation of this class can be used in one of three ways:
    /// 1. To determine if an existing object satisfies the specification
    /// 2. To return one or more objects that satisfy the criteria through a Linq Expression tree
    /// 3. To build an object that satisifes the specification.
    /// </remarks>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Specification<T>
    {
        private readonly Expression<Func<T, bool>> _expression;
        private Func<T, bool> _delegate;
        private Predicate<T> _predicate;
        protected Specification(Expression<Func<T, bool>> expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Determines if an existing object satisfies the specification criteria
        /// </summary>
        /// <param name="candidate">The object to compare to the specification criteria.</param>
        /// <returns>true if the specification is satisfied, otherwise false</returns>
        public virtual bool IsSatisfiedBy(T candidate)
        {
            return Delegate.Invoke(candidate);
        }

        /// <summary>
        /// Returns a Linq Expression Tree for use in querying objects that satisfy the specification criteria.
        /// Usage of this method is optional since the implicit cast will acheive the same thing.
        /// </summary>
        /// <returns>a Linq Expression Tree</returns>
        public virtual Expression<Func<T, bool>> IsSatisfied()
        {
            return Expression;
        }

        /// <summary>
        /// Builds an object that satisfies the specification criteria. This is typically accomplished
        /// by passing the specification into a Factory. The factory will create an object instance and allow each
        /// specification to alter the object to meet its specification.
        /// <remarks>
        /// Overriding types should alter the candidate such that it will satisfy the criteria of the specification.
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public virtual void BuildSatisfying(T candidate)
        {
            // default implementation is to do nothing
        }
        internal Expression<Func<T, bool>> Expression
        {
            get { return _expression; }
        }
        internal Func<T, bool> Delegate
        {
            get
            {
                if (_delegate == null)
                {
                    _delegate = Expression.Compile();
                }

                return _delegate;
            }
        }
        internal Predicate<T> Predicate
        {
            get
            {
                if (_predicate == null)
                {
                    _predicate = new Predicate<T>(Delegate);
                }

                return _predicate;
            }
        }

        /// <summary>
        /// The & operation.
        /// </summary>
        /// <param name="leftSide">
        /// The left side specification.
        /// </param>
        /// <param name="rightSide">
        /// The right side specification.
        /// </param>
        /// <returns>
        /// The merged specification.
        /// </returns>
        public static Specification<T> operator &(Specification<T> leftSide, Specification<T> rightSide)
        {
            return new AndSpecification<T>(leftSide, rightSide);
        }

        /// <summary>
        /// The | operation.
        /// </summary>
        /// <param name="leftSide">
        /// The left side specification.
        /// </param>
        /// <param name="rightSide">
        /// The right side specification.
        /// </param>
        /// <returns>
        /// The merged specification.
        /// </returns>
        public static Specification<T> operator |(Specification<T> leftSide, Specification<T> rightSide)
        {
            return new OrSpecification<T>(leftSide, rightSide);
        }

        /// <summary>
        /// The ! operation.
        /// </summary>
        /// <param name="current">
        /// The current specification.
        /// </param>
        /// <returns>
        /// The inverse specification.
        /// </returns>
        public static Specification<T> operator !(Specification<T> current)
        {
            return new NotSpecification<T>(current);
        }
        /// <summary>
        /// Implicit cast to Expression predicate (lambda)
        /// </summary>
        /// <param name="specification">An Specification</param>
        /// <returns></returns>
        public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
        {
            return specification.Expression;
        }

        /// <summary>
        /// Implicit cast to delegate predicate
        /// </summary>
        /// <param name="specification">An Specification</param>
        /// <returns></returns>
        public static implicit operator Func<T, bool>(Specification<T> specification)
        {
            return specification.Delegate;
        }

        /// <summary>
        /// Implicit cast to Predicate
        /// </summary>
        /// <param name="specification">An Specification</param>
        /// <returns></returns>
        public static implicit operator Predicate<T>(Specification<T> specification)
        {
            return specification.Predicate;
        }
    }
}