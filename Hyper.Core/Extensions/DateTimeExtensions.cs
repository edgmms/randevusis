using System;
using System.Globalization;

namespace Hyper.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToTurkishDateTime(this DateTime dateTime)
        {
            var format = new CultureInfo("tr-TR", false).DateTimeFormat;
            return dateTime.ToString(format);
        }

        public static DateTime ToTurkishDateTime(this string dateTime)
        {
            var format = new CultureInfo("tr-TR", false).DateTimeFormat;
            return Convert.ToDateTime(dateTime, format);
        }

        public static string ToTurkishDateTimeNull(this DateTime? dateTime)
        {
            if (dateTime is null)
                return string.Empty;

            var format = new CultureInfo("tr-TR", false).DateTimeFormat;
            return dateTime.Value.ToString(format);
        }

        public static DateTime? ToTurkishDateTimeNull(this string dateTime)
        {

            if (dateTime.IsNullOrEmpty())
                return null;

            var format = new CultureInfo("tr-TR", false).DateTimeFormat;
            return Convert.ToDateTime(dateTime, format);
        }

        public static string ToTurkishHour(this DateTime dateTime)
        {
            string hour = dateTime.Hour < 10 ? $"0{dateTime.Hour}" : dateTime.Hour.ToString();
            string minute = dateTime.Minute < 10 ? $"0{dateTime.Minute}" : dateTime.Minute.ToString();
            return $"{hour}:{minute}";
        }

        public static string ToTurkishDate(this DateTime dateTime)
        {
            string day = dateTime.Day < 10 ? $"0{dateTime.Day}" : dateTime.Day.ToString();
            string month = dateTime.Month < 10 ? $"0{dateTime.Month}" : dateTime.Month.ToString();
            return $"{day}/{month}/{dateTime.Year}";
        }
    }
}
