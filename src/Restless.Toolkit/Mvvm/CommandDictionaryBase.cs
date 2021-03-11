using System;
using System.Collections;
using System.Collections.Generic;

namespace Restless.Toolkit.Mvvm
{
    /// <summary>
    /// Represents a generic dictionary of commands. This class must be inherited.
    /// </summary>
    /// <remarks>
    /// A CommandDictionary collection is used by the various view models and associated controllers
    /// to create commands without the need to declare a separate property for each one.
    /// <see cref="CommandDictionary"/> extends this class to use a dictionary with string keys.
    /// A consumer can extend this class to use other types of keys, for example integers or enums.
    /// </remarks>
    public abstract class CommandDictionaryBase<T> : IDictionary<T, RelayCommand>
    {
        #region Private
        private readonly Dictionary<T, RelayCommand> storage;
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Acceses the dictionary value according to the key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The RelayCommand object, or null if not present</returns>
        public RelayCommand this [T key]
        {
            get 
            {
                if (storage.ContainsKey(key))
                {
                    return storage[key];
                }
                return null;
            }
            set
            {
                if (storage.ContainsKey(key))
                {
                    storage[key] = value;
                }
            }
        }

        /// <summary>
        /// Gets the dictionary keys.
        /// </summary>
        public ICollection<T> Keys => ((IDictionary<T, RelayCommand>)storage).Keys;

        /// <summary>
        /// Gets the dictionary values.
        /// </summary>
        public ICollection<RelayCommand> Values => ((IDictionary<T, RelayCommand>)storage).Values;

        /// <summary>
        /// Gets the count of items in the dictionary.
        /// </summary>
        public int Count => ((ICollection<KeyValuePair<T, RelayCommand>>)storage).Count;

        /// <summary>
        /// Gets a boolean value that indicates whether the dictionary is read-only.
        /// </summary>
        public bool IsReadOnly => ((ICollection<KeyValuePair<T, RelayCommand>>)storage).IsReadOnly;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDictionaryBase"/> class.
        /// </summary>
        protected CommandDictionaryBase()
        {
            storage = new Dictionary<T, RelayCommand>();
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Adds a command to the dictionary.
        /// </summary>
        /// <param name="key">The key of the command in the dictionary</param>
        /// <param name="command">The RelayCommand object</param>
        public void Add(T key, RelayCommand command)
        {
            if (key == null) throw new ArgumentException(nameof(key));
            if (command == null) throw new ArgumentNullException(nameof(command));

            if (ContainsKey(key))
            {
                throw new InvalidOperationException(string.Format("The command with key {0} already exists.", key));
            }

            storage.Add(key, command);
        }

        /// <summary>
        /// Adds a command to the dictionary.
        /// </summary>
        /// <param name="key">The command key.</param>
        /// <param name="runCommand">The action to run the command.</param>
        /// <param name="canRunCommand">The predicate to determine if the command can run, or null if it can always run.</param>
        /// <param name="supported">A value that determines if the command is supported.</param>
        /// <param name="parameter">An optional parameter that if set will always be passed to the command method.</param>
        public void Add(T key, Action<object> runCommand, Predicate<object> canRunCommand, CommandSupported supported, object parameter)
        {
            Add(key, RelayCommand.Create(runCommand, canRunCommand, supported, parameter));
        }

        /// <summary>
        /// Adds a command to the dictionary.
        /// </summary>
        /// <param name="key">The command key.</param>
        /// <param name="runCommand">The action to run the command.</param>
        /// <param name="canRunCommand">The predicate to determine if the command can run, or null if it can always run.</param>
        /// <param name="parameter">An optional parameter that if set will always be passed to the command method.</param>
        public void Add(T key, Action<object> runCommand, Predicate<object> canRunCommand, object parameter = null)
        {
            Add(key, RelayCommand.Create(runCommand, canRunCommand, CommandSupported.Yes, parameter));
        }

        /// <summary>
        /// Adds a command without a predicate.
        /// </summary>
        /// <param name="key">The command key.</param>
        /// <param name="runCommand">The action to run the command.</param>
        public void Add(T key, Action<object> runCommand)
        {
            Add(key, RelayCommand.Create(runCommand));
        }

        /// <summary>
        /// Gets a boolean value that indicates if the collection contains the specified key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>true if the key already exists; otherwise, false.</returns>
        public bool ContainsKey(T key)
        {
            return storage.ContainsKey(key);
        }

        /// <summary>
        /// Clears all items from the command dictionary.
        /// </summary>
        public void Clear()
        {
            storage.Clear();
        }

        public bool Remove(T key)
        {
            return ((IDictionary<T, RelayCommand>)storage).Remove(key);
        }

        public bool TryGetValue(T key, out RelayCommand value)
        {
            return ((IDictionary<T, RelayCommand>)storage).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<T, RelayCommand> item)
        {
            ((ICollection<KeyValuePair<T, RelayCommand>>)storage).Add(item);
        }

        public bool Contains(KeyValuePair<T, RelayCommand> item)
        {
            return ((ICollection<KeyValuePair<T, RelayCommand>>)storage).Contains(item);
        }

        public void CopyTo(KeyValuePair<T, RelayCommand>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<T, RelayCommand>>)storage).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<T, RelayCommand> item)
        {
            return ((ICollection<KeyValuePair<T, RelayCommand>>)storage).Remove(item);
        }

        public IEnumerator<KeyValuePair<T, RelayCommand>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<T, RelayCommand>>)storage).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)storage).GetEnumerator();
        }
        #endregion
    }
}