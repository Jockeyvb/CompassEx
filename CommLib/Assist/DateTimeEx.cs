using System;
using System.Collections.Generic;
using System.Text;

namespace CommLib.Assist
{
    public static class DateTimeEx
    {

        /// <summary>只换年，保留月日时分秒</summary>
        public static DateTime WithYear(this DateTime d, int year)
        {
            int month = d.Month;
            int day = d.Day;
            // 闰年 2/29 换到平年自动落 2/28
            if (month == 2 && day == 29 && !DateTime.IsLeapYear(year))
                day = 28;

            return new DateTime(year, month, day,
                d.Hour, d.Minute, d.Second, d.Millisecond, d.Kind);
        }

        /// <summary>只换月，保留年日时分秒（日超限按当月最大日裁剪）</summary>
        public static DateTime WithMonth(this DateTime d, int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), "月份必须 1-12");

            int maxDay = DateTime.DaysInMonth(d.Year, month);
            int day = Math.Min(d.Day, maxDay);

            return new DateTime(d.Year, month, day,
                d.Hour, d.Minute, d.Second, d.Millisecond, d.Kind);
        }

        /// <summary>只换日，保留年月时分秒</summary>
        public static DateTime WithDay(this DateTime d, int day)
        {
            int maxDay = DateTime.DaysInMonth(d.Year, d.Month);
            if (day < 1 || day > maxDay)
                throw new ArgumentOutOfRangeException(nameof(day), $"日必须 1-{maxDay}");

            return new DateTime(d.Year, d.Month, day,
                d.Hour, d.Minute, d.Second, d.Millisecond, d.Kind);
        }

        /// <summary>只换时，保留年月日分秒</summary>
        public static DateTime WithHour(this DateTime d, int hour)
        {
            if (hour < 0 || hour > 23)
                throw new ArgumentOutOfRangeException(nameof(hour), "小时必须 0-23");

            return new DateTime(d.Year, d.Month, d.Day,
                hour, d.Minute, d.Second, d.Millisecond, d.Kind);
        }

        /// <summary>换分（顺手补，排盘偶尔用）</summary>
        public static DateTime WithMinute(this DateTime d, int minute)
        {
            if (minute < 0 || minute > 59)
                throw new ArgumentOutOfRangeException(nameof(minute));
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, minute, d.Second, d.Millisecond, d.Kind);
        }
    }
}
