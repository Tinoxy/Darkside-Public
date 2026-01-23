//KeyedCollection keeps an internal lookup dictionary for better performance.
//With normal implementations of an observable KeyedCollection, when used for
//data bindings in WPF, once the key of the item is changed with bindings, the
//corresponding key internal dictionary is not changed, which leads to hidden
//problems when the dictionary is used.
//Below is a custom implementation of KeyedCollection to solve that problem.

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DarksideApi.Util.Collections
{
    public class ObservableKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly Func<TItem, TKey>? _getKeyForItemDelegate;
        private readonly PropertyChangingEventHandler ChildPropertyChanging;
        private readonly PropertyChangedEventHandler ChildPropertyChanged;
        private readonly string? keyPropertyName;

        /// <summary>
        /// Needs either a delegate or a property name to get the key from the child item.
        /// Using a delegate should be faster than the property name which will use reflection.
        /// If TItem implements both INotifyCollectionChanged and INotifyCollectionChanging, the name of the key property is also needed for key updating to work.
        /// </summary>
        public ObservableKeyedCollection(Func<TItem, TKey>? getKeyForItemDelegate = null, string? keyPropName = null)
        {
            if (getKeyForItemDelegate == null && keyPropName == null)
                throw new ArgumentException(@"getKeyForItemDelegate and KeyPropertyName cannot both be null.");
            this.keyPropertyName = keyPropName;
            this._getKeyForItemDelegate = getKeyForItemDelegate;

            if (this.keyPropertyName != null &&
                typeof(TItem).GetInterface(nameof(INotifyPropertyChanged)) != null &&
                typeof(TItem).GetInterface(nameof(INotifyPropertyChanging)) != null)
            {
                this.ChildPropertyChanging = (o, e) =>
                {
                    if (e.PropertyName != this.keyPropertyName) return;
                    var item = (TItem)o;
                    this.Dictionary?.Remove(this.GetKeyForItem(item));
                };
                this.ChildPropertyChanged = (o, e) =>
                {
                    if (e.PropertyName != this.keyPropertyName) return;
                    var item = (TItem)o;
                    this.Dictionary?.Add(this.GetKeyForItem(item), item);
                };
            }
        }

        protected override TKey GetKeyForItem(TItem item)
        {
            return this._getKeyForItemDelegate != null
                ? this._getKeyForItemDelegate(item)
                : this.keyPropertyName != null
                ? (TKey)item.GetType().GetProperty(this.keyPropertyName).GetValue(item)
                : throw new ArgumentException(@"getKeyForItemDelegate and KeyPropertyName cannot both be null.");
        }

        protected override void SetItem(int index, TItem newitem)
        {
            //need old item to use Replace action below.
            TItem olditem = base[index];
            this.UpdatePropChangeHandlers(olditem, false);
            this.UpdatePropChangeHandlers(newitem, true);
            base.SetItem(index, newitem);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newitem, olditem, index));
        }

        //Add method calls this internally. Lookup dictionary is updated automatically.
        protected override void InsertItem(int index, TItem item)
        {
            this.UpdatePropChangeHandlers(item, true);
            base.InsertItem(index, item);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        protected override void ClearItems()
        {
            this.UpdatePropChangeHandlers(this.Items, null);
            base.ClearItems();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void RemoveItem(int index)
        {
            TItem item = this[index];
            this.UpdatePropChangeHandlers(item, false);
            base.RemoveItem(index);
            //using the overload without index causes binding problem when used in CompositeCollection.
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        private bool _deferNotifyCollectionChanged = false;
        public void AddRange(IEnumerable<TItem> items)
        {
            this._deferNotifyCollectionChanged = true;
            foreach (TItem? item in items) this.Add(item);//Add will call Insert internally.
            this._deferNotifyCollectionChanged = false;

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool TryGetValue(TKey key, out TItem item)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (this.Dictionary != null)
            {
                return this.Dictionary.TryGetValue(key, out item);
            }

            try
            {
                foreach (TItem itemInItems in this.Items)
                {
                    TKey keyInItems = this.GetKeyForItem(itemInItems);
                    if (keyInItems != null && this.Comparer.Equals(key, keyInItems))
                    {
                        item = itemInItems;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            item = default;
            return false;
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this._deferNotifyCollectionChanged) return;

            //if you get InvalidOperation here and the collection is on the UI thread,
            //verify if there is any reference to the item to be deleted.
            /*if (!Application.Current.Dispatcher.CheckAccess())
                Application.Current.Dispatcher.Invoke(() => NotifyChanges(e));
            else NotifyChanges(e);*/
            this.NotifyChanges(e);
        }

        private void UpdatePropChangeHandlers(TItem item, bool addOrRemove)
        {
            if (item == null || this.keyPropertyName == null || this.ChildPropertyChanging == null || this.ChildPropertyChanged == null) return;

            if (addOrRemove)
            {
                ((INotifyPropertyChanging)item).PropertyChanging += this.ChildPropertyChanging;
                ((INotifyPropertyChanged)item).PropertyChanged += this.ChildPropertyChanged;
            }
            else
            {
                ((INotifyPropertyChanging)item).PropertyChanging -= this.ChildPropertyChanging;
                ((INotifyPropertyChanged)item).PropertyChanged -= this.ChildPropertyChanged;
            }
        }

        private void UpdatePropChangeHandlers(IEnumerable<TItem> olditems, IEnumerable<TItem> newitems)
        {
            if (this.keyPropertyName == null || this.ChildPropertyChanging == null || this.ChildPropertyChanged == null) return;

            if (olditems != null)
            {
                foreach (TItem? olditem in olditems)
                {
                    ((INotifyPropertyChanging)olditem).PropertyChanging -= this.ChildPropertyChanging;
                    ((INotifyPropertyChanged)olditem).PropertyChanged -= this.ChildPropertyChanged;
                }
            }
            if (newitems != null)
            {
                foreach (TItem? newitem in newitems)
                {
                    ((INotifyPropertyChanging)newitem).PropertyChanging += this.ChildPropertyChanging;
                    ((INotifyPropertyChanged)newitem).PropertyChanged += this.ChildPropertyChanged;
                }
            }
        }

        public void NotifyPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Count)));
        }

        private void NotifyChanges(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Count)));
        }

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}