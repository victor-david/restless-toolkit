using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a collection of <see cref="NavigatorItem"/> objects that are organized into groups.
    /// </summary>
    public class NavigatorItemCollection
    {
        #region Private
        private readonly MaxSizeObservableCollection<ObservableCollection<NavigatorItem>> backingGroups;
        private readonly List<NavigatorItem> storage;
        private bool selectedItemUpdateInProgress;
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the navigator groups. Each group is a ListCollectionView that gets its data from
        /// an ObservableCollection of <see cref="NavigatorItem"/>.
        /// </summary>
        public MaxSizeObservableCollection<ListCollectionView> Groups
        {
            get;
        }

        /// <summary>
        /// Gets the ObservableCollection of selected items.
        /// </summary>
        public MaxSizeObservableCollection<object> SelectedItems
        {
            get;
        }

        /// <summary>
        /// Gets the total count of <see cref="NavigatorItem"/> objects in the collection.
        /// </summary>
        public int Count
        {
            get => storage.Count;
        }

        /// <summary>
        /// Gets the <see cref="NavigatorItem"/> at the specified index.
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The item at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range.</exception>
        public NavigatorItem this[int index]
        {
            get => storage[index];
        }
        #endregion

        /************************************************************************/

        #region Events
        /// <summary>
        /// Raised when an element of <see cref="SelectedItems"/> changes.
        /// </summary>
        public event EventHandler<NavigatorItem> SelectedItemChanged;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigatorItemCollection"/> class.
        /// </summary>
        /// <param name="groupCount">The number of groups to create.</param>
        public NavigatorItemCollection(int groupCount)
        {
            if (groupCount <= 0) throw new ArgumentOutOfRangeException(nameof(groupCount));
            storage = new List<NavigatorItem>();

            backingGroups = new MaxSizeObservableCollection<ObservableCollection<NavigatorItem>>(groupCount);
            Groups = new MaxSizeObservableCollection<ListCollectionView>(groupCount);

            SelectedItems = new MaxSizeObservableCollection<object>(groupCount);

            for (int k=0; k < groupCount; k++)
            {
                backingGroups.Add(new ObservableCollection<NavigatorItem>());
                Groups.Add(new ListCollectionView(backingGroups[k]));
                SelectedItems.Add(null);
            }

            SelectedItems.CollectionChanged += SelectedItemsCollectionChanged;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Adds a <see cref="NavigatorItem"/>
        /// </summary>
        /// <typeparam name="T">The target type of the item</typeparam>
        /// <param name="groupIdx">The navigation group this item belongs to.</param>
        /// <param name="title">The title of the item.</param>
        /// <param name="allowMultiple">Whether multiple instance with the same target type may be included.</param>
        /// <param name="iconGeometry">The item's icon geometry.</param>
        /// <param name="id">The id associated with the navigator item.</param>
        public void Add<T>(int groupIdx, string title, bool allowMultiple = false, Geometry iconGeometry = null, long id = 0) where T: INavigator
        {
            if (allowMultiple || !Contains<T>())
            {
                NavigatorItem item = new NavigatorItem(groupIdx, typeof(T), id)
                {
                    Title = title,
                    IconGeometry = iconGeometry,
                };
                storage.Add(item);
                backingGroups[groupIdx].Add(item);
            }
        }

        /// <summary>
        /// Attempts to obtain the <see cref="NavigatorItem"/> with the specified target type.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <returns>The item, or null if not found.</returns>
        public NavigatorItem TryGet<T>() where T: INavigator
        {
            foreach (NavigatorItem item in storage)
            {
                if (item.TargetType == typeof(T))
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Attempts to obtain the <see cref="NavigatorItem"/> with the target type and id.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="id">The id</param>
        /// <returns>The item, or null if not found.</returns>
        public NavigatorItem TryGet<T>(long id) where T: INavigator
        {
            foreach (NavigatorItem item in storage)
            {
                if (item.TargetType == typeof(T) && item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets a boolean value that indicates if the collection contains
        /// a <see cref="NavigatorItem"/> with the specified target type.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <returns>true if an item with the specified target type exists in the collection; otherwise, false.</returns>
        public bool Contains<T>() where T: INavigator
        {
            NavigatorItem item = TryGet<T>();
            return item != null;
        }

        /// <summary>
        /// Selects the <see cref="NavigatorItem"/> with the specified target type.
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        public void Select<T>() where T: INavigator
        {
            NavigatorItem item = TryGet<T>();
            if (item != null)
            {
                SelectedItems[item.GroupIndex] = item;
            }
        }

        /// <summary>
        /// Selects the <see cref="NavigatorItem"/> with the specified target type and id.
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="id">The id</param>
        public void Select<T>(long id) where T: INavigator
        {
            NavigatorItem item = TryGet<T>(id);
            if (item != null)
            {
                SelectedItems[item.GroupIndex] = item;
            }
        }

        /// <summary>
        /// Removes the <see cref="NavigatorItem"/> with the specified target type and id.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="id">The id</param>
        public void Remove<T>(long id) where T: INavigator
        {
            NavigatorItem item = TryGet<T>(id);
            if (item != null)
            {
                backingGroups[item.GroupIndex].Remove(item);
                storage.Remove(item);
            }
        }

        /// <summary>
        /// Clears all items with the specified target type.
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        public void Clear<T>() where T: INavigator
        {
            foreach (NavigatorItem item in storage.ToList())
            {
                if (item.TargetType == typeof(T))
                {
                    foreach (var group in backingGroups)
                    {
                        group.Remove(item);
                    }
                    storage.Remove(item);
                }
            }
        }

        /// <summary>
        /// Clears all items in the collection.
        /// </summary>
        public void Clear()
        {
            for (int k = 0; k < Groups.Count; k++)
            {
                backingGroups[k].Clear();
                SelectedItems[k] = null;
            }
            storage.Clear();
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void SelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!selectedItemUpdateInProgress)
            {
                selectedItemUpdateInProgress = true;

                for (int idx = 0; idx < SelectedItems.Count; idx++)
                {
                    if (idx != e.NewStartingIndex)
                    {
                        SelectedItems[idx] = null;
                    }
                }

                if (e.NewItems[0] is NavigatorItem item)
                {
                    SelectedItemChanged?.Invoke(sender, e.NewItems[0] as NavigatorItem);
                }
                selectedItemUpdateInProgress = false;
            }
        }
        #endregion
    }
}