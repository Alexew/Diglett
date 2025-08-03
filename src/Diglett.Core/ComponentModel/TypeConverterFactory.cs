using Diglett.Core.ComponentModel.TypeConverters;
using System.Collections.Concurrent;

namespace Diglett.Core.ComponentModel
{
    public static class TypeConverterFactory
    {
        private static readonly ConcurrentDictionary<Type, ITypeConverter> _typeConverters = new();

        public static ITypeConverter GetConverter<T>()
        {
            return GetConverter(typeof(T));
        }

        public static ITypeConverter GetConverter(Type type)
        {
            Guard.NotNull(type);

            return _typeConverters.GetOrAdd(type, Get);

            ITypeConverter Get(Type t)
            {
                // TODO: Add providers

                // Default fallback
                return new DefaultTypeConverter(type);
            }
        }
    }
}
