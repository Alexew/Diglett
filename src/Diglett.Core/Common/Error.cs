using System.Globalization;

namespace Diglett
{
    public static class Error
    {
        public static Exception Application(string message, params object[] args)
        {
            return new ApplicationException(message.FormatCurrent(args));
        }

        public static Exception Application(Exception innerException, string message, params object[] args)
        {
            return new ApplicationException(message.FormatCurrent(args), innerException);
        }

        public static Exception ArgumentOutOfRange<T>(Func<T> arg)
        {
            return new ArgumentOutOfRangeException(arg.Method.Name);
        }

        public static Exception ArgumentOutOfRange(string argName)
        {
            return new ArgumentOutOfRangeException(argName);
        }

        public static Exception ArgumentOutOfRange(string argName, string message, params object[] args)
        {
            return new ArgumentOutOfRangeException(argName, string.Format(CultureInfo.CurrentCulture, message, args));
        }

        public static Exception Argument(string argName, string message, params object[] args)
        {
            return new ArgumentException(string.Format(CultureInfo.CurrentCulture, message, args), argName);
        }

        public static Exception InvalidCast(Type fromType, Type toType)
        {
            return InvalidCast(fromType, toType, null);
        }

        public static Exception InvalidCast(Type fromType, Type toType, Exception? innerException)
        {
            return new InvalidCastException("Cannot convert from type '{0}' to '{1}'.".FormatCurrent(fromType?.FullName ?? "NULL", toType.FullName), innerException);
        }

        public static Exception NoElements()
        {
            return new InvalidOperationException("Sequence contains no elements.");
        }

        public static Exception MoreThanOneElement()
        {
            return new InvalidOperationException("Sequence contains more than one element.");
        }
    }
}
