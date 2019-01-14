using System;
using System.Collections.Generic;

namespace DomainDriven.Collections
{
    public static class RangeExtensions
    {
        /// <summary>
        /// Returns all the integers within the range in ascending order
        /// </summary>
        /// <param name="range"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="int"/></returns>
        public static IEnumerable<int> Values(this Range<int> range)
        {
            var current = range.Start;
            while (current.CompareTo(range.End) <= 0)
            {
                yield return current++;
            }
        }

        /// <summary>
        /// Returns all the days within the range in ascending order
        /// </summary>
        /// <param name="range"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="DateTime"/></returns>
        public static IEnumerable<DateTime> Days(this Range<DateTime> range)
        {
            var current = range.Start;
            while (current.CompareTo(range.End) <= 0)
            {
                yield return current;
                current = current.AddDays(1);
            }
        }

        /// <summary>
        /// Returns all the weekdays within the range in ascending order
        /// </summary>
        /// <param name="range"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="DateTime"/></returns>
        public static IEnumerable<DateTime> WeekDays(this Range<DateTime> range)
        {
            var current = range.Start;
            while (current.CompareTo(range.End) <= 0)
            {
                if (current.IsWeekDay())
                {
                    yield return current;
                }

                current = current.AddDays(1);
            }
        }

        /// <summary>
        /// Indicates if a DateTime is a weekday
        /// </summary>
        /// <param name="current"></param>
        /// <returns>true if the day of the week is not Saturday or Sunday</returns>
        public static bool IsWeekDay(this DateTime current)
        {
            return current.DayOfWeek != DayOfWeek.Saturday &&
                   current.DayOfWeek != DayOfWeek.Sunday;
        }

        /// <summary>
        /// Does DateTime range A exist within DateTime range B
        /// </summary>
        /// <param name="rangeA">DateTime Range A</param>
        /// <param name="rangeB">DateTime Range B</param>
        /// <returns>True if time range B start or end point is within time range A, otherwise false.</returns>
        public static bool Overlaps(this Range<DateTime> rangeA, Range<DateTime> rangeB)
        {
            return rangeA.Contains(rangeB.Start) || rangeA.Contains(rangeB.End);
        }

        #region GetRandomValue Extension Methods

        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Gets a random integer from a range of integers
        /// </summary>
        /// <param name="range"></param>
        /// <returns>an integer within the range</returns>
        public static int GetRandomValue(this Range<int> range)
        {
            if (range.Start == range.End)
            {
                return range.Start;
            }

            return Random.Next(range.Start, range.End);
        }

        /// <summary>
        /// Gets a random DateTime from a range of DateTimes
        /// </summary>
        /// <param name="range"></param>
        /// <returns>a DateTime within the range</returns>
        public static DateTime GetRandomValue(this Range<DateTime> range)
        {
            if (range.Start == range.End)
            {
                return range.Start;
            }

            var daysInRange = range.End - range.Start;

            var maxDaysToAdd = daysInRange.Days;
            var numDaysToAdd = Random.Next(0, maxDaysToAdd);
            return range.Start.AddDays(numDaysToAdd);
        }

        /// <summary>
        /// Gets a random TimeSpan from a range of TimeSpans
        /// </summary>
        /// <param name="range"></param>
        /// <returns>a TimeSpan within the range</returns>
        public static TimeSpan GetRandomValue(this Range<TimeSpan> range)
        {
            if (range.Start == range.End)
            {
                return range.Start;
            }

            var day = Random.Next(range.Start.Days, range.End.Days);
            var hour = Random.Next(range.Start.Hours, range.End.Hours);
            var minute = Random.Next(range.Start.Minutes, range.End.Minutes);
            var second = Random.Next(range.Start.Seconds, range.End.Seconds);

            return new TimeSpan(day, hour, minute, second);
        }

        /// <summary>
        /// Gets a random decimal from a range of decimals
        /// </summary>
        /// <param name="range"></param>
        /// <returns>a decimal within the range</returns>
        public static decimal GetRandomValue(this Range<decimal> range)
        {
            if (range.Start == range.End)
            {
                return range.Start;
            }

            var decRange = range.End - range.Start;

            return ((decimal)Random.NextDouble() * decRange) + range.Start;
        }

        /// <summary>
        /// Gets a random double from a range of doubles
        /// </summary>
        /// <param name="range"></param>
        /// <returns>a double within the range</returns>
        public static double GetRandomValue(this Range<double> range)
        {
            double randomDouble;
            if (Math.Abs(range.End - range.Start) < Double.Epsilon)
            {
                randomDouble = range.End;
            }
            else
            {
                do
                {
                    randomDouble = Random.NextDouble();
                }
                while (randomDouble < range.Start ||
                       randomDouble > range.End);
            }

            return randomDouble;
        }

        #endregion
    }
}