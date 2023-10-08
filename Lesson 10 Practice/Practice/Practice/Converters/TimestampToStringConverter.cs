using System;
using System.Globalization;
using System.Windows.Data;

namespace Practice.Converters
{
    [ValueConversion(typeof(long), typeof(string))]
    public class TimestampToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeStamp = (long)value;

            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));//当地时区  
            var time = startTime.AddMilliseconds(timeStamp);
            return time.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
