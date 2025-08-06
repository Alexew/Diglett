using Diglett.Core.Utilities;
using System.Globalization;

namespace Diglett
{
    public static class ObjectExtensions
    {
        public static T? Convert<T>(this object? value)
        {
            if (ConvertUtility.TryConvert(value, typeof(T), CultureInfo.InvariantCulture, out object? result))
                return (T)result!;

            return default;
        }
    }
}
