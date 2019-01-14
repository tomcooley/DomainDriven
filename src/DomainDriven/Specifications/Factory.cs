using System;

namespace DomainDriven.Specifications
{
    /// <summary>
    /// Generic Factory that can create most domain objects based upon specifications passed in
    /// </summary>
    /// <typeparam name="T">any type that has a public default constructor</typeparam>
    public static class Factory<T> where T : new()
    {
        /// <summary>
        /// Creates an object according to the specification(s) passed
        /// </summary>
        /// <param name="accordingTo">a Specification or CompositeSpecification which allows for composite specs"></typeparam></param>
        /// <returns>a fully constructed object that conforms to the specification(s) passed in</returns>
        /// <remarks>When a CompositeSpecification composed of two or more Specifications is passed into this method,
        /// it is possible that the combination of changes made by each builder is in conflict with one another. 
        /// Therefore, after the object has been passed through each builder, it is passed through each validator
        /// to ensure that all specifications have been satisfied and the object is not in a conflicted state.</remarks>
        /// <exception cref="InvalidOperationException">thrown when the combined specifications are incompatible.</exception>
        public static T Create(Specification<T> accordingTo)
        {
            if (accordingTo == null)
            {
                throw new NullReferenceException("A value must be provided for accordingTo");
            }

            var obj = new T();

            accordingTo.BuildSatisfying(obj);

            // since this may be a chained (composite) specification and some specifications can contradict one another leaving
            //  the object in an invalid state, make sure all specifications have been satisfied
            if (!accordingTo.IsSatisfiedBy(obj))
            {
                throw new InvalidOperationException("The combination of specifications passed into the factory are not compatible");
            }

            return obj;
        }
    }
}