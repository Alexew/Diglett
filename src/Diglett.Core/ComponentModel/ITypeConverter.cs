using System.Globalization;

namespace Diglett.Core.ComponentModel
{
    public interface ITypeConverter
    {
        bool CanConvertFrom(Type type);
        bool CanConvertTo(Type type);

        object? ConvertFrom(CultureInfo culture, object value);
        object? ConvertTo(CultureInfo culture, string? format, object value, Type to);
    }
}
