using System;

namespace DomainDriven
{
    public static class DateTimeExtensions
    {
        public static TimeSpan Years(this int i)
        {
            var now = DateTime.Now;
            return now.AddYears(i) - now;
        }

        public static int Years(this TimeSpan timespan)
        {
            return (int)Math.Round(timespan.Days / 365.2425);
        }

        public static string ToLegalDate(this DateTime date)
        {
            return GetOrdinal(date.Day) + " day of " + date.ToString("MMMM") + ", " + date.Year;
        }

        public static TimeSpan Months(this int i)
        {
            var now = DateTime.Now;
            return now.AddMonths(i) - now;
        }

        public static int Months(this TimeSpan timespan)
        {
            return (int)Math.Round(timespan.Days / 30.436875);
        }

        public static TimeSpan Weeks(this int i)
        {
            return (i * 7).Days();
        }

        public static TimeSpan Days(this int i)
        {
            return new TimeSpan(i, 0, 0, 0);
        }

        public static TimeSpan Hours(this int i)
        {
            return new TimeSpan(i, 0, 0);
        }

        public static TimeSpan Minutes(this int i)
        {
            return new TimeSpan(0, i, 0);
        }

        public static TimeSpan Seconds(this int i)
        {
            return new TimeSpan(0, 0, i);
        }

        public static DateTime Ago(this TimeSpan timeSpan)
        {
            return DateTime.Now.Subtract(timeSpan);
        }

        public static DateTime FromNow(this TimeSpan timeSpan)
        {
            return DateTime.Now.Add(timeSpan);
        }

        public static DateTime FromToday(this TimeSpan timespan)
        {
            return DateTime.Today.Add(timespan);
        }

        public static DateTime From(this TimeSpan timeSpan, DateTime from)
        {
            return from.Add(timeSpan);
        }

        private static string GetOrdinal(int date)
        {
            string suffix;

            int ones = date%10;
            int tens = (int) Math.Floor(date/10M)%10;

            if (tens == 1)
            {
                suffix = "th";
            }
            else
            {
                switch (ones)
                {
                    case 1:
                        suffix = "st";
                        break;

                    case 2:
                        suffix = "nd";
                        break;

                    case 3:
                        suffix = "rd";
                        break;

                    default:
                        suffix = "th";
                        break;
                }
            }
            return date + suffix;
        }
    }    
}