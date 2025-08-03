namespace Diglett
{
    public static class TypeExtensions
    {
        public static bool IsBasicType(this Type type)
        {
            return
                type.IsPrimitive ||
                type.IsEnum ||
                type == typeof(string) ||
                type == typeof(decimal) ||
                type == typeof(DateTime) ||
                type == typeof(TimeSpan) ||
                type == typeof(Guid) ||
                type == typeof(byte[]);
        }
    }
}
