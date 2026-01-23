namespace DarksideApi.Util.Extensions
{
    public static class ListExtension
    {
        public static T Pop<T>(this List<T> list)
        {
            if (list.Count <= 0)
            {
                throw new IndexOutOfRangeException("List is Empty!");
            }
            T? element = list[0];
            list.RemoveAt(0);
            return element;
        }
    }
}
