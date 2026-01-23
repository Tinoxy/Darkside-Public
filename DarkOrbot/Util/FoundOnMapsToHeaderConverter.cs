using System.Globalization;
using System.Windows.Data;

namespace DarkOrbot.Util
{
    // Konverter: FoundOnMaps (0/1/2) -> lesbarer Gruppen-Header
    public class FoundOnMapsToHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "Other";

            if (value is int i)
            {
                return i switch
                {
                    0 => "Lower Maps",
                    1 => "Upper Maps",
                    2 => "Other",
                    _ => "Other",
                };
            }

            if (int.TryParse(value.ToString(), out int parsed))
            {
                return parsed switch
                {
                    0 => "Lower Maps",
                    1 => "Upper Maps",
                    2 => "Other",
                    _ => "Other",
                };
            }

            return value.ToString() ?? "Other";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}