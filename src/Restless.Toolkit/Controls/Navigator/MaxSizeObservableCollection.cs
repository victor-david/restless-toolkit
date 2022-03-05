using System;
using System.Collections.ObjectModel;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents an ObservableCollection with a maximum size.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MaxSizeObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Gets the maximum size of this collection.
        /// </summary>
        public int MaxSize
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxSizeObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="maxSize">The maximum size</param>
        public MaxSizeObservableCollection(int maxSize)
        {
            if (maxSize < 1) throw new ArgumentException(nameof(maxSize));
            MaxSize = maxSize;
        }

        /// <inheritdoc/>
        protected override void InsertItem(int index, T item)
        {
            if (Count == MaxSize)
            {
                throw new InvalidOperationException($"Max size of {MaxSize} has been reached.");
            }
            base.InsertItem(index, item);
        }
    }
}