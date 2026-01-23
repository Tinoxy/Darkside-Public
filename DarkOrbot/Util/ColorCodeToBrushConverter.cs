using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DarkOrbot.Util
{
    // Robuster Konverter: akzeptiert Brush, Color, String (#RRGGBB / Named), int/long ARGB/RGB
    public class ColorCodeToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null) return Brushes.White;

                // Wenn bereits Brush
                if (value is Brush br) return br;

                // Wenn Color
                if (value is Color c) return FreezeBrush(new SolidColorBrush(c));

                // Wenn string -> versuche Farb-Parsing (#RRGGBB, Named colors)
                if (value is string s)
                {
                    if (string.IsNullOrWhiteSpace(s)) return Brushes.White;
                    try
                    {
                        var maybeColor = (Color)ColorConverter.ConvertFromString(s);
                        return FreezeBrush(new SolidColorBrush(maybeColor));
                    }
                    catch
                    {
                        // fallthrough -> default
                        return Brushes.White;
                    }
                }

                // Wenn numerisch (int/long) -> interpretieren als 0xAARRGGBB oder 0xRRGGBB
                if (value is IConvertible conv)
                {
                    try
                    {
                        var longVal = conv.ToInt64(CultureInfo.InvariantCulture);
                        byte a, r, g, b;
                        if (longVal > 0xFFFFFF)
                        {
                            // ARGB
                            a = (byte)((longVal >> 24) & 0xFF);
                            r = (byte)((longVal >> 16) & 0xFF);
                            g = (byte)((longVal >> 8) & 0xFF);
                            b = (byte)(longVal & 0xFF);
                        }
                        else
                        {
                            a = 255;
                            r = (byte)((longVal >> 16) & 0xFF);
                            g = (byte)((longVal >> 8) & 0xFF);
                            b = (byte)(longVal & 0xFF);
                        }
                        var col = Color.FromArgb(a, r, g, b);
                        return FreezeBrush(new SolidColorBrush(col));
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            catch
            {
                // fallback
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private static SolidColorBrush FreezeBrush(SolidColorBrush brush)
        {
            if (brush.CanFreeze) brush.Freeze();
            return brush;
        }
    }
}