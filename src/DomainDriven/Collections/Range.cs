using System;

namespace DomainDriven.Collections
{
    /// <summary>
    /// Provides a range of values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Range<T> where T : struct, IComparable<T>
    {
        private readonly T _start;
        private readonly T _end;

        #region Construction

        /// <summary>
        /// Creates a new range of one value of type T
        /// </summary>
        /// <param name="singleValue">The value to be used as both the start and end of the range.</param>
        public Range(T singleValue) : this(singleValue, singleValue) { }

        /// <summary>
        /// Creates a new range of values of type T
        /// </summary>
        /// <param name="start">The first value in the range.</param>
        /// <param name="end">The last value in the range.</param>
        public Range(T start, T end)
        {
            if (start.CompareTo(end) <= 0)
            {
                _start = start;
                _end = end;
            }
            else
            // if parameters passed in backwards, just make them right
            {
                _start = end;
                _end = start;
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Gets first value in the range
        /// </summary>
        public T Start
        {
            get { return _start; }
        }

        /// <summary>
        /// Gets the last value in the range
        /// </summary>
        public T End
        {
            get { return _end; }
        }

        /// <summary>
        /// Indicates whether the value provided is contained within the range of values
        /// </summary>
        /// <param name="valueToFind">The value to find</param>
        /// <returns>An indicator of whether the value is contained within the range.</returns>
        public bool Contains(T valueToFind)
        {
            return valueToFind.CompareTo(Start) >= 0 && valueToFind.CompareTo(End) <= 0;
        }

        #endregion
    }
}