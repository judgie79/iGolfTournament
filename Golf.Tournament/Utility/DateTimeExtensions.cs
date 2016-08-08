using System;
using System.Configuration;

namespace Golf.Tournament.Utility
{
    public static class DateTimeExtensions
    {
        static string timeZoneId = ConfigurationManager.AppSettings["TimeZoneId"] ?? "W. Europe Standard Time";

        public static DateTime ToLocalTime(this DateTime dt)
        {
            // dt.DateTimeKind should be Utc!
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dt, DateTimeKind.Utc), tzi);
        }

        public static DateTime ToUtcTime(this DateTime dt)
        {
            var tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(dt, tzi);
        }

        public static DateTime RoundDown(this DateTime dateTime, int minutes)
        {
            return new DateTime(dateTime.Year, dateTime.Month,
                 dateTime.Day, dateTime.Hour, (dateTime.Minute / minutes) * minutes, 0);
        }

        public static DateTime FromUnixTime(this DateTime dotNetTime, double unixTime)
        {
            System.DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return result.AddSeconds(unixTime).ToLocalTime();
        }

        public static long ToUnixTime(this DateTime dotNetTime)
        {
            var result = (dotNetTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
            return (long)result;
        }
    }
}
