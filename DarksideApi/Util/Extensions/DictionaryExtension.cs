namespace DarksideApi.Util.Extensions
{
    public static class DictionaryExtension
    {
        public static bool TryGetKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value, out TKey key)
        {
            key = default;
            if (dictionary == null)
            {
                return false;
            }
            for (var i = 0; i < dictionary.Count; i++)
            {
                KeyValuePair<TKey, TValue> element = dictionary.ElementAt(i);
                if (!element.Value.Equals(value))
                {
                    continue;
                }
                key = element.Key;
                return true;
            }
            return false;
        }

        public static TValue Remove<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
            {
                return default;
            }
            if (dictionary.TryGetValue(key, out TValue value))
            {
                dictionary.Remove(key);
                return value;
            }

            return default;
        }
    }
}
