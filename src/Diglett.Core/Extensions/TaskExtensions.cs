namespace Diglett
{
    public static class TaskExtensions
    {
        public static void Await(this Task task)
        {
            task.GetAwaiter().GetResult();
        }

        public static T Await<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }
    }
}
