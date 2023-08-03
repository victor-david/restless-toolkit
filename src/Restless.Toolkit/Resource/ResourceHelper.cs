using System.Windows;

namespace Restless.Toolkit.Resource
{
    /// <summary>
    /// Provides static methods to obtain resources.
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// Gets the resource specified by <paramref name="resourceId"/> if it exists
        /// </summary>
        /// <typeparam name="T">The type of resource</typeparam>
        /// <param name="resourceId">The resource id</param>
        /// <returns>The resource as <typeparamref name="T"/>, or null if it doesn't exist</returns>
        public static T Get<T>(object resourceId) where T : class
        {
            return Application.Current.TryFindResource(resourceId) as T;
        }

        /// <summary>
        /// Gets the resource specified by <paramref name="resourceId"/> if it exists
        /// </summary>
        /// <param name="resourceId">The resource id</param>
        /// <returns>The resource as an object, or null if it doesn't exist.</returns>
        public static object Get(object resourceId)
        {
            return Application.Current.TryFindResource(resourceId);
        }
    }
}