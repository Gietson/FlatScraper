using System;

namespace FlatScraper.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToTimestamp(this DateTime datetime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var time = datetime.Subtract(new TimeSpan(epoch.Ticks));

            return time.Ticks / 10000;
        }
    }
}
