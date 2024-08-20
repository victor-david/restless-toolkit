using System.Collections.Generic;

namespace Restless.Toolkit.Core
{
    /// <summary>
    /// Represents an extended property collection that may be attached via <see cref="Property.ExtendedProperty"/>
    /// </summary>
    public class ExtendedPropertyCollection : Dictionary<string, object>
    {
        /// <summary>
        /// Gets the value for the specified key, or null
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The value for <paramref name="key"/>, or null</returns>
        public object GetValue(string key)
        {
            if (!string.IsNullOrEmpty(key) && ContainsKey(key))
            {
                return this[key];
            }
            return null;
        }
    }
}