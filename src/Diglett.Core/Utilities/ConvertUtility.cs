using Diglett.Core.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Diglett.Core.Utilities
{
    public static class ConvertUtility
    {
        public static bool TryConvert<T>(object? value, [MaybeNullWhen(false)] out T? convertedValue)
        {
            convertedValue = default;

            if (TryConvert(value, typeof(T), CultureInfo.InvariantCulture, out object? result))
            {
                convertedValue = (T)result!;
                return true;
            }

            return false;
        }

        public static bool TryConvert<T>(object? value, CultureInfo? culture, [MaybeNullWhen(false)] out T? convertedValue)
        {
            convertedValue = default;

            if (TryConvert(value, typeof(T), culture, out object? result))
            {
                convertedValue = (T)result!;
                return true;
            }

            return false;
        }

        public static bool TryConvert(object? value, Type to, [MaybeNullWhen(false)] out object? convertedValue)
        {
            return TryConvert(value, to, CultureInfo.InvariantCulture, out convertedValue);
        }

        public static bool TryConvert(object? value, Type to, CultureInfo? culture, [MaybeNullWhen(false)] out object? convertedValue)
        {
            ArgumentNullException.ThrowIfNull(to);

            convertedValue = null;

            if (value == null || value == DBNull.Value)
                return to == typeof(string) || to.IsBasicType() == false;

            if (to == typeof(object) || (to != typeof(object) && to.IsInstanceOfType(value)))
            {
                convertedValue = value;
                return true;
            }

            Type from = value.GetType();

            culture ??= CultureInfo.InvariantCulture;

            try
            {
                var converter = TypeConverterFactory.GetConverter(to);
                if (converter != null && converter.CanConvertFrom(from))
                {
                    convertedValue = converter.ConvertFrom(culture, value);
                    return true;
                }

                converter = TypeConverterFactory.GetConverter(from);
                if (converter != null && converter.CanConvertTo(to))
                {
                    convertedValue = converter.ConvertTo(culture, null, value, to);
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}
