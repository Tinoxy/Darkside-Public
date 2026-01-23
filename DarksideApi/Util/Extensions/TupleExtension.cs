namespace DarksideApi.Util.Extensions
{
    public static class TupleExtension
    {
        public static T GetKey<T>(this Tuple<T, T> tuple)
        {
            return tuple.Item1;
        }

        public static List<T> GetKeys<T>(this List<Tuple<T, T>> tuples)
        {
            List<T> result = [];
            tuples.ForEach(t => result.Add(t.GetKey()));
            return result;
        }

        public static T? GetValue<T>(this List<Tuple<T, T>> tuples, T key)
        {
            foreach (Tuple<T, T> tuple in tuples)
            {
                if (!tuple.Item2.Equals(key))
                {
                    continue;
                }
                return tuple.Item2;
            }
            return default;
        }
    }
}
