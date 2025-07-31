using System.Runtime.CompilerServices;

namespace Diglett.Core.Common
{
    public class Guard
    {
        const string IsPositiveMessage = "Argument '{0}' must be a positive value. Value: '{1}'.";
        const string NotNegativeMessage = "Argument '{0}' cannot be a negative value. Value: '{1}'.";
        const string NotEmptyStringMessage = "String parameter '{0}' cannot be null or all whitespace.";
        const string NotZeroMessage = "Argument '{0}' must be greater or less than zero. Value: '{1}'.";

        public static void NotNull(string? arg, [CallerArgumentExpression(nameof(arg))] string? argName = null)
        {
            if (arg == null)
                throw new ArgumentNullException(argName);
        }

        public static T NotNull<T>(T? arg, [CallerArgumentExpression(nameof(arg))] string? argName = null)
        {
            return arg ?? throw new ArgumentNullException(argName);
        }

        public static void NotEmpty(string? arg, [CallerArgumentExpression(nameof(arg))] string? argName = null)
        {
            if (arg == null)
                throw new ArgumentNullException(argName);
            else if (arg.Trim().Length == 0)
                throw new ArgumentException(string.Format(NotEmptyStringMessage, argName), argName);
        }

        public static T NotNegative<T>(T arg, [CallerArgumentExpression(nameof(arg))] string? argName = null,
            string message = NotNegativeMessage) where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default) < 0)
                throw new ArgumentOutOfRangeException(argName, string.Format(message, argName, arg));

            return arg;
        }

        public static T IsPositive<T>(T arg, [CallerArgumentExpression(nameof(arg))] string? argName = null,
            string message = IsPositiveMessage) where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default) < 1)
                throw new ArgumentOutOfRangeException(argName, string.Format(message, argName, arg));

            return arg;
        }

        public static T NotZero<T>(T arg, [CallerArgumentExpression(nameof(arg))] string? argName = null,
            string message = NotZeroMessage) where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default) == 0)
                throw new ArgumentOutOfRangeException(argName, string.Format(message, argName, arg));

            return arg;
        }
    }
}
