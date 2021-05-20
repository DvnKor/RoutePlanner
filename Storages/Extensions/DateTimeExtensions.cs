using System;

namespace Storages.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startOfWeekDay = DayOfWeek.Monday)
        {
            var diff = (7 + (dateTime.DayOfWeek - startOfWeekDay)) % 7;
            return dateTime.AddDays(-1 * diff).Date;
        }
        
        public static DateTime StartOfNextWeek(this DateTime dateTime, DayOfWeek startOfWeekDay = DayOfWeek.Monday)
        {
            var startOfWeek = dateTime.StartOfWeek(startOfWeekDay);
            return startOfWeek.AddDays(7);
        }
    }
}