﻿using System.Windows;
using System.Windows.Controls;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides convienence extension methods for <see cref="MenuItem"/>.
    /// </summary>
    public static class MenuItemExtensions
    {
        /// <summary>
        /// Adds an image resource to the menu item.
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="imageResource">The string name of the image resource</param>
        /// <returns>The item</returns>
        public static MenuItem AddImageResource(this MenuItem item, string imageResource)
        {
            if (!string.IsNullOrEmpty(imageResource))
            {
                item.Icon = Application.Current.TryFindResource(imageResource);
            }
            return item;
        }

        /// <summary>
        /// Adds a resource with the specified key to the icon of the menu item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns>The item</returns>
        public static MenuItem AddIconResource(this MenuItem item, object resourceKey)
        {
            if (resourceKey != null)
            {
                item.Icon = Application.Current.TryFindResource(resourceKey);
            }
            return item;
        }

        /// <summary>
        /// Adds a command parameter to the menu item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parm"></param>
        /// <returns>The item</returns>
        public static MenuItem AddCommandParm(this MenuItem item, object parm)
        {
            item.CommandParameter = parm;
            return item;
        }

        /// <summary>
        /// Adds a tag to the menu item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="tag"></param>
        /// <returns>The item</returns>
        public static MenuItem AddTag(this MenuItem item, object tag)
        {
            item.Tag = tag;
            return item;
        }
    }
}
