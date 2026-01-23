using DarksideApi.Util.Collections;

namespace DarksideApi.Util.Extensions
{
    public static class ObservableKeyedCollectionExtension
    {
        public static bool TryAdd<TKey, TItem>(this ObservableKeyedCollection<TKey, TItem> collection, TKey key, TItem item)
        {
            if (collection.Contains(key))
            {
                return false;
            }
            collection.Add(item);
            return true;
        }
    }
}
