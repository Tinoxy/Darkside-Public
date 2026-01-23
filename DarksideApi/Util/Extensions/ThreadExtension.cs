namespace DarksideApi.Util.Extensions
{
    public static class ThreadExtension
    {
        public static void Sleep(this Thread thread, int millisecondsTimeout, bool condition)
        {
            while (condition && millisecondsTimeout > 0)
            {
                Thread.Sleep(1);
                millisecondsTimeout--;
            }
        }
    }
}
