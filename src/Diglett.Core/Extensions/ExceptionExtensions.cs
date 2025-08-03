using System.Runtime.ExceptionServices;
using System.Text;

namespace Diglett
{
    public static class ExceptionExtensions
    {
        public static void ReThrow(this Exception exception)
            => ExceptionDispatchInfo.Capture(exception).Throw();

        public static void Dump(this Exception ex)
        {
            try
            {
                ex.StackTrace.Dump();
                ex.Message.Dump();
            }
            catch { }
        }

        public static string GetInnerMessage(this Exception exception)
        {
            while (true)
            {
                if (exception.InnerException == null)
                {
                    return exception.Message;
                }

                exception = exception.InnerException;
            }
        }

        public static string ToAllMessages(this Exception? exception, bool includeStackTrace = false)
        {
            if (exception == null)
                return string.Empty;

            var sb = new StringBuilder(includeStackTrace ? exception.Message.Length * 3 : exception.Message.Length);

            while (exception != null)
            {
                // TODO: Find a better way to skip redundant messages
                if (includeStackTrace)
                {
                    if (sb.Length > 0)
                    {
                        sb.AppendLine();
                        sb.AppendLine();
                    }
                    sb.AppendLine(exception.ToString());
                }
                else
                {
                    sb.Grow(exception.Message, " * ");
                }

                exception = exception.InnerException;
            }

            return sb.ToString();
        }
    }
}
